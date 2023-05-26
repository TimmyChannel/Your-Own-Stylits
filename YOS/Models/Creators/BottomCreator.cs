using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;
using YOS.Models.Items;

namespace YOS.Models.Creators
{
    public class BottomCreator : CloseItemCreator
    {
        public override IClosetItemModel CreateClosetItem(string name) => new BottomItem(name, Gender, Pose);
    }
}
