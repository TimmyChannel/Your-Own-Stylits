using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YOS.Models
{
    public class TopCreator : CloseItemCreator
    {
        public override IClosetItem CreateClosetItem(string name) => new TopItem(name, Somatotype, Pose);
    }
}
