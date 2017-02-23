using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Windows.Forms;
using FOS_Utils.PDF.PDFControl;
using System.Drawing;

namespace FOS_Utils.PDF.PDFLib
{
    public static class PrintHelper
    {
        #region variable
        private static PrintDocument doc;
        private static PrintPreviewDialog prDialog;
        private static FPdfPanel PanelMain;
        //tong so trang Pdf
        private static int NumberPage = 1;
        //trang Pdf hien tai
        private static int CurPage = 1;
        //tong so dong detail co trong mot trang
        private static int MaxRow = 0;
        //Tong hop tat ca cac line trong trang
        private static List<FosLine> lsLineInpage = new List<FosLine>();
        #endregion
        #region FunPrint
        public static void BeginPrint(FPdfPanel panelMain)
        {
            if (doc == null && prDialog == null)
            {
                doc = new PrintDocument();                
                doc.PrintPage += new PrintPageEventHandler(On_PrintPage);
                prDialog = new PrintPreviewDialog();
                prDialog.Document = doc;
                prDialog.Width = Screen.PrimaryScreen.WorkingArea.Width;
                prDialog.Height = Screen.PrimaryScreen.WorkingArea.Height;
                prDialog.Document.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 595, 842);
                PanelMain = panelMain;
                prDialog.ShowDialog();
            }
        }
        private static void On_PrintPage(object sender, PrintPageEventArgs ev)
        {          
            Graphics g = ev.Graphics;
            
        }
        private static void PrintString(Graphics g, FPdfLabel FPdfLabel, FosPoint rootPoint, int curPage)
        {
            //Inborder
            PrintBorderLabel(FPdfLabel, rootPoint);
            // tao bien tao do
            FosPoint point = CreatePointFromLabel(FPdfLabel);
            // Tuy vao control cha la gi ma chinh lai toa do
            point.XPoint += rootPoint.XPoint;
            point.YPoint += rootPoint.YPoint;           
            //Lay du lieu cua dataSource bo vao text
            //GetDataFPdfLabel(FPdfLabel, curPage);
            string text = FPdfLabel.Text;
        }
        #endregion
        #region Fun
        /// <summary>
        /// In border cho label
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="page"></param>
        /// <param name="rootPoint"></param>
        public static void PrintBorderLabel(FPdfLabel tb, FosPoint rootPoint)
        {
            if (tb.IsShowLineTop)
            {
                FosLine lineTop = new FosLine();
                lineTop.PointStart = new FosPoint(rootPoint.XPoint + tb.Location.X, rootPoint.YPoint + tb.Location.Y);
                lineTop.PointDest = new FosPoint(rootPoint.XPoint + tb.Location.X + tb.Width, rootPoint.YPoint + tb.Location.Y);
                if (tb.LineStyleTop == System.Drawing.Drawing2D.DashStyle.Dot)
                {
                    lineTop.LineStyle = LineStyle.Dot;
                }
                if (CurPage == 1)
                    lsLineInpage.Add(lineTop);
            }
            if (tb.IsShowLineBottom)
            {
                FosLine lineBottom = new FosLine();
                lineBottom.PointStart = new FosPoint(rootPoint.XPoint + tb.Location.X, rootPoint.YPoint + tb.Location.Y + tb.Height);
                lineBottom.PointDest = new FosPoint(rootPoint.XPoint + tb.Location.X + tb.Width, rootPoint.YPoint + tb.Location.Y + tb.Height);
                if (tb.LineStyleBottom == System.Drawing.Drawing2D.DashStyle.Dot)
                {
                    lineBottom.LineStyle = LineStyle.Dot;
                }
                if (CurPage == 1)
                    lsLineInpage.Add(lineBottom);
            }
            if (tb.IsShowLineLeft)
            {
                FosLine lineLeft = new FosLine();
                lineLeft.PointStart = new FosPoint(rootPoint.XPoint + tb.Location.X, rootPoint.YPoint + tb.Location.Y);
                lineLeft.PointDest = new FosPoint(rootPoint.XPoint + tb.Location.X, rootPoint.YPoint + tb.Location.Y + tb.Height);
                if (tb.LineStyleLeft == System.Drawing.Drawing2D.DashStyle.Dot)
                {
                    lineLeft.LineStyle = LineStyle.Dot;
                }
                if (CurPage == 1)
                    lsLineInpage.Add(lineLeft);
            }
            if (tb.IsShowLineRight)
            {
                FosLine lineRight = new FosLine();
                lineRight.PointStart = new FosPoint(rootPoint.XPoint + tb.Location.X + tb.Width, rootPoint.YPoint + tb.Location.Y);
                lineRight.PointDest = new FosPoint(rootPoint.XPoint + tb.Location.X + tb.Width, rootPoint.YPoint + tb.Location.Y + tb.Height);
                if (tb.LineStyleRight == System.Drawing.Drawing2D.DashStyle.Dot)
                {
                    lineRight.LineStyle = LineStyle.Dot;
                }
                if (CurPage == 1)
                    lsLineInpage.Add(lineRight);
            }
        }
        /// <summary>
        /// Tu Label lay ra toa do de in text 
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static FosPoint CreatePointFromLabel(FPdfLabel label)
        {
            int height = label.Size.Height;
            int size = (int)label.Font.Size;
            int xPoint = label.Location.X + 4;
            int yPoint = 0;
            //center
            if (label.TextAlign == ContentAlignment.MiddleLeft ||
                label.TextAlign == ContentAlignment.MiddleCenter ||
                label.TextAlign == ContentAlignment.MiddleRight)
            {
                yPoint = label.Location.Y + size + (label.Size.Height - size) / 2;
            }
            //top
            if (label.TextAlign == ContentAlignment.TopLeft ||
                label.TextAlign == ContentAlignment.TopCenter ||
                label.TextAlign == ContentAlignment.TopRight)
            {
                yPoint = label.Location.Y + size;
            }
            ////bottom
            if (label.TextAlign == ContentAlignment.BottomLeft ||
                label.TextAlign == ContentAlignment.BottomCenter ||
                label.TextAlign == ContentAlignment.BottomRight)
            {
                yPoint = label.Location.Y + label.Size.Height;
            }
            FosPoint point = new FosPoint(xPoint, yPoint);
            return point;
        }
        #endregion
    }
}
