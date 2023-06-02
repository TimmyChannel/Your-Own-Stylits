using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.Mannequin;
using YOS.Models.Items;
using YOS.Models;
using HelixToolkit.Wpf.SharpDX;

namespace YOS.ViewModels
{
    public class ModelColorControlViewModel : OnPropertyChangedClass
    {
        private MannequinSettings _mannequinSettings = MannequinSettings.Instance;
        public MeshGeometry3D CurrentModelMesh
        {
            get => (MeshGeometry3D)_mannequinSettings.SelectedItem?.Geometry;          
        }
        public Material CurrentModelMaterial
        {
            get => _mannequinSettings.SelectedItem?.Material;
        }
        public ModelColorControlViewModel()
        {
            _mannequinSettings.PropertyChanged += _mannequinSettings_PropertyChanged;
        }

        private void _mannequinSettings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnAllPropertyChanged();
        }
    }
}
