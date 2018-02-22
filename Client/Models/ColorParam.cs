using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class ColorParam : BindableBase
    {
        private string _redValue = "0";
        public string RedValue
        {
            get
            {
                return _redValue;
            }
            set
            {
                SetProperty(ref _redValue, value);
            }
        }
        private string _greenValue = "255";
        public string GreenValue
        {
            get
            {
                return _greenValue;
            }
            set
            {
                SetProperty(ref _greenValue, value);
            }
        }
        private string _blueValue = "0";
        public string BlueValue
        {
            get
            {
                return _blueValue;
            }
            set
            {
                SetProperty(ref _blueValue, value);
            }
        }
    }
}
