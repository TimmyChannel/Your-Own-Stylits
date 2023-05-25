using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelixToolkit.Wpf.SharpDX;

namespace YOS.Models.Settings
{
    public class ViewPortSettings
    {
        private List<GroupElement3D> _lightPresets;
        private byte _indexOfCurrentLightPreset;
        public GroupElement3D CurrentLightPreset
        {
            get => _lightPresets[_indexOfCurrentLightPreset];            
        }

    }
}
