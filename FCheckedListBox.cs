using System;
using System.Collections.Generic;

using System.Text;

namespace FOS_Utils
{
    public class FCheckedListBox:System.Windows.Forms.CheckedListBox,IControl
    {
        private string _columnName = "";
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        public object DBValue
        {
            get { return this.Text; }
            set { if (value != null) this.Text = value.ToString(); }
        }
    }
}
