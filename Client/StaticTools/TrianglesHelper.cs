using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.StaticTools
{
    public static class TrianglesHelper
    {
        public static List<Triangle2DF> GetTraingles(UMat thresholeded)
        {
            List<Triangle2DF> _triangleList = new List<Triangle2DF>();

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                //try to find contours on previously prepared image -->
                CvInvoke.FindContours(thresholeded, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                int count = contours.Size;
                for (int i = 0; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    using (VectorOfPoint approxContour = new VectorOfPoint())
                    {
                        CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.04, true);
                        if (CvInvoke.ContourArea(approxContour, false) > 1)
                        {
                            //triangle has 3 contours -->
                            if (approxContour.Size == 3)
                            {
                                Point[] pts = approxContour.ToArray();
                                _triangleList.Add(new Triangle2DF(
                                   pts[0],
                                   pts[1],
                                   pts[2]
                                   ));
                            }
                        }
                    }
                }
            }
            return _triangleList;
        }

        public static int GetDiameter(Triangle2DF triangle2DF)
        {
            var listX = new List<float>();
            listX.Add(triangle2DF.V0.X);
            listX.Add(triangle2DF.V1.X);
            listX.Add(triangle2DF.V2.X);
            var min = listX.Min();
            var max = listX.Max();
            return (int)(max - min + 15);
        }
    }
}
