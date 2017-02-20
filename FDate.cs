using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Globalization;

namespace FOS_Utils
{
    
    public class FDate : MaskedTextBox,IControl
    {
       
        string strDateValueFormat = "dd/MM/yyyy";

        DateTimePicker dtpDate = null;
        DateTimePicker dtpCheckInput = null;

        bool cancelKeyPressed = false;
        private string _columnName = "";

        public static bool IsDateValid = true;
        private string oldText = "";

        public static bool AllowValidating = true;
       
        [DefaultValue("dd/MM/yyyy")]
        public string DateValueFormat
        {
            get { return strDateValueFormat; }
            set {
                strDateValueFormat = value; 

            }
        }

       
        public DateTimePicker DatePicker
        {
            get { return dtpDate; }
            set {
                dtpDate = value;
                if (dtpDate != null)
                {
                    dtpDate.Format = DateTimePickerFormat.Custom;
                    dtpDate.CustomFormat = " "; //’l‚ð•\Ž¦‚µ‚È‚¢‚½‚ß
                    dtpDate.TabStop = false;
                    dtpDate.CloseUp += new EventHandler(dtpDate_CloseUp);
                }
            }

        }

      
        public object DBValue
        {
            get
            {
               
                return Text;
            }
            set
            {               
                DateTime result;
                if (TryParseDate(value.ToString(), "yyyyMMdd", out result))
                {
                    Text = result.ToString(DateValueFormat);
                }
                else
                {
                    Text = "";
                }
            }
        }

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
        public object Value
        {
            get {
                if (Text.Trim().Equals(""))
                    return DBNull.Value;
                return dtpCheckInput.Value; 
            }
            set {
                if (value != null && value.GetType().Equals(typeof(DateTime)))
                {
                    dtpCheckInput.Value = (DateTime)value;
                    base.Text = dtpCheckInput.Value.ToString(strDateValueFormat);
                }
                else
                {
                    Text = "";
                }
            }
        }

        public string OldText
        {
            get
            {
                return oldText;
            }
            set
            {
                oldText = value;
            }
        }
       

       
        public FDate()
        {
            InsertKeyMode = InsertKeyMode.Overwrite;
            TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            Mask = "00/00/0000";
            PromptChar = ' ';

            Validating += new CancelEventHandler(KDate_Validating);
            Validated += new EventHandler(KDate_Validated);
            Resize += new EventHandler(KDate_Resize);

            DatePicker = new DateTimePicker();
            Controls.Add(DatePicker);
            KDate_Resize(null, null);
            dtpCheckInput = new DateTimePicker();
            dtpCheckInput.Format = DateTimePickerFormat.Custom;
            dtpCheckInput.CustomFormat = strDateValueFormat;            
        }
                   

        #region OVERRIDE METHOD

       
        public bool TryParseDate(String date, String dateFormat, out DateTime result)
        {
            bool valid = true;
            result = DateTime.Today;
            if (!DateTime.TryParseExact(date, dateFormat, DateTimeFormatInfo.CurrentInfo
                , DateTimeStyles.None
                 ,out result))
            {
                result = DateTime.Today;
                valid = true;
            }
            else
            {
                valid = false;
            }
            return valid;
        }

       
        protected override void OnKeyDown(KeyEventArgs e)
        {
            StringBuilder blankChar = null;
            int len = 0;
            int start = 0;

            if (e.KeyCode == Keys.Delete)
            {
                int oldLen = 0;
                len = this.SelectionLength;
                start = this.SelectionStart;
                oldLen = len;

                if (len == 0) len = len + 1;
                this.SelectionLength = len;
                
                blankChar = new StringBuilder();
                while (blankChar.Length < len) blankChar.Append(" ");

                this.SelectedText = blankChar.ToString();

                this.SelectionLength = 0;
                if (oldLen == len)
                {
                    this.SelectionStart = start;
                }
                else
                {
                    this.SelectionStart = start + len;
                }

                blankChar.Length = 0;

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Back)
            {
                start = this.SelectionStart;   
                len = this.SelectionLength;
                if (len == 0)
                {
                    if (start > 0) this.SelectionStart = start - 1;
                    len = len + 1;
                }
                this.SelectionLength = len;

                blankChar = new StringBuilder();
                while (blankChar.Length < len) blankChar.Append(" ");

                this.SelectedText = blankChar.ToString();

                this.SelectionLength = 0;
                if (start > 0)
                {
                    this.SelectionStart = start - 1;
                }
                else
                {
                    this.SelectionStart = 0;
                }

                blankChar.Length = 0;


                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else
            {
                base.OnKeyDown(e);
            }
        }

        #endregion

        #region EVENT
       
        void KDate_Resize(object sender, EventArgs e)
        {
            DatePicker.Top = -2;
            DatePicker.Width = 22;
            DatePicker.Left = Width - DatePicker.Width;
        }
               
        public void KDate_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (!AllowValidating)
                    return;
                IsDateValid = true;
                if (!Text.Trim().Equals(""))
                {
                    dtpCheckInput.Value = DateTime.ParseExact(Text, strDateValueFormat, DateTimeFormatInfo.CurrentInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid Date");
                e.Cancel = true;
                IsDateValid = false;                
            }
        }

        public bool IsDateTimeValid()
        {
            try
            {                
                if (!Text.Trim().Equals(""))
                {
                    DateTime.ParseExact(Text, strDateValueFormat, DateTimeFormatInfo.CurrentInfo);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid Date");
                return false;
            }
        }
       
        
        void KDate_Validated(object sender, EventArgs e)
        {
            cancelKeyPressed = false;

            if (dtpDate == null) return;

            DateTime result;
            TryParseDate(Text, DateValueFormat, out result);
            dtpDate.Value = result;
        }

        void dtpDate_CloseUp(object sender, EventArgs e) {
            
            if (!cancelKeyPressed)
            {
                
                Text = dtpDate.Value.ToString(DateValueFormat);
            }
            Focus();
        }
        
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (!this.Focused)
            {
                try
                {
                    string txt = GetYYYYMMDD(Text);
                    if (txt.Length > 0)
                    {
                        DateTime result;
                        TryParseDate(txt, DateValueFormat, out result);
                        dtpCheckInput.Value = result;
                    }
                }
                catch (Exception ex)
                {                   
                    
                }
            }
        }

        private string GetYYYYMMDD(string date)
        {
            string yyyymmdd = "";            
            for (int i = 0; i < date.Length; i++)
                if (Char.IsDigit(date[i]))
                    yyyymmdd += date[i];
            return yyyymmdd;
        }
       
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            AllowValidating = true;
        }
        #endregion
    }
}
