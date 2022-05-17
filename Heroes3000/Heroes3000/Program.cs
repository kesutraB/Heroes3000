using System;
using System.Collections.Generic;
using Heroes3000.Models;
using Heroes3000.Constants;

namespace Heroes3000
{
	internal class Program
	{
		static void Main(string[] args)
		{
			List<Attack> physicalAttacks = new List<Attack>();
			List<Attack> magicalAttacks = new List<Attack>();
			List<Defense> physicalDefenses = new List<Defense>();
			List<Defense> magicalDefenses = new List<Defense>();
			List<Hero> fighters = new List<Hero>();

			physicalAttacks.Add(new Attack("Right Hook", ActionTypes.Physical, 10, 1000));
			physicalAttacks.Add(new Attack("Kick In The Nuts", ActionTypes.Physical, 20, 1000));
			physicalAttacks.Add(new Attack("Punch In The Guts", ActionTypes.Physical, 50, 1000));

			magicalAttacks.Add(new Attack("Lightning Strike", ActionTypes.Magical, 20, 2000));
			magicalAttacks.Add(new Attack("Fireball", ActionTypes.Magical, 30, 2000));
			magicalAttacks.Add(new Attack("Meteor Shower", ActionTypes.Magical, 60, 2000));

			physicalDefenses.Add(new Defense("Shield", ActionTypes.Physical, 1000));
			physicalDefenses.Add(new Defense("Body Armor", ActionTypes.Physical, 1000));

			magicalDefenses.Add(new Defense("Force Shield", ActionTypes.Magical, 2000));
			magicalDefenses.Add(new Defense("Enchanted Netherite Armor", ActionTypes.Magical, 2000));

			fighters.Add(new Hero("Mirek", FighterClass.Fighter, 100, physicalAttacks[0], magicalAttacks[0], physicalDefenses[0], magicalDefenses[0]));
			fighters.Add(new Hero("Eidam", FighterClass.Wizard, 120, physicalAttacks[1], magicalAttacks[1], physicalDefenses[0], magicalDefenses[1]));
			fighters.Add(new Hero("Honza", FighterClass.Fighter, 150, physicalAttacks[2], magicalAttacks[2], physicalDefenses[1], magicalDefenses[0]));
			fighters.Add(new Hero("Adam", FighterClass.Wizard, 75, physicalAttacks[1], magicalAttacks[2], physicalDefenses[1], magicalDefenses[1]));
			fighters.Add(new Hero("Matěj", FighterClass.Fighter, 145, physicalAttacks[2], magicalAttacks[1], physicalDefenses[0], magicalDefenses[0]));
			fighters.Add(new Hero("Regr", FighterClass.Wizard, 500, physicalAttacks[0], magicalAttacks[2], physicalDefenses[1], magicalDefenses[1]));

			Random rnd = new Random(Convert.ToInt32(DateTime.Now.Second));
			int attacker = 0;
			int defender = 0;

			while (attacker == defender)
			{
				attacker = PickAFighter(rnd, fighters);
				defender = PickAFighter(rnd, fighters);
			}

			PrintOpponents(fighters, attacker, defender);

			while (!(fighters[attacker].HealthState == FighterHealthState.Dead || fighters[defender].HealthState == FighterHealthState.Dead))
			{
				fighters[attacker].Match(fighters[defender]);
				fighters[defender].Match(fighters[attacker]);
			}
		}
		private static void PrintOpponents(List<Hero> fighters, int attacker, int defender)
		{
			Console.Write($"{fighters[attacker].FighterName} vs {fighters[defender].FighterName}\n\n");
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("currently attacking\n");
			Console.ForegroundColor = ConsoleColor.DarkMagenta;
			Console.Write("currently defending\n\n");
			Console.ResetColor();
		}
		private static int PickAFighter(Random rnd, List<Hero> fighters)
		{
			return rnd.Next(0, fighters.Count);
		}
	}
}
