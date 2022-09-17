var spells = new[] {
    new Spell("Magic Missile", 53, ApplyModifiedMissleEffect, 0),
    new Spell("Drain", 73, ApplyDrainEffect, 0),
    new Spell("Shield", 113, ApplyShieldEffect, 6),
    new Spell("Poison", 173, ApplyPoisonEffect, 6),
    new Spell("Recharge", 229, ApplyRechrageEffect, 5),
};

var OP_player = new Player(50, 500, 0, 0);
var OP_monster = new Player(51, 0, 9, 0);

//var OP_player = new Player(10, 250, 0, 0);
//var OP_monster = new Player(14, 0, 8, 0);

var appliedSpells = SimulateFight(OP_player, OP_monster, Enumerable.Empty<(Spell spell, int ttl)>(), true, Array.Empty<Spell>());

foreach (var spell in appliedSpells)
{
    Console.WriteLine(spell.Name);
}

Console.WriteLine($"Part 1: {appliedSpells.Sum(w => w.ManaCost)}");

IEnumerable<Spell>? SimulateFight(Player player, Player monster, IEnumerable<(Spell spell, int ttl)> spellsInEffect, bool isPlayerTurn, Spell[] appliedSpells)
{
    var spellsWithUpdatedTTL = new List<(Spell, int ttl)>();

    foreach (var (spell, ttl) in spellsInEffect)
    {
        (player, monster) = spell.Effect(player, monster);

        if (ttl > 1) spellsWithUpdatedTTL.Add((spell, ttl - 1));
    }

    if (monster.HP <= 0)
        return appliedSpells;

    if (isPlayerTurn)
    {
        var usableSpells = spells.Where(w => !spellsWithUpdatedTTL.Select(ww => ww.Item1).Contains(w) && player.Mana >= w.ManaCost).ToList();

        if (usableSpells.Count == 0) 
            return null;

        List<IEnumerable<Spell>?> results = new();

        foreach (var spell in usableSpells)
        {
            Player newPlayer = player;
            Player newMonster = monster;
            var newSpells = spellsWithUpdatedTTL.ToList();

            if (spell.EffectLength == 0)
            {
                (newPlayer, newMonster) = spell.Effect(player, monster);
            }
            else
            {
                newSpells.Add((spell, spell.EffectLength));
            }

            newPlayer = new Player(newPlayer.HP, newPlayer.Mana - spell.ManaCost, 0, 0);

            var newAppliedSpells = appliedSpells.Append(spell).ToArray();

            if (newMonster.HP <= 0)
                results.Add(newAppliedSpells);
            else
                results.Add(SimulateFight(newPlayer, newMonster, newSpells, false, newAppliedSpells));
        }

        return results.OrderBy(w => w?.Sum(w => w.ManaCost) ?? int.MaxValue).First();
    }
    else
    {
        var newPlayer = new Player(player.HP - Math.Max(1, monster.AttackDamage - player.Armor), player.Mana, 0, 0);

        if (newPlayer.HP <= 0) 
            return null;
        else 
            return SimulateFight(newPlayer, monster, spellsWithUpdatedTTL, true, appliedSpells);
    }
}

static (Player modifiedAttacker, Player modifiedDefender) ApplyNormalAttack(Player attacker, Player defender, int damage)
{
    return (attacker, new Player(defender.HP - damage, defender.Mana, defender.AttackDamage, defender.Armor));
}

static (Player modifiedAttacker, Player modifiedDefender) ApplyModifiedMissleEffect(Player attacker, Player defender)
{
    return ApplyNormalAttack(attacker, defender, 4);
}

static (Player modifiedAttacker, Player modifiedDefender) ApplyDrainEffect(Player attacker, Player defender)
{
    return (
        new Player(attacker.HP + 2, attacker.Mana, attacker.AttackDamage, attacker.Armor), 
        new Player(defender.HP - 2, defender.Mana, defender.AttackDamage, defender.Armor)
        );
}

static (Player modifiedAttacker, Player modifiedDefender) ApplyShieldEffect(Player attacker, Player defender)
{
    return (
        new Player(attacker.HP, attacker.Mana, attacker.AttackDamage, 7),
        defender
        );
}

static (Player modifiedAttacker, Player modifiedDefender) ApplyPoisonEffect(Player attacker, Player defender)
{
    return ApplyNormalAttack(attacker, defender, 3);
}

static (Player modifiedAttacker, Player modifiedDefender) ApplyRechrageEffect(Player attacker, Player defender)
{
    return (
        new Player(attacker.HP, attacker.Mana + 101, attacker.AttackDamage, attacker.Armor),
        defender
        );
}

record Player (int HP, int Mana, int AttackDamage, int Armor);
record Spell(string Name, int ManaCost, Func<Player, Player, (Player, Player)> Effect, int EffectLength);