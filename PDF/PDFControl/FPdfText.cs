using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace FOS_Utils.PDF.PDFControl
{
    public class FPdfText : TextBox
    {
       
        #region Variable
        FPdfProperties fPdfProperties = new FPdfProperties();
        
        #endregion
        #region Field
        public FPdfProperties FPdfProperties
        {
            get { return fPdfProperties; }
            set { fPdfProperties = value; }
        }
        
        #endregion
        #region Override
        
        #endregion


    }
}
