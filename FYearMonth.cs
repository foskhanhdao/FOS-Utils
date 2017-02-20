using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Globalization;

namespace FOS_Utils
{
    
    public class FYearMonth : MaskedTextBox
    {


        private string _columnName = "";
        private string oldText = "";
      

       
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        public string OldText
        {
            get { return oldText; }
            set { oldText = value; }
        }
       
      

      
        public FYearMonth()
        {
            ImeMode = ImeMode.Disable;
            InsertKeyMode = InsertKeyMode.Overwrite;
            TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            Mask = "0000/00";
            PromptChar = ' '; 
        }
           

      
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            try
            {
               IsValid(e, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
      

    
        public bool IsValid(CancelEventArgs e, bool showMsg)
        {
            try
            {
                if (this.Text.Replace("/", "").Trim().Length > 0
                        && this.Text.Replace("/", "").Replace(" ","").Trim().Length < 6)
                {
                    e.Cancel = true;
                    if (showMsg)
                        MessageBox.Show("Invalid Year Month");
                    return false;
                }
                else if (this.Text.Replace("/", "").Trim().Length == 6)
                {
                    int year = Convert.ToInt32(this.Text.Replace("/", "").Substring(0, 4));
                    int month = Convert.ToInt32(this.Text.Replace("/", "").Substring(4, 2));
                    if (year < 1900 || month > 12 || month == 0)
                    {
                        e.Cancel = true;
                        if (showMsg)
                            MessageBox.Show("Invalid Year Month");
                        return false;
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());                
            }
            return true;
        }
      
    }
}
