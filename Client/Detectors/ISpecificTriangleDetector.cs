using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Detectors
{
    public interface ISpecificTriangleDetector : ITriangleDetector
    {
        ColorParam Color { get; set; }
    }
}
