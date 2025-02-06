using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon_TextRPG_Solution
{
    enum JobType
    {
        Warrior,
        assassin
    }

    internal class Player
    {
        public int Level {  get; set; }
        public string Name {  get; }
        public JobType Job {  get;}
        public int EquipAtk {  get; set; }
        public int EquipDef { get; set; }
        public int Atk {  get; set; }
        public int Def { get; set; }
        public int Hp { get; set; }
        public int MaxHp {  get;}
        public int Gold {  get; set; }

        public Player(int level, string name, int atk, int def, int maxHp, int gold ) 
        {
            Level = level;
            Name = name;
            Job = JobType.Warrior; 
            Atk = atk;
            Def = def;
            EquipAtk = 0;
            EquipDef = 0;
            Hp = maxHp;
            Gold = gold;
        }

        public void StatusDisplay()
        {
            Console.WriteLine($"Lv. {Level.ToString("00")}");
            Console.WriteLine($"{Name} ({Job})");
            string str = EquipAtk == 0 ? $"공격력 : {Atk}" : $"공격력 : {Atk} + ({EquipAtk})";
            Console.WriteLine(str);
            str = EquipDef == 0 ? $"공격력 : {Def}" : $"방어력 : {Def + EquipDef} + ({EquipDef})";
            Console.WriteLine(str);
            Console.WriteLine($"체 력 :  {Hp}");
            Console.WriteLine($"Gold : {Gold}");
        }

        public void EquipItem(Item item)
        {
            if (item.isEquip)
            {
                UnEquip(item);
            }
            else
            {
                item.isEquip = true;

                if (item.Type == ItemType.Weapon)
                {
                    EquipAtk += item.Value;
                }
                else
                {
                    EquipDef += item.Value;
                }
            }
            
        }

        public void UnEquip(Item item)
        {
            item.isEquip = false;

            if (item.Type == ItemType.Weapon)
                EquipAtk -= item.Value;
            else
                EquipDef -= item.Value;

        }
    }
}
