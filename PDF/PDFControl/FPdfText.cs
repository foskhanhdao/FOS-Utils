﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FOS_Utils.PDF.PDFControl
{
    class FPdfText : TextBox
    {

        FPdfProperties fPdfProperties = new FPdfProperties();
        public FPdfProperties FPdfProperties
        {
            get { return fPdfProperties; }
            set { fPdfProperties = value; }
        }
    }
}
