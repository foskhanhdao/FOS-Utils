using System;
using System.Windows.Forms;
namespace FOS_Utils
{
    public class HourMinuteColumn : DataGridViewColumn
    {
        public HourMinuteColumn(String format)
            : base(new HourMinuteCell(format))
        {
        }
        public HourMinuteColumn()
            : base(new HourMinuteCell())
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
                !value.GetType().IsAssignableFrom(typeof(HourMinuteCell)))
            {
                throw new InvalidCastException("Must be a CalendarCell");
            }
            base.CellTemplate = value;
        }
    }
        }

}

