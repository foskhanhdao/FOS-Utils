using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace FOS_Utils
{
    public class TextBoxColumn : DataGridViewColumn
    {
        public TextBoxColumn()
            : base(new TextBoxCell())
        {
        }
        public TextBoxColumn(int maxLength)
            : base(new TextBoxCell(maxLength))
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
                    !value.GetType().IsAssignableFrom(typeof(TextBoxCell)))
                {
                    throw new InvalidCastException("Must be a TextBoxCell");
                }
                base.CellTemplate = value;
            }
        }
    }
}
