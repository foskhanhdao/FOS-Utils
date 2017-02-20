using System;
using System.Collections.Generic;

using System.Text;

namespace FOS_Utils
{
    public class FRichTextBox:System.Windows.Forms.RichTextBox
    {
        private string _columnName = "";
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
    }
}
