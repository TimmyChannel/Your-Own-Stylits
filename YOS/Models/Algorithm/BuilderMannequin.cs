//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using YOS.Models.EnumParams;
//using YOS.Models.Items;
//using YOS.Models.Mannequin;


//namespace YOS.Models.Algorithm
//{
//    public class BuilderMannequin : IBuilderMannequin
//    {
//        private MannequinSettings result;

//        public BuilderMannequin() => Reset();

//        public MannequinSettings GetResult() => result;

//        public void Reset() => result = new(GenderTypes.Male);

//        public void SetAccessories() => result.AddClosetItem(ClosetItemList.GetItem("AviatorGlasses"));

//        public void SetBottom() => result.AddClosetItem(ClosetItemList.GetItem("AviatorGlasses"));

//        public void SetHeadWear() => result.AddClosetItem();

//        public void SetShoes() => result.AddClosetItem(ClosetItemList.GetItem("AviatorGlasses"));

//        public void SetTop()=>result.AddClosetItem(ClosetItemList.GetItem("Shirt"));
//    }
//}
