using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using FOS_Utils.PDF.PDFLib;
using System.Drawing;



namespace FOS_Utils.PDF.PDFControl
{
    public delegate void AddControl(Control ct);
    public class FPdfPanel : Panel
    {
        #region Variable
        //loai giay A4,A3
        private PageType pageType;        
        //tong so dong trong panel >1 la detail
        private int maxRow;
        //danh sach cac duong thang trong panel
        public List<FosLine> lsPdfLine = new List<FosLine>();
        //du lieu di kem voi detail
        public DataTable DataSource = null;
        //PanelDetail
        public FPdfPanel pnDetail;

        
        //event
        public event AddControl AddControlEx;
        #endregion
        #region Field
        public PageType PageType
        {
            get
            {
                return pageType;
            }
            set
            {
                pageType = value;
                SetSizePageByType();
            }
        }
        
        public int MaxRow
        {
            get { return maxRow; }
            set { maxRow = value; }
        }
        public FPdfPanel PnDetail
        {
            get { return pnDetail; }
            set { pnDetail = value; }
        }
        #endregion
        #region Contructer
        public FPdfPanel()
        {
            pageType = PageType.A4;            
            maxRow = 1;
            SetSizePageByType();
            BackColor = Color.White;
        }
        #endregion
        #region Fun
        private void SetSizePageByType()
        {
            switch (pageType)
            {
                case PageType.A4:
                    this.Size = new System.Drawing.Size(595, 842);
                    break;
                case PageType.A3:
                    base.Size = new System.Drawing.Size(100, 100);
                    break;
                case PageType.Custom:
                    base.Size = new System.Drawing.Size(50, 50);
                    break;
            }
        }

        public void RaiseAddControlEvent(Control ct)
        {
            if (AddControlEx != null)
            {
                AddControlEx(ct);
                
            }
        }
        public void CoppyAddEvent(FPdfPanel panelCoppy)
        {
            panelCoppy.AddControlEx = this.AddControlEx;
        }
        #endregion
        #region Override
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            pageType = PageType.Custom;
        }
        #endregion
    }
}
