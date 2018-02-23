using Client.Models;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Detectors
{
    public interface ITriangleDetector
    {
        IPicture Picture { get; set; }

        List<Triangle2DF> Triangles { get; set; }

        List<Triangle2DF> Detect();

        void Draw();
    }
}
