using System;
using System.Collections.Generic;

using System.Text;

namespace FOS_Utils
{
    public class FComboBox:System.Windows.Forms.ComboBox,IControl
    {
        private string columnName  = "";

        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        public object DBValue
        {
            get { return this.Text; }
            set { if (value != null) Text = value.ToString(); }
        }
    }
}
