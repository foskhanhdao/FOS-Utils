using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace FOS_Utils
{
    public class CustomCell : DataGridViewTextBoxCell
    {
        private String type = "";
        public CustomCell()
            : base()
        {
            //this.Style.Format = "d";
        }
        public CustomCell(String type)
            : base()
        {
            this.type = type;
            //this.Style.Format = "d";
        }

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);

            CustomEditingControl ctl = 
                DataGridView.EditingControl as CustomEditingControl;
            
            if (this.Value != null)
            {
            
                ctl.Text = this.Value.ToString();
            
            }
        }

        public override Type EditType
        {
            get
            {
                return typeof(CustomEditingControl);
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
