using Client.Models;
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

namespace Client
{
    public class TriangleDetection : ShapeDetection
    {

        private List<Triangle2DF> _triangleList = new List<Triangle2DF>();

        public TriangleDetection(IPicture picture)
            :base(picture)
        { }

        public override void Detect()
        {
            UMat cannyEdges = new UMat();
            CvInvoke.Canny(_image, cannyEdges, 50, 150);

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                int count = contours.Size;
                for (int i = 0; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    using (VectorOfPoint approxContour = new VectorOfPoint())
                    {
                        CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
                        if (CvInvoke.ContourArea(approxContour, false) > 1)
                        {
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
            foreach (var triangle in _triangleList)
            {
                var listX = new List<float>();
                listX.Add(triangle.V0.X);
                listX.Add(triangle.V1.X);
                listX.Add(triangle.V2.X);
                var min = listX.Min();
                var max = listX.Max();
                var height = max - min + 15;
                CvInvoke.Circle(_image, new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y),(int)height/2, new Bgr(Color.Red).MCvScalar, 1);
            }
        }

    }
}
