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

        public Bitmap OriginalBitmap
        {
            get { return _image.Bitmap; }
        }

        public ShapeDetection(IPicture picture)
        {
            _image = picture.Image
            .Resize(650, 550, Emgu.CV.CvEnum.Inter.Linear, true);
            picture.Image = new Image<Bgr, byte>(_image.Bitmap);
        }



        public virtual void Detect()
        {

        }

    }
}
