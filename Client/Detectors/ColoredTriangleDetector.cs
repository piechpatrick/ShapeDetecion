using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Client.Detectors
{
    public class ColoredTriangleDetector : ISpecificTriangleDetector
    {
        public ColorParam Color { get; set; }

        public IPicture Picture { get; set; }

        public List<Triangle2DF> Triangles { get; set; }

        public List<Triangle2DF> Detect()
        {
            Image<Bgr, byte> img = Picture.Image;
            var image = img.InRange(new Bgr(Convert.ToDouble(Color.BlueValue),
                Convert.ToDouble(Color.GreenValue),
                Convert.ToDouble(Color.RedValue)),
                new Bgr(
                    Convert.ToDouble(Color.BlueValue),
                    Convert.ToDouble(Color.GreenValue),
                    Convert.ToDouble(Color.RedValue)));

            UMat cannyEdges = new UMat();

            foreach (var gray in image.Split())
            {
                CvInvoke.Canny(image, cannyEdges, 80, 160);
            }

            //UMat pyrDown = new UMat();
            //CvInvoke.PyrDown(image, pyrDown);
            //CvInvoke.PyrUp(pyrDown, cannyEdges);

            //_customImage.Bitmap = cannyEdges.Bitmap;

            Triangles =  GetTraingles(cannyEdges).OrderBy(t => t.Area).ToList();
            return Triangles;
        }

        public void Draw()
        {
            Triangles.Reverse();

            var undpued = from t in Triangles
                          group t by (int)(t.Centeroid.X + t.Centeroid.Y) into g
                          select g.First();

            if (undpued.Count() >= 3)
            {
                var tooked3 = undpued.Take(3);


                // draw circle on all triangles at customImage
                int idx = 1;
                foreach (var triangle in tooked3)
                {
                    CvInvoke.Circle(Picture.Image, new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y),
                            GetDiameter(triangle) / 2, new Bgr(System.Drawing.Color.Red).MCvScalar, 1);


                    CvInvoke.PutText(Picture.Image, idx.ToString(), new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y),
                            FontFace.HersheyComplex, 1, new Bgr(0, 0, 255).MCvScalar, 2);

                    idx++;
                }
            }
        }

        private List<Triangle2DF> GetTraingles(UMat cannyEdges)
        {
            //find triangles logic 

            List<Triangle2DF> _triangleList = new List<Triangle2DF>();

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                int count = contours.Size;
                for (int i = 0; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    using (VectorOfPoint approxContour = new VectorOfPoint())
                    {
                        CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.04, true);
                        if (CvInvoke.ContourArea(approxContour, false) > 1)
                        {
                            if (approxContour.Size < 7)
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
