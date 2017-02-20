using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace FOS_Utils
{
    public class FNumberEditingControl : FTextBox, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;
        private int maxLength = 5;
        private int decimalLength = 0;
        private bool decimalShowZero = false;
        CultureInfo culture = new CultureInfo("ja-JP");
        private int havingDecimalLength = 0;
        private string format = "#,##0";
        public FNumberEditingControl()
        {            
            culture.NumberFormat.NumberDecimalSeparator = ".";
            culture.NumberFormat.NumberGroupSeparator = ",";
            this.ImeMode = ImeMode.Disable;
            this.TextAlign = HorizontalAlignment.Right;
            this.Enter += new EventHandler(inputControl_Enter);
            this.Leave += new EventHandler(inputControl_Leave);
            this.KeyPress += new KeyPressEventHandler(inputControl_KeyPress);
            this.KeyDown += new KeyEventHandler(inputControl_KeyDown);
        }
        public object EditingControlFormattedValue
        {
            get
            {
                return this.Text.Replace(",", "");               
            }
            set
            {
                if (value != null)
                {
                    this.Text = value.ToString();
                }
            }
        }

        public bool DecimalShowZero
        {
            get { return decimalShowZero; }
            set { decimalShowZero = value; }
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
       
        private void inputControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((this.Text.Replace(".", "").Length == MaxLength && !Char.IsControl(e.KeyChar))
                || (this.Text.Length == 0 && e.KeyChar == '.'))// && this.SelectionLength == 0)
            {
                e.Handled = true;
            }
            else
            {
                if (DecimalLength > 0 && !this.Text.Contains(".") && e.KeyChar == '.')
                {
                }
                else if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
                else if (char.IsDigit(e.KeyChar))
                {
                    string[] value = this.Text.Split(new char[] { '.' });
                    if (value.Length > 1)
                    {
                        if (value[0].Length == MaxLength - DecimalLength && this.Text.IndexOf('.') > this.SelectionStart)//&& !Char.IsControl(e.KeyChar))
                        {
                            e.Handled = true;
                        }

                        else if (value[1].Length == DecimalLength && this.Text.IndexOf('.') < this.SelectionStart)//&& !Char.IsControl(e.KeyChar))
                        {
                            e.Handled = true;
                        }
                    }
                    else if (this.Text.Length >= MaxLength - DecimalLength)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void inputControl_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod)
                && DecimalLength > 0 && !this.Text.Contains("."))
            {
                int pos = this.SelectionStart;
                this.Text = this.Text.Insert(this.SelectionStart, ".");
                this.SelectionStart = pos + 1;
            } 
            else if ((e.KeyCode == Keys.Delete && this.SelectionStart == this.Text.IndexOf('.') && this.Text.Replace(".", "").Length > MaxLength - DecimalLength)
                || (e.KeyCode == Keys.Back && this.SelectionStart == this.Text.IndexOf('.') + 1 && this.Text.Replace(".", "").Length > MaxLength - DecimalLength)
                || ((e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod)
                && this.Text.Contains(".")))
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.V && e.Control)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                GetNumberFromClipboard();
            }
        }
        private void GetNumberFromClipboard()
        {
            try
            {
                StringBuilder str = new StringBuilder();
                IDataObject clipboardData = Clipboard.GetDataObject();
                object clipboard = clipboardData.GetData(DataFormats.UnicodeText);
                if (clipboard != null && !clipboard.Equals(DBNull.Value))
                {
                    string cText = clipboard.ToString();
                    string[] sepa = this.Text.Replace(",", "").Split(new char[] { '.' });
                    string chan = sepa[0];
                    string le = "";
                    if (sepa.Length > 1)
                        le = sepa[1];

                    if (this.SelectionLength == this.Text.Length)
                    {
                        base.Text = "";
                        sepa = cText.Replace(",", "").Split(new char[] { '.' });
                        chan = sepa[0];
                        if (sepa.Length > 1)
                            le = sepa[1];
                        for (int i = chan.Length - 1; i >= 0; i--)
                        {
                            if (char.IsDigit(chan[i]) && str.ToString().Length < MaxLength - DecimalLength)
                                str.Append(chan[i]);
                        }
                        
                        //Reverse
                        char[] chars = str.ToString().ToCharArray();
                        Array.Reverse(chars);
                        str.Length = 0;
                        str.Append(new string(chars));

                        StringBuilder strLe = new StringBuilder();
                        for (int i = 0; i < le.Length; i++)
                        {
                            if (char.IsDigit(le[i]) && strLe.ToString().Length < DecimalLength)
                                strLe.Append(le[i]);
                        }
                        if (strLe.ToString().Length > 0)
                            str.Append(".");
                        str.Append(strLe);
                        base.Text = str.ToString();
                        return;
                    }
                   

                    for (int i = 0; i < cText.Length; i++)
                    {
                        if (char.IsDigit(cText[i]))
                        {
                            if (SelectionStart + SelectionLength <= chan.Length
                                && chan.Length + str.Length < MaxLength - DecimalLength)
                            {
                                str.Append(cText[i]);
                            }
                            else if (SelectionStart + SelectionLength > chan.Length + havingDecimalLength
                                && le.Length + str.Length < DecimalLength && this.Text.Contains("."))
                            {
                                str.Append(cText[i]);
                            }
                        }                        
                    }
                    base.Text = this.Text.Insert(this.SelectionStart, str.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void inputControl_Leave(object sender, EventArgs e)
        {
            if (this.Text.Equals("") || this.Text.Equals("."))
                this.Text = "0";            
            this.Value = double.Parse(this.Text.Replace(",", ""));
        }

        private void inputControl_Enter(object sender, EventArgs e)
        {
            this.Text = this.Text.Replace(",", "");
            this.SelectAll();
            this.Text.PadRight(Text.Length);

        }

        public double Value
        {
            get
            {
                double dResult = 0;
                if (this.Text.Length > 0)
                {
                    dResult = double.Parse(this.Text.Replace(",", ""), culture.NumberFormat);
                }
                return dResult;
            }
            set
            {
                this.Text = value.ToString();
            }
        }
        public string Format
        {
            get { return format; }
            set { format = value; }
        }
        public override int MaxLength
        {
            get
            {
                return maxLength;
            }
            set
            {
                maxLength = value;
            }
        }
        public int DecimalLength
        {
            get { return decimalLength; }
            set { 
                decimalLength = value;
                if (decimalLength == 0)
                    havingDecimalLength = 0;
                else
                    havingDecimalLength = 1;
            }
        }

    }
}
