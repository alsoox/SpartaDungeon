using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace SpartaDungeon_TextRPG
{
    //게임시작화면
    public class MainScene
    {
        Player player; 
        static ItemData itemData = new ItemData();    //아이템 목록 생성
        List<Item> itemList = itemData.GetItemList(); 

        public MainScene(Player player)
        {
            this.player = player;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine();
                ActInputConsole();

                int select = int.Parse(Console.ReadLine());

                if (select == 1) 
                {
                    PlayerInfoScene();           // 상태 보기 화면 실행
                }

                else if (select == 2) 
                {
                    InventoryScene();            // 인벤토리 창 보기
                }

                else if (select == 3)
                {
                    StoreScene();                // 상점 창 보기
                } 
                else                 
                {
                    WorngInput();                // 잘못 입력시
                }
            }
        }

        // 상태 보기 화면 실행 메서드
        public void PlayerInfoScene()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Lv {player.Level}");
                Console.WriteLine($"{player.Name} ( {player.Job} )");
                Console.WriteLine($"공격력 : {player.AttackPower}");
                Console.WriteLine($"방어력 : {player.DefensePower}");
                Console.WriteLine($"체력 : {player.Health}");
                Console.WriteLine($"Gold : {player.Gold}");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                ActInputConsole();  

                int select = int.Parse(Console.ReadLine());

                if (select == 0)
                {
                    return;       // 이전 화면 다시 돌아가기
                }
                else
                {
                    WorngInput();
                }
            }
        }

        // 상점 창 보기 메서드
        public void StoreScene()
        {
            while (true)
            {
                ItemMenuConsole();
                foreach (Item item in itemList)
                {
                    item.DisplayeItemBuy();
                }
                
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                ActInputConsole();

                int select = int.Parse(Console.ReadLine());

                if (select == 0)
                {
                    return;
                }
                else if (select == 1)
                {
                    ItemBuy();
                }
                else
                {
                    WorngInput();
                }
            }
        }

        // 인벤토리 창 보기 메서드
        public void InventoryScene()
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine("[아이템 목록]");

                if (player.Inventory.Count == 0) // 인벤토리에 없을 때
                {
                    Console.WriteLine("보유한 아이템이 없습니다.");
                }

                else  //인벤토리 목록 보여주기
                {
                    for (int i = 0; i < player.Inventory.Count; i++)
                    {
                        player.Inventory[i].DisplayeItemEquip();
                    }
                }

                Console.WriteLine();
                Console.WriteLine("1. 장착관리");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                ActInputConsole();

                int selcet = int.Parse(Console.ReadLine());

                if (selcet == 0)
                {
                    return;  // 나가기
                }
                else if (selcet == 1)
                {
                    ItemEquip();  //장비 장착 화면 실행
                }
                else
                {
                    WorngInput();
                }           
            }
        }

        //장비 장착 화면 실행 메서드
        public void ItemEquip()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("[아이템 목록]");

                if (player.Inventory.Count == 0) // 인벤토리에 없을 때
                {
                    Console.WriteLine("보유한 아이템이 없습니다.");
                }

                else
                {
                    for (int i = 0; i < player.Inventory.Count; i++) // 인벤토리에서 장착된 아이템 표시
                    {
                        player.Inventory[i].DisplayeItemEquipIn(i);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine("장착할 아이템 번호를 입력하세요.");
                Console.Write(">> ");
                int select = int.Parse(Console.ReadLine());
                
                if (select == 0)
                {
                    return; //나가기
                }

                if (select < 1 || select > player.Inventory.Count)
                {
                    WorngInput();
                }

                //선택한 아이템 인덱스 확인
                Item selectedItem = player.Inventory[select - 1];

                if (selectedItem.IsEquip) //장비 해제 - 플레이어 스텟 변경 및 착용 여부 변경
                {
                    selectedItem.IsEquip = false;
                    player.EquipItem.Remove(selectedItem);
                    player.AttackPower -= selectedItem.AttackPower;
                    player.DefensePower -= selectedItem.DefensePower;
                    Console.WriteLine($"{selectedItem.Name}을(를) 해제했습니다!");
                }

                else  //장비 장착 - 플레이어 스텟 변경 및 착용 여부 변경
                {

                    selectedItem.IsEquip = true;
                    player.EquipItem.Add(selectedItem);
                    player.AttackPower += selectedItem.AttackPower;
                    player.DefensePower += selectedItem.DefensePower;
                    Console.WriteLine($"{selectedItem.Name}을(를) 장착했습니다!");
                }
                Console.ReadLine();
            }

        }

        //아이템을 산다고 했을때 구매에 따른 메서드
        public void ItemBuy()
        {
            ItemMenuConsole();
            for (int i = 0; i < itemList.Count; i++)   //상점 창 아이템 보여주기
            {
                itemList[i].DisplayeItemBuyIn(i);  
            }
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("구매를 원하는 아이템 번호를 적으세요.");
            Console.Write(">> ");
            int select = int.Parse(Console.ReadLine());

            if(select == 0) return; //나가기

            if (select < 1 || select > itemList.Count)  // 잘못된 입력 방지
            {
                WorngInput();
                return;
            }

            Item selectedItem = itemList[select - 1];  // 선택한 아이템 가져오기

            if (selectedItem.IsPurchase)  // 이미 구매한 경우
            {
                Console.WriteLine("이미 구매한 아이템입니다.");
            }
            else if (player.Gold < selectedItem.Price)  // 골드 부족
            {
                Console.WriteLine("골드가 부족합니다.");
            }
            else
            {
                player.Gold -= selectedItem.Price; // 플레이어 골드 차감
                selectedItem.IsPurchase = true;
                player.Inventory.Add(selectedItem);  // 인벤토리에 추가

                Console.WriteLine($"{selectedItem.Name}을(를) 구매했습니다!");
            }
            Console.ReadLine();
        }

        //아이템 인벤토리 리스트 가져오는 메서드
        public List<Item> Iventory()
        {
            List<Item> Inventory = new List<Item>();
            foreach (Item item in itemList)
            {
                if (item.IsPurchase)  //구매한 목록 확인 후 리스트 추가
                {
                    Inventory.Add(item);
                }
            }
            return Inventory;
        }

        //아이템 상점 메뉴 Console
        public void ItemMenuConsole()
        {
            Console.Clear();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold}G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
        }

        //행동 입력을 명령하는 Console
        public void ActInputConsole()
        {
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
        }


        // 잘못 입력시
        public void WorngInput()
        {
            Console.WriteLine("잘못된 입력입니다.");
            Console.ReadLine();
        }
    }

    //아이템 클래스
    public class Item
    {
        public string Name { get; }
        public string Description { get; set; }
        public int Type { get; }  // 방어구 : 0 / 무기 :1 
        public int DefensePower { get; }
        public int AttackPower { get; }
        public int Price { get; set; }
        public bool IsPurchase {  get; set; }
        public bool IsEquip {  get; set; }

        //아이템 생성자 
        public Item(string name, string description, int type, int defensePower, int attackPower, int price, bool isEquip)
        {
            Name = name;
            Type = type;
            DefensePower = defensePower;
            AttackPower = attackPower;
            Description = description;
            IsEquip = false;
            Price = price;
            IsPurchase = false;
        }

        //아이템 구매창표시 (구매한 아이템 - 구매완료)
        public void DisplayeItemBuy()
        {
            if (Type == 0)
            {
                if (IsPurchase == true)
                {
                    Console.WriteLine($"- {Name} | 방어력 +{DefensePower} | {Description} | 구매완료");
                }
                else Console.WriteLine($"- {Name} | 방어력 +{DefensePower} | {Description} | {Price} G");
            }
            else
            {
                if (IsPurchase == true)
                {
                    Console.WriteLine($"- {Name} | 공격력 +{AttackPower} | {Description} | 구매완료");
                }
                else Console.WriteLine($"- {Name} | 공격력 +{AttackPower} | {Description} | {Price} G");
            }
            
        }

        //아이템 구매한다고 들어갔을때 표시 (구매한 아이템 - 구매완료, 미구매 아이템 - 구매번호)
        public void DisplayeItemBuyIn(int index)
        {
            if (Type == 0)
            {
                if (IsPurchase == true)
                {
                    Console.WriteLine($"- {Name} | 방어력 +{DefensePower} | {Description} | 구매완료");
                }
                else Console.WriteLine($"{index + 1} {Name} | 방어력 +{DefensePower} | {Description} | {Price} G");
            }
            else
            {
                if (IsPurchase == true)
                {
                    Console.WriteLine($"- {Name} | 공격력 +{AttackPower} | {Description} | 구매완료");
                }
                else Console.WriteLine($"{index + 1} {Name} | 공격력 +{AttackPower} | {Description} | {Price} G");
            }
        }

        //아이템 선택창에서 보여주는 표시 (장착한 아이템 - [E])
        public void DisplayeItemEquip()
        {
            if (Type == 0)
            {
                if (IsEquip == true) // 장착시 [E] 표시
                {
                    Console.WriteLine($"- [E]{Name} | 방여력 +{DefensePower} | {Description}");
                }
                else Console.WriteLine($"- {Name} | 방여력+{DefensePower} | {Description}");
            }
            else
            {
                if (IsEquip == true) // 장착시 [E] 표시
                {
                    Console.WriteLine($"- [E]{Name} | 공격력 +{AttackPower} | {Description}");
                }
                else Console.WriteLine($"- {Name} | 공격력 +{AttackPower} | {Description}");
            }
        }

        //선택한다고 들어갔을때 보여주는 표시(장착한 아이템 - [E], 미장착 아이템 - 구매번호)
        public void DisplayeItemEquipIn(int index)
        {
            if (Type == 0)
            {
                if (IsEquip == true) // 장착시 [E] 표시
                {
                    Console.WriteLine($"{index + 1} [E]{Name} | 방어력 +{DefensePower} | {Description}");
                }
                else Console.WriteLine($"{index + 1} {Name} | 방어력 +{DefensePower} | {Description}");
            }
            else
            {
                if (IsEquip == true) // 장착시 [E] 표시
                {
                    Console.WriteLine($"{index + 1} [E]{Name} | 공격력 +{AttackPower} | {Description}");
                }
                else Console.WriteLine($"{index + 1} {Name} | 공격력 +{AttackPower} | {Description}");
            }
        }
    }


    //아이템 정보관련 및 리스트화
    public class ItemData
    {
        private List<Item> items;

        public ItemData()
        {
            items = new List<Item>
            {
                new Item("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", 0, 5, 0, 1000, false),
                new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 9, 0, 2000, false),
                new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 0, 15, 0, 3500, false),
                new Item("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.   ", 1, 0, 2, 600, false),
                new Item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다. ", 1, 0, 5, 1500, false),
                new Item("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 1, 0, 7, 2000, false)
            };
        }
        //기존 리스트 반환
        public List<Item> GetItemList()
        {
            return items;
        }

    }


    //플레이어 클래스
    public class Player
    {
        public string Name { get; }
        public string Job { get;}
        public int Level { get; set; }
        public int AttackPower { get; set; }
        public int DefensePower { get; set; }
        public int Health { get; set; }
        public int Gold { get; set; }

        //인벤토리 및 장착 아이템 리스트
        public List<Item> Inventory { get; set; }
        public List<Item> EquipItem { get; set; }

        //플레이어 생성자
        public Player()
        {
            Name = "Chad";
            Job = "전사";
            Level = 1;
            AttackPower = 10;
            DefensePower = 5;
            Health = 100;
            Gold = 5000;
            Inventory = new List<Item>();
            EquipItem = new List<Item>();
        }
    }

    //실행 프로그램
    class Program
    {
        static Player player = new Player();

        static void Main(string[] args)
        {
            MainScene gameStartScene = new MainScene(player);
        } 
    }
}
