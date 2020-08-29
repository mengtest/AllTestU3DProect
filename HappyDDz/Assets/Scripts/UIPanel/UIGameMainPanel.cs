using System.Collections.Generic;
using UnityEngine;

public class UIGameMainPanel : UIBase<UIGameMainPanel> {
	public Transform lastPokerRoot;
	public List<Poker> lastPoker = new List<Poker> ();
	public override void Load (string _uiName) {
		base.Load (_uiName);
		lastPokerRoot = trans.Find ("root/lastPoker");
	}
	public override void Init () {
		Name = "UIGameMainPanel";
		base.Init ();
	}
}
