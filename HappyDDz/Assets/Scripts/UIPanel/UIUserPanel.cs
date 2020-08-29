using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUserPanel : UIBase<UIUserPanel> {
	public List<UIUser> userList = new List<UIUser> ();
	public override void Load (string _uiName) {
		base.Load (_uiName);
		for (int i = 1; i <= 3; i++) {
			userList.Add (new UIUser (trans.Find ("root/" + i), i));
		}
	}
	public override void Init () {
		Name = "UIUserPanel";
		base.Init ();
	}

	/// <summary>
	/// 发牌协程
	/// </summary>
	public IEnumerator DealPoker (int[] pokerData) {
		int playerIndex = 0;

		for (int i = 0; i < pokerData.Length - 3; i++) {
			PokerInfo info = new PokerInfo ();
			info.Id = pokerData[i] + 1;
			PokerListManage list = userList[playerIndex].handsPoker;
			Tools.isAnimaOk = false;
			list.SetInfo (info);
			list.Sort ();
			list.MovePos ();
			playerIndex = (++playerIndex) % 3;
			yield return new WaitForSeconds (0.1f);
		}

		userList[0].handsPoker.SetClickEvent (true);

		for (int i = pokerData.Length - 3; i < pokerData.Length; i++) {
			Poker poker = new Poker (UIGameMainPanel.Self.lastPokerRoot);
			UIGameMainPanel.Self.lastPoker.Add (poker);
		}
		UIGameOperPanel.Self.ShowOperBtn ();

		// yield return new WaitForSeconds(1f);
		// //初始化显示底牌
		// InsShowBackPoker();
		// //显示玩家手牌
		// for (int i = 0; i < player.Length; i++)
		// {
		//     player[i].PokerSort();
		//     player[i].ShowPoker();
		// }
		// //发牌完毕为抢地主状态
		// if (state == GameState.Empty)
		// {
		// gameState = GameState.GrabLandLording;
		// }
		// //抢地主按钮
		// if (maxPlayerIndex == -1)
		// {
		//     View_GrabLandLord.SetActive(true);
		// }
	}
}

public class UIUser : IObjBase {
	public GameObject obj { get; set; }
	public Transform trans { get; set; }
	public void Show () { }
	public void Hide () { }
	public Transform handsRoot;
	public Transform lastRoot;
	public PokerListManage handsPoker;
	public PokerListManage lastPoker;
	public UIUserInfo info;
	public PokerPosType posType;
	Transform time;
	Text txt_time;
	public UIUser (Transform _trans, int _type) {
		trans = _trans;
		obj = _trans.gameObject;

		handsRoot = trans.Find ("handsPoker");
		lastRoot = trans.Find ("lastPoker");
		posType = (PokerPosType) _type;
		handsPoker = new PokerListManage (handsRoot, posType);
		lastPoker = new PokerListManage (lastRoot, posType);
		info = new UIUserInfo (trans.Find ("info"));
		time = trans.Find ("time");
		txt_time = time.Find ("Text").GetComponent<Text> ();
		time.gameObject.SetActive (false);
		txt_time.text = "";
	}

	public class UIUserInfo : IObjBase {
		public GameObject obj { get; set; }
		public Transform trans { get; set; }
		public void Show () { }
		public void Hide () { }
		Image img_headImg;
		Text txt_socre;
		Text txt_name;
		Text txt_lastNum;
		Text txt_roleType;
		UserData data;
		public UIUserInfo (Transform _trans) {
			obj = _trans.gameObject;
			trans = _trans;

			img_headImg = trans.Find ("headImg").GetComponent<Image> ();
			txt_socre = trans.Find ("socre").GetComponent<Text> ();
			txt_name = trans.Find ("name").GetComponent<Text> ();
			txt_lastNum = trans.Find ("lastpai").GetComponent<Text> ();
			txt_roleType = trans.Find ("roleType").GetComponent<Text> ();
		}

		public void SetInfo (UserData _data) {
			data = _data;
			txt_name.text = _data.Name;
			txt_socre.text = _data.Score;
			DDZMain.Self.DonlowdImage (_data.HeadUrl, (spr) => {
				img_headImg.sprite = spr;
			});
		}
		public void SetScore (UserData _data) {
			txt_socre.text = _data.Score;
		}
		public void SetRoleType (RoleType _type) {
			if (_type == RoleType.landlord) {
				txt_roleType.text = "地主";
			} else if (_type == RoleType.peasant) {
				txt_roleType.text = "农民";
			} else {
				txt_roleType.text = "";
			}
		}
		public void SetLastNum (int _num) {
			txt_lastNum.text = _num.ToString ();
		}
	}
}

public class UserData {
	public string Name { get; set; }
	public string Score { get; set; }
	public string HeadUrl { get; set; }
	public RoleType RType { get; set; }
}