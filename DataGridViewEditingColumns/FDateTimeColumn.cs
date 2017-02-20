using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace FOS_Utils
{
    public class DateTimeColumn : DataGridViewColumn
    {
        public DateTimeColumn()
            : base(new DateTimeCell())
        {
            
        }

        public DateTimeColumn(string format)
            : base(new DateTimeCell(format))
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
                    !value.GetType().IsAssignableFrom(typeof(DateTimeCell)))
                {
                    throw new InvalidCastException("Must be a TextBoxCell");
                }
                base.CellTemplate = value;
            }
        }
    }
}
