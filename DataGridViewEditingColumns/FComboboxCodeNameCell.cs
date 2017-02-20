using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FOS_Utils
{
    public class FComboBoxCodeNameCell: DataGridViewTextBoxCell
    {
        private ImeMode ime = ImeMode.Hiragana;        
        public FComboBoxCodeNameCell()
            : base()
        {           
            //this.Style.Format = "d";                      
        }

        public FComboBoxCodeNameCell(ImeMode ime)
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
            FComboboxCodeNameEditingControl ctl =
                DataGridView.EditingControl as FComboboxCodeNameEditingControl;

            if (this.Value != null)
            {
                ctl.Text = this.Value.ToString();
            }
        }

        public override Type EditType
        {
            get
            {
                return typeof(FComboboxCodeNameEditingControl);
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
