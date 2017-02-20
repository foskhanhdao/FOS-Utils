using System;
using System.Windows.Forms;
namespace FOS_Utils
{
    public class FDateTimeEditingControl : FDate, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;
        private int maxLength = 5;
        private int decimalLength = 0;
        
        public FDateTimeEditingControl()
        {
            
            //this.TextAlign = HorizontalAlignment.Right;
            //this.Enter += new EventHandler(inputControl_Enter);
            //this.Leave += new EventHandler(inputControl_Leave);
            //this.KeyPress += new KeyPressEventHandler(inputControl_KeyPress);
            //this.KeyDown += new KeyEventHandler(inputControl_KeyDown);
        }
        public object EditingControlFormattedValue
        {
            get
            {
                //MessageBox.Show(this.Text);
                //if (this.Value.ToString().Trim().Length == 0 || this.Value.GetType() != typeof(DateTime))
                //    return DBNull.Value;
                                
                return this.Value;
                
            }
            set
            {
                if (value != null && value.ToString().Length > 0 && value is DateTime)
                {
                    this.Text = Convert.ToDateTime(value).ToShortDateString();
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
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                case Keys.Back:
                case Keys.Enter:
                    return true;
                default:
                    return false;
            }
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
                
        //private void inputControl_KeyPress(object sender, KeyPressEventArgs e)
        //{

        //    if (this.Text.Length == maxLength && !Char.IsControl(e.KeyChar) && this.SelectionLength == 0)
        //    {
        //        e.Handled = true;
        //    }
        //    else
        //    {
        //        if (decimalLength > 0 && !this.Text.Contains(".") && e.KeyChar == '.')
        //        {
        //        }
        //        else if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
        //        {
        //            e.Handled = true;
        //        }
        //        else
        //        {
        //            string[] value = this.Text.Split(new char[] { '.' });
        //            if (value.Length > 1)
        //            {
        //                if (value[1].Length == decimalLength && this.SelectionLength == 0)//&& !Char.IsControl(e.KeyChar))
        //                {
        //                    e.Handled = true;
        //                }
        //            }
        //            else if (((this.Text.Length == maxLength - decimalLength - 1 && decimalLength > 0)
        //                || (this.Text.Length == maxLength - decimalLength && decimalLength == 0))
        //                && !Char.IsControl(e.KeyChar) && this.SelectionLength == 0)
        //            {
        //                e.Handled = true;
        //            }
        //        }
        //    }
        //}

        //private void inputControl_KeyDown(object sender, KeyEventArgs e)
        //{
        //    //MessageBox.Show(e.KeyCode + "");
        //    if (decimalLength > 0 && !this.Text.Contains(".")
        //        && (e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod))
        //    {
        //        this.Text = this.Text + ".";
        //        this.Select(this.Text.Length, 1);
        //    }
        //}
        private void inputControl_Leave(object sender, EventArgs e)
        {


        }

        //private void inputControl_Enter(object sender, EventArgs e)
        //{
        //    if (this.Value != null && this.Value.ToString().Length > 0)
        //    {
        //        this.Text = Convert.ToDateTime(this.Value).ToShortDateString();
        //    }
        //}

        public override int MaxLength
        {
            get
            {
                return maxLength;
            }
            set
            {
                maxLength = value;

                if (decimalLength > 0)
                {
                    maxLength = maxLength + 1;
                }
            }
        }
        public int DecimalLength
        {
            get { return decimalLength; }
            set { decimalLength = value; }
        }

    }
}
