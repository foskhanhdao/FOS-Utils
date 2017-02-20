using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace FOS_Utils
{
    public class TextBoxCell : DataGridViewTextBoxCell
    {
       
        public TextBoxCell()
            : base()
        {
            //this.Style.Format = "d";
        }
        public TextBoxCell(int maxLength)
            : base()
        {
            this.MaxInputLength = maxLength;
            //this.Style.Format = "d";
        }
        protected override object GetValue(int rowIndex)
        {
            if (rowIndex >= 0)
                return base.GetValue(rowIndex);
            return "";
        }
        

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            FTextBoxEditingControl ctl =
                DataGridView.EditingControl as FTextBoxEditingControl;
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
                return typeof(FTextBoxEditingControl);
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
                return "";
            }
        }
    }

}
