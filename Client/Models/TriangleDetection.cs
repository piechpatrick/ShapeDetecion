using Client.Detectors;
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
        public ISpecificTriangleDetector Detector { get; private set; }

        public TriangleDetection(ISpecificTriangleDetector detector)
            : base(detector.Picture)
        {
            this.Detector = detector;
        }

        public override void Detect()
        {
            Detector.Detect();
        }

        public void Draw()
        {
            Detector.Draw();
        }





        //public override void Detect()
        //{
        //    //pre-process image 

        //    UMat cannyEdges = new UMat();
        //    CvInvoke.CvtColor(_image, cannyEdges, ColorConversion.Bgr2Gray);

        //    UMat pyrDown = new UMat();

        //    CvInvoke.PyrDown(cannyEdges, pyrDown);
        //    CvInvoke.PyrUp(pyrDown, cannyEdges);
        //    CvInvoke.Canny(_image, cannyEdges, 80, 50);

        //    //assign pre-processed image as customImage
        //    _customImage.Bitmap = cannyEdges.Bitmap;


        //    // Order by Area
        //    var sortedTriangle = GetTraingles(cannyEdges).OrderBy(t => t.Area).ToList();
        //    sortedTriangle.Reverse();
        //    if (sortedTriangle.Count >= 3)
        //    {
        //        //undupe by precision to an integer
        //        //TO DO: is it possible to more equitable unduping?!
        //        var undpued = from t in sortedTriangle
        //                      group t by (int)(t.Centeroid.X + t.Centeroid.Y) into g
        //                      select g.First();

        //        //take 3 largest as part of bussiness logic 
        //        var tooked3 = undpued.Take(3);

        //        //draw circle and mark sign 3 largest 
        //        int idx = 1;
        //        foreach (var triangle in tooked3)
        //        {
        //            CvInvoke.Circle(_image, new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y), 
        //                GetDiameter(triangle) / 2, new Bgr(Color.Red).MCvScalar, 1);

        //            CvInvoke.PutText(_image, idx.ToString(), new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y),
        //                FontFace.HersheyComplex, 1, new Bgr(255,0,0).MCvScalar,2);
        //            idx++;
        //        }
        //    }

        //    // draw circle on all triangles at customImage
        //    foreach (var triangle in sortedTriangle)
        //    {
        //        CvInvoke.Circle(_customImage, new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y),
        //                GetDiameter(triangle) / 2, new Bgr(Color.Red).MCvScalar, 1);
        //    }

        //}

        //public void Detect(ColorParam color)
        //{
        //    Image<Bgr, byte> img = new Image<Bgr, byte>(_image.Bitmap);
        //    var image = img.InRange(new Bgr(Convert.ToDouble(color.BlueValue),
        //        Convert.ToDouble(color.GreenValue),
        //        Convert.ToDouble(color.RedValue)), 
        //        new Bgr(
        //            Convert.ToDouble(color.BlueValue),
        //            Convert.ToDouble(color.GreenValue),
        //            Convert.ToDouble(color.RedValue)));

        //    UMat cannyEdges = new UMat();

        //    foreach (var gray in image.Split())
        //    {
        //        CvInvoke.Canny(image, cannyEdges, 80, 160);
        //    }

        //    //UMat pyrDown = new UMat();
        //    //CvInvoke.PyrDown(image, pyrDown);
        //    //CvInvoke.PyrUp(pyrDown, cannyEdges);

        //    _customImage.Bitmap = cannyEdges.Bitmap;

        //    var sortedTriangles = GetTraingles(cannyEdges).OrderBy(t => t.Area).ToList();
        //    sortedTriangles.Reverse();

        //    var undpued = from t in sortedTriangles
        //                  group t by (int)(t.Centeroid.X + t.Centeroid.Y) into g
        //                  select g.First();

        //    if (undpued.Count() >= 3)
        //    {
        //        var tooked3 = undpued.Take(3);


        //        // draw circle on all triangles at customImage
        //        int idx = 1;
        //        foreach (var triangle in tooked3)
        //        {
        //            CvInvoke.Circle(_customImage, new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y),
        //                    GetDiameter(triangle) / 2, new Bgr(Color.Red).MCvScalar, 1);

        //            CvInvoke.Circle(_image, new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y),
        //                    GetDiameter(triangle) / 2, new Bgr(Color.Red).MCvScalar, 1);

        //            CvInvoke.PutText(_customImage, idx.ToString(), new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y),
        //                    FontFace.HersheyComplex, 1, new Bgr(0, 0, 255).MCvScalar, 2);


        //            CvInvoke.PutText(_image, idx.ToString(), new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y),
        //                    FontFace.HersheyComplex, 1, new Bgr(0, 0, 255).MCvScalar, 2);

        //            idx++;
        //        }

        //        foreach (var triangle in undpued)
        //        {
        //            CvInvoke.Circle(_customImage, new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y),
        //            GetDiameter(triangle) / 2, new Bgr(Color.Red).MCvScalar, 1);
        //        }
        //    }

        //}

        //private List<Triangle2DF> GetTraingles(UMat cannyEdges)
        //{
        //    //find triangles logic 

        //    List<Triangle2DF> _triangleList = new List<Triangle2DF>();

        //    using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
        //    {
        //        CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
        //        int count = contours.Size;
        //        for (int i = 0; i < count; i++)
        //        {
        //            using (VectorOfPoint contour = contours[i])
        //            using (VectorOfPoint approxContour = new VectorOfPoint())
        //            {
        //                CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.04, true);
        //                if (CvInvoke.ContourArea(approxContour, false) > 1)
        //                {
        //                    if (approxContour.Size < 7)
        //                    {
        //                        Point[] pts = approxContour.ToArray();
        //                        _triangleList.Add(new Triangle2DF(
        //                           pts[0],
        //                           pts[1],
        //                           pts[2]
        //                           ));
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return _triangleList;
        //}

        //private int GetDiameter(Triangle2DF triangle2DF)
        //{
        //    var listX = new List<float>();
        //    listX.Add(triangle2DF.V0.X);
        //    listX.Add(triangle2DF.V1.X);
        //    listX.Add(triangle2DF.V2.X);
        //    var min = listX.Min();
        //    var max = listX.Max();
        //    return (int)(max - min + 15);
        //}    



    }
}
