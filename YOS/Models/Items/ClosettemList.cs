using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YOS.Models.EnumParams;

namespace YOS.Models.Items
{
    public static class ClosetItemList
    {
        private static Dictionary<string, Item> items = new()
        {
    { "Tshirt", new("Tshirt", GenderTypes.Unisex, Type.Top, Styles.Casual | Styles.Summer, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
    { "Blouse", new("Blouse", GenderTypes.Female, Type.Top, Styles.Casual | Styles.Summer | Styles.Formal, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
    { "Shirt", new("Shirt", GenderTypes.Male, Type.Top, Styles.Casual, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
    { "RoundGlasses", new("RoundGlasses", GenderTypes.Unisex, Type.Accessories, Styles.Casual | Styles.Formal, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
    { "WomenGlasses", new("WomenGlasses", GenderTypes.Female, Type.Accessories, Styles.Casual | Styles.Formal, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
    { "AviatorGlasses", new("AviatorGlasses", GenderTypes.Male, Type.Accessories, Styles.Casual | Styles.Summer, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
    { "OxfordShoes", new("OxfordShoes", GenderTypes.Male, Type.Shoes, Styles.Formal | Styles.Casual, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
    { "Sneakers", new("Sneakers", GenderTypes.Unisex, Type.Shoes, Styles.Casual | Styles.Summer, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
    { "HeeledShoes", new("HeeledShoes", GenderTypes.Female, Type.Shoes, Styles.Formal | Styles.Summer | Styles.Casual, Weather.Sunny | Weather.Cloudy |Weather.Indoor) },
    { "Shorts", new("Shorts", GenderTypes.Male, Type.Bottom, Styles.Casual | Styles.Summer, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
    { "Skirt", new("Skirt", GenderTypes.Female, Type.Bottom, Styles.Summer | Styles.Formal | Styles.Casual, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
    { "Trousers", new("Trousers", GenderTypes.Unisex, Type.Bottom, Styles.Casual | Styles.Formal, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
    { "Beret", new("Beret", GenderTypes.Female, Type.Headwear, Styles.Casual | Styles.Formal, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
    { "Kippah", new("Kippah", GenderTypes.Male, Type.Headwear, Styles.Formal, Weather.Sunny | Weather.Cloudy | Weather.Indoor) },
    { "KnitHat", new("KnitHat", GenderTypes.Unisex, Type.Headwear, Styles.Casual, Weather.Cloudy)},
    { "BeanieHat", new("BeanieHat", GenderTypes.Unisex, Type.Headwear, Styles.Casual, Weather.Cloudy) }
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
