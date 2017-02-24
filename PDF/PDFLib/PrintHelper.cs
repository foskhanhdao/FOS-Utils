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
            if (doc != null)
            {
                if (PanelMain.pnDetail != null && PanelMain.pnDetail.DataSource != null)
                {
                    FPdfPanel detail = PanelMain.pnDetail;
                    NumberPage = detail.DataSource.Rows.Count / detail.MaxRow;
                    if ((detail.DataSource.Rows.Count % detail.MaxRow) != 0)
                        NumberPage++;
                    MaxRow = detail.MaxRow;
                }
                for (int i = 1; i <= NumberPage; i++)
                {
                    // moi vong lap la in mot trang 
                    CurPage = i;
                    // In het cac control trong trang
                    PrintAllControlInPanel(g,PanelMain, CurPage, new FosPoint(0, 0));
                    //In het cac line trong trang sau cung
                    foreach (FosLine line in lsLineInpage)
                    {
                        PrintLine(g,line);
                    }
                    //Neu khong phai trang cuoi thi tao moi trang de in tiep
                    if (i != NumberPage)
                    {
                        //CreateNewPage();
                    }
                }

            }
            
        }
        /// <summary>
        /// In tat ca cac control co trong Panel
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="page"></param>
        /// <param name="curPage"></param>
        /// <param name="rootPoint"></param>
        public static void PrintAllControlInPanel(Graphics g,FPdfPanel panel, int curPage, FosPoint rootPoint)
        {
            //Add line
            if (panel.lsPdfLine.Count > 0)
            {
                foreach (FosLine line in panel.lsPdfLine)
                {
                    if (curPage == 1)
                    {
                        FosLine lineNew = new FosLine(new FosPoint(), new FosPoint());
                        lineNew.PointStart.XPoint = rootPoint.XPoint + line.PointStart.XPoint;
                        lineNew.PointStart.YPoint = rootPoint.YPoint + line.PointStart.YPoint;
                        lineNew.PointDest.XPoint = rootPoint.XPoint + line.PointDest.XPoint;
                        lineNew.PointDest.YPoint = rootPoint.YPoint + line.PointDest.YPoint;
                        lsLineInpage.Add(lineNew);
                    }
                }
            }
            // In hinh dau tien
            foreach (Control c in panel.Controls)
            {

                if (c is PictureBox)
                {
                    PictureBox pB = c as PictureBox;
                    PrintImage(g,pB, rootPoint);
                }
            }
            //Sau do in chu
            foreach (Control c in panel.Controls)
            {
                //if (c is FPdfText)
                //{
                //    FPdfText FPdfText = c as FPdfText;
                //    PrinString(FPdfText, page, rootPoint, curPage);
                //}
                if (c is FPdfLabel)
                {
                    FPdfLabel FPdfLabel = c as FPdfLabel;
                    PrintString(g,FPdfLabel, rootPoint, curPage);
                }
                //in Panel con
                if (c is FPdfPanel)
                {
                    FPdfPanel FPdfPanelChirld = c as FPdfPanel;
                    //inborder
                    if (FPdfPanelChirld.BorderStyle == BorderStyle.FixedSingle)
                        PrintBorderForControl(FPdfPanelChirld, rootPoint);
                    PrintAllControlInPanel(g, FPdfPanelChirld, curPage, new FosPoint(rootPoint.XPoint + FPdfPanelChirld.Location.X, rootPoint.YPoint + FPdfPanelChirld.Location.Y));
                }
            }
        }
        /// <summary>
        /// Dua vao Label Viet chu len PrintDocument
        /// </summary>
        /// <param name="g"></param>
        /// <param name="FPdfLabel"></param>
        /// <param name="rootPoint"></param>
        /// <param name="curPage"></param>
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
            point.XPoint += AlignforFPdfLabel(FPdfLabel as FPdfLabel);
            g.DrawString(text, FPdfLabel.Font, new SolidBrush(FPdfLabel.ForeColor), point.XPoint, point.YPoint);
        }
        /// <summary>
        /// Ve mot duong thang tren PrintDocument
        /// </summary>
        /// <param name="g"></param>
        /// <param name="line"></param>   
        public static void PrintLine(Graphics g, FosLine line)
        {
           
            Pen blackPen = new Pen(Color.Black, 1);
            if (line.LineStyle == LineStyle.Dot)
            {
                float[] dashValues = { 1, 1, 1, 1 };
                blackPen.DashPattern = dashValues;
            }
            g.DrawLine(blackPen, line.PointStart.XPoint, line.PointStart.YPoint, line.PointDest.XPoint, line.PointDest.YPoint);

        }
        /// <summary>
        /// In Hinh anh len PrintDocument
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pB"></param>
        /// <param name="rootPoint"></param>
        public static void PrintImage(Graphics g, PictureBox pB, FosPoint rootPoint)
        {
            Image img = pB.Image;
            g.DrawImage(img, pB.Location.X + rootPoint.XPoint, pB.Location.Y+rootPoint.YPoint, pB.Width, pB.Height);
        }
        #endregion
        #region Fun
        /// <summary>
        /// tao border cho cac control
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="page"></param>
        /// <param name="rootPoint"></param>
        public static void PrintBorderForControl(Control ct, FosPoint rootPoint)
        {
            FosLine line1 = new FosLine();
            line1.PointStart = new FosPoint(rootPoint.XPoint + ct.Location.X, rootPoint.YPoint + ct.Location.Y);
            line1.PointDest = new FosPoint(rootPoint.XPoint + ct.Location.X + ct.Width, rootPoint.YPoint + ct.Location.Y);
            if (CurPage == 1)
                lsLineInpage.Add(line1);

            FosLine line2 = new FosLine();
            line2.PointStart = new FosPoint(rootPoint.XPoint + ct.Location.X, rootPoint.YPoint + ct.Location.Y + ct.Height);
            line2.PointDest = new FosPoint(rootPoint.XPoint + ct.Location.X + ct.Width, rootPoint.YPoint + ct.Location.Y + ct.Height);
            if (CurPage == 1)
                lsLineInpage.Add(line2);

            FosLine line3 = new FosLine();
            line3.PointStart = new FosPoint(rootPoint.XPoint + ct.Location.X, rootPoint.YPoint + ct.Location.Y);
            line3.PointDest = new FosPoint(rootPoint.XPoint + ct.Location.X, rootPoint.YPoint + ct.Location.Y + ct.Height);
            if (CurPage == 1)
                lsLineInpage.Add(line3);

            FosLine line4 = new FosLine();
            line4.PointStart = new FosPoint(rootPoint.XPoint + ct.Location.X + ct.Width, rootPoint.YPoint + ct.Location.Y);
            line4.PointDest = new FosPoint(rootPoint.XPoint + ct.Location.X + ct.Width, rootPoint.YPoint + ct.Location.Y + ct.Height);
            if (CurPage == 1)
                lsLineInpage.Add(line4);

        }
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
            int xPoint = label.Location.X ;
            int yPoint = 0;
            //center
            if (label.TextAlign == ContentAlignment.MiddleLeft ||
                label.TextAlign == ContentAlignment.MiddleCenter ||
                label.TextAlign == ContentAlignment.MiddleRight)
            {
                yPoint = label.Location.Y + (label.Size.Height - size) / 2;
            }
            //top
            if (label.TextAlign == ContentAlignment.TopLeft ||
                label.TextAlign == ContentAlignment.TopCenter ||
                label.TextAlign == ContentAlignment.TopRight)
            {
                yPoint = label.Location.Y ;
            }
            ////bottom
            if (label.TextAlign == ContentAlignment.BottomLeft ||
                label.TextAlign == ContentAlignment.BottomCenter ||
                label.TextAlign == ContentAlignment.BottomRight)
            {
                yPoint = label.Location.Y + (label.Size.Height - size -2);
            }
            FosPoint point = new FosPoint(xPoint, yPoint);
            return point;
        }
        /// <summary>
        ///  Canh trai, phai, giua cho label
        /// </summary>
        /// <param name="lb"></param>
        /// <returns></returns>
        public static int AlignforFPdfLabel(FPdfLabel lb)
        {
            double align;
            double width = lb.Size.Width;
            using (Bitmap tempImage = new Bitmap(lb.Size.Width, lb.Size.Height))
            {
                SizeF stringSize = Graphics.FromImage(tempImage).MeasureString(lb.Text, lb.Font);
                align = width - stringSize.Width;
            }
            if (lb.TextAlign == ContentAlignment.TopCenter ||
                lb.TextAlign == ContentAlignment.MiddleCenter ||
                lb.TextAlign == ContentAlignment.BottomCenter)
            {
                align = align / 2;
            }
            if (lb.TextAlign == ContentAlignment.TopLeft ||
                lb.TextAlign == ContentAlignment.MiddleLeft ||
                lb.TextAlign == ContentAlignment.BottomLeft)
            {
                align = 0;
            }
            if (lb.TextAlign == ContentAlignment.TopRight ||
                lb.TextAlign == ContentAlignment.MiddleRight ||
                lb.TextAlign == ContentAlignment.BottomRight)
            {
                align = align - 4;
            }
            return (int)Math.Round(align, MidpointRounding.AwayFromZero);
        }       
        #endregion
    }
}
