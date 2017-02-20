using System;
using System.Collections.Generic;

using System.Text;

namespace FOS_Utils
{
    public class FListView:System.Windows.Forms.ListView,IControl
    {
        private string _columnName = "";
        private object _dbValue = DBNull.Value;
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
        public object DBValue
        {
            get { return _dbValue; }
            set { _dbValue = value; }
        }
    }
}
