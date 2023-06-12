using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;
using YOS.Models.Items;
using YOS.Models.Mannequin;

namespace YOS.Models.Algorithm
{
    class SelectClozzes : AlgorytmForWear
    {
        Random random = new();
        private MannequinSettings _mannequinSettings = MannequinSettings.Instance;


        public void WearMe(GenderTypes _g, Weather _w, Styles _s)
        {
            List<Item> Wordrobe = ClosetItemList.SelectItems(_g, _w, _s);
            List<Item> Top = new();
            List<Item> Bottom = new();
            List<Item> Shoes = new();
            List<Item> Accessory = new();

            _mannequinSettings.ResetItems();

            foreach (var itm in Wordrobe)
            {
                if (itm.Type == Type.Top)
                {
                    Top.Add(itm);
                }
                if (itm.Type == Type.Accessories)
                {
                    Accessory.Add(itm);
                }
                if (itm.Type == Type.Shoes)
                {
                    Shoes.Add(itm);
                }
                if (itm.Type == Type.Bottom)
                {
                    Bottom.Add(itm);
                }
            }

            if (Top.Count > 0)
                _mannequinSettings.AddClosetItem(Top[random.Next(Top.Count)]);
            if (Accessory.Count > 0)
                _mannequinSettings.AddClosetItem(Accessory[random.Next(Accessory.Count)]);
            if (Bottom.Count > 0)
                _mannequinSettings.AddClosetItem(Bottom[random.Next(Bottom.Count)]);
            if (Shoes.Count > 0)
                _mannequinSettings.AddClosetItem(Shoes[random.Next(Shoes.Count)]);

        }
    }
}
