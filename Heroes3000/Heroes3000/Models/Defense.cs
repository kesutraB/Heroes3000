using Heroes3000.Constants;

namespace Heroes3000.Models
{
	public class Defense
	{
		public string DefenseName { get; protected set; }
		public ActionTypes Type { get; protected set; }
		public int DefenseCooldown { get; protected set; }

		public Defense(string defenseName, ActionTypes type, int defenseCooldown)
		{
			this.DefenseName = defenseName;
			this.Type = type;
			this.DefenseCooldown = defenseCooldown;
		}
	}
}
