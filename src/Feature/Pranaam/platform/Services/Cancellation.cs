using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.Data.Fields;
using System;
using System.Collections.Generic;
using Sitecore.Mvc.Presentation;
using System.Reflection;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class Cancellation : ICancellation
    {
        private readonly ILogRepository _logRepository;

        public Cancellation(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public CancellationModel GetCancellationData(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            CancellationModel table = new CancellationModel();
            try
            {
                var ds = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                MultilistField headerField = ds?.Fields[Templates.Cancellation.Fields.TableHeader];
                table.Header = new List<CancellationHeader>();
                table.Rows = new List<CancellationRow>();
                if (ds != null)
                {
                    foreach (var tableItem in headerField.GetItems())
                    {
                        CancellationHeader header = new CancellationHeader();
                        header.Title = tableItem?.Fields[Templates.CancellationHeader.Fields.Title]?.Value;
                        header.Value = tableItem?.Fields[Templates.CancellationHeader.Fields.Value]?.Value;
                        table.Header.Add(header);
                    }
                }
                MultilistField chargesField = ds?.Fields[Templates.Cancellation.Fields.TableRows];
                if (chargesField != null && chargesField.GetItems().Length > 0)
                {
                    foreach (var tableRowItem in chargesField.GetItems())
                    {
                        CancellationRow row = new CancellationRow();
                        row.SrNo = tableRowItem?.Fields[Templates.CancellationRow.Fields.SrNo]?.Value;
                        row.Services = tableRowItem?.Fields[Templates.CancellationRow.Fields.CancellationService]?.Value;
                        row.Charges = tableRowItem?.Fields[Templates.CancellationRow.Fields.CancellationCharge]?.Value;
                        table.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return table;
        }

    }
}