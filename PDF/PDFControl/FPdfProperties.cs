using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FOS_Utils.PDF.PDFControl
{

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FPdfProperties
    {
        #region variable
        private int tableRow;
        private string tableColumn;
        private int rotation;
        #endregion
        #region Contructer
        public FPdfProperties()
        {
            tableRow = 0;
            tableColumn = "";
        }
        #endregion
        #region Field
        public int TableRow
        {
            get { return tableRow; }
            set { tableRow = value; }
        }
        public string TableColumn
        {
            get { return tableColumn; }
            set { tableColumn = value; }
        }
        public int Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        #endregion
    }
}
