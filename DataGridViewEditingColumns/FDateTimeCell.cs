using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace FOS_Utils
{
    public class DateTimeCell : DataGridViewTextBoxCell
    {
        public DateTimeCell()
            : base()
        {
            //this.Style.Format = "#,##0";
            
        }
        public DateTimeCell(string format)
            : base()
        {
            //this.Style.Format = format;
        }

        public void setFormat(string format)
        {
           // this.Style.Format = format;
        }
        protected override object GetValue(int rowIndex)
        {
            if (rowIndex >= 0)
                return base.GetValue(rowIndex);
            return DBNull.Value;
        }
        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            FDateTimeEditingControl ctl =
                DataGridView.EditingControl as FDateTimeEditingControl;
            object val = GetValue(rowIndex);
            if (val != null)
            {
                ctl.Text = val.ToString();
            }
        }

        public override Type EditType
        {
            get
            {
                return typeof(FDateTimeEditingControl);
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
                return null;
            }
        }
    }

}
