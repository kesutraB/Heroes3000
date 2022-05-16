using System;
using System.Threading;
using Heroes3000.Constants;

namespace Heroes3000.Models
{
	public class Hero
	{

		private const int MinDelay = 1500;
		private const int MaxDelay = 5000;
		private const int CriticalHitChance = 2;
		public string FighterName { get; private set; }
		public FighterClass Class { get; private set; } = FighterClass.None;
		public FighterHealthState HealthState { get; private set; } = FighterHealthState.None;
		public Attack PhysicalAttack { get; private set; }
		public Attack MagicalAttack { get; private set; }
		public int CurrentHP { get; private set; }
		public int MaxHP { get; private set; }

		public Hero (string fighterName, FighterClass fighterClass, int maxHP, Attack physicalAttack, Attack magicalAttack)
		{
			this.FighterName = fighterName;
			this.Class = fighterClass;
			this.MaxHP = maxHP;
			this.CurrentHP = maxHP;
			this.PhysicalAttack = physicalAttack;
			this.MagicalAttack = magicalAttack;
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

		private void PrintFighterStatus(Hero fighter1, Hero fighter2)
		{
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine($"{fighter1.FighterName} now has {fighter1.CurrentHP} / {fighter1.MaxHP} HP left.");
			Console.WriteLine($"{fighter2.FighterName} now has {fighter2.CurrentHP} / {fighter2.MaxHP} HP left.");
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
		private void AttackMessage(Hero fighter1, Hero fighter2, Attack attack)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine($"{fighter1.FighterName} attacked {fighter2.FighterName} with {attack.AttackName} and decreased {fighter2.FighterName}'s HP by {attack.AttackDamage}.");
			Console.ResetColor();
		}
		private void CriticalAttackMessage(Hero fighter1, Hero fighter2, Attack attack)
		{
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine($"{fighter1.FighterName} scored a critical hit!");
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine($"{fighter1.FighterName} attacked {fighter2.FighterName} with {attack.AttackName} and decreased {fighter2.FighterName}'s HP by {attack.AttackDamage*3}.");
			Console.ResetColor();
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
