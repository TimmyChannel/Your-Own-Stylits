using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.Mannequin;
using YOS.Models.Items;
using YOS.Models;
using HelixToolkit.Wpf.SharpDX;
using System.Windows.Media;
using SharpDX;
using System.Diagnostics;
using System.Drawing.Printing;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using YOS.Models.EnumParams;
namespace YOS.ViewModels
{
    public class ModelColorControlViewModel : OnPropertyChangedClass
    {
        private IClosetItemModel _closetItemModel;
        private MannequinSettings _mannequinSettings = MannequinSettings.Instance;
        private MeshGeometry3D _currentModelMesh;
        private PBRMaterial _currentModelMaterial;
        private SolidColorBrush _oldBrush = new SolidColorBrush(Colors.White);
        private SolidColorBrush _colorBrush = new SolidColorBrush(Colors.Orange);
        private System.Windows.Media.Color _selectedColor = Colors.Orange;
        private Materials _selectedmaterial;

        #region Props

        public Materials SelectedMaterial
        {
            get => _selectedmaterial;
            set
            {
                _closetItemModel.SetMaterial(value);
                _selectedmaterial = value;
                OnPropertyChanged(nameof(CurrentModelMaterial));
                Debug.WriteLine("Model material was changed");
            }
        }
        private List<Materials> _materials;
        public List<Materials> Materials
        {
            get
            {
                return _materials;
            }
            private set
            {
                SetProperty(ref _materials, value);
            }
        }
        public MeshGeometry3D CurrentModelMesh
        {
            get => _currentModelMesh;
        }
        public PBRMaterial CurrentModelMaterial
        {
            get => _currentModelMaterial;
        }
        public SolidColorBrush ColorBrush
        {
            get => _colorBrush;
            set
            {
                OldColorBrush = _colorBrush;
                _colorBrush = value;
                SelectedColor = value.Color;
                var color = value.Color.ToColor4();
                _closetItemModel?.SetColor(color);
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentModelMaterial));
                Debug.WriteLine("Model color was changed");
            }
        }
        public SolidColorBrush OldColorBrush
        {
            get => _oldBrush;
            set
            {
                _oldBrush = value;
                OnPropertyChanged();
            }
        }
        public System.Windows.Media.Color SelectedColor
        {
            get => _selectedColor;
            set
            {
                _selectedColor = value;
                OnPropertyChanged();
            }
        }
        public Camera Camera { get; init; }
        #endregion
        public ModelColorControlViewModel()
        {
            _materials = new List<Materials>();
            _mannequinSettings.PropertyChanged += _mannequinSettings_PropertyChanged;
            Camera = new PerspectiveCamera()
            {
                LookDirection = new System.Windows.Media.Media3D.Vector3D(0, 0, -108),
                Position = new System.Windows.Media.Media3D.Point3D(-1, 90, 119)
            };
        }

        private ICommand _applyMaterial;
        public ICommand ApplyMaterial => _applyMaterial ??= new RelayCommand(OnApplyMaterial);
        private void OnApplyMaterial()
        {
            if (_currentModelMaterial != null)
            {
                if (!Materials.Contains(SelectedMaterial))
                    SelectedMaterial = Materials[0];
                _mannequinSettings.SetMaterialToSelectedItem(SelectedMaterial, CurrentModelMaterial.AlbedoColor);
                Debug.WriteLine("Material was applied");
            }
        }
        private void _mannequinSettings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(MannequinSettings.Instance.SelectedItem))
                return;
            _closetItemModel = (IClosetItemModel)MannequinSettings.Instance.SelectedItem.Clone();
            Materials = _closetItemModel.AvaliableMaterials;
            _currentModelMesh = (MeshGeometry3D)_closetItemModel.Geometry;
            _currentModelMaterial = _closetItemModel.Material;
            _currentModelMaterial.EmissiveColor = new Color4(0);
            _colorBrush.Color = _currentModelMaterial.AlbedoColor.ToColor();
            var oldPos = Camera.Position;
            var itemModelPoints = _closetItemModel.Geometry.Positions;
            var maxPointOfCurrentModel = itemModelPoints.MaxBy(x => x.Y);
            var camLookPoint = new System.Windows.Media.Media3D.Point3D(maxPointOfCurrentModel.X, maxPointOfCurrentModel.Y, maxPointOfCurrentModel.Z);
            Camera.LookAt(camLookPoint, 100);
            Debug.WriteLine("Model was load to preview");
            OnAllPropertyChanged();
            //OnPropertyChanged(nameof(CurrentModelMesh));
            //OnPropertyChanged(nameof(CurrentModelMaterial));
            //OnPropertyChanged(nameof(SelectedColor));
            //OnPropertyChanged(nameof(SelectedMaterial));
            //OnPropertyChanged(nameof(OldColorBrush));
            //OnPropertyChanged(nameof(Camera));
            //OnPropertyChanged(nameof(ColorBrush));
        }
    }
}
