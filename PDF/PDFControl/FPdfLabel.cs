using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FOS_Utils.PDF.PDFControl
{
    public class FPdfLabel : FLabelLine
    {
        public FPdfLabel()
        {
            this.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        }
        FPdfProperties fPdfProperties = new FPdfProperties();

        public FPdfProperties FPdfProperties
        {
            get { return fPdfProperties; }
            set { fPdfProperties = value; }
        }
    }
}
