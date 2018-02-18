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
        private RelayCommand _triangleDetectionCommand;

        private TriangleDetection _triangleDeteciton;

        private IPicture _picture;


        public ICommand LoadImageCommand { get { return _loadImageCommand; } }
        public ICommand TriangleDetectionCommand { get { return _triangleDetectionCommand; } }

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

        private bool _isTriangleDetetcionChecked;
        public bool IsTriangleDetecionChecked
        {
            get
            {
                return _isTriangleDetetcionChecked;
            }
            set
            {
                SetProperty(ref _isTriangleDetetcionChecked, value);
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
            _triangleDetectionCommand = new RelayCommand(TriangleDetection, CanTriangleDetect);
        }

        private async void LoadImage(object ob)
        {
            var task = Task.Factory.StartNew(() =>
            {
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|";
                if (openFileDialog.ShowDialog() == true && openFileDialog.CheckPathExists)
                {
                    _picture = new Picture(openFileDialog.FileName);
                    Bitmap = _picture.Bitmap;
                    Path = openFileDialog.FileName;
                    IsTriangleDetecionChecked = false;
                }
                _triangleDetectionCommand.RaiseCanExecuteChanged();
            });
            await task;
        }
        private bool CanLoadImage(object ob)
        {
            return true;
        }

        private async void TriangleDetection(object ob)
        {
            if(IsTriangleDetecionChecked)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    _triangleDeteciton = new TriangleDetection(_picture);
                    _triangleDeteciton.Detect();
                });
                await task;
                Bitmap = _triangleDeteciton.OriginalBitmap;
            }
            else
            {
                var task = Task.Run(() =>
                {
                    Bitmap = _picture.Bitmap;
                });
            }
        }
        private bool CanTriangleDetect(object ob)
        {
            return _picture != null;
        }

    }
}
