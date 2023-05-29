using HelixToolkit.Wpf.SharpDX;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;

namespace YOS.Models.Mannequin
{
    public class RealisticModel : IMannequinModel
    {
        public string Name { get; init; }
        bool _isVisible;
        Poses _pose;
        GenderTypes _gender;
        private PBRMaterial _defaultMaterial;
        private Geometry3D _geometry;
        public bool IsVisible
        {
            get => _isVisible;
            set => _isVisible = value;
        }
        public Poses Pose
        {
            get => _pose;
            set => _pose = value;
        }
        public GenderTypes Gender
        {
            get => _gender;
            set => _gender = value;
        }
        public PBRMaterial DefaultMaterial
        {
            get => _defaultMaterial;
            private set => _defaultMaterial = value;
        }
        public Geometry3D Geometry
        {
            get => _geometry;
            set => _geometry = value;
        }
        public RealisticModel(string name, bool visible, GenderTypes gender = GenderTypes.Male, Poses pose = Poses.A)
        {
            Name = name;
            Gender = gender;
            Pose = pose;
            IsVisible = visible;
        }
    }
}
