using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;

namespace FOS_Utils
{
    public class FDateTimePicker:System.Windows.Forms.DateTimePicker,IControl
    {
        private string _columnName = "";
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        public object DBValue
        {
            get { return this.Value; }
            set { if (value is DateTime) this.Value = Convert.ToDateTime( value); }
        }
    }
}
