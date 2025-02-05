using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon_TextRPG_Solution
{
    enum ItemType
    {
        Weapon,
        Armor
    }
    internal class Item
    {
        public string Name { get;}
        public string Description { get;}
        public ItemType Type { get;}
        public int Value {  get;}
        public int Price {  get;}
        public bool isPurchase { get; set; }
        public bool isEquip {  get; set; }

        public Item(string name, ItemType type, int value, string description,   int price) 
        {
            Name = name;
            Description = description;
            Type = type;
            Value = value;
            Price = price;
            isPurchase = false;
            isEquip = false;
        }

        public string ItemDisPlay()
        {
            string str = isEquip ? "[E]" : "";
            str += $"{Name} | {GetTypeString()} | {Description}";
            return str;
        }

        public string GetTypeString()
        {
            string str = Type == 0 ? $"공격력 +{Value}" : $"방어력 +{Value}";
            return str;
        }

        public string GetPriceString()
        {
            string str = isPurchase == true ? "구매완료" : $"{Price}";
            return str;
        }
    }
}
