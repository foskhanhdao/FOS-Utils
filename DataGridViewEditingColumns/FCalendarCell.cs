using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FOS_Utils
{
    public class CalendarCell : DataGridViewTextBoxCell
    {
        private String format = "dd/MM/yyyy";
        public CalendarCell()
            : base()
        {
            this.Style.Format = format;
        }
        public CalendarCell(String strFormat)
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
                CalendarEditingControl ctl =
                    DataGridView.EditingControl as CalendarEditingControl;
                if (this.Value != null && !this.Value.ToString().Equals(""))
                {                    
                    ctl.Value = DateTime.Parse(this.Value.ToString());
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
                return typeof(CalendarEditingControl);
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
                return DateTime.Now.ToShortDateString();
            }
        }
    }


}
