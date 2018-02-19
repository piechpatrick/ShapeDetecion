using System;
using System.Drawing;
using Client.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Client
{
    public abstract class ShapeDetection
    {
        protected Image<Bgr, Byte> _image;

        protected Image<Bgr, Byte> _customImage;

        public Bitmap OriginalBitmap
        {
            get { return _image.Bitmap; }
        }

        public Bitmap CustomImage
        {
            get { return _customImage.Bitmap; }
            set { _customImage.Bitmap = value; }
        }

        public ShapeDetection(IPicture picture)
        {
            _image = new Image<Bgr, byte>(picture.Bitmap)
            .Resize(800, 600, Emgu.CV.CvEnum.Inter.Linear, true);
            _customImage = new Image<Bgr, byte>(picture.Bitmap)
               .Resize(800, 600, Emgu.CV.CvEnum.Inter.Linear, true);
        }



        public virtual void Detect()
        {

        }

    }
}
