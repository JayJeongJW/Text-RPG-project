using System;
using System.Collections.Generic;

class Item
{
    //프로퍼티
    public string Name { get; set; } // 아이템 이름   
    public string Description { get; set; } // 아이템 설명(방어력, 공격력 + @)
   
    public int Price { get; set; } // 가격
    public int AttackBonus { get; set; } // 공격력 보너스
    public int DefenseBonus { get; set; } // 방어력 보너스
  
    public bool IsPurchased { get; set; } // 구매 여부 변수
    public bool IsEquipped { get; set; } // 장착 여부

    //생성자
    public Item(string name, string description, int price, int attackBonus = 0, int defenseBonus = 0)
    {
        Name = name;
        Description = description;
        Price = price;
        AttackBonus = attackBonus;
        DefenseBonus = defenseBonus;
        IsPurchased = false;
        IsEquipped = false;
    }

    //삼항 연산자
    //purchasedStatus : 구매된 상태 //equippedStatus : 장착된 상태
    public void DisplayItem(int index)
    {
        string purchasedStatus = IsPurchased ? "구매완료" : $"{Price} G";
        string equippedStatus = IsEquipped ? "[E]" : "";
        Console.WriteLine($"{index + 1}. {equippedStatus} {Name} | {Description} | {purchasedStatus}");
    }
}

//프로퍼티
class Character
{
    //이름 레벨 공격력 방어력 체력 골드
    public string Name { get; set; }
  
    public int Level { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Health { get; set; }
    public int Gold { get; set; }
   
    public List<Item> Inventory { get; set; }

    //생성자
    public Character(string name)
    {
        Name = name;
        Level = 1;
        Attack = 10;
        Defense = 5;
        Health = 100;
        Gold = 1500;
        Inventory = new List<Item>();
    }

    // out 키워드 > 장착된 아이템들에 따라 공격력, 방어력 변경
    public void UpdateStats(out string attackDetails, out string defenseDetails)
    {
        Attack = 10;  // 기본 공격력
        Defense = 5;  // 기본 방어력

        attackDetails = ""; // 공격력 보너스 문자열
        defenseDetails = ""; // 방어력 보너스 문자열

        bool hasEquippedItem = false; // 장착된 아이템이 있는지 확인

        //foreach
        foreach (var item in Inventory)
        {
            if (item.IsEquipped) // 장착된 아이템만 처리
            {
                hasEquippedItem = true; // 장착된 아이템이 있음을 확인
                Attack += item.AttackBonus;  // 아이템으로 증가한 공격력
                Defense += item.DefenseBonus; // 아이템으로 증가한 방어력

                // 아이템 이름과 공격력 보너스 문자열 업데이트
                if (item.AttackBonus > 0)
                {
                    if (attackDetails == "")
                        attackDetails = $"{item.Name} +{item.AttackBonus} 공격력";
                    else
                        attackDetails += $", {item.Name} +{item.AttackBonus} 공격력";
                }

                // 아이템 이름과 방어력 보너스 문자열 업데이트
                if (item.DefenseBonus > 0)
                {
                    if (defenseDetails == "")
                        defenseDetails = $"{item.Name} +{item.DefenseBonus} 방어력";
                    else
                        defenseDetails += $", {item.Name} +{item.DefenseBonus} 방어력";
                }
            }
        }

        // 장착된 아이템이 없다면, 공격력/방어력에 대한 보너스 항목을 빈 문자열로 유지
        if (!hasEquippedItem)
        {
            attackDetails = "없음"; // 공격력 보너스가 없을 경우 "없음" 출력
            defenseDetails = "없음"; // 방어력 보너스가 없을 경우 "없음" 출력
        }
    }

    public void DisplayStats()
    {
        // 상태 보기 출력 형식
        UpdateStats(out string attackDetails, out string defenseDetails);

        Console.WriteLine("\n상태 보기");
        Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

        Console.WriteLine($"Lv. {Level:00}");
        Console.WriteLine($"이름: {Name}");
        Console.WriteLine($"직업: 전사");

        // 공격력 출력, 보너스가 없으면 그냥 기본값 출력
        if (attackDetails == "없음")
            Console.WriteLine($"공격력: {Attack}");
        else
            Console.WriteLine($"공격력: {Attack} ({attackDetails})");

        // 방어력 출력, 보너스가 없으면 그냥 기본값 출력
        if (defenseDetails == "없음")
            Console.WriteLine($"방어력: {Defense}");
        else
            Console.WriteLine($"방어력: {Defense} ({defenseDetails})");

        Console.WriteLine($"체력: {Health}");
        Console.WriteLine($"Gold: {Gold} G");
    }

    // 인벤토리: 보유 아이템 출력
    public void DisplayInventory()
    {
        Console.WriteLine("\n인벤토리");
        if (Inventory.Count == 0)
        {
            Console.WriteLine("보유한 아이템이 없습니다.");
        }
        else
        {
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            for (int i = 0; i < Inventory.Count; i++)
            {
                Inventory[i].DisplayItem(i);
            }
        }
    }

    // 장착 관리: 아이템 장착/해제
    public void EquipItem()
    {
        Console.WriteLine("\n[장착 관리]");

        // 장착 가능한 아이템 목록 출력
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i].IsPurchased)
            {
                Inventory[i].DisplayItem(i);
            }
        }

        Console.WriteLine("\n장착할 아이템 번호를 선택하세요");
        Console.WriteLine("0. 돌아가기");

        int itemChoice = int.Parse(Console.ReadLine()) - 1;

        if (itemChoice == -1) return; // 돌아가기

        if (itemChoice >= 0 && itemChoice < Inventory.Count && Inventory[itemChoice].IsPurchased)
        {
            Item selectedItem = Inventory[itemChoice];

            // 장착 여부 확인
            Console.WriteLine($"\n{selectedItem.Name} 장착하시겠습니까? (y/n): ");
            string confirmChoice = Console.ReadLine().ToLower();

            if (confirmChoice == "y")
            {
                if (selectedItem.IsEquipped)
                {
                    selectedItem.IsEquipped = false;
                    Console.WriteLine($"{selectedItem.Name} 장착 해제되었습니다.");
                }
                else
                {
                    selectedItem.IsEquipped = true;
                    Console.WriteLine($"{selectedItem.Name} 장착되었습니다.");
                }
            }
            else if (confirmChoice == "n")
            {
                // 'n'을 눌렀을 때도 장착 해제
                if (selectedItem.IsEquipped)
                {
                    selectedItem.IsEquipped = false;
                    Console.WriteLine($"{selectedItem.Name} 장착 해제되었습니다.");
                }
                else
                {
                    Console.WriteLine($"{selectedItem.Name}는 이미 장착되지 않았습니다.");
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // 캐릭터 생성
        Character character = new Character("Chad");

        // 아이템 예시 생성
        //("string name", "string description", "int price", "int attackBonus", "int defenseBonus")
        Item item1 = new Item("무쇠갑옷", "방어력 +5", 1000, 0, 5);
        Item item2 = new Item("스파르타의 창", "공격력 +7", 1500, 7, 0);
        Item item3 = new Item("낡은 검", "공격력 +2", 600, 2, 0);

        // 인벤토리에 아이템 추가
        character.Inventory.Add(item1);
        character.Inventory.Add(item2);
        character.Inventory.Add(item3);

        // 구매된 아이템으로 설정
        item1.IsPurchased = true;
        item2.IsPurchased = true;
        item3.IsPurchased = true;

        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine("\n1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("\n원하시는 행동을 입력해주세요: ");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    // 상태 보기 창
                    character.DisplayStats();
                    Console.WriteLine("\n0. 나가기");
                    Console.ReadLine();
                    break;
                case 2:
                    // 인벤토리 창
                    character.DisplayInventory();
                    if (character.Inventory.Count > 0)
                    {
                        Console.WriteLine("\n1. 장착 관리");
                    }
                    Console.WriteLine("0. 나가기");
                    int inventoryChoice = int.Parse(Console.ReadLine());

                    if (inventoryChoice == 1)
                    {
                        character.EquipItem();
                    }
                    break;
                case 3:
                    // 상점 기능 창
                    OpenStore(character);
                    break;
                case 0:
                    running = false;
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }
    }

    // 상점 로직만 추가
    static void OpenStore(Character character)
    {
        // 상점에 판매되는 아이템들
        Item storeItem1 = new Item("수련자 갑옷", "방어력 +5", 1000, 0, 5);
        Item storeItem2 = new Item("무쇠갑옷", "방어력 +9", 1000, 0, 9);
        Item storeItem3 = new Item("스파르타의 갑옷", "방어력 +15", 3500, 0, 15);
        Item storeItem4 = new Item("낡은 검", "공격력 +2", 600, 2, 0);
        Item storeItem5 = new Item("청동 도끼", "공격력 +5", 1500, 5, 0);
        Item storeItem6 = new Item("스파르타의 창", "공격력 +7", 1500, 7, 0);

        List<Item> storeItems = new List<Item> { storeItem1, storeItem2, storeItem3, storeItem4, storeItem5, storeItem6 };

        bool storeRunning = true;

        while (storeRunning)
        {
            Console.Clear();
            Console.WriteLine("상점에 오신 것을 환영합니다!");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

            Console.WriteLine($"[보유 골드]\n{character.Gold} G\n");

            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < storeItems.Count; i++)
            {
                // 구매 완료된 아이템에 대해 "구매완료" 상태 표시
                // character.Inventory에서 해당 아이템이 이미 구매되었는지 확인
                string itemStatus = character.Inventory.Exists(item => item.Name == storeItems[i].Name && item.IsPurchased)?"구매완료": $"{storeItems[i].Price} G";

                Console.WriteLine($"- {i + 1}. {storeItems[i].Name} | {storeItems[i].Description} | {itemStatus}");
            }

            Console.WriteLine("\n0. 나가기");
            Console.WriteLine("구매할 아이템 번호를 선택해주세요:");

            int storeChoice = int.Parse(Console.ReadLine());

            if (storeChoice == 0)
            {
                storeRunning = false;
            }
            else if (storeChoice >= 1 && storeChoice <= storeItems.Count)
            {
                Item selectedItem = storeItems[storeChoice - 1];

                // 이미 구매했는지 여부 확인
                if (character.Inventory.Exists(item => item.Name == selectedItem.Name && item.IsPurchased))
                {
                    Console.WriteLine("이미 구매한 아이템입니다.");
                }
                else if (character.Gold >= selectedItem.Price)
                {
                    character.Gold -= selectedItem.Price;
                    selectedItem.IsPurchased = true;
                    character.Inventory.Add(selectedItem); // 인벤토리에 아이템 추가
                    Console.WriteLine($"{selectedItem.Name} 아이템을 구매했습니다!");
                    Console.WriteLine($"남은 Gold: {character.Gold} G");
                }
                else
                {
                    Console.WriteLine("Gold가 부족합니다!");
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }

            Console.WriteLine("\n상점으로 돌아가려면 아무 키나 누르세요...");
            Console.ReadKey();
        }
    }
}
