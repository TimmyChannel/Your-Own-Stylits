using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using YOS.Models.EnumParams;
using YOS.Models.Mannequin;

namespace YOS.ViewModels
{
    class LookControllViewModel
    {
        private ICommand _generateLook;
        public ICommand GenerateLook => _generateLook ??= new RelayCommand(OnLookButtonPressed);
        private void OnLookButtonPressed()
        {
        //    if (_currentModelMaterial != null)
        //    {
        //        if (!Materials.Contains(SelectedMaterial))
        //            SelectedMaterial = Materials[0];
        //        _mannequinSettings.SetMaterialToSelectedItem(SelectedMaterial, CurrentModelMaterial.AlbedoColor);
        //        Debug.WriteLine("Material was applied");
        //    }
        }
    }
}
