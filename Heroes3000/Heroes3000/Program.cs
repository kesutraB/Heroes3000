using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
			List<Hero> fighters = new List<Hero>();

			physicalAttacks.Add(new Attack("Right Hook", Attack.AttackType.Physical, 10, 1000));
			physicalAttacks.Add(new Attack("Kick In The Nuts", Attack.AttackType.Physical, 20, 1000));
			physicalAttacks.Add(new Attack("Punch In The Guts", Attack.AttackType.Physical, 50, 1000));

			magicalAttacks.Add(new Attack("Lightning Strike", Attack.AttackType.Magical, 20, 2000));
			magicalAttacks.Add(new Attack("Fireball", Attack.AttackType.Magical, 30, 2000));
			magicalAttacks.Add(new Attack("Meteor Shower", Attack.AttackType.Magical, 60, 2000));

			fighters.Add(new Hero("Mirek", FighterClass.Fighter, 100, physicalAttacks[0], magicalAttacks[0]));
			fighters.Add(new Hero("Eidam", FighterClass.Wizard, 120, physicalAttacks[1], magicalAttacks[1]));
			fighters.Add(new Hero("Honza", FighterClass.Fighter, 150, physicalAttacks[2], magicalAttacks[2]));
			fighters.Add(new Hero("Adam", FighterClass.Wizard, 75, physicalAttacks[1], magicalAttacks[2]));
			fighters.Add(new Hero("Matěj", FighterClass.Fighter, 145, physicalAttacks[2], magicalAttacks[1]));
			fighters.Add(new Hero("Regr", FighterClass.Wizard, 500, physicalAttacks[0], magicalAttacks[2]));

			Random rnd = new Random(Convert.ToInt32(DateTime.Now.Second));
			int fighter1 = 0;
			int fighter2 = 0;

			while (fighter1 == fighter2)
			{
				fighter1 = PickAFighter(rnd, fighters);
				fighter2 = PickAFighter(rnd, fighters);
			}

			while (!(fighters[fighter1].HealthState == FighterHealthState.Dead || fighters[fighter2].HealthState == FighterHealthState.Dead))
			{
				fighters[fighter1].Match(fighters[fighter2]);
				fighters[fighter2].Match(fighters[fighter1]);
			}
		}
		private static int PickAFighter(Random rnd, List<Hero> fighters)
		{
			return rnd.Next(0, fighters.Count);
		}
	}
}
