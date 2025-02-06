using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SpartaDungeon_TextRPG_Solution
{
    internal class GameManager
    {
        Player player;
        List<Item> items;
        List<Item> inventory = new List<Item>();

        public GameManager() 
        {
            player = new Player(1, "Chad" ,10, 5, 100, 3000 );
            //int level, string name, int atk, int def, int maxHp, int gold
            items = new List<Item>()
            //(string name, string description, ItemType type, int value, int price)
            {
                new Item("수련자의 갑옷", ItemType.Armor, 4, "수련에 도움을 주는 갑옷입니다. ", 1000),
            new Item("무쇠갑옷", ItemType.Armor, 9, "무쇠로 만들어져 튼튼한 갑옷입니다. ", 2000),
            new Item("스파르타의 갑옷", ItemType.Armor, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ", 3500),
            new Item("낣은 검", ItemType.Weapon, 5, "쉽게 볼 수 있는 낡은 검 입니다. ", 600),
            new Item("청동 도끼", ItemType.Weapon, 10, "어디선가 사용됐던거 같은 도끼입니다. ", 1500),
            new Item("스파르타의 창", ItemType.Weapon, 20, "스파르타의 전사들이 사용했다는 전설의 창입니다. ", 2500),
            };
        }

        //메인씬
        public void MainScene()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine();


            int input = ConsoleManager.GetInput(1,3);

            switch(input)
            {
                case 1:
                    StatusScene();
                    break;
                case 2:
                    IventoryScene();
                    break;
                case 3:
                    StoreScene();
                    break;
            }
        }

        public void StatusScene()
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine();
            player.StatusDisplay();
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            int input = ConsoleManager.GetInput(0, 0);
            MainScene();
        }

        public void IventoryScene()
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            //아이템목록보여주기

            for (int i = 0; i < inventory.Count; i++)
            {
                Console.WriteLine("- " +inventory[i].ItemDisPlay());
            }

            Console.WriteLine();
            Console.WriteLine("1. 장착관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            int input = ConsoleManager.GetInput(0, 1);
            switch(input)
            {
                case 1:
                    EquipScene();
                    break;
            }

            MainScene();
        }

        public void EquipScene()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 - 장착관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < inventory.Count; i++)
            {
                Console.WriteLine($"- {i+1} {inventory[i].ItemDisPlay()}");
            }

            Console.WriteLine();
            Console.WriteLine("0. 나가기");

            int input = ConsoleManager.GetInput(0, inventory.Count);
            if (input != 0)
            {
                Equip(input);
                
            }

            IventoryScene();

        }

        public void Equip(int input)
        {
            Item select = inventory[input - 1];

            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].isEquip)
                {
                    player.EquipItem(select);
                }
            }
            player.EquipItem(select);
        }

        public void StoreScene()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold}");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            foreach (var item in items)
            {
                Console.WriteLine($"- {item.ItemDisPlay()} | {item.GetPriceString()} " );
            }

            Console.WriteLine();
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            int input = ConsoleManager.GetInput(0, 1);
            switch(input)
            {
                case 1:
                    BuyScene();
                    break;
            }
            MainScene();
        }

        public void BuyScene(bool needGold = false , bool hasItem = false)
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 구매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold}");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            for (int i = 1; i <= items.Count; i++)
            {
                Console.WriteLine($"- {i} {items[i-1].ItemDisPlay()} | {items[i-1].GetPriceString()} ");
            }

            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            if (needGold)
                Console.WriteLine("골드가 부족합니다!!");
            else if (hasItem)
                Console.WriteLine("이미 보유한 아이템입니다!!");

            int input = ConsoleManager.GetInput(0, items.Count);
            switch (input)
            {
                case 0:
                    MainScene(); 
                    break;
                default:
                    Item select = items[input - 1];

                    if(inventory.Contains(select))
                    {
                        BuyScene(false,true);
                    }
                    else
                    {
                        Buy(select);
                    }

                    break;

            }
        }

        public void Buy(Item item)
        {
            if(item.Price > player.Gold)
            {
                BuyScene(true, false);
            }

            else
            {
                player.Gold -= item.Price;
                item.isPurchase = true;
                inventory.Add(item);
                BuyScene();
            }

        }
    }
}
