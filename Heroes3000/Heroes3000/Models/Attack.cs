using Heroes3000.Constants;

namespace Heroes3000.Models
{
	public class Attack
	{
		public string AttackName { get; private set; }
		public ActionTypes Type { get; private set; }
		public int AttackDamage { get; private set; }
		public int AttackCooldown { get; private set; }

		public Attack(string attackName, ActionTypes type, int attackDamage, int attackCooldown)
		{
			this.AttackName = attackName;
			this.Type = type;
			this.AttackDamage = attackDamage;
			this.AttackCooldown = attackCooldown;
		}
	}
}
