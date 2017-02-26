using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Forms;
using FOS_Utils.PDF.PDFControl;
using System.Drawing;
using System.Data;
using System.IO;

namespace FOS_Utils.PDF.PDFLib
{
    public static class PdfHelper
    {
        #region Variable
        //tuong ung voi 1 file Pdf
        public static Document doc = null;
        //dang nhu mot cay viet de ve
        public static PdfWriter writer = null;
        //tong so trang Pdf
        private static int NumberPage = 1;
        //trang Pdf hien tai
        private static int CurPage = 1;
        //tong so dong detail co trong mot trang
        private static int MaxRow = 0; 
        //Tong hop tat ca cac line trong trang
        private static List<FosLine> lsLineInpage = new List<FosLine>();
        #endregion
        #region ComonFun
        /// <summary>
        /// Chuyen doi toa đo giua Form va file Pdf
        /// </summary>
        /// <param name="point"></param>
        /// <param name="page"></param>
        public static void ConvertToPointPdf(FosPoint point, PagePdf page)
        {
            point.YPoint = page.Height - point.YPoint;
        }
        /// <summary>
        /// Tu textbox lay ra toa do de in text 
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static FosPoint CreatePointFromControl(FPdfText textbox)
        {
            int height = textbox.Size.Height;
            int size = (int)textbox.Font.Size;
            int xPoint = textbox.Location.X + 4;
            int yPoint = textbox.Location.Y + size + (textbox.Size.Height - size) / 2;
            FosPoint point = new FosPoint(xPoint, yPoint);
            return point;
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
        /// <summary>
        /// Canh trai, phai, giua cho textbox
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static int AlignforFPdfText(FPdfText tb)
        {
            double align;
            double width = tb.Size.Width;
            using (Bitmap tempImage = new Bitmap(tb.Size.Width, tb.Size.Height))
            {
                SizeF stringSize = Graphics.FromImage(tempImage).MeasureString(tb.Text, tb.Font);
                align = width - stringSize.Width;
            }
            if (tb.TextAlign == HorizontalAlignment.Center)
            {
                align = align / 2;
            }
            if (tb.TextAlign == HorizontalAlignment.Left)
            {
                align = 0;
            }
            return (int)Math.Round(align, MidpointRounding.AwayFromZero);
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
            if (lb.TextAlign == ContentAlignment.TopCenter||
                lb.TextAlign == ContentAlignment.MiddleCenter||
                lb.TextAlign == ContentAlignment.BottomCenter)
            {
                align = align / 2;
            }
            if (lb.TextAlign == ContentAlignment.TopLeft||
                lb.TextAlign == ContentAlignment.MiddleLeft||
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
        /// <summary>
        /// Ve cac dong detail
        /// </summary>
        /// <param name="panel"></param>
        public static void DrawAllDetail(FPdfPanel panel)
        {
            if(panel.pnDetail!=null)
            {
                FPdfPanel detail = panel.pnDetail;               
                for (int i = 1; i < detail.MaxRow; i++)
                {
                    FPdfPanel detailCoppy = new FPdfPanel();
                    Point locationDetail = new Point(detail.Location.X, detail.Location.Y + (detail.Size.Height * i));
                    detailCoppy.Location = locationDetail;
                    CoppyFPFPanelDetail(detail, detailCoppy, i);
                    detail.Parent.Controls.Add(detailCoppy);
                }                
            }
        }
        public static void CoppyFPFPanelDetail(FPdfPanel panelSource, FPdfPanel panelDest, int row)
        {
            panelDest.Size = panelSource.Size;
            panelDest.DataSource = panelSource.DataSource;
            panelDest.lsPdfLine = panelSource.lsPdfLine;
            panelDest.BorderStyle = panelSource.BorderStyle;
            //panelDest.AddControlEx += new AddControl(  panelSource.AddControlEx);
            panelSource.CoppyAddEvent(panelDest);
            foreach (Control ct in panelSource.Controls)
            {
                if (ct is FPdfText)
                {
                    FPdfText tb = new FPdfText();
                    CopyFPdfText(ct as FPdfText, tb, row);
                    panelDest.Controls.Add(tb);
                    panelDest.RaiseAddControlEvent(tb);
                }
                else if (ct is FPdfLabel)
                {
                    FPdfLabel lb = new FPdfLabel();
                    CopyFPdfLabel(ct as FPdfLabel, lb, row);
                    panelDest.Controls.Add(lb);
                    panelDest.RaiseAddControlEvent(lb);
                }
            }
        }
        public static void CopyFPdfText(FPdfText tbSource, FPdfText tbDest, int row)
        {
            tbDest.Size = tbSource.Size;
            tbDest.Font = tbSource.Font;
            tbDest.Location = tbSource.Location;
            tbDest.TextAlign = tbSource.TextAlign;
            tbDest.BorderStyle = tbSource.BorderStyle;
            tbDest.BackColor = tbSource.BackColor;
            //tbDest.Text = tbSource.Text;
            tbDest.FPdfProperties.TableRow = row;
            tbDest.FPdfProperties.TableColumn = tbSource.FPdfProperties.TableColumn;
        }
        public static void CopyFPdfLabel(FPdfLabel lbSource, FPdfLabel lbDest, int row)
        {
            lbDest.Size = lbSource.Size;
            lbDest.Font = lbSource.Font;
            lbDest.Location = lbSource.Location;
            lbDest.TextAlign = lbSource.TextAlign;
            //lbDest.Text = lbSource.Text;
            lbDest.BorderStyle = lbSource.BorderStyle;
            lbDest.BackColor = lbSource.BackColor;
            lbDest.FPdfProperties.TableRow = row;
            lbDest.FPdfProperties.TableColumn = lbSource.FPdfProperties.TableColumn;
            lbDest.IsShowLineTop = lbSource.IsShowLineTop;
            lbDest.LineStyleTop = lbSource.LineStyleTop;

            lbDest.IsShowLineBottom = lbSource.IsShowLineBottom;
            lbDest.LineStyleBottom = lbSource.LineStyleBottom;

            lbDest.IsShowLineRight = lbSource.IsShowLineRight;
            lbDest.LineStyleRight = lbSource.LineStyleRight;

            lbDest.IsShowLineLeft = lbSource.IsShowLineLeft;
            lbDest.LineStyleLeft = lbSource.LineStyleLeft;
        }
        /// <summary>
        /// Lay du lieu cho textbox dua vao DataSource cua Detail
        /// </summary>
        /// <param name="FPdfText"></param>
        /// <param name="curPage"></param>
        public static void GetDataFPdfText(FPdfText FPdfText, int curPage)
        {
            if (FPdfText.Parent is FPdfPanel && (FPdfText.Parent as FPdfPanel).DataSource != null
                && (FPdfText.Parent as FPdfPanel).DataSource.Rows.Count != 0
                && (FPdfText.Parent as FPdfPanel).DataSource.Columns.Contains(FPdfText.FPdfProperties.TableColumn))
            {
                DataTable dt = (FPdfText.Parent as FPdfPanel).DataSource;
                int row = ((curPage - 1) * MaxRow + FPdfText.FPdfProperties.TableRow);
                if (row >= dt.Rows.Count)
                {
                    FPdfText.Text = "";
                    return;
                }
                FPdfText.Text = dt.Rows[row][FPdfText.FPdfProperties.TableColumn] + "";
            }
        }
        /// <summary>
        /// Lay du lieu cho Label dua vao DataSource cua Detail
        /// </summary>
        /// <param name="FPdfText"></param>
        /// <param name="curPage"></param>
        public static void GetDataFPdfLabel(FPdfLabel FPdfLabel, int curPage)
        {
            if (FPdfLabel.Parent is FPdfPanel && (FPdfLabel.Parent as FPdfPanel).DataSource != null
                && (FPdfLabel.Parent as FPdfPanel).DataSource.Rows.Count != 0
                && (FPdfLabel.Parent as FPdfPanel).DataSource.Columns.Contains(FPdfLabel.FPdfProperties.TableColumn))
            {
                DataTable dt = (FPdfLabel.Parent as FPdfPanel).DataSource;
                int row = ((curPage - 1) * MaxRow + FPdfLabel.FPdfProperties.TableRow);
                if (row >= dt.Rows.Count)
                {
                    FPdfLabel.Text = "";
                    return;
                }
                FPdfLabel.Text = dt.Rows[row][FPdfLabel.FPdfProperties.TableColumn] + "";
            }
        }
        /// <summary>
        /// tao border cho cac control
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="page"></param>
        /// <param name="rootPoint"></param>
        public static void PrintBorderForControl(Control ct, PagePdf page, FosPoint rootPoint)
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
        public static void PrintBorderLabel(FPdfLabel tb, PagePdf page, FosPoint rootPoint)
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
        #endregion
        #region PDFFun
        /// <summary>
        /// goi ham de chuan bi in mot file Pdf
        /// </summary>
        /// <param name="output"></param>
        /// <param name="pagePdf"></param>
        public static void BeginPrint(string output, PagePdf pagePdf)
        {
            if (doc == null && writer == null)
            {
                doc = new Document();
                writer = PdfWriter.GetInstance(doc, new FileStream(output, FileMode.Create));
                switch (pagePdf.PageType)
                {
                    case PageType.A4:
                        doc.SetPageSize(new iTextSharp.text.Rectangle(800,1100));
                        break;
                    case PageType.A3:
                        doc.SetPageSize(new iTextSharp.text.Rectangle(PageSize.A3));
                        break;
                }
                doc.Open();
            }
        }
        /// <summary>
        /// Sau khi in xong thi goi ham nay de ket thuc
        /// </summary>
        public static void EndPrint()
        {
            if (doc != null && writer != null)
            {
                doc.Close();
                doc = null;
                writer = null;
                NumberPage = 1;
                MaxRow = 0;
                CurPage = 1;
                lsLineInpage = new List<FosLine>();
            }
        }
        /// <summary>
        /// Neu dang in ma muon in qua mot trang pdf moi thi goi ham nay
        /// </summary>
        public static void CreateNewPage()
        {
            if (doc != null)
            {
                doc.NewPage();
            }
        }
        public static void PrintPdfFile(FPdfPanel panel, PagePdf pagePdf)
        {
            if (doc != null)
            {
                if(panel.pnDetail!=null&&panel.pnDetail.DataSource!=null)
                {
                    FPdfPanel detail = panel.pnDetail;
                    NumberPage = detail.DataSource.Rows.Count / detail.MaxRow;
                    if ((detail.DataSource.Rows.Count % detail.MaxRow) != 0)
                        NumberPage++;
                    MaxRow = detail.MaxRow;
                }
                for (int i = 1; i <= NumberPage; i++)
                {
                    // moi vong lap la in mot trang Pdf
                    CurPage = i;
                    // In het cac control trong trang
                    PrintAllControlInPanel(panel, pagePdf, CurPage, new FosPoint(0, 0));
                    //In het cac line trong trang sau cung
                    foreach (FosLine line in lsLineInpage)
                    {
                        PrintPdfLine(line, pagePdf);
                    }
                    //Neu khong phai trang cuoi thi tao moi trang de in tiep
                    if (i != NumberPage)
                    {
                        CreateNewPage();
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
        public static void PrintAllControlInPanel(FPdfPanel panel, PagePdf page, int curPage, FosPoint rootPoint)
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
                    PrinPdfImage(pB, page, rootPoint);
                }
            }
            //Sau do in chu
            foreach (Control c in panel.Controls)
            {                
                if (c is FPdfText)
                {
                    FPdfText FPdfText = c as FPdfText;
                    PrintPdfString(FPdfText, page, rootPoint, curPage);
                }
                if (c is FPdfLabel)
                {
                    FPdfLabel FPdfLabel = c as FPdfLabel;
                    PrintPdfString(FPdfLabel, page, rootPoint, curPage);
                }
                //in Panel con
                if (c is FPdfPanel)
                {
                    FPdfPanel FPdfPanelChirld = c as FPdfPanel;
                    //inborder
                    if (FPdfPanelChirld.BorderStyle == BorderStyle.FixedSingle)
                        PrintBorderForControl(FPdfPanelChirld, page, rootPoint);
                    PrintAllControlInPanel(FPdfPanelChirld, page, curPage, new FosPoint(rootPoint.XPoint + FPdfPanelChirld.Location.X, rootPoint.YPoint + FPdfPanelChirld.Location.Y));
                }
            }
        }
        /// <summary>
        /// In chu len file Pdf dua vao textbox
        /// </summary>
        /// <param name="FPdfText"></param>
        /// <param name="page"></param>
        /// <param name="rootPoint"></param>
        /// <param name="curPage"></param>
        public static void PrintPdfString(FPdfText FPdfText, PagePdf page, FosPoint rootPoint, int curPage)
        {
            //inborder
            if (FPdfText.BorderStyle == BorderStyle.FixedSingle)
                PrintBorderForControl(FPdfText, page, rootPoint);
            //khoi tao bien de in text  
            PdfContentByte cb = writer.DirectContent;
            BaseFont bf;
            if (FPdfText.Font.Italic)
            {
                bf = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\ariali.TTF", BaseFont.IDENTITY_H, true);
            }
            else
            {
                bf = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\Arial.TTF", BaseFont.IDENTITY_H, true);
            }
            //int size = (int)control.Font.Size;
            float size = (float)FPdfText.Font.Size + (float)FPdfText.Font.Size / 3;
            cb.SetFontAndSize(bf, size);
            if (FPdfText.Font.Bold)
            {
                cb.SetLineWidth(0.5);
                cb.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_FILL_STROKE);
            }
            else
            {
                cb.SetLineWidth(0);
                cb.SetTextRenderingMode(PdfContentByte.ALIGN_LEFT);
            }

            // tao bien tao do
            FosPoint point = CreatePointFromControl(FPdfText);
            // Tuy vao control cha la gi ma chinh lai toa do
            point.XPoint += rootPoint.XPoint;
            point.YPoint += rootPoint.YPoint;
            //canh chinh lai toa do cho phu hop
            PdfHelper.ConvertToPointPdf(point, page);
            //Lay du lieu cua dataSource bo vao text
            GetDataFPdfText(FPdfText, curPage);
            string text = FPdfText.Text;
            //if (text.Trim() == "")
            //    return;
            //canh lai center hoac right
            point.XPoint += AlignforFPdfText(FPdfText as FPdfText);
            cb.BeginText();
            cb.ShowTextAligned(0, text, point.XPoint, point.YPoint, FPdfText.FPdfProperties.Rotation);
            cb.EndText();

        }
        /// <summary>
        /// In chu len file Pdf dua vao label
        /// </summary>
        /// <param name="FPdfText"></param>
        /// <param name="page"></param>
        /// <param name="rootPoint"></param>
        /// <param name="curPage"></param>
        public static void PrintPdfString(FPdfLabel FPdfLabel, PagePdf page, FosPoint rootPoint, int curPage)
        {
            //Inborder
            PrintBorderLabel(FPdfLabel, page, rootPoint);
            //InBackColor
            PrintBackColor(FPdfLabel, page, rootPoint);
            
            //khoi tao bien de in text  
            PdfContentByte cb = writer.DirectContent;
            cb.SaveState();
            BaseFont bf;
            if (FPdfLabel.Font.Italic)
            {
                bf = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\ariali.TTF", BaseFont.IDENTITY_H, true);
            }
            else
            {
                bf = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\Arial.TTF", BaseFont.IDENTITY_H, true);
            }
            //set Font
            float size = (float)FPdfLabel.Font.Size + (float)FPdfLabel.Font.Size / 3;
            cb.SetFontAndSize(bf, size);
            if (FPdfLabel.Font.Bold)
            {
                cb.SetLineWidth(0.5);
                cb.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_FILL_STROKE);
            }
            else
            {
                cb.SetLineWidth(0);
                cb.SetTextRenderingMode(PdfContentByte.ALIGN_LEFT);
            }

            // tao bien tao do
            FosPoint point = CreatePointFromLabel(FPdfLabel);
            // Tuy vao control cha la gi ma chinh lai toa do
            point.XPoint += rootPoint.XPoint;
            point.YPoint += rootPoint.YPoint;
            //canh chinh lai toa do cho phu hop
            PdfHelper.ConvertToPointPdf(point, page);
            //Lay du lieu cua dataSource bo vao text
            GetDataFPdfLabel(FPdfLabel, curPage);
            string text = FPdfLabel.Text;
            //if (text.Trim() == "")
            //    return;
            //canh lai center hoac right
            point.XPoint += AlignforFPdfLabel(FPdfLabel as FPdfLabel);
            cb.BeginText();
            cb.ShowTextAligned(0, text, point.XPoint, point.YPoint, FPdfLabel.FPdfProperties.Rotation);
            cb.EndText();
            cb.RestoreState();

        }
        /// <summary>
        /// Ve mot duong thang tren file Pdf
        /// </summary>
        /// <param name="line"></param>
        /// <param name="page"></param>
        public static void PrintPdfLine(FosLine line, PagePdf page)
        {
            PdfContentByte cb = writer.DirectContent;
            cb.SaveState();
            //canh chinh lai toa do cho phu hop
            //covan de
            PdfHelper.ConvertToPointPdf(line.PointStart, page);
            PdfHelper.ConvertToPointPdf(line.PointDest, page);
            cb.SetLineWidth(line.LineSize);
            switch (line.LineStyle)
            {
                case LineStyle.Dot:
                    cb.SetLineDash(1f, 1f);
                    break;
                case LineStyle.Nomar:
                    cb.SetLineCap(1);
                    break;
            }

            cb.MoveTo(line.PointStart.XPoint, line.PointStart.YPoint);
            cb.LineTo(line.PointDest.XPoint, line.PointDest.YPoint);
            cb.Stroke();
            cb.RestoreState();
            //sau khi ve xong phai tra lai toa do de ve lai trang khac
            PdfHelper.ConvertToPointPdf(line.PointStart, page);
            PdfHelper.ConvertToPointPdf(line.PointDest, page);
        }
        /// <summary>
        /// In mot picturebox tren form ra mot hinh anh tren file Pdf
        /// </summary>
        /// <param name="pB"></param>
        /// <param name="page"></param>
        /// <param name="rootPoint"></param>
        public static void PrinPdfImage(PictureBox pB, PagePdf page, FosPoint rootPoint)
        {
            BaseColor bc = new BaseColor(255, 255, 255);
            try
            {
                var logo = iTextSharp.text.Image.GetInstance(pB.Image, bc);
                //xet toa do, goc toa do la goc phan |_
                FosPoint point = new FosPoint(pB.Location.X + rootPoint.XPoint, pB.Location.Y + pB.Size.Height + rootPoint.YPoint);
                PdfHelper.ConvertToPointPdf(point, page);
                logo.SetAbsolutePosition(point.XPoint, point.YPoint);
                //xet kich thuoc
                logo.ScaleAbsoluteHeight(pB.Size.Height);
                logo.ScaleAbsoluteWidth(pB.Size.Width);
                doc.Add(logo);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }        
        /// <summary>
        /// In BackColor cho control
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="page"></param>
        /// <param name="rootPoint"></param>
        public static void PrintBackColor(Control ct,PagePdf page, FosPoint rootPoint)
        {
            if (ct.BackColor == Color.White||ct.BackColor==Color.Transparent)
                return;
            PdfContentByte over = writer.DirectContent;
            BaseColor bc = new BaseColor(ct.BackColor);
            over.SaveState();
            FosPoint point = new FosPoint(ct.Location.X , ct.Location.Y + ct.Height);
            point.XPoint += rootPoint.XPoint;
            point.YPoint += rootPoint.YPoint;
            PdfHelper.ConvertToPointPdf(point, page);              
            over.Rectangle(point.XPoint, point.YPoint , ct.Width, ct.Height);
            over.SetColorFill(bc);
            over.Fill();
            over.RestoreState();
        }
        #endregion
    }
}
