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
        public List<Materials> Materials
        {
            get
            {
                if (_closetItemModel == null) return null;
                return _closetItemModel.AvaliableMaterials;
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
                if (CurrentModelMaterial != null)
                    CurrentModelMaterial.AlbedoColor = color;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentModelMaterial));
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
        public ModelColorControlViewModel()
        {
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
                _mannequinSettings.SetMaterialToSelectedItem((PBRMaterial)_currentModelMaterial.Clone());
        }
        private void _mannequinSettings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(MannequinSettings.Instance.SelectedItem)) return;
            _closetItemModel = (IClosetItemModel)MannequinSettings.Instance.SelectedItem.Clone();
            _currentModelMesh = (MeshGeometry3D)_closetItemModel.Geometry;
            _currentModelMaterial = _closetItemModel.Material;
            _currentModelMaterial.EmissiveColor = new Color4(0);
            _colorBrush.Color = _currentModelMaterial.AlbedoColor.ToColor();
            OnAllPropertyChanged();
        }
    }
}
