using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class Tools {
	public static bool isHandLock = false;
	public static int upIdx = 0;
	public static int downIdx = 0;
	public static bool isAnimaOk = false;

	/// <summary>
	/// 让超过宽度的文字跑起来
	/// </summary>
	/// <param name="_parent">Mesk父节点</param>
	/// <param name="_text">Text组件</param>
	public static void RunText (RectTransform _parent, Text _text) {
		if ((_text.preferredWidth - _parent.sizeDelta.x) > 0) {
			Transform _trans = _text.transform;
			float time = 1 + (_text.preferredWidth / _parent.sizeDelta.x);
			Sequence queue = DOTween.Sequence ();
			queue.AppendInterval (0.6f);
			queue.Append (_trans.DOLocalMove (new Vector3 (-_text.preferredWidth + (_parent.sizeDelta.x / 2), 0, 0), time).SetEase (Ease.Linear));
			queue.AppendInterval (0.6f);
			queue.Append (_trans.DOLocalMove (new Vector3 (-_parent.sizeDelta.x + (_parent.sizeDelta.x / 2), 0, 0), time).SetEase (Ease.Linear));
			queue.AppendInterval (1f);
			queue.AppendCallback (() => {
				_trans.localPosition = new Vector3 (-_parent.sizeDelta.x + (_parent.sizeDelta.x / 2), 0, 0);
			});
			queue.SetLoops (-1);
			queue.SetAutoKill (true);
			queue.SetUpdate (true);
			queue.Play ();
		}
	}
}