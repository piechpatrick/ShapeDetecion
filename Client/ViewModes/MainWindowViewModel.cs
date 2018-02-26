using Client.Detectors;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Client.ViewModes
{
    public class MainWindowViewModel : BindableBase
    {

        private RelayCommand _loadImageCommand;
        private RelayCommand _specificColorTriangleDetectionCommad;

        private TriangleDetection _triangleDeteciton;

        private IPicture _originalPictureHost;
        public IPicture OriginalPictureHost
        {
            get
            {
                return _originalPictureHost;
            }
            set
            {
                SetProperty(ref _originalPictureHost, value);
            }
        }

        ISpecificTriangleDetector _specificTriangleDetector;
        public ISpecificTriangleDetector SpecificTriangleDetector
        {
            get
            {
                return _specificTriangleDetector;
            }
            set
            {
                SetProperty(ref _specificTriangleDetector, value);
            }
        }


        public ICommand LoadImageCommand { get { return _loadImageCommand; } }
        public ICommand SpecificColorTriangleDetectionCommand { get { return _specificColorTriangleDetectionCommad; } }

        private bool _isSpecificColorTriangleDetecionChecked;
        public bool IsSpecificColorTriangleDetecionChecked
        {
            get
            {
                return _isSpecificColorTriangleDetecionChecked;
            }
            set
            {
                SetProperty(ref _isSpecificColorTriangleDetecionChecked, value);
            }
        }

        private ColorParam _colorParam = new ColorParam();
        public ColorParam ColorParam
        {
            get
            {
                return _colorParam;
            }
            set
            {
                SetProperty(ref _colorParam, value);
            }
        }


        private string _path = "Load image...";
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                SetProperty(ref _path, value);
            }
        }


        public MainWindowViewModel()
        {
            _loadImageCommand = new RelayCommand(LoadImage, CanLoadImage);
            _specificColorTriangleDetectionCommad = new RelayCommand(SpecificColorTriangleDetection, CanTriangleDetect);
        }

        private async void LoadImage(object ob)
        {
            var task = Task.Factory.StartNew(() =>
            {
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|";
                if (openFileDialog.ShowDialog() == true && openFileDialog.CheckPathExists)
                {
                    OriginalPictureHost = new Picture(openFileDialog.FileName);
                    Path = openFileDialog.FileName;
                    IsSpecificColorTriangleDetecionChecked = false;

                    //TO DO:
                    //that can be new EmptyTriangleDetector() object -->
                    SpecificTriangleDetector = new ColoredTriangleDetector()
                    {
                        Picture = new Picture(openFileDialog.FileName)
                    };
                }
                _specificColorTriangleDetectionCommad.RaiseCanExecuteChanged();
            });
            await task;
        }
        private bool CanLoadImage(object ob)
        {
            return true;
        }

        private async void SpecificColorTriangleDetection(object ob)
        {
            if (IsSpecificColorTriangleDetecionChecked)
            {
                SpecificTriangleDetector = new ColoredTriangleDetector()
                {
                    Color = ColorParam,
                    Picture = new Picture(OriginalPictureHost),
                };
                var task = Task.Factory.StartNew(() =>
                {
                    _triangleDeteciton = new TriangleDetection(SpecificTriangleDetector);
                    _triangleDeteciton.Detect();
                    _triangleDeteciton.Draw();
                    });
                await task;
            }
            else
            {
                var task = Task.Run(() =>
                {
                    SpecificTriangleDetector = new ColoredTriangleDetector()
                    {
                        Picture = new Picture(OriginalPictureHost)
                    };
                });
            }
        }

        private bool CanTriangleDetect(object ob)
        {
            return SpecificTriangleDetector != null;
        }

    }
}
