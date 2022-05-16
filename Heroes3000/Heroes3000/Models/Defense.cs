namespace Heroes3000.Models
{
	public class Defense
	{
		public enum DefenseType
		{ None, Physical, Magical }

		public string DefenseName { get; private set; }
		public DefenseType Type { get; private set; }
		public int DefenseCooldown { get; private set; }

		public Defense(string attackName, DefenseType type, int defenseCooldown)
		{
			this.DefenseName = attackName;
			this.Type = type;
			this.DefenseCooldown = defenseCooldown;
		}
	}
}
