using System;
using System.Windows.Forms;
namespace FOS_Utils
{
    public class CalendarColumn : DataGridViewColumn
    {
        public CalendarColumn(String format)
            : base(new CalendarCell(format))
        {
        }
        public CalendarColumn()
            : base(new CalendarCell())
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
                !value.GetType().IsAssignableFrom(typeof(CalendarCell)))
            {
                throw new InvalidCastException("Must be a CalendarCell");
            }
            base.CellTemplate = value;
        }
    }
        }

}

