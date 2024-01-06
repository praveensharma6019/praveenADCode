/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using DocumentFormat.OpenXml.Office2010.Excel;
using Farmpik.Domain.Common;
using Farmpik.Domain.Entities;
using Farmpik.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Farmpik.Infrastructure.Repository
{
    [ExcludeFromCodeCoverage]
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : AuditEntity
    {
        private readonly AppDbContext _context;
        private DbSet<TEntity> _entities;

        protected GenericRepository(AppDbContext context)
        {
            _entities = context.Set<TEntity>();
            _context = context;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            this._entities = _context.Set<TEntity>();
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;
            this._entities.Add(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? entity : null;
        }

        public virtual async Task<bool> AddAsync(List<TEntity> entities, Guid createdBy)
        {
            this._entities = _context.Set<TEntity>();
            double seconds = 0;
            entities.ForEach(x =>
            {
                x.CreatedOn = DateTime.UtcNow.AddMilliseconds(seconds);
                x.IsActive = true;
                x.CreatedBy = createdBy;
                seconds++;
            });

            this._entities.AddRange(entities);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _entities.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                entity.IsActive = false;
                entity.ChangedOn = DateTime.UtcNow;
                return (await _context.SaveChangesAsync()) > 0;
            }
            return false;
        }

        public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            var entities = _entities.Where(filter).ToList();
            foreach (var entity in entities)
            {
                entity.IsActive = false;
                entity.ChangedOn = DateTime.UtcNow;
            }
            return (await _context.SaveChangesAsync()) > 0;
        }

        public virtual async Task<PaginatedList<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            int pageNumber = 1,
            int pageSize = int.MaxValue,
            Expression<Func<TEntity, object>> orderBy = null,
            bool sortbyAsce = true
        )
        {
            this._entities = _context.Set<TEntity>();

            var query =
                filter != null ? this._entities.Where(filter) : this._entities.Where(x => true);

            return new PaginatedList<TEntity>
            {
                TotalCount = await query.CountAsync(),
                Value =
                    orderBy != null
                        ? (sortbyAsce ? await query
                            .OrderBy(orderBy)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync() :
                            await query
                            .OrderByDescending(orderBy)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync())

                        : await query
                            .OrderByDescending(x => x.CreatedOn)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync()
            };
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool onlyActive = true)
        {
            return await _entities.Where(x => !onlyActive || x.IsActive).OrderByDescending(x => x.CreatedOn).ToListAsync();
        }

        public virtual async Task<TEntity> GetAsync(Guid id, bool onlyActive = true)
        {
            return await _context
                .Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id == id && (!onlyActive || x.IsActive));
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(filter);
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            entity.ChangedOn = DateTime.UtcNow;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public bool BulkSave(string tablename, List<TEntity> entities, Guid createdBy, string conditions = null)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Inside BulkSave Method", this);
                if (entities.Count > 0)
                {
                    Sitecore.Diagnostics.Log.Info("entities.Count is:" + entities.Count, this);
                    double seconds = 10;
                    entities.ForEach(x =>
                    {
                        x.Id = Guid.NewGuid();
                        x.CreatedOn = DateTime.UtcNow.AddMilliseconds(seconds);
                        x.IsActive = true;
                        x.CreatedBy = createdBy;
                        seconds= seconds+10;
                    });
                    DataTable dataTable = ConvertToDataTable(entities);
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["FarmpikContext"].ConnectionString))
                    {
                        Sitecore.Diagnostics.Log.Info("Sql connection created inside Bulk save method", this);
                        using (SqlCommand sqlCommand = new SqlCommand())
                        {
                            connection.Open();
                            sqlCommand.Connection = connection;
                            sqlCommand.CommandTimeout = 0;
                            sqlCommand.CommandText = $"DELETE FROM {tablename}" + (string.IsNullOrEmpty(conditions) ? ";" : $" WHERE {conditions};");
                            sqlCommand.ExecuteNonQuery();
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
                            {
                                Sitecore.Diagnostics.Log.Info("Started using SQLBulkCopy class", this);
                                sqlBulkCopy.BulkCopyTimeout = 0;
                                sqlBulkCopy.DestinationTableName = tablename;
                                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(TEntity));
                                foreach (PropertyDescriptor prop in properties)
                                    sqlBulkCopy.ColumnMappings.Add(prop.Name, prop.Name);

                                sqlBulkCopy.WriteToServer(dataTable);
                                Sitecore.Diagnostics.Log.Info("SQLBulk copy executed and file written to server", this);
                            }
                            connection.Close();
                            return true;
                        }
                    }
                }

                Sitecore.Diagnostics.Log.Info("Entities counts is less than 0", this);
                return false;

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Info("Exception in BulkSave method, exception:" + ex, this);
                return false;
            }
        }

        private static DataTable ConvertToDataTable(IList<TEntity> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(TEntity));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                if (prop.Name == "Id")
                {
                    table.Columns.Add(prop.Name, typeof(Guid));
                }
                else
                {
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
            }

            foreach (TEntity item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }
}