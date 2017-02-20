using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
namespace FOS_Utils
{
    public class NumberCell : DataGridViewTextBoxCell
    {
        public NumberCell()
            : base()
        {
            this.Style.Format = "#,##0";            
        }
        public NumberCell(string format)
            : base()
        {
            this.Style.Format = format;
        }

        protected override object GetValue(int rowIndex)
        {
            if (rowIndex >= 0)
                return base.GetValue(rowIndex);
            return "";
        }
        public void setFormat(string format) {
            this.Style.Format = format;
        }
        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            FNumberEditingControl ctl =
                DataGridView.EditingControl as FNumberEditingControl;
            object val = GetValue(rowIndex);
            if (val != null && !val.ToString().Equals(""))
            {
                CultureInfo culture = new CultureInfo("ja-JP");
                culture.NumberFormat.NumberDecimalSeparator = ".";
                culture.NumberFormat.NumberGroupSeparator = ",";
                ctl.Value = double.Parse(val.ToString().Replace(",", ""));
            }
        }

        public override Type EditType
        {
            get
            {
                return typeof(FNumberEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(Decimal);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return 0;
            }
        }
    }

}
