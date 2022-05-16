using Heroes3000.Constants;

namespace Heroes3000.Models
{
	public class Defense
	{
		public string DefenseName { get; private set; }
		public ActionTypes Type { get; private set; }
		public int DefenseCooldown { get; private set; }

		public Defense(string defenseName, ActionTypes type, int defenseCooldown)
		{
			this.DefenseName = defenseName;
			this.Type = type;
			this.DefenseCooldown = defenseCooldown;
		}
	}
}
