using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace FOS_Utils
{
    public class TimeCell : DataGridViewTextBoxCell
    {        
        public TimeCell()
            : base()
        {            
            
        }        

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);

            TimeEditingControl ctl =
                DataGridView.EditingControl as TimeEditingControl;

            if (this.Value != null)
            {
                ctl.Text = this.Value.ToString();
            }
        }

        public override Type EditType
        {
            get
            {
                return typeof(TimeEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {

                return typeof(String);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return "00:00";
            }
        }
    }
}
