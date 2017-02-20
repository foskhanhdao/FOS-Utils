using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FOS_Utils
{
    public class FTextBoxEditingControl : FTextBox, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public FTextBoxEditingControl()
        {
            DBNullValueWhenBlank = true;
        }        
        public object EditingControlFormattedValue
        {
            
            get
            {
                if (this.CheckMinMaxChar && FillZeroWhenHavingText
                    && this.Text.Trim().Length > 0)
                {                   
                    return Convert.ToInt32(this.Text.Trim()).ToString(new string('0', MaxLength));
                }
                return this.DBValue;                
            }
            set
            {
                String newValue = value as String;
                if (newValue != null)
                {
                    this.Text = newValue;                    
                }
            }
        }

        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
        }

        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {
            return true;
            //Console.WriteLine("KEY:" + (key & Keys.KeyCode).ToString());
            //switch (key & Keys.KeyCode)
            //{
            //    case Keys.Left:
            //    case Keys.Up:
            //    case Keys.Down:
            //    case Keys.Right:
            //    case Keys.Home:
            //    case Keys.End:
            //    case Keys.PageDown:
            //    case Keys.PageUp:
            //    case Keys.OemPeriod:
            //    case Keys.Decimal:                
            //        return true;
            //    default:
            //        return false;
            //}
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }

        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        protected override void OnTextChanged(EventArgs eventargs)
        {
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnTextChanged(eventargs);
        }
    }
}
