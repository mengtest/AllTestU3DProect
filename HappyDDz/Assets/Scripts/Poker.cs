using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Poker : IObjBase {
	public Transform trans { get; set; }
	public GameObject obj { get; set; }
	Image img;
	bool isSelect = false;
	bool tempSelect = false;
	public bool isClickEvent = false;
	public Vector3 initPos;
	public PokerInfo info;
	public Poker () { }
	public Poker (Transform _root) : this (_root, Vector3.zero) { }
	public Poker (Transform _root, Vector3 _pos) {
		GameObject _go = DDZMain.Instantiate (ResourcesManage.dictionary["poker"] as GameObject);
		obj = _go;
		trans = _go.transform;
		trans.SetParent (_root);
		trans.localPosition = _pos;
		trans.localScale = Vector3.one;
		trans.localEulerAngles = Vector3.zero;
		img = obj.GetComponent<Image> ();

		UIEventListener.Get (_go).onClick = (g, e) => {
			if (!isClickEvent) return;
			SetSelectPoker ();
		};
		UIEventListener.Get (_go).onDrag = (g, v, e) => {
			if (!isClickEvent) return;
			if (tempSelect) {
				tempSelect = false;
				SetSelectPoker ();
			}
		};
		UIEventListener.Get (_go).onClickDown = (g, e) => {
			if (!isClickEvent) return;
			Tools.isLock = true;
			tempSelect = true;
		};
		UIEventListener.Get (_go).onClickUp = (g, e) => {
			if (!isClickEvent) return;
			Tools.isLock = false;
			tempSelect = false;
		};
		UIEventListener.Get (_go).onEnter = (g) => {
			if (!isClickEvent) return;
			if (Tools.isLock) {
				SetSelectPoker ();
			}
		};
		Hide ();
	}
	public void SetPoker (PokerInfo _info, bool isShow = true) {
		info = _info;
		img.sprite = ResourcesManage.GetPokerSprite (info.Id);
		if (isShow) {
			Show ();
		}
	}
	public void Hide () {
		isSelect = false;
		tempSelect = false;
		isClickEvent = false;
		trans.localPosition = initPos;
		info = null;
		obj.SetActive (false);
	}
	public void Show () {
		obj.SetActive (true);
	}
	public bool GetActive () {
		return obj.activeSelf;
	}
	public void Init () {
		Hide ();
	}
	private void SetSelectPoker () {
		isSelect = !isSelect;
		if (isSelect) {
			trans.DOLocalMove (new Vector3 (initPos.x, initPos.y + 20, initPos.z), 0.1f);
		} else {
			trans.DOLocalMove (initPos, 0.1f);
		}
	}
}
public class PokerInfo {
	int id = 0;
	PokerHouse house = PokerHouse.none;
	PokerValueType paiVal = PokerValueType._0;
	public int Id {
		get {
			return id;
		}
		set {
			id = value;

			if (id == (int) PokerValueType._0) {
				paiVal = PokerValueType._0;
			} else if (id == (int) PokerValueType._maxJoker) {
				paiVal = PokerValueType._maxJoker;
			} else if (id == (int) PokerValueType._minJoker) {

				paiVal = PokerValueType._minJoker;
			} else {
				paiVal = (PokerValueType) ((id - 1) % 13);
				house = (PokerHouse) (id / 14);
			}
		}
	}

	public PokerValueType PaiVal {
		get {
			return paiVal;
		}
		set {
			paiVal = value;
			int id = (int) PokerValueType._0;
			if (paiVal == PokerValueType._0) { } else if (paiVal == PokerValueType._minJoker) {
				id = (int) PokerValueType._minJoker;
			} else if (paiVal == PokerValueType._maxJoker) {
				id = (int) PokerValueType._maxJoker;
			} else {
				id = (int) paiVal * (int) house;
			}
		}
	}
	public PokerHouse House {
		get {
			return house;
		}
	}
}