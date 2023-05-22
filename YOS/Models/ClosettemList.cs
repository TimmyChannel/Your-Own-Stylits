using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;

namespace YOS.Models
{
    public static class ClosetItemList
    {
        private static Dictionary<string, Item> items = new Dictionary<string, Item>
    {
        { "Tshirt", new Item("Tshirt", GenderTypes.Unisex, Type.Top, Styles.Casual, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
        { "Blouse", new Item("Blouse", GenderTypes.Female, Type.Top, Styles.Casual | Styles.Summer, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
        { "Shirt", new Item("Shirt", GenderTypes.Unisex, Type.Top, Styles.Casual, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
        { "RoundGlasses", new Item("RoundGlasses", GenderTypes.Unisex, Type.Accessories, Styles.Casual, Weather.Sunny | Weather.Cloudy) },
        { "WomenGlasses", new Item("WomenGlasses", GenderTypes.Female, Type.Accessories, Styles.Casual, Weather.Sunny | Weather.Cloudy) },
        { "AviatorGlasses", new Item("AviatorGlasses", GenderTypes.Male, Type.Accessories, Styles.Casual, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
        { "OxfordShoes", new Item("OxfordShoes", GenderTypes.Male, Type.Shoes, Styles.Formal, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
        { "Sneakers", new Item("Sneakers", GenderTypes.Unisex, Type.Shoes, Styles.Casual, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
        { "HeeledShoes", new Item("HeeledShoes", GenderTypes.Female, Type.Shoes, Styles.Formal, Weather.Sunny | Weather.Cloudy) }
    };


        public static Item GetItem(string name)
        {
            if (items.ContainsKey(name))
            {
                return items[name];
            }
            else
            {
                throw new ArgumentException($"Item with name {name} not found. Before creating the item, add it to the ClosetItemList");
            }

        }
    }

    public class Item
    {
        public string Name { get; init; }
        public GenderTypes Gender { get; init; }
        public Type Type { get; init; }
        public Styles Style { get; init; }
        public Weather Weather { get; init; }
        public List<PBRMaterial> AvaliableMaterials { get; }

        public Item(string name, GenderTypes gender, Type type, Styles style, Weather weather)
        {
            Name = name;
            Gender = gender;
            Type = type;
            Style = style;
            Weather = weather;
        }
    }
}
