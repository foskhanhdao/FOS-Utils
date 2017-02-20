using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace FOS_Utils
{
    public class FNumber : TextBox
    {
        
        private int maxLength = 8;
        private int decimalLength = 3;
        private bool decimalShowZero = false;        
        private bool thousandsSeparator = true;
        private string _columnName = "";
        private int havingDecimalLength = 0;
        private int havingMinusLength = 0;
        CultureInfo culture = new CultureInfo("ja-JP");
        private double valueMax = 99999.999;
        private double valueMin = 0;
        private bool SetTextWhenValueChanged = true;
        private bool autoResize = true;
        private double oldValue = 0;
       

     
        public FNumber()
        {            
            culture.NumberFormat.NumberDecimalSeparator = ".";
            culture.NumberFormat.NumberGroupSeparator = ",";
            this.ImeMode = ImeMode.Disable;
            this.TextAlign = HorizontalAlignment.Right;
            this.ContextMenu = new ContextMenu();
        }
        
        public double ValueMax
        {
            get { return valueMax; }
            set { valueMax = value; }
        }
        public double ValueMin
        {
            get { return valueMin; }
            set { valueMin = value; }
        }
        public override int MaxLength
        {
            get
            {                
                return maxLength;
            }
            set
            {
                try
                {
                    if (value < DecimalLength)
                    {
                        maxLength = DecimalLength;
                    }
                    else
                    {
                        maxLength = value;
                        if (value > 0 && maxLength - DecimalLength > 0)
                            ValueMax = double.Parse(new string('9', maxLength - DecimalLength) + "." + new string('9', DecimalLength), culture.NumberFormat);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                     
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
        public bool DecimalShowZero
        {
            get { return decimalShowZero; }
            set { decimalShowZero = value; }
        }
        public bool ThousandsSeparator
        {
            get { return thousandsSeparator; }
            set { thousandsSeparator = value; }
        }

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
        public double Value
        {
            get
            {
                try
                {
                    double dResult = 0;
                    if (this.Text.Length > 0)
                    {
                        dResult = double.Parse(this.Text, culture.NumberFormat);
                    }
                    return dResult;
                }
                catch (Exception ex)
                {                    
                    return 0;
                }
            }
            set
            {
                if (SetTextWhenValueChanged)
                    base.Text = string.Format(culture.NumberFormat, "{0:" + getFormat() + "}", value);
            }
        }

        public double OldValue
        {
            get { return oldValue; }
            set { oldValue = value; }
        }
      

       

       
        public bool AutoResize
        {
            get
            {
                return autoResize;
            }
            set
            {
                autoResize = value;
                base.AutoSize = autoResize;
            }
        }
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                try
                {
                    if (value != null && !value.Equals(""))
                    {
                        base.Text = string.Format(culture.NumberFormat, "{0:" + getFormat() + "}", double.Parse(value.Replace(",", ""), culture.NumberFormat));
                    }
                    else
                        base.Text = value;
                }
                catch (Exception ex)
                {
                    base.Text = "0";
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if ((e.KeyCode == Keys.Delete && this.SelectionStart == this.Text.IndexOf('.') && this.Text.Replace(".", "").Length > MaxLength - DecimalLength)
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
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (e.KeyChar == '-')
            {
                e.Handled = true;
                if (!this.Text.Contains("-") && ValueMin < 0)
                {
                    string strText = this.Text;
                    if (strText.Trim().Length == 0)
                        strText = "0";
                    if (TryParse("-" + strText, culture.NumberFormat) < ValueMin)
                    {
                        
                    }
                    else
                    {
                        base.Text = "-" + this.Text;
                        havingMinusLength = 1;
                    }
                }
                else if (this.Text.Contains("-"))
                {
                    if (TryParse(this.Text.Replace("-", ""), culture.NumberFormat) > ValueMax)
                    {
                        
                    }
                    else
                    {
                        base.Text = this.Text.Replace("-", "");
                        havingMinusLength = 0;
                    }

                }
                this.SelectionStart = this.Text.Length;

            }
            else if (this.Text.Replace(".", "").Length == MaxLength + havingMinusLength && !Char.IsControl(e.KeyChar)
                && !Char.IsDigit(e.KeyChar))// && this.SelectionLength == 0)
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
                    if (this.SelectionLength == this.Text.Length)
                    {
                    }
                    else if (TryParse(this.Text.Insert(this.SelectionStart, e.KeyChar.ToString()), culture.NumberFormat) < ValueMin
                        || TryParse(this.Text.Insert(this.SelectionStart, e.KeyChar.ToString()), culture.NumberFormat) > ValueMax)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        string[] value = this.Text.Split(new char[] { '.' });
                        if (value.Length > 1)
                        {
                            if (value[0].Length == MaxLength + havingMinusLength - DecimalLength && this.Text.IndexOf('.') > this.SelectionStart)//&& !Char.IsControl(e.KeyChar))
                            {
                                e.Handled = true;
                            }

                            else if (value[1].Length == DecimalLength && this.Text.IndexOf('.') < this.SelectionStart)//&& !Char.IsControl(e.KeyChar))
                            {
                                e.Handled = true;
                            }
                        }
                        else if (this.Text.Length >= MaxLength + havingMinusLength - DecimalLength && !this.SelectedText.Equals(this.Text))
                        {
                            e.Handled = true;
                        }
                    }
                }
            }
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            try
            {
                SetTextWhenValueChanged = false;
                if (this.Text.Equals(""))
                {
                    this.Value = 0;
                }
                else
                {
                    this.Value = TryParse(this.Text.Replace(",", ""), culture.NumberFormat);
                }
                SetTextWhenValueChanged = true;
            }
            catch (Exception ex)
            {
                this.Value = 0;
            }
        }
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            SetTextWhenValueChanged = false;
            base.Text = this.Text.Replace(",", "");
            this.SelectAll();
            this.Text.PadRight(Text.Length);
            SetTextWhenValueChanged = true;
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            try
            {
                if (this.Text.Equals(""))
                    this.Text = "0";
                this.Value = TryParse(this.Text, culture.NumberFormat);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.Value = 0;
            }
        }
       

      
        private String getFormat()
        {
            StringBuilder format = new StringBuilder();
            if (ThousandsSeparator)
            {
                format.Append("#,##0");
            }
            else
            {
                format.Append("###0");
            }
            if (DecimalLength > 0)
            {
                format.Append(".");
                char f = '#';
                if (DecimalShowZero)
                    f = '0';
                format.Append(new string(f, DecimalLength));

                //for (int i = 0; i < DecimalLength; i++)
                //{
                //    format.Append(f);
                //}
            }            
            return format.ToString();
        }                

        private double TryParse(string num, NumberFormatInfo cul)
        {
            try
            {
                return double.Parse(num, cul);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private bool IsAllDigit(string textDigit)
        {
            try
            {
                for (int i = 0; i < textDigit.Length; i++)
                {
                    if (!char.IsDigit(textDigit[i]) && textDigit[i] != ',' && textDigit[i] != '.')
                        return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return true;
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
                    if (!IsAllDigit(clipboard.ToString()))
                        return;
                    string cText = clipboard.ToString();
                    string[] sepa = this.Text.Replace(",", "").Split(new char[] { '.' });
                    string chan = sepa[0];
                    string le = "";
                    if (sepa.Length > 1)
                        le = sepa[1];

                    for (int i = 0; i < cText.Length; i++)
                    {
                        if (char.IsDigit(cText[i]))
                        {
                            if (SelectionStart + SelectionLength <= chan.Length
                                && chan.Length + str.Length < MaxLength - DecimalLength )
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
     
    }
}
