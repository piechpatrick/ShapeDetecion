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
        private Bitmap _bitmap;
        public Bitmap Bitmap
        {
            get
            {
                return _bitmap;
            }
            set
            {
                SetProperty(ref _bitmap, value);
            }
        }
        public Picture(string path)
        {
            _bitmap = new Bitmap(path);
        }
    }
}
