using System;
using System.Collections.Generic;

using System.Text;

namespace FOS_Utils
{
    public class FCheckBox:System.Windows.Forms.CheckBox,IControl
    {
        private string _columnName = "";
        private object valueWhenTrue = 1;
        private object valueWhenFalse = 0;
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
        public object ValueWhenTrue
        {
            get { return valueWhenTrue; }
            set { valueWhenTrue = value; }
        }
        public object ValueWhenFalse
        {
            get { return valueWhenFalse; }
            set { valueWhenFalse = value; }
        }

        public object DBValue
        {
            get { return this.Checked ? "1" : "0"; }
            set { if (value != null) this.Checked = value.ToString() == "1"; }
        }
    }
}
