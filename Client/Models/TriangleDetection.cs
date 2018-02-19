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

            //Convert the image to grayscale and filter out the noise
            UMat uimage = new UMat();
            CvInvoke.CvtColor(_image, cannyEdges, ColorConversion.Bgr2Gray);


            //use image pyr to remove noise
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(cannyEdges, pyrDown);
            CvInvoke.PyrUp(pyrDown, cannyEdges);

            CvInvoke.Canny(_image, cannyEdges, 80, 50);

            _customImage.Bitmap = cannyEdges.Bitmap;


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

            var sortedTriangle = _triangleList.OrderBy(t => t.Area).ToList();          
            if(sortedTriangle.Count >= 3)
            {
                sortedTriangle.Reverse();
                var tooked3 = sortedTriangle.Take(3);
                int idx = 1;
                foreach (var triangle in tooked3)
                {
                    CvInvoke.Circle(_image, new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y), 
                        GetDiameter(triangle) / 2, new Bgr(Color.Red).MCvScalar, 1);

                    CvInvoke.PutText(_image, idx.ToString(), new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y),
                        FontFace.HersheyComplex, 1, new Bgr(255,0,0).MCvScalar,2);
                    idx++;
                }
            }

            foreach (var triangle in _triangleList)
            {
                CvInvoke.Circle(_customImage, new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y),
                        GetDiameter(triangle) / 2, new Bgr(Color.Red).MCvScalar, 1);
            }

        }

        private int GetDiameter(Triangle2DF triangle2DF)
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
