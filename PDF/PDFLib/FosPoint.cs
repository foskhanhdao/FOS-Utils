using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FOS_Utils.PDF.PDFLib
{
    public class FosPoint
    {
        #region Varirable
        private float xPoint;
        private float yPoint;
        #endregion
        #region Field
        public float XPoint
        {
            get { return xPoint; }
            set
            {
                xPoint = value;
            }
        }
        public float YPoint
        {
            get { return yPoint; }
            set
            {
                yPoint = value;
            }
        }
        #endregion
        #region Contructer
        public FosPoint()
        { }
        public FosPoint(float xPoint, float yPoint)
        {
            this.xPoint = xPoint;
            this.yPoint = yPoint;
        }
        #endregion
    }
}
