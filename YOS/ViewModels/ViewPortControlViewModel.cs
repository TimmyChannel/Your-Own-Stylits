using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HelixToolkit.Wpf.SharpDX;

namespace YOS.ViewModels
{
    public class ViewPortControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "", bool allProperties = false) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(allProperties ? null : propertyName));
        private string cameraModel;
        private bool SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
        {
            if (object.Equals(backingField, value))
            {
                return false;
            }

            backingField = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }
        public const string Orthographic = "Orthographic Camera";

        public const string Perspective = "Perspective Camera";
        protected OrthographicCamera defaultOrthographicCamera = new OrthographicCamera { Position = new System.Windows.Media.Media3D.Point3D(0, 0, 5), LookDirection = new System.Windows.Media.Media3D.Vector3D(-0, -0, -5), UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0), NearPlaneDistance = 1, FarPlaneDistance = 100 };

        protected PerspectiveCamera defaultPerspectiveCamera = new PerspectiveCamera { Position = new System.Windows.Media.Media3D.Point3D(0, 0, 5), LookDirection = new System.Windows.Media.Media3D.Vector3D(-0, -0, -5), UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0), NearPlaneDistance = 0.5, FarPlaneDistance = 150 };

        private Camera camera;
        public List<string> CameraModelCollection { get; private set; }

        public string CameraModel
        {
            get
            {
                return cameraModel;
            }
            set
            {
                if (SetValue(ref cameraModel, value, "CameraModel"))
                {
                    OnPropertyChanged();
                }
            }
        }

        public Camera Camera
        {
            get
            {
                return camera;
            }

            protected set
            {
                SetValue(ref camera, value, "Camera");
                CameraModel = value is PerspectiveCamera
                                       ? Perspective
                                       : value is OrthographicCamera ? Orthographic : null;
            }
        }
        public Geometry3D Geometry { private set; get; }
        public Material Material { private set; get; }

        public ViewPortControlViewModel()
        {
            // camera models
            CameraModelCollection = new List<string>()
            {
                Orthographic,
                Perspective,
            };

            // on camera changed callback
            PropertyChanged += (s, e) =>
            {
                if (cameraModel == Orthographic)
                {
                    if (!(Camera is OrthographicCamera))
                        Camera = defaultOrthographicCamera;
                }
                else if (cameraModel == Perspective)
                {
                    if (!(Camera is PerspectiveCamera))
                        Camera = defaultPerspectiveCamera;
                }
                else
                {
                    throw new HelixToolkitException("Camera Model Error.");
                }
            };

            // default camera model
            CameraModel = Perspective;
            Camera = new PerspectiveCamera()
            {
                Position = new System.Windows.Media.Media3D.Point3D(0, 0, 10),
                LookDirection = new System.Windows.Media.Media3D.Vector3D(0, 0, -10),
                UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0)
            };
            var reader = new ObjReader();
            //TODO
            var models = reader.Read("C:\\Users\\Аська\\Desktop\\Cursed_Work\\team-1131\\YOS\\Resources\\Tshirt.obj");
            Geometry = models[0].Geometry;
            Material = PhongMaterials.White;

        }
    }
}
