using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelixToolkit.Wpf.SharpDX;
using YOS.Models.Mannequin;

namespace YOS.Models.Settings
{
    public class ViewPortSettings
    {
        private List<GroupElement3D> _lightPresets;
        private byte _indexOfCurrentLightPreset;
        private MannequinSettings _mannequinSettings;
        public GroupElement3D CurrentLightPreset
        {
            get => _lightPresets[_indexOfCurrentLightPreset];            
        }
        public List<GroupElement3D> LightPresetList
        {
            get => _lightPresets;
        }
        public Viewport3DX Viewport { get; init; }
        public EnvironmentMap3D Environment { get; init; }
        public ViewPortSettings()
        {
            
        }
    }
}
