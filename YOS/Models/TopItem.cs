using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;

namespace YOS.Models
{
    class TopItem : IClosetItem
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        private Styles _style;
        public Styles Style { get => Style; set => throw new NotImplementedException(); }
        public Weather Weather { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Poses Pose { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Somatotypes Somatotype { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public PBRMaterial DefaultMaterial { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Geometry3D Geometry { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool SomatotypeChangeModel => throw new NotImplementedException();

        public List<PBRMaterial> AvaliableMaterials => throw new NotImplementedException();
 //       public TopItem(string name, Sty)
//        {
            
 //       }
    }
}
