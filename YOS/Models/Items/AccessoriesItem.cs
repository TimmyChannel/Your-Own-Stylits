using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;
using SharpDX;

namespace YOS.Models.Items
{
    public class AccessoriesItem : ModelItemBase
    {
        public AccessoriesItem(string name, 
            GenderTypes gender = GenderTypes.Male, 
            Poses pose = Poses.Idle, 
            Styles style = Styles.Casual,
            Weather weather = Weather.Indoor) : base(name, gender, pose, style, weather)
        {
        }
    }

}
