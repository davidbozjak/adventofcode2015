//var examplePlayer = new Player(8, 5, 5);
//var exampleMonster = new Player(12, 7, 2);

//Console.WriteLine($"Test: Player 1 " + (SimulateFight(examplePlayer, exampleMonster) ? "wins" : "loses"));


var weapons = new Equipment?[]
{
    new Equipment(8, 4, 0),
    new Equipment(10, 5, 0),
    new Equipment(25, 6, 0),
    new Equipment(40, 7, 0),
    new Equipment(74, 8, 0)
};

var armors = new Equipment?[]
{
    null,
    new Equipment(13, 0, 1),
    new Equipment(31, 0, 2),
    new Equipment(53, 0, 3),
    new Equipment(75, 0, 4),
    new Equipment(102, 0, 5)
};

var rings = new Equipment?[]
{
    null,
    new Equipment(25, 1, 0),
    new Equipment(50, 2, 0),
    new Equipment(100, 3, 0),
    new Equipment(20, 0, 1),
    new Equipment(40, 0, 2),
    new Equipment(80, 0, 3),
};

var battleLog = new List<(bool playerWin, int cost)>();

foreach (var weapon in weapons)
{
    foreach (var armor in armors)
    {
        foreach (var ring1 in rings)
        {
            foreach (var ring2 in rings)
            {
                if (ring1 != null && ring2 != null && ring1 == ring2) continue;

                var monster = new Player(104, 8, 1);
                var player = new Player(100, 0, 0);

                player.Equip(weapon);
                player.Equip(armor);
                player.Equip(ring1);
                player.Equip(ring2);

                bool playerWin = SimulateFight(player, monster);

                battleLog.Add((playerWin, player.EquipmentCost));
            }
        }
    }
}

Console.WriteLine($"Part 1: {battleLog.Where(w => w.playerWin == true).Min(w => w.cost)}");
Console.WriteLine($"Part 2: {battleLog.Where(w => w.playerWin == false).Max(w => w.cost)}");

static bool SimulateFight(Player player1, Player player2)
{
    var attacker = player1;
    var defender = player2;

    while (attacker.HP > 0)
    {
        defender.GetHit(attacker.AttackDamage);

        (defender, attacker) = (attacker, defender);
    }

    return player1.HP > 0;
}

class Player
{
    public int HP { get; private set; }

    public int AttackDamage { get; private set; }

    public int Armor { get; private set; }

    public int EquipmentCost { get; private set; }

    public Player(int startingHP, int baseDamage, int baseArmor)
    {
        this.HP = startingHP;
        this.AttackDamage = baseDamage;
        this.Armor = baseArmor;
    }

    public void GetHit(int attackStrength)
    {
        int damage = Math.Max(1, attackStrength - this.Armor);
        this.HP -= damage;
    }

    public void Equip(Equipment? equipment)
    {
        if (equipment == null) return;

        this.EquipmentCost += equipment.Cost;
        this.AttackDamage += equipment.Damage;
        this.Armor += equipment.Armor;
    }
}

record Equipment(int Cost, int Damage, int Armor);