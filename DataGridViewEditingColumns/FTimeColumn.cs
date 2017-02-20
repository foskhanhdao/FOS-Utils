using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace FOS_Utils
{
    public class TimeColumn : DataGridViewColumn
    {
        public String cellType = CellTypes.STRING;
        public TimeColumn()
            : base(new TimeCell())
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
                    !value.GetType().IsAssignableFrom(typeof(TimeCell)))
                {
                    throw new InvalidCastException("Must be a TimeCell");
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
