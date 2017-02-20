using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace FOS_Utils
{
    public partial class FInputControl : UserControl
    {
        public static string STRING = "STRING";
        public static string INTERGER = "INTERGER";
        public static string DECIMAL = "DECIMAL";
        private String inputType = STRING;
        private int maxLength = 5;
        private int decimalLength = 0;
        private bool thousandsSeparator = false;       


        private HorizontalAlignment textAlignment = new HorizontalAlignment();
        
        public FInputControl()
        {
            InitializeComponent();
            InputType = STRING;                        
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                inputControl.BackColor = value;
            }
        }

        public override string Text
        {
            get
            {
                return inputControl.Text;
            }
            set
            {
                inputControl.Text = value;
            }
        }
        
        //public override String Text
        //{
        //    get
        //    {
        //        return inputControl.Text;
        //    }
        //    set
        //    {
        //        inputControl.Text = value;
        //    }
        //}
        public TextBox InputControl
        {
            get
            {
                return inputControl;
            }
            set
            {
                inputControl = value;
            }
        }
        public HorizontalAlignment TextAlignment
        {
            get
            {
                return textAlignment;
            }
            set
            {
                textAlignment = value;
                inputControl.TextAlign = value;                
            }
        }
        public int MaxLength
        {
            get
            {
                return maxLength;
            }
            set
            {
                maxLength = value;
                
                if (decimalLength > 0 && inputType.Equals(DECIMAL))
                {
                    maxLength = maxLength + 1;                    
                }                
            }
        }
        public int DecimalLength
        {
            get
            {
                return decimalLength;
            }
            set
            {
                decimalLength = value;
                if (decimalLength > 0 && inputType.Equals(DECIMAL))
                {
                    maxLength = maxLength + 1;
                }                
            }
        }
        public bool ThousandsSeparator
        {
            get
            {
                return thousandsSeparator;
            }
            set
            {
                thousandsSeparator = value;
            }
        }   
       
        public String InputType
        {
            get
            {
                return inputType;
            }
            set
            {
                if (value != null && (value.Equals(STRING) || value.Equals(INTERGER) || value.Equals(DECIMAL)))
                {
                    inputType = value;
                }
                else {
                    MessageBox.Show(STRING + " –”‚Í " + INTERGER + " –”‚Í " + DECIMAL + " ‚ð“ü—Í‚µ‚Ä‚­‚¾‚³‚¢B");                    
                }
                if (!inputType.Equals(STRING))
                {
                    inputControl.TextAlign = HorizontalAlignment.Right;
                    textAlignment = HorizontalAlignment.Right;
                    inputControl.ImeMode = ImeMode.NoControl;
                    if (inputType.Equals(DECIMAL) && decimalLength > 0)
                    {
                        maxLength = maxLength + 1;
                    }
                }
                else
                {
                    inputControl.TextAlign = HorizontalAlignment.Left;
                    textAlignment = HorizontalAlignment.Left;
                }
            }
        }       
        private void InputControl_SizeChanged(object sender, EventArgs e)
        {
            this.Size = inputControl.Size;
        }

        private void FInputControl_SizeChanged(object sender, EventArgs e)
        {
            inputControl.Size = this.Size;
        }

        private void inputControl_ImeModeChanged(object sender, EventArgs e)
        {
            if (!inputType.Equals(STRING))
            {
                inputControl.ImeMode = ImeMode.NoControl;
            }
        }

        private void FInputControl_ImeModeChanged(object sender, EventArgs e)
        {
            if (!inputType.Equals(STRING))
            {
                inputControl.ImeMode = ImeMode.NoControl;
            }
        }

        private void inputControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (inputControl.Text.Length == maxLength && !Char.IsControl(e.KeyChar) && inputControl.SelectionLength == 0)
            {
                e.Handled = true;
            }
            else if (inputType.Equals(STRING))
            {

            }
            else if (inputType.Equals(INTERGER))
            {
                if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            else if (inputType.Equals(DECIMAL))
            {                
                if (decimalLength > 0 && !inputControl.Text.Contains(".") && e.KeyChar == '.')
                {
                }
                else if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
                else
                {
                    string[] value = inputControl.Text.Split(new char[] { '.' });
                    if (value.Length > 1)
                    {
                        if (value[1].Length == decimalLength && !Char.IsControl(e.KeyChar))
                        {
                            e.Handled = true;
                        }
                    }
                    else if (((inputControl.Text.Length == maxLength - decimalLength - 1 && decimalLength  > 0) 
                        || (inputControl.Text.Length == maxLength - decimalLength && decimalLength  == 0))
                        && !Char.IsControl(e.KeyChar) && inputControl.SelectionLength == 0)
                    {
                        e.Handled = true;
                    }
                }
            }            
        }

        private void inputControl_Leave(object sender, EventArgs e)
        {
            if (thousandsSeparator)
            {
                if (!inputType.Equals(STRING))
                {
                    if (inputControl.Text.Equals(""))
                    {
                        inputControl.Text = "0";
                        return;
                    }
                    string[] value = inputControl.Text.Split(new char[] { '.' });
                    StringBuilder tempValue = new StringBuilder();
                    StringBuilder newValue = new StringBuilder();
                    int kai = 0;
                    for (int i = value[0].Length - 1; i >= 0; i--)
                    {
                        tempValue.Append(value[0][i]);
                        kai++;
                        if (kai == 3 && i > 0)
                        {
                            kai = 0;
                            tempValue.Append(",");
                        }                        
                    }
                    for (int i = tempValue.Length - 1; i >= 0; i--)
                    {
                        newValue.Append(tempValue[i]);
                    }
                    if (value.Length > 1 && value[1].Length > 0)
                    {
                        inputControl.Text = newValue.ToString() + "." + value[1];
                    }
                    else
                    {
                        inputControl.Text = newValue.ToString();
                    }                    
                }               
            }
        }

        private void inputControl_Enter(object sender, EventArgs e)
        {
            if (!inputType.Equals(STRING))
            {
                inputControl.Text = inputControl.Text.Replace(",", "");
            }
            inputControl.SelectAll();
        }
    }
}
