using System;

namespace Game {
	[Serializable]
	public class NoAction : IAction {
		public void ProcessAction () { }
	}
}