using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.Globalization;
namespace FOS_Utils
{
   public class FNumberTextBox:FTextBox,IControl
   {
       #region VARIABLES
       /// <summary>
       /// So ky tu phan thap phan
       /// </summary>
        private int iDecimalNumberDigit = 2;        
       /// <summary>
       /// Hien thi 0 phan thap phan hay ko
       /// </summary>
        private bool bShowDecimalZero = true;
        private string _columnName = "";
       #endregion
       #region PROPERTIES
        public int DecimalNumberDigit
        {
            get { return iDecimalNumberDigit; }
            set {
                if (value < 0)
                {
                    value = 0;
                }
                iDecimalNumberDigit = value; 
            }
        }
        public bool ShowDecimalZero
        {
            get { return bShowDecimalZero; }
            set { bShowDecimalZero = value; }
        }

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
        #endregion       
       #region CONTRUCTORS
        public FNumberTextBox()
        {
            this.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ImeMode = ImeMode.Disable;
        }
        #endregion
        #region METHODS

       /// <summary>
       /// Lay gia tri cua numbertextbox
       /// </summary>
       /// <returns></returns>
        public double GetValue()
        {
            double dResult = 0;
            if (this.Text.Length > 0)
            {              
                CultureInfo culture = new CultureInfo("ja-JP");
                culture.NumberFormat.NumberDecimalSeparator = ".";
                culture.NumberFormat.NumberGroupSeparator = ",";        
                dResult= double.Parse(this.Text,culture.NumberFormat);                
            }            
            return dResult;
        }
        public void SetValue(object value)
        {
            if (value.ToString().Length > 0)
            {
                CultureInfo culture = new CultureInfo("ja-JP");
                culture.NumberFormat.NumberDecimalSeparator = ".";
                culture.NumberFormat.NumberGroupSeparator = ",";
                String strFormat = "";
                for (int i = 0; i < iDecimalNumberDigit; i++)
                {
                    if (i < iDecimalNumberDigit - 1)
                    {
                        strFormat += "#";
                    }
                    else
                    {
                        if (bShowDecimalZero)
                            strFormat += "0";
                        else
                            strFormat += "#";
                    }
                }               
                this.Text = string.Format(culture.NumberFormat, "{0:#,##0." + strFormat + "}", double.Parse(value.ToString()));
            }
        }
        #endregion
        #region EVENTS
        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {            
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar!='.')
            {
                e.Handled = true;
            }
            else if (e.KeyChar == '.')
            {
                if (DecimalNumberDigit == 0 || this.Text.IndexOf('.') >= 0 || this.Text.Length == 0)
                {
                    e.Handled = true;
                }              
            }
        }
        protected override void OnLeave(EventArgs e)
        {
            if (this.Text.Length > 0)
            {
                CultureInfo culture = new CultureInfo("ja-JP");
                culture.NumberFormat.NumberDecimalSeparator = ".";
                culture.NumberFormat.NumberGroupSeparator=",";               
                String strFormat = "";
                for (int i = 0; i < iDecimalNumberDigit; i++)
                {
                    if (i < iDecimalNumberDigit - 1)
                    {
                        strFormat += "#";
                    }
                    else
                    {
                        if (bShowDecimalZero)
                            strFormat += "0";
                        else
                            strFormat += "#";
                    }
                }                
                this.Text = string.Format(culture.NumberFormat, "{0:#,##0." + strFormat + "}", double.Parse(this.Text,culture.NumberFormat));                
            }
        }
        protected override void OnEnter(EventArgs e)
        {
            if (this.Text.Length > 0)
            {               
                this.Text = this.Text.Replace(",", "");
            }
        }
        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // KNumberTextBox
            // 
            this.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ResumeLayout(false);

        }
   }
}
