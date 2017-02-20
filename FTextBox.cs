using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;

namespace FOS_Utils
{
    public class FTextBox : TextBox,IControl
    {
       
        /// <summary>
        /// char min
        /// </summary>
        private char minChar = '0';        
        /// <summary>
        /// char max
        /// </summary>
        private char maxChar = '9';
        /// <summary>
        /// check min max char
        /// </summary>
        private bool checkMinMaxChar = false;
        /// <summary>
        /// get text by byte lenght
        /// </summary>
        private bool maxLengthFromByte = false;
        /// <summary>
        /// fill 0 when having text ()
        /// </summary>
        private bool fillZeroWhenHavingText = false;
        private string _columnName = "";

        private Control outputControl = null;

        private bool dBNullValueWhenBlank = false;

        private string oldText = "";
       

      
        public FTextBox()
        {
            this.AutoSize = false;
            this.ContextMenu = new ContextMenu();
        }
  
        /// <summary>
        /// 
        /// </summary>
        public char MinChar {
            get { return minChar; }
            set { minChar = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public char MaxChar
        {
            get { return maxChar; }
            set { maxChar = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool CheckMinMaxChar
        {
            get { return checkMinMaxChar; }
            set { checkMinMaxChar = value;
            if (checkMinMaxChar)
                ImeMode = ImeMode.Disable;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool MaxLengthFromByte
        {
            get { return maxLengthFromByte; }
            set { maxLengthFromByte = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool FillZeroWhenHavingText
        {
            get { return fillZeroWhenHavingText; }
            set { fillZeroWhenHavingText = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Control OutputControl
        {
            get { return outputControl; }
            set { outputControl = value; }
        }

        public bool DBNullValueWhenBlank
        {
            get { return dBNullValueWhenBlank; }
            set { dBNullValueWhenBlank = value; }
        }
        public object DBValue
        {
            get {
                if (DBNullValueWhenBlank && Text.Equals(""))
                    return DBNull.Value;
                return Text; 
            }
            set
            {
                if (value != null) Text = value.ToString();
            }
        }
        public string OldText
        {
            get { return oldText; }
            set { oldText = value; }
        }
      

        

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            try
            {
                if (checkMinMaxChar && (e.KeyChar < minChar || e.KeyChar > maxChar ||
                     !char.IsDigit(e.KeyChar)) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.SelectionStart = this.Text.Length;
        }
        protected override void OnLeave(EventArgs e)
        {
            try
            {
                base.OnLeave(e);
                GetTextFromLength();
                this.SelectionStart = 0;
            }
            catch (Exception ex)
            {
                base.Text = "";
            }
        }

        private void GetTextFromLength()
        {
            try
            {
                if (fillZeroWhenHavingText && checkMinMaxChar
                    && this.Text.Trim().Length > 0)
                {
                    base.Text = Convert.ToInt32(this.Text).ToString(new string('0', MaxLength));
                }
                else if (maxLengthFromByte && this.Text.Length > 0)
                {
                    StringBuilder strResult = new StringBuilder();
                    int l = 0;
                    for (int i = 0; i < this.Text.Length; i++)
                    {
                        l += System.Text.Encoding.GetEncoding("shift-jis").GetByteCount(this.Text[i].ToString());
                        if (l <= MaxLength)
                        {
                            strResult.Append(this.Text[i]);
                        }
                    }
                    base.Text = strResult.ToString();

                }
            }
            catch (Exception ex)
            {
                base.Text = "";
            }
        }
                
        protected override void OnTextChanged(EventArgs e)
        {
            try
            {
                base.OnTextChanged(e);
                
                if (OutputControl != null)
                {
                    OutputControl.Text = this.Text;
                }
                if (!IsMinMax(this.Text))
                {
                    base.Text = "";
                }
                if (!this.Focused )
                {
                    GetTextFromLength();
                    if (this.Text.Trim().Length > 0 && CheckMinMaxChar && FillZeroWhenHavingText)
                        base.Text = Convert.ToInt32(this.Text).ToString(new string('0', MaxLength));
                }
            }
            catch (Exception ex)
            {
                base.Text = ""; 
            }
        }

        private bool IsMinMax(string txt)
        {
            try
            {
                if (CheckMinMaxChar)
                {
                    for (int i = 0; i < txt.Length; i++)
                    {
                        if (txt[i] < MinChar || txt[i] > MaxChar)
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            try
            {
                base.OnKeyDown(e);
                if (e.KeyCode == Keys.V && e.Control)
                {
                    IDataObject clipboardData = Clipboard.GetDataObject();
                    object clipboard = clipboardData.GetData(DataFormats.UnicodeText);
                    if (clipboard != null && !clipboard.Equals(DBNull.Value))
                    {
                        if (CheckMinMaxChar && !IsClipboardValid(clipboard.ToString(), MinChar, MaxChar))
                        {
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                        else
                        {

                        }
                    }

                    
                    //GetValueFromClipboard();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void GetValueFromClipboard()
        {
            try
            {
                IDataObject clipboardData = Clipboard.GetDataObject();
                object clipboard = clipboardData.GetData(DataFormats.UnicodeText);
                if (clipboard != null && !clipboard.Equals(DBNull.Value))
                {
                    string clip = "";
                    string text = this.Text.Substring(0, this.SelectionStart - this.SelectionLength)
                        + this.Text.Substring(this.SelectionStart, this.Text.Length - this.SelectionStart);
                    if (CheckMinMaxChar)
                    {
                        if (IsClipboardValid(clipboard.ToString(), MinChar, MaxChar))
                            base.Text = text.Insert(this.SelectionStart, clip);
                    }
                    else
                    {
                        int max = MaxLength - this.Text.Length;
                        if (max > clipboard.ToString().Length)
                            max = clipboard.ToString().Length;
                        if (max <= clipboard.ToString().Length)
                        {
                            
                            base.Text = text.Insert(this.SelectionStart, clipboard.ToString().Substring(0, max));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool IsClipboardValid(string text, char minC, char maxC)
        {
            if (!CheckMinMaxChar)
                return true;
            int max = MaxLength - this.Text.Length;
            if (max > text.Length)
                max = text.Length;
            for (int i = 0; i < max; i++)
            {
                if (text[i] > maxC || text[i] < minC)
                    return false;                
            }
            return true;
        }        
    }
}
