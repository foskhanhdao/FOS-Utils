using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace FOS_Utils
{
    public class FComboBoxColumn : DataGridViewColumn
    {
        public FComboBoxColumn()
            : base(new FComboBoxCell())
        {
        }       
        public FComboBoxColumn(ImeMode ime)
            : base(new FComboBoxCell(ime))
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
                    !value.GetType().IsAssignableFrom(typeof(FComboBoxCell)))
                {
                    throw new InvalidCastException("Must be a TextBoxCell");
                }
                base.CellTemplate = value;
            }
        }
    }
}
