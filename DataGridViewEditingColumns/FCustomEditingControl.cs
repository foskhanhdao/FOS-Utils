using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
namespace FOS_Utils
{
    class CustomEditingControl : Panel, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        private String customCellType = CellTypes.DATE;
        int rowIndex;
        private Label lbText = new Label();
        private String sqlSelect = "";
        private Control ctrlEditing = null;
        private int maxLength = 0;
        private int decimalLength = 0;
        private bool thousandsSeparator = false;
        
        public CustomEditingControl()
        {
            lbText.Dock = DockStyle.Fill;
            lbText.Text = "";
            this.Controls.Add(lbText);            
            this.Enter += new System.EventHandler(this.PanelEnter);           
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
        public int MaxLength
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
            get
            {
                return decimalLength;
            }
            set
            {
                decimalLength = value;
            }
        }
        public Control CtrlEditing
        {
            get
            {
                return ctrlEditing;
            }
            set
            {
                ctrlEditing = value;
            }
        }
        public object EditingControlFormattedValue
        {
            get
            {
                for (int i = 0; i < this.Controls.Count; i++)
                {
                    if (this.Controls[i] is Label)
                    {
                    }
                    else if (this.Controls[i] is DateTimePicker && ((DateTimePicker)this.Controls[i]).Checked == false)
                    {                        
                        return "";
                    }
                    else {
                        if (this.Controls[1] is CheckBox)
                        {
                            if (((CheckBox)this.Controls[1]).Checked)
                                return "1";
                            return "0";
                        }
                        return this.Controls[i].Text;
                    }
                }                
                return "";                
            }
            set
            {
                String newValue = value as String;
                if (newValue != null)
                {
                    if (this.Controls.Count > 1)
                    {
                        if (this.Controls[1] is CheckBox)
                        {
                            ((CheckBox)this.Controls[1]).Checked = true;
                            if (newValue.Equals("0"))
                                ((CheckBox)this.Controls[1]).Checked = false;
                        }
                        else
                            this.Controls[1].Text = newValue;                       
                    }                    
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

        public String SqlSelect
        {
            get {
                return sqlSelect;
            }
            set
            {
                sqlSelect = value;
            }
        }
        public String CustomCellType
        {
            get
            {
                return customCellType;
            }
            set
            {
                customCellType = value;
                for (int i = 0; i < this.Controls.Count; i++)
                {
                    if (this.Controls[i] is Label)
                    {
                    }
                    else
                    {
                        this.Controls.RemoveAt(i);
                    }
                }
                if (customCellType.Equals(CellTypes.STRING)
                    || customCellType.Equals(CellTypes.NUMBER))
                {
                    FInputControl text = new FInputControl();
                    text.Dock = DockStyle.Fill;
                    text.Location = lbText.Location;
                    lbText.Visible = false;
                    text.MaxLength = maxLength;
                    if (customCellType.Equals(CellTypes.NUMBER))
                    {
                        text.InputType = FInputControl.DECIMAL;
                        text.ThousandsSeparator = thousandsSeparator;                        
                        text.DecimalLength = decimalLength;
                        text.TextAlignment = HorizontalAlignment.Right;
                    }
                    else
                        text.InputType = FInputControl.STRING;
                    text.InputControl.TextChanged += new EventHandler(control_TextChanged);                    
                    this.Controls.Add(text);
                    this.CtrlEditing = text;
                    
                }
                else if (customCellType.Equals(CellTypes.DATE))
                {
                    DateTimePicker date = new DateTimePicker();
                    date.Dock = DockStyle.Fill;
                    date.Location = lbText.Location;
                    lbText.Visible = false;                    
                    date.ValueChanged += new EventHandler(control_TextChanged);
                    date.Value = DateTime.Now;
                    date.Format = DateTimePickerFormat.Custom;
                    date.CustomFormat = "yyyy”NMMŒŽdd“ú";
                    date.ShowCheckBox = true;
                    valueChanged = true;
                    this.EditingControlDataGridView.NotifyCurrentCellDirty(true);    

                    this.Controls.Add(date);
                    this.CtrlEditing = date;
                }
                else if (customCellType.Equals(CellTypes.COMBO_BOX))
                {
                    ComboBox combo = new ComboBox();
                    combo.Dock = DockStyle.Fill;
                    combo.Location = lbText.Location;
                    lbText.Visible = false;
                    ArrayList values = new ArrayList();
                    values.Add(new CodeName("", ""));
                    if (!sqlSelect.Equals(""))
                    {
                        //TODODataSet ds = DataBase.getData(sqlSelect);
                        //if (ds != null && ds.Tables.Count > 0)
                        //{
                        //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        //    {
                        //        values.Add(new Common.CodeMei(ds.Tables[0].Rows[i][0].ToString(),ds.Tables[0].Rows[i][1].ToString()));
                        //    }
                        //    combo.DataSource = values;
                        //    combo.DisplayMember = "Mei";
                        //    combo.ValueMember = "Code";
                        //}                            
                    }
                    combo.DropDownStyle = ComboBoxStyle.DropDownList;
                    combo.TextChanged += new EventHandler(control_TextChanged);
                    valueChanged = true;
                    this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
                    
                    this.Controls.Add(combo);
                    this.CtrlEditing = combo;
                }                
                else if (customCellType.Equals(CellTypes.CHECK_BOX))
                {
                    CheckBox chk = new CheckBox();
                    chk.Dock = DockStyle.Fill;
                    chk.Location = lbText.Location;
                    lbText.Visible = false;
                    chk.Text = "0";
                    chk.CheckedChanged += new EventHandler(checkBoxChecked);
                    valueChanged = true;
                    this.EditingControlDataGridView.NotifyCurrentCellDirty(true);

                    this.Controls.Add(chk);
                    this.CtrlEditing = chk;
                }
            }
        }
        private void PanelEnter(object sender, EventArgs e)
        {          
            for (int i = 0; i < this.Controls.Count; i++)
            {
                this.Controls[i].Text = "";
                if (this.Controls[i] is Label)
                {
                }                
                else
                {
                    this.Controls[i].Focus();
                    if (dataGridView.Rows[rowIndex].Cells[dataGridView.CurrentCell.ColumnIndex].Value != null)
                        this.Controls[i].Text = dataGridView.Rows[rowIndex].Cells[dataGridView.CurrentCell.ColumnIndex].Value.ToString();
                    break;
                }
            }
        }
        private void control_TextChanged(object sender, EventArgs e)
        {
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnTextChanged(e);            
        }
        private void control_KeyPressed(object sender, KeyPressEventArgs e)
        {

        }
        private void checkBoxChecked(object sender, EventArgs e)
        {

        }        
    }
}
