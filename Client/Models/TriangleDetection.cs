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
            //pre-process image 

            UMat cannyEdges = new UMat();
            CvInvoke.CvtColor(_image, cannyEdges, ColorConversion.Bgr2Gray);

            UMat pyrDown = new UMat();

            CvInvoke.PyrDown(cannyEdges, pyrDown);
            CvInvoke.PyrUp(pyrDown, cannyEdges);
            CvInvoke.Canny(_image, cannyEdges, 80, 50);

            //assign pre-processed image as customImage
            _customImage.Bitmap = cannyEdges.Bitmap;


            //find triangles logic 
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
                        if (CvInvoke.ContourArea(approxContour, false) > 20)
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
            // Order by Area
            var sortedTriangle = _triangleList.OrderBy(t => t.Area).ToList();
            sortedTriangle.Reverse();
            if (sortedTriangle.Count >= 3)
            {
                //undupe by precision to an integer
                //TO DO: is it possible to more equitable unduping?!
                var undpued = from t in sortedTriangle
                              group t by (int)(t.Centeroid.X + t.Centeroid.Y) into g
                              select g.First();
              
                //take 3 largest as part of bussiness logic 
                var tooked3 = undpued.Take(3);

                //draw circle and mark sign 3 largest 
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

            // draw circle on all triangles at customImage
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
