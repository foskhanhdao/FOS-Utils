using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FOS_Utils.PDF.PDFLib
{
    public enum LineStyle
    {
        Nomar,
        Dot,
    }
    public class FosLine
    {
        #region Variable
        private FosPoint pointStart;
        private FosPoint pointDest;
        private int lineSize;
        private LineStyle lineStyle;

        #endregion
        #region Field
        public FosPoint PointStart
        {
            get { return pointStart; }
            set { pointStart = value; }
        }
        public FosPoint PointDest
        {
            get { return pointDest; }
            set { pointDest = value; }
        }
        public int LineSize
        {
            get { return lineSize; }
            set { lineSize = value; }
        }
        public LineStyle LineStyle
        {
            get { return lineStyle; }
            set { lineStyle = value; }
        }

        #endregion
        #region Contructer
        public FosLine()
        {
            this.lineSize = 1;
            this.LineStyle = LineStyle.Nomar;
        }
        public FosLine(FosPoint pointStart, FosPoint pointDest)
        {
            this.pointStart = pointStart;
            this.pointDest = pointDest;
            this.lineSize = 1;
            this.LineStyle = LineStyle.Nomar;
        }
        #endregion
    }
}
