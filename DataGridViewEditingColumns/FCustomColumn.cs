using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace FOS_Utils
{
    public class CustomColumn : DataGridViewColumn
    {
        public String cellType = CellTypes.STRING;
        public CustomColumn()
            : base(new CustomCell())
        {
        }
        
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(CustomCell)))
                {
                    throw new InvalidCastException("Must be a TextBoxCell");
                }
                base.CellTemplate = value;
            }
        }

        public void setCellType(String type)
        {
            this.cellType = type;
        }
    }
}
