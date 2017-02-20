using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace FOS_Utils
{
    public class FComboboxCodeNameEditingControl : ComboBox, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;
        private bool maxLengthFromByte = false;

        public FComboboxCodeNameEditingControl()
        {            
            this.DropDownStyle = ComboBoxStyle.DropDownList;
            this.ImeMode = ImeMode.Hiragana;
            this.Leave += new EventHandler(ComboBox_Leave);     
        }


        public bool MaxLengthFromByte
        {
            get { return maxLengthFromByte; }
            set { maxLengthFromByte = value; }
        }
       
        public void setData(DataTable dt) {
            try
            {
                
                this.DataSource = dt;
                this.DisplayMember = dt.Columns [1].ColumnName ;
                this.ValueMember = dt.Columns[0].ColumnName;
            }
            catch (Exception ex)
            {

            }
           
        }
        public object EditingControlFormattedValue
        {
            get
            {
                return this.Text;
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
        private void ComboBox_Leave(object sender, EventArgs e)
        {
            if (maxLengthFromByte && this.Text.Length > 0)
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
                this.Text = strResult.ToString();

            }
        }
    }
}
