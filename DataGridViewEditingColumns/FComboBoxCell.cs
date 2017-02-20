using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
namespace FOS_Utils
{
    public class FComboBoxCell : DataGridViewTextBoxCell
    {
        private ImeMode ime = ImeMode.Hiragana;        
        public FComboBoxCell()
            : base()
        {           
            //this.Style.Format = "d";                      
        }
        
        public FComboBoxCell(ImeMode ime)
            : base()
        {
            this.ime = ime;
            //this.Style.Format = "d";           
        }
        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            ComboBoxEditingControl ctl =
                DataGridView.EditingControl as ComboBoxEditingControl;

            if (this.Value != null)
            {
                ctl.Text = this.Value.ToString();
            }
        }

        public override Type EditType
        {
            get
            {
                return typeof(ComboBoxEditingControl);
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
