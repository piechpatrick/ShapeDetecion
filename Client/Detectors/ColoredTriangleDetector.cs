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
    public class ColoredTriangleDetector : BindableBase, ISpecificTriangleDetector
    {
        public ColorParam Color { get; set; }

        private IPicture _picture;
        public IPicture Picture
        {
            get
            {
                return _picture;
            }
            set
            {
                SetProperty(ref _picture, value);
            }
        }

        public List<Triangle2DF> Triangles { get; set; }

        public List<Triangle2DF> Detect()
        {
            Image<Bgr, byte> img = Picture.Image;

            //filter user typed specific color -->
            var image = img.InRange(new Bgr(Convert.ToDouble(Color.BlueValue),
                Convert.ToDouble(Color.GreenValue),
                Convert.ToDouble(Color.RedValue)),
                new Bgr(
                    Convert.ToDouble(Color.BlueValue),
                    Convert.ToDouble(Color.GreenValue),
                    Convert.ToDouble(Color.RedValue)));

            //Blur & threshold image -->
            UMat blured = new UMat();
            UMat thresholeded = new UMat();
            CvInvoke.Blur(image, blured, new Size(1, 1),new Point(1,1));
            CvInvoke.Threshold(blured, thresholeded, 70.0f, 255, ThresholdType.Binary);
      
            //founded triangles ordered by area -->
            Triangles = StaticTools.TrianglesHelper.GetTraingles(thresholeded).OrderBy(t => t.Area).ToList();
            return Triangles;
        }

        

        public void Draw()
        {
            var drawOnMe = new Image<Bgr, byte>(Picture.Image.Bitmap);

            //heres our business logic for drawing 3 biggest triangles -->
            Triangles.Reverse();

            //eliminate possibility of duplication, this has to be more testable -->
            var undpued = from t in Triangles
                          group t by (int)(t.Centeroid.X + t.Centeroid.Y) into g
                          select g.First();

            //no one said that picutre has less than 3 triangles :) -->
            if (undpued.Count() >= 3)
            {
                var tooked3 = undpued.Take(3);


                // draw circle on all triangles at originalImage -->
                int idx = 1;
                foreach (var triangle in tooked3)
                {
                    CvInvoke.Circle(drawOnMe, new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y),
                            StaticTools.TrianglesHelper.GetDiameter(triangle) / 2, new Bgr(System.Drawing.Color.Red).MCvScalar, 1);


                    CvInvoke.PutText(drawOnMe, idx.ToString(),
                        new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y),
                            FontFace.HersheyComplex, 1, new Bgr(0x00, 0x00, 0xFF).MCvScalar, 3);

                    CvInvoke.PutText(drawOnMe, "Area: " + triangle.Area.ToString(),
                        new Point((int)triangle.Centeroid.X, (int)triangle.Centeroid.Y + 30),
                            FontFace.HersheyComplex, 0.3, new Bgr(0x93, 0x14, 0xFF).MCvScalar, 1);

                    idx++;
                }


                //heres the solution for binding works -->
                Picture.Image = drawOnMe;
            }
        }
    }
}
