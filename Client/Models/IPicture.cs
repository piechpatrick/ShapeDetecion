using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public interface IPicture
    {
        Image<Bgr,byte> Image { get; set; }
        Image<Bgr,byte> TrainingImage { get; set; }
    }
}
