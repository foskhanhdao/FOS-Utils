using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace FOS_Utils
{
    class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;        
        private String format = "dd/MM/yyyy";

        public CalendarEditingControl()
        {            
            this.Format = DateTimePickerFormat.Custom;
            this.CustomFormat = format;
            this.Leave += new EventHandler(calenda_Leave);            
        }
        public CalendarEditingControl(String format)
        {
            this.Format = DateTimePickerFormat.Custom;
            this.CustomFormat = format;
            if (format.Equals("hh:MM"))
            {
                this.ShowUpDown = true;
            }
        }
        
        public object EditingControlFormattedValue
        {
            get
            {                
                   return this.Value.ToShortDateString();             
            }
            set
            {                
                if (value != null && !value.ToString().Equals(""))
                {
                    this.Value = DateTime.Parse(value.ToString());
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
            this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
            this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
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

        protected override void OnValueChanged(EventArgs eventargs)
        {
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }
        private void calenda_Leave(object sender, EventArgs e)
        {
            this.valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);         
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
        public void setTimeFormat(string f)
        {
            this.Format = DateTimePickerFormat.Custom;
            this.CustomFormat = f;
            this.ShowCheckBox = false;
            this.ShowUpDown = true;
        }
    }
}
