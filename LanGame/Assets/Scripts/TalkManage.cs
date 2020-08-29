using System.Collections;
using System.Collections.Generic;
using System.Net;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
	public class TalkManage {
		private static TalkManage _self;
		public static TalkManage Self {
			get {
				if (_self == null) {
					_self = new TalkManage ();
					_self.trans = GameObject.Find ("TalkManage").transform;
					_self.template = _self.trans.Find ("template").gameObject;
				}
				return _self;
			}
		}
		private TalkManage () {

		}
		public Transform trans;
		public GameObject template;
		public List<Talk> list = new List<Talk> ();
		public int MaxCount = 12;
		public void SetTalk (IPEndPoint _p, string _str) {
			if (list.Count >= MaxCount) {
				Talk talk = list[0];
				list.Remove(talk);
				list.Add(talk);
				talk.SetData (_p, _str);
				talk.trans.localPosition = Vector3.zero;
				talk.trans.localScale = new Vector3 (0, 1, 1);
				talk.trans.DOScale(Vector3.one,0.1f);
			}
			else if (list.Count < MaxCount) {
				Transform _trans = GameObject.Instantiate (_self.template).transform;
				_trans.SetParent (trans);
				_trans.localPosition = Vector3.zero;
				Talk talk = new Talk (_trans.gameObject);
				talk.SetData (_p, _str);
				list.Add (talk);
				_trans.name = list.Count + "";
			}
			Vector2 sizeDelta = Vector2.zero;
			float endPosY = 0;
			for (int i = list.Count - 1; i >= 0; i--) {
				list[i].endPosY = endPosY + sizeDelta.y;
				Vector2 endPos = new Vector3 (0, list[i].endPosY, 0);
				endPosY = endPos.y;
				sizeDelta = list[i].sizeDelta;
				list[i].DOLocalMove (endPos);
			}
		}

		public class Talk {
			public GameObject obj = null;
			public Transform trans = null;
			public Talk (GameObject _obj) {
				obj = _obj;
				trans = _obj.transform;
				rectTransform = _obj.GetComponent<RectTransform> ();
				talk = trans.Find ("head/Text").GetComponent<Text> ();
				head = trans.Find ("head").GetComponent<Image> ();
				obj.transform.DOScale (Vector3.one, 0.1f);
			}
			RectTransform rectTransform;
			Image head;
			Text talk;
			public void SetData (IPEndPoint _p, string _str) {
				talk.text = _p.ToString () + " 说 :" + _str;
				Vector2 vet = this.rectTransform.sizeDelta;
				if (talk.preferredHeight <= head.rectTransform.rect.size.y + 10) {
					this.rectTransform.sizeDelta = new Vector2 (vet.x, head.rectTransform.rect.size.y + 10);
				} else {
					this.rectTransform.sizeDelta = new Vector2 (vet.x, talk.preferredHeight + 5);
				}
				sizeDelta = this.rectTransform.sizeDelta;
				Main.Self.SetHand(head);
			}
			public void DOLocalMove (Vector2 endPos) {
				trans.DOLocalMove (endPos, 0.25f);
			}
			public Vector2 sizeDelta;
			public float endPosY;
		}
	}
}