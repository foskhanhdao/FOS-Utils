using System;
using System.Collections.Generic;

using System.Text;

namespace FOS_Utils
{
    public class FPictureBox:System.Windows.Forms.PictureBox,IControl
    {
        private string _columnName = "";
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        public object DBValue
        {
            get { return this.ImageLocation; }
            set { if (value != null) this.ImageLocation = value.ToString(); }
        }
    }
}
