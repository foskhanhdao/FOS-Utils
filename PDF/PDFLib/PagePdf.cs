using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FOS_Utils.PDF.PDFLib
{
    public enum PageType
    {
        A4,
        A3,
        Custom,
    }
    public class PagePdf
    {
        #region Variable
        private float with;
        private float height;
        private PageType pageType;
        #endregion
        #region Field
        public float Height
        {
            get { return height; }
        }
        public float With
        {
            get { return with; }
        }
        public PageType PageType
        {
            get { return pageType; }
            set
            {
                pageType = value;
            }
        }
        #endregion
        #region Contructer
        public PagePdf()
        { }
        public PagePdf(PageType pageType)
        {
            this.pageType = pageType;
            SetSizePageByType();
        }
        #endregion
        #region Fun
        private void SetSizePageByType()
        {
            switch (pageType)
            {
                case PageType.A4:
                    this.with = 800;
                    this.height = 1100;
                    break;
                case PageType.A3:
                    this.with = 100;
                    this.height = 100;
                    break;
            }
        }
        #endregion

    }
}
