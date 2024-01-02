﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Adani.BAU.AdaniWelspunSXA.Project
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Sitecore.ExperienceForms")]
	public partial class AdaniWelspunEntityDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertFormEntry(FormEntry instance);
    partial void UpdateFormEntry(FormEntry instance);
    partial void DeleteFormEntry(FormEntry instance);
    partial void InsertFieldData(FieldData instance);
    partial void UpdateFieldData(FieldData instance);
    partial void DeleteFieldData(FieldData instance);
    #endregion
		
		public AdaniWelspunEntityDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["experienceforms"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public AdaniWelspunEntityDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AdaniWelspunEntityDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AdaniWelspunEntityDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AdaniWelspunEntityDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<FormEntry> FormEntries
		{
			get
			{
				return this.GetTable<FormEntry>();
			}
		}
		
		public System.Data.Linq.Table<FieldData> FieldDatas
		{
			get
			{
				return this.GetTable<FieldData>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="sitecore_forms_storage.FormEntries")]
	public partial class FormEntry : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _Id;
		
		private System.Guid _FormDefinitionId;
		
		private System.Nullable<System.Guid> _ContactId;
		
		private bool _IsRedacted;
		
		private System.DateTime _Created;
		
		private EntitySet<FieldData> _FieldDatas;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(System.Guid value);
    partial void OnIdChanged();
    partial void OnFormDefinitionIdChanging(System.Guid value);
    partial void OnFormDefinitionIdChanged();
    partial void OnContactIdChanging(System.Nullable<System.Guid> value);
    partial void OnContactIdChanged();
    partial void OnIsRedactedChanging(bool value);
    partial void OnIsRedactedChanged();
    partial void OnCreatedChanging(System.DateTime value);
    partial void OnCreatedChanged();
    #endregion
		
		public FormEntry()
		{
			this._FieldDatas = new EntitySet<FieldData>(new Action<FieldData>(this.attach_FieldDatas), new Action<FieldData>(this.detach_FieldDatas));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FormDefinitionId", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid FormDefinitionId
		{
			get
			{
				return this._FormDefinitionId;
			}
			set
			{
				if ((this._FormDefinitionId != value))
				{
					this.OnFormDefinitionIdChanging(value);
					this.SendPropertyChanging();
					this._FormDefinitionId = value;
					this.SendPropertyChanged("FormDefinitionId");
					this.OnFormDefinitionIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ContactId", DbType="UniqueIdentifier")]
		public System.Nullable<System.Guid> ContactId
		{
			get
			{
				return this._ContactId;
			}
			set
			{
				if ((this._ContactId != value))
				{
					this.OnContactIdChanging(value);
					this.SendPropertyChanging();
					this._ContactId = value;
					this.SendPropertyChanged("ContactId");
					this.OnContactIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsRedacted", DbType="Bit NOT NULL")]
		public bool IsRedacted
		{
			get
			{
				return this._IsRedacted;
			}
			set
			{
				if ((this._IsRedacted != value))
				{
					this.OnIsRedactedChanging(value);
					this.SendPropertyChanging();
					this._IsRedacted = value;
					this.SendPropertyChanged("IsRedacted");
					this.OnIsRedactedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Created", DbType="DateTime2 NOT NULL")]
		public System.DateTime Created
		{
			get
			{
				return this._Created;
			}
			set
			{
				if ((this._Created != value))
				{
					this.OnCreatedChanging(value);
					this.SendPropertyChanging();
					this._Created = value;
					this.SendPropertyChanged("Created");
					this.OnCreatedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="FormEntry_FieldData", Storage="_FieldDatas", ThisKey="Id", OtherKey="FormEntryId")]
		public EntitySet<FieldData> FieldDatas
		{
			get
			{
				return this._FieldDatas;
			}
			set
			{
				this._FieldDatas.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_FieldDatas(FieldData entity)
		{
			this.SendPropertyChanging();
			entity.FormEntry = this;
		}
		
		private void detach_FieldDatas(FieldData entity)
		{
			this.SendPropertyChanging();
			entity.FormEntry = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="sitecore_forms_storage.FieldData")]
	public partial class FieldData : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _Id;
		
		private System.Guid _FormEntryId;
		
		private System.Guid _FieldDefinitionId;
		
		private string _FieldName;
		
		private string _Value;
		
		private string _ValueType;
		
		private EntityRef<FormEntry> _FormEntry;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(System.Guid value);
    partial void OnIdChanged();
    partial void OnFormEntryIdChanging(System.Guid value);
    partial void OnFormEntryIdChanged();
    partial void OnFieldDefinitionIdChanging(System.Guid value);
    partial void OnFieldDefinitionIdChanged();
    partial void OnFieldNameChanging(string value);
    partial void OnFieldNameChanged();
    partial void OnValueChanging(string value);
    partial void OnValueChanged();
    partial void OnValueTypeChanging(string value);
    partial void OnValueTypeChanged();
    #endregion
		
		public FieldData()
		{
			this._FormEntry = default(EntityRef<FormEntry>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FormEntryId", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid FormEntryId
		{
			get
			{
				return this._FormEntryId;
			}
			set
			{
				if ((this._FormEntryId != value))
				{
					if (this._FormEntry.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnFormEntryIdChanging(value);
					this.SendPropertyChanging();
					this._FormEntryId = value;
					this.SendPropertyChanged("FormEntryId");
					this.OnFormEntryIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FieldDefinitionId", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid FieldDefinitionId
		{
			get
			{
				return this._FieldDefinitionId;
			}
			set
			{
				if ((this._FieldDefinitionId != value))
				{
					this.OnFieldDefinitionIdChanging(value);
					this.SendPropertyChanging();
					this._FieldDefinitionId = value;
					this.SendPropertyChanged("FieldDefinitionId");
					this.OnFieldDefinitionIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FieldName", DbType="NVarChar(256) NOT NULL", CanBeNull=false)]
		public string FieldName
		{
			get
			{
				return this._FieldName;
			}
			set
			{
				if ((this._FieldName != value))
				{
					this.OnFieldNameChanging(value);
					this.SendPropertyChanging();
					this._FieldName = value;
					this.SendPropertyChanged("FieldName");
					this.OnFieldNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Value", DbType="NVarChar(MAX)")]
		public string Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				if ((this._Value != value))
				{
					this.OnValueChanging(value);
					this.SendPropertyChanging();
					this._Value = value;
					this.SendPropertyChanged("Value");
					this.OnValueChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ValueType", DbType="NVarChar(256)")]
		public string ValueType
		{
			get
			{
				return this._ValueType;
			}
			set
			{
				if ((this._ValueType != value))
				{
					this.OnValueTypeChanging(value);
					this.SendPropertyChanging();
					this._ValueType = value;
					this.SendPropertyChanged("ValueType");
					this.OnValueTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="FormEntry_FieldData", Storage="_FormEntry", ThisKey="FormEntryId", OtherKey="Id", IsForeignKey=true)]
		public FormEntry FormEntry
		{
			get
			{
				return this._FormEntry.Entity;
			}
			set
			{
				FormEntry previousValue = this._FormEntry.Entity;
				if (((previousValue != value) 
							|| (this._FormEntry.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._FormEntry.Entity = null;
						previousValue.FieldDatas.Remove(this);
					}
					this._FormEntry.Entity = value;
					if ((value != null))
					{
						value.FieldDatas.Add(this);
						this._FormEntryId = value.Id;
					}
					else
					{
						this._FormEntryId = default(System.Guid);
					}
					this.SendPropertyChanged("FormEntry");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
