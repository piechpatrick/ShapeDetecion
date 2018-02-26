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

    }
}
