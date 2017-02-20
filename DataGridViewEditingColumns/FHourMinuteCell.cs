using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FOS_Utils
{
    public class HourMinuteCell : DataGridViewTextBoxCell
    {
        private String format = "HH:mm";
        public HourMinuteCell()
            : base()
        {
            this.Style.Format = format;            
        }
        public HourMinuteCell(String strFormat)
            : base()
        {
            this.format = strFormat;
            this.Style.Format = format;           
                
        }

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            try
            {
                base.InitializeEditingControl(rowIndex, initialFormattedValue,
                    dataGridViewCellStyle);
                HourMinuteEditingControl ctl =
                    DataGridView.EditingControl as HourMinuteEditingControl;
                if (this.Value != null && !this.Value.ToString().Equals(""))
                {                    
                    ctl.Value =  (DateTime)this.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public override Type EditType
        {
            get
            {
                return typeof(HourMinuteEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(DateTime);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return DateTime.Now;
            }
        }
    }


}
