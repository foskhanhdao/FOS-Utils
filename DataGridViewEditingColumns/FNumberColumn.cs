using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace FOS_Utils
{
    public class NumberColumn : DataGridViewColumn
    {
        public NumberColumn()
            : base(new NumberCell())
        {
        }

        public NumberColumn(string format)
            : base(new NumberCell(format))
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
                    !value.GetType().IsAssignableFrom(typeof(NumberCell)))
                {
                    throw new InvalidCastException("Must be a TextBoxCell");
                }
                base.CellTemplate = value;
            }
        }
    }
}
