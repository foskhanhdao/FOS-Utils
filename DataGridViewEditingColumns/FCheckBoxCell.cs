using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace FOS_Utils
{
    public class CheckBoxCell : DataGridViewCheckBoxCell
    {
        public CheckBoxCell()
            : base()
        {
            //this.Style.Format = "d";
        }

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            CheckBoxEditingControl ctl =
                DataGridView.EditingControl as CheckBoxEditingControl;
            if (this.Value != null)
            {
                ctl.Checked = (bool)this.Value;
            }
        }

        public override Type EditType
        {
            get
            {
                return typeof(CheckBoxEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(bool);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return false;
            }
        }
    }

}
