namespace Heroes3000.Models
{
	public class Attack
	{
		public enum AttackType
		{ None, Physical, Magical }

		public string AttackName { get; private set; }
		public AttackType Type { get; private set; }
		public int AttackDamage { get; private set; }
		public int AttackCooldown { get; private set; }

		public Attack(string attackName, AttackType type, int attackDamage, int attackCooldown)
		{
			this.AttackName = attackName;
			this.Type = type;
			this.AttackDamage = attackDamage;
			this.AttackCooldown = attackCooldown;
		}
	}
}
