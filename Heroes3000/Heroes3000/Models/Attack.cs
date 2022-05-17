using Heroes3000.Constants;

namespace Heroes3000.Models
{
	public class Attack
	{
		public string AttackName { get; protected set; }
		public ActionTypes Type { get; protected set; }
		public int AttackDamage { get; protected set; }
		public int AttackCooldown { get; protected set; }

		public Attack(string attackName, ActionTypes type, int attackDamage, int attackCooldown)
		{
			this.AttackName = attackName;
			this.Type = type;
			this.AttackDamage = attackDamage;
			this.AttackCooldown = attackCooldown;
		}
	}
}
