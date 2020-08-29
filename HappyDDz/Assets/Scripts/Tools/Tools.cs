using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class Tools {
	public static bool isLock = false;
	public static bool isAnimaOk = false;
	public static Transform UIRoot {
		get {
			return DDZMain.Self.rootTrans;
		}
	}
	public static void Sort (List<PokerInfo> _list) {
		////排列大小  返回值小于0表示a小于b  值大于0 a大于b  值等于0 a等于b
		// _list.Sort((a, b) => b.PaiVal > a.PaiVal ? 1 : -1);
		//排列花色s
		_list.Sort ((a, b) => {
			if (a.PaiVal < b.PaiVal) {
				// Debug.Log(a.PaiVal + " < " + b.PaiVal + " : 往后排！");
				return 1;
			} else if (a.PaiVal > b.PaiVal) {
				// Debug.Log(a.PaiVal + " > " + b.PaiVal + " : 往前排！");
				return -1;
			} else if (a.House < b.House) {
				return -1;
			}
			return 0;
		});
	}

	/// <summary>
	/// 让超过宽度的文字跑起来
	/// </summary>
	/// <param name="_parent">Mesk父节点</param>
	/// <param name="_text">Text组件</param>
	public static void RunText (RectTransform _parent, Text _text) {
		if ((_text.preferredWidth - _parent.sizeDelta.x) > 0) {
			Transform _trans = _text.transform;
			float time = 1 + (_text.preferredWidth / _parent.sizeDelta.x);
			Sequence sqe = DOTween.Sequence ();
			sqe.AppendInterval (0.6f);
			sqe.Append (_trans.DOLocalMove (new Vector3 (-_text.preferredWidth + (_parent.sizeDelta.x / 2), 0, 0), time).SetEase (Ease.Linear));
			sqe.AppendInterval (0.6f);
			sqe.Append (_trans.DOLocalMove (new Vector3 (-_parent.sizeDelta.x + (_parent.sizeDelta.x / 2), 0, 0), time).SetEase (Ease.Linear));
			sqe.AppendCallback (() => {
				_trans.localPosition = new Vector3 (-_parent.sizeDelta.x + (_parent.sizeDelta.x / 2), 0, 0);
			});
			sqe.SetLoops (-1);
			sqe.SetAutoKill (true);
			sqe.SetUpdate (true);
			sqe.Play ();
		}
	}
}