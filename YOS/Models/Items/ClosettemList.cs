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
    { "Tshirt",
                new("Tshirt", GenderTypes.Unisex, Type.Top, Styles.Casual | Styles.Summer, 
                    Weather.Sunny | Weather.Cloudy | Weather.Indoor, "Футболка") },
    { "Blouse", 
                new("Blouse", GenderTypes.Female, Type.Top, Styles.Casual | Styles.Summer | Styles.Formal, 
                    Weather.Sunny | Weather.Cloudy | Weather.Indoor, "Блузка") },
    { "Shirt", 
                new("Shirt", GenderTypes.Male, Type.Top, Styles.Casual, 
                    Weather.Sunny | Weather.Cloudy | Weather.Indoor, "Рубашка") },
    { "RoundGlasses", 
                new("RoundGlasses", GenderTypes.Unisex, Type.Accessories, Styles.Casual | Styles.Formal, 
                    Weather.Sunny | Weather.Cloudy | Weather.Indoor, "Круглые очки") },
    { "WomenGlasses", 
                new("WomenGlasses", GenderTypes.Female, Type.Accessories, Styles.Casual | Styles.Formal, 
                    Weather.Sunny | Weather.Cloudy | Weather.Indoor, "Женские очки") },
    { "AviatorGlasses", 
                new("AviatorGlasses", GenderTypes.Male, Type.Accessories, Styles.Casual | Styles.Summer, 
                    Weather.Sunny | Weather.Cloudy | Weather.Indoor, "Очки-авиаторы") },
    { "OxfordShoes", 
                new("OxfordShoes", GenderTypes.Male, Type.Shoes, Styles.Formal | Styles.Casual, 
                    Weather.Sunny | Weather.Cloudy | Weather.Indoor, "Оксфордские подкрадули") },
    { "Sneakers", 
                new("Sneakers", GenderTypes.Unisex, Type.Shoes, Styles.Casual | Styles.Summer,
                    Weather.Sunny | Weather.Cloudy | Weather.Indoor, "Сникерсы, сникерсы") },
    { "HeeledShoes",
                new("HeeledShoes", GenderTypes.Female, Type.Shoes, Styles.Formal | Styles.Summer | Styles.Casual,
                    Weather.Sunny | Weather.Cloudy |Weather.Indoor, "Туфли с кирилом") },
    { "Shorts", 
                new("Shorts", GenderTypes.Male, Type.Bottom, Styles.Casual | Styles.Summer,
                    Weather.Sunny | Weather.Cloudy | Weather.Indoor, "Шорты") },
    { "Skirt", 
                new("Skirt", GenderTypes.Female, Type.Bottom, Styles.Summer | Styles.Formal | Styles.Casual,
                    Weather.Sunny | Weather.Cloudy | Weather.Indoor, "Юбка") },
    { "Trousers",
                new("Trousers", GenderTypes.Unisex, Type.Bottom, Styles.Casual | Styles.Formal, 
                    Weather.Sunny | Weather.Cloudy | Weather.Indoor, "Брюки") },
    { "Beret", 
                new("Beret", GenderTypes.Female, Type.Headwear, Styles.Casual | Styles.Formal,
                    Weather.Sunny | Weather.Cloudy | Weather.Indoor, "Кандибобер") },
    { "Kippah", 
                new("Kippah", GenderTypes.Male, Type.Headwear, Styles.Formal,
                    Weather.Sunny | Weather.Cloudy | Weather.Indoor, "Арабская шапка") },
    { "KnitHat", 
                new("KnitHat", GenderTypes.Unisex, Type.Headwear, Styles.Casual, 
                    Weather.Cloudy, "Шапка")},
    { "BeanieHat", 
                new("BeanieHat", GenderTypes.Unisex, Type.Headwear, Styles.Casual, 
                    Weather.Cloudy, "Шапка зайко-девочки") }
};

        public static List<Item> SelectItems(GenderTypes G, Weather W, Styles S)
        {
            var res = new List<Item>();
            foreach (var Itemon in items)
            {
                if ((((int)Itemon.Value.Gender & (int)G) > 0) || (((int)Itemon.Value.Weather & (int)W) > 0) || (((int)Itemon.Value.Style & (int)S) > 0))
                    res.Add(Itemon.Value);
            }
            return res;
        }
        public static List<string> SelectItems(GenderTypes G, Type T)
        {
            var res = new List<string>() { "Нет" };
            foreach (var Itemon in items)
            {
                if ((((int)Itemon.Value.Gender & (int)G) > 0) && (Itemon.Value.Type == T))
                    res.Add(Itemon.Value.ViewName);
            }
            return res;
        }

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
        public static Item GetItem2(string name)
        {
            foreach (var item in items)
            {
                if (item.Value.ViewName == name) return item.Value;
            }
            throw new ArgumentException($"Item with name {name} not found. Before creating the item, add it to the ClosetItemList");
        }
    }

    public class Item
    {
        public string ViewName { get; init; }
        public string Name { get; init; }
        public GenderTypes Gender { get; init; }
        public Type Type { get; init; }
        public Styles Style { get; init; }
        public Weather Weather { get; init; }
        public Item(string name, GenderTypes gender, Type type, Styles style, Weather weather, string translation)
        {
            Name = name;
            Gender = gender;
            Type = type;
            Style = style;
            Weather = weather;
            ViewName = translation;
        }
    }
}
