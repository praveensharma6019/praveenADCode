using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Models
{
    public class TableData
    {
        public List<TableModel> tableData { get; set; }

        public TableData() 
        {
            tableData = new List<TableModel>();
        }
    }

    public class TableModel
    {
        public string rowTitle { get; set; }
        public Column column { get; set; }

        public TableModel()
        {
            column = new Column();
        }
    }
    public class Column
    {
        public List<string> columnValue { get; set; }
    }

}