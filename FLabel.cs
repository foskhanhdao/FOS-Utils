using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;

namespace FOS_Utils
{
    public class FLabel:System.Windows.Forms.Label
    {
        private Control outputControl = null;
        private string oldText = "";
        private string _columnName = "";
        public FLabel()
        {
            this.TextChanged += new EventHandler(KLabel_TextChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        public Control OutputControl
        {
            get { return outputControl; }
            set { outputControl = value; }
        }
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
        private void KLabel_TextChanged(object sender, EventArgs e)
        {
            if (OutputControl != null)
            {
                OutputControl.Text = this.Text;
            }
                
        }
    }
}
