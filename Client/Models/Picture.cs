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
    public class Picture : BindableBase, IPicture
    {
        private Image<Bgr,byte> _image;
        public Image<Bgr, byte> Image
        {
            get
            {
                return _image;
            }
            set
            {
                SetProperty(ref _image, value);
            }
        }
        private Image<Bgr, byte> _trainingimage;
        public Image<Bgr, byte> TrainingImage
        {
            get
            {
                return _trainingimage;
            }
            set
            {
                SetProperty(ref _trainingimage, value);
            }
        }

        public Picture(string path)
        {
            _image = new Image<Bgr, byte>(path);
            _trainingimage = new Image<Bgr, byte>(path);
        }

        public Picture(IPicture picture)
        {
            _image = picture.Image;
        }
    }
}
