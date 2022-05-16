using System;
using System.Threading;
using Heroes3000.Constants;

namespace Heroes3000.Models
{
	public class Hero
	{

		private const int MinDelay = 150;
		private const int MaxDelay = 500;
		private const int CriticalHitChance = 2;
		private const int MinDefenseChance = 10;
		private const int MaxDefenseChance = 80;
		public string FighterName { get; private set; }
		public FighterClass Class { get; private set; } = FighterClass.None;
		public FighterHealthState HealthState { get; private set; } = FighterHealthState.None;
		public Attack PhysicalAttack { get; private set; }
		public Attack MagicalAttack { get; private set; }
		public Defense PhysicalDefense { get; private set; }
		public Defense MagicalDefense { get; private set; }
		public int CurrentHP { get; private set; }
		public int MaxHP { get; private set; }

		public Hero (string fighterName, FighterClass fighterClass, int maxHp, Attack physicalAttack, Attack magicalAttack, Defense physicalDefense, Defense magicalDefense)
		{
			this.FighterName = fighterName;
			this.Class = fighterClass;
			this.MaxHP = maxHp;
			this.CurrentHP = maxHp;
			this.PhysicalAttack = physicalAttack;
			this.MagicalAttack = magicalAttack;
			this.PhysicalDefense = physicalDefense;
			this.MagicalDefense = magicalDefense;
		}

		public void Match (Hero fighter)
		{
			PrintFighterStatus(this, fighter);

			Random rnd = new Random(Convert.ToInt32(DateTime.Now.Second));

			Attack attack = ReturnAttack(rnd);

			DealDamage(fighter, attack, rnd);

			if(MaybeCriticalHit(rnd))
				CriticalAttackMessage(this, fighter, attack);
			else
				AttackMessage(this, fighter, attack);

			if (MaybeKillFighter(fighter))
				KillFighter(fighter);
			if(MaybeKillFighter(this))
				KillFighter(this);

			if (IsHeroDead(fighter))
			{
				DeathMessage(fighter);
				VictoryMessage(this);
				return;
			}
			else if (IsHeroDead(this))
			{
				Console.ResetColor();
				DeathMessage(this);
				VictoryMessage(fighter);
				return;
			}

			Thread.Sleep(rnd.Next(MinDelay, MaxDelay));
		}

		#region Printing

		private void PrintFighterStatus(Hero attacker, Hero defender)
		{
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine($"{attacker.FighterName} now has {attacker.CurrentHP} / {attacker.MaxHP} HP left.");
			Console.WriteLine($"{defender.FighterName} now has {defender.CurrentHP} / {defender.MaxHP} HP left.");
			Console.ResetColor();
		}
		private void VictoryMessage (Hero fighter)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write($"\n\n{fighter.FighterName} won!\n");
			Console.ResetColor();
		}
		private static void DeathMessage (Hero fighter)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write($"\n{fighter.FighterName} died!\n");
			Console.ResetColor();
		}
		private void AttackMessage(Hero attacker, Hero defender, Attack attack)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine($"{attacker.FighterName} attacked {defender.FighterName} with {attack.AttackName} and decreased {defender.FighterName}'s HP by {attack.AttackDamage}.");
			Console.ResetColor();
		}
		private void CriticalAttackMessage(Hero attacker, Hero fighter2, Attack attack)
		{
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine($"{attacker.FighterName} scored a critical hit!");
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine($"{attacker.FighterName} attacked {fighter2.FighterName} with {attack.AttackName} and decreased {fighter2.FighterName}'s HP by {attack.AttackDamage*3}.");
			Console.ResetColor();
		}
		private void DefenseMessage(Hero attacker, Hero defender, Defense defense)
		{
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine($"{defender.FighterName} protected himself by using {defense.DefenseName} and returned {attacker.FighterName} 35 % of damage.");
		}
		#endregion

		#region Attacking

		private bool MaybeCriticalHit(Random rnd)
		{
			if (ImFeelingLucky(rnd, CriticalHitChance))
				return true;
			return false;
		}
		private void DealDamage(Hero fighter, Attack attack, Random rnd)
		{
			if (MaybeCriticalHit(rnd))
				fighter.CurrentHP -= attack.AttackDamage * 3;
			else
				fighter.CurrentHP -= attack.AttackDamage;
		}
		private Attack ReturnAttack(Random rnd)
		{
			Attack attack = new Attack("", ActionTypes.None, 0, 0);
			if (ImFeelingLucky(rnd, 50))
				attack = this.PhysicalAttack;
			else
				attack = this.MagicalAttack;

			return attack;
		}
		#endregion

		#region Defending

		private bool MaybeDefendYourself(Random rnd)
		{
			if (GetChance(rnd) >= MinDefenseChance && GetChance(rnd) <= MaxDefenseChance)
				return true;
			return false;
		}
		private Defense ReturnDefense(Random rnd)
		{
			Defense defense = new Defense("", ActionTypes.None, 0);
			if (ImFeelingLucky(rnd, 50))
				defense = this.PhysicalDefense;
			else
				defense = this.MagicalDefense;

			return defense;
		}
		#endregion

		#region Helpers

		private void KillFighter(Hero fighter)
		{
			fighter.CurrentHP = 0;
			fighter.HealthState = FighterHealthState.Dead;
		}
		private bool MaybeKillFighter(Hero fighter)
		{
			if (fighter.CurrentHP <= 0)
				return true;

			return false;
		}
		private bool IsHeroDead (Hero fighter)
		{
			if (fighter.HealthState == FighterHealthState.Dead)
				return true;

			return false;
		}
		private static bool ImFeelingLucky(Random rnd, int luckyValue)
		{
			return GetChance(rnd) <= luckyValue;
		}
		private static int GetChance(Random rnd)
		{
			return rnd.Next(0, 100);
		}
		#endregion
	}
}
