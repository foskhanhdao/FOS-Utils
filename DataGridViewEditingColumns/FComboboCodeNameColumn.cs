using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FOS_Utils
{
    public class FComboBoxCodeNameColumn : DataGridViewColumn
    {
        public FComboBoxCodeNameColumn()
            : base(new FComboBoxCodeNameCell())
        {
        }
        public FComboBoxCodeNameColumn(ImeMode ime)
            : base(new FComboBoxCodeNameCell(ime))
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
