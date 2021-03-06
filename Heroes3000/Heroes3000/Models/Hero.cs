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
		public string FighterName { get; protected set; }
		public FighterClass Class { get; protected set; } = FighterClass.None;
		public FighterHealthState HealthState { get; protected set; } = FighterHealthState.None;
		public Attack PhysicalAttack { get; protected set; }
		public Attack MagicalAttack { get; protected set; }
		public Defense PhysicalDefense { get; protected set; }
		public Defense MagicalDefense { get; protected set; }
		public int CurrentHp { get; protected set; }
		public int MaxHp { get; protected set; }

		public Hero (string fighterName, FighterClass fighterClass, int maxHp, Attack physicalAttack, Attack magicalAttack, Defense physicalDefense, Defense magicalDefense)
		{
			this.FighterName = fighterName;
			this.Class = fighterClass;
			this.MaxHp = maxHp;
			this.CurrentHp = maxHp;
			this.PhysicalAttack = physicalAttack;
			this.MagicalAttack = magicalAttack;
			this.PhysicalDefense = physicalDefense;
			this.MagicalDefense = magicalDefense;
		}

		public void Match (Hero fighter)
		{
			PrintFighterStatus(this, fighter);


			Random rnd_ = new Random(Convert.ToInt32(DateTime.Now.Second));
			int rnd = rnd_.Next(0, 100);

			var attack = ReturnAttack(rnd);
			var defense = ReturnDefense(rnd, attack);

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
				Environment.Exit(1);
			}
			if (IsHeroDead(this))
			{
				Console.ResetColor();
				DeathMessage(this);
				VictoryMessage(fighter);
				Environment.Exit(1);
			}

			Thread.Sleep(rnd_.Next(MinDelay, MaxDelay));
		}

		
		#region Printing

		private static void PrintFighterStatus(Hero attacker, Hero defender)
		{
			Console.ForegroundColor = ConsoleColor.Blue;
			PrintAttackerName(attacker);
			Console.WriteLine($" now has {attacker.CurrentHp} / {attacker.MaxHp} HP left.");
			PrintDefenderName(defender);
			Console.WriteLine($" now has {defender.CurrentHp} / {defender.MaxHp} HP left.");
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
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			PrintAttackerName(attacker);
			Console.Write(" attacked ");
			PrintDefenderName(defender);
			Console.Write($" with { attack.AttackName} and decreased his HP by {attack.AttackDamage}.\n");
			Console.ResetColor();
		}
		private static void CriticalAttackMessage(Hero attacker, Hero defender, Attack attack)
		{
			Console.ForegroundColor = ConsoleColor.DarkRed;
			PrintAttackerName(attacker);
			Console.Write(" scored a critical hit!\n");
			PrintAttackerName(attacker);
			Console.Write(" attacked ");
			PrintDefenderName(defender);
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
			var previousColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write(attacker.FighterName);
			Console.ForegroundColor = previousColor;

		}
		private static void PrintDefenderName(Hero defender)
		{
			var previousColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkMagenta;
			Console.Write(defender.FighterName);
			Console.ForegroundColor = previousColor;
		}
		#endregion

		#region Attacking

		private static bool MaybeCriticalHit(int rnd)
		{
			if (ImFeelingLucky(rnd, CriticalHitChance))
				return true;
			return false;
		}
		private static void DealDamage(Hero fighter, Attack attack, int rnd)
		{
			if (MaybeCriticalHit(rnd))
				fighter.CurrentHp -= attack.AttackDamage * 3;
			else if (MaybeDefendYourself(rnd))
				fighter.CurrentHp -= (int)Math.Round(attack.AttackDamage * .65);
			else
				fighter.CurrentHp -= attack.AttackDamage;
		}
		private Attack ReturnAttack(int rnd)
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

		private static bool MaybeDefendYourself(int rnd)
		{
			if (ImFeelingLucky(rnd, DefenseChance))
				return true;
			return false;
		}
		private Defense ReturnDefense(int rnd, Attack attack)
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
			fighter.CurrentHp = 0;
			fighter.HealthState = FighterHealthState.Dead;
		}
		private static bool MaybeKillFighter(Hero fighter)
		{
			if (fighter.CurrentHp <= 0)
				return true;

			return false;
		}
		private static bool IsHeroDead (Hero fighter)
		{
			if (fighter.HealthState == FighterHealthState.Dead)
				return true;

			return false;
		}
		private static bool ImFeelingLucky(int random, int luckyValue)
		{
			return random <= luckyValue;
		}
		#endregion
	}
}
