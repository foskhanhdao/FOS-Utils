using System;
using System.Collections.Generic;

using System.Text;

namespace FOS_Utils
{
    public class FMaskedTextBox:System.Windows.Forms.MaskedTextBox
    {
        private string _columnName = "";
        private string oldText = "";
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
        public string OldText
        {
            get { return oldText; }
            set { oldText = value; }
        }
        public string ValueWithoutMask(char mask)
        {
            return this.Text.Replace(mask.ToString(), "");
        }

    }
}
