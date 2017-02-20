using System;
using System.Collections.Generic;

using System.Text;

namespace FOS_Utils
{
    public class FRadioButton:System.Windows.Forms.RadioButton
    {
        private string _columnName = "";
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
    }
}
