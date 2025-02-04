using System;
using System.Collections.Generic;

// 플레이어 정보
public class Player
{
    public string Name { get; set; }
    public string Job { get; set; }
    public int Level { get; set; } = 1;
    public int AttackPower { get; set; } = 10;
    public int DefensePower { get; set; } = 5;
    public int Health { get; set; } = 100;
    public int Gold { get; set; } = 1500;
    public List<Item> Inventory { get; private set; } = new List<Item>();
}

// 아이템 정보
public class Item
{
    public string Name { get; }
    public string Description { get; }
    public int AttackBonus { get; }
    public int DefenseBonus { get; }
    public int Price { get; }
    public bool IsEquipped { get; set; }
    public bool IsPurchased { get; set; }

    public Item(string name, string description, int attack, int defense, int price)
    {
        Name = name;
        Description = description;
        AttackBonus = attack;
        DefenseBonus = defense;
        Price = price;
    }
}

// 게임 시작 화면
public class GameStartScene
{
    private Player _player;
    public GameStartScene(Player player)
    {
        _player = player;
        ShowMenu();
    }

    public void ShowMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("0. 게임 종료");
            Console.Write("원하는 행동을 입력해주세요: ");
            string input = Console.ReadLine();

            if (input == "1") new PlayerInfoScene(_player);
            else if (input == "2") new InventoryScene(_player);
            else if (input == "3") new ShopScene(_player);
            else if (input == "0") break;
            else Console.WriteLine("잘못된 입력입니다.");
        }
    }
}

// 상태 보기
public class PlayerInfoScene
{
    public PlayerInfoScene(Player player)
    {
        Console.Clear();
        Console.WriteLine($"Lv. {player.Level}");
        Console.WriteLine($"{player.Name} ({player.Job})");
        Console.WriteLine($"공격력 : {player.AttackPower}");
        Console.WriteLine($"방어력 : {player.DefensePower}");
        Console.WriteLine($"체력 : {player.Health}");
        Console.WriteLine($"Gold : {player.Gold} G");
        Console.WriteLine("0. 나가기");
        Console.ReadLine();
    }
}

// 인벤토리 관리
public class InventoryScene
{
    private Player _player;
    public InventoryScene(Player player)
    {
        _player = player;
        ShowInventory();
    }

    public void ShowInventory()
    {
        Console.Clear();
        Console.WriteLine("[아이템 목록]");
        for (int i = 0; i < _player.Inventory.Count; i++)
        {
            var item = _player.Inventory[i];
            Console.WriteLine($"- {i + 1} {(item.IsEquipped ? "[E]" : "")} {item.Name} | {item.Description}");
        }
        Console.WriteLine("1. 장착 관리");
        Console.WriteLine("0. 나가기");
        string input = Console.ReadLine();
        if (input == "1") ManageEquipment();
    }

    public void ManageEquipment()
    {
        Console.Write("장착/해제할 아이템 번호 입력: ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= _player.Inventory.Count)
        {
            var item = _player.Inventory[choice - 1];
            item.IsEquipped = !item.IsEquipped;
            Console.WriteLine(item.IsEquipped ? "장착 완료" : "장착 해제");
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
        }
        Console.ReadLine();
    }
}

// 상점
public class ShopScene
{
    private Player _player;
    private List<Item> _shopItems;
    public ShopScene(Player player)
    {
        _player = player;
        _shopItems = new List<Item>
        {
            new Item("수련자 갑옷", "방어력 +5", 0, 5, 1000),
            new Item("낡은 검", "공격력 +2", 2, 0, 600)
        };
        ShowShop();
    }

    public void ShowShop()
    {
        Console.Clear();
        Console.WriteLine("[보유 골드] " + _player.Gold + " G");
        for (int i = 0; i < _shopItems.Count; i++)
        {
            var item = _shopItems[i];
            Console.WriteLine($"- {i + 1} {item.Name} | {item.Description} | {(item.IsPurchased ? "구매완료" : item.Price + " G")}");
        }
        Console.WriteLine("1. 아이템 구매");
        Console.WriteLine("0. 나가기");
        string input = Console.ReadLine();
        if (input == "1") BuyItem();
    }

    public void BuyItem()
    {
        Console.Write("구매할 아이템 번호 입력: ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= _shopItems.Count)
        {
            var item = _shopItems[choice - 1];
            if (item.IsPurchased) Console.WriteLine("이미 구매한 아이템입니다.");
            else if (_player.Gold >= item.Price)
            {
                _player.Gold -= item.Price;
                item.IsPurchased = true;
                _player.Inventory.Add(item);
                Console.WriteLine("구매 완료");
            }
            else Console.WriteLine("Gold가 부족합니다.");
        }
        else Console.WriteLine("잘못된 입력입니다.");
        Console.ReadLine();
    }
}

class Program
{
    static void Main()
    {
        Player player = new Player { Name = "Chad", Job = "전사" };
        new GameStartScene(player);
    }
}
