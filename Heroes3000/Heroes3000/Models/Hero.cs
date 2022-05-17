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
		private const int DefenseChance = 30;
		private const string Blue = "ConsoleColor.Blue";
		private const string DarkYellow = "ConsoleColor.DarkYellow";
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
			Defense defense = ReturnDefense(rnd);

			DealDamage(fighter, attack, rnd);

			if (MaybeDefendYourself(rnd))
			{
				if (MaybeCriticalHit(rnd))
				{
					CriticalAttackMessage(this, fighter, attack);
					DefenseMessage(fighter, defense);
					Console.WriteLine();
				}
				else
				{
					AttackMessage(this, fighter, attack);
					DefenseMessage(fighter, defense);
					Console.WriteLine();
				}
			}
			else if (MaybeCriticalHit(rnd))
			{
				CriticalAttackMessage(this, fighter, attack);
				Console.WriteLine();
			}
			else
			{
				AttackMessage(this, fighter, attack);
				Console.WriteLine();
			}

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
			if (IsHeroDead(this))
			{
				Console.ResetColor();
				DeathMessage(this);
				VictoryMessage(fighter);
				return;
			}

			Thread.Sleep(rnd.Next(MinDelay, MaxDelay));
		}

		

		#region Printing

		private static void PrintFighterStatus(Hero attacker, Hero defender)
		{
			PrintAttackerName(attacker);
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine($" now has {attacker.CurrentHP} / {attacker.MaxHP} HP left.");
			PrintDefenderName(defender);
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine($" now has {defender.CurrentHP} / {defender.MaxHP} HP left.");
			Console.ResetColor();
		}
		private static void VictoryMessage (Hero fighter)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write($"{fighter.FighterName} won!\n\n");
			Console.ResetColor();
		}
		private static void DeathMessage (Hero fighter)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write($"\n{fighter.FighterName} died!\n");
			Console.ResetColor();
		}
		private static void AttackMessage(Hero attacker, Hero defender, Attack attack)
		{
			PrintAttackerName(attacker);
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.Write(" attacked ");
			PrintDefenderName(defender);
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.Write($" with { attack.AttackName} and decreased his HP by {attack.AttackDamage}.\n");
			Console.ResetColor();
		}
		private static void CriticalAttackMessage(Hero attacker, Hero defender, Attack attack)
		{
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.Write($"{attacker.FighterName} scored a critical hit!");
			PrintAttackerName(attacker);
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.Write(" attacked ");
			PrintDefenderName(defender);
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.Write($" with { attack.AttackName} and decreased his HP by {attack.AttackDamage * 3}.\n");
			Console.ResetColor();
		}
		private static void DefenseMessage(Hero defender, Defense defense)
		{
			PrintDefenderName(defender);
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.Write($" protected himself by using {defense.DefenseName} and got 35 % less damage.\n");
			Console.ResetColor();
		}
		private static void PrintAttackerName(Hero attacker)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write(attacker.FighterName);
		}
		private static void PrintDefenderName(Hero defender)
		{
			Console.ForegroundColor = ConsoleColor.DarkMagenta;
			Console.Write(defender.FighterName);
		}
		#endregion

		#region Attacking

		private static bool MaybeCriticalHit(Random rnd)
		{
			if (ImFeelingLucky(rnd, CriticalHitChance))
				return true;
			return false;
		}
		private static void DealDamage(Hero fighter, Attack attack, Random rnd)
		{
			if (MaybeCriticalHit(rnd))
				fighter.CurrentHP -= attack.AttackDamage * 3;
			else if (MaybeDefendYourself(rnd))
				fighter.CurrentHP -= (int)Math.Round(attack.AttackDamage * .35);
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

		private static bool MaybeDefendYourself(Random rnd)
		{
			if (ImFeelingLucky(rnd, DefenseChance))
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

		private static void KillFighter(Hero fighter)
		{
			fighter.CurrentHP = 0;
			fighter.HealthState = FighterHealthState.Dead;
		}
		private static bool MaybeKillFighter(Hero fighter)
		{
			if (fighter.CurrentHP <= 0)
				return true;

			return false;
		}
		private static bool IsHeroDead (Hero fighter)
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
