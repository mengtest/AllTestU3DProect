using DG.Tweening;
using UnityEngine;

public class UISettingPanel : UIBase<UISettingPanel> {
	Tween tween;
	Transform rootTrans;
	RectTransform rootRectTrans;
	GameObject btn_show;
	RectTransform btn_closeBg;
	bool isOpen = false;
	Vector3 oldInitPos;
	Vector3 movePos;
	public override void Load (string _uiName) {
		base.Load (_uiName);
		rootTrans = trans.Find ("root/panel");
		rootRectTrans = rootTrans.GetComponent<RectTransform> ();
		btn_show = trans.Find ("root/btn_show").gameObject;
		btn_closeBg = trans.Find ("root/btn_closeBg").GetComponent<RectTransform> ();
		btn_closeBg.offsetMax = new Vector2 (0, -rootRectTrans.sizeDelta.y);
		btn_closeBg.gameObject.SetActive (false);
		oldInitPos = rootRectTrans.localPosition;
		movePos = rootRectTrans.localPosition;
		movePos.y = movePos.y - rootRectTrans.sizeDelta.y;
		UIEventListener.Get (btn_show).onClick = (_, e) => {
			ShowMenu ();
		};
		UIEventListener.Get (btn_closeBg.gameObject).onClick = (_, e) => {
			ShowMenu ();
		};
	}
	public override void Init()
	{
		Name = "UISettingPanel";
		base.Init();
	}
	public void ShowMenu () {
		if (isOpen) {
			isOpen = false;
			btn_closeBg.gameObject.SetActive (false);
			rootRectTrans.DOLocalMove (oldInitPos, 0.1f).SetUpdate (true).SetAutoKill ();
			btn_show.transform.DOLocalMove (oldInitPos, 0.1f).SetUpdate (true).SetAutoKill ();
		} else {
			isOpen = true;
			btn_closeBg.gameObject.SetActive (true);
			rootRectTrans.DOLocalMove (movePos, 0.1f).SetUpdate (true).SetAutoKill ();
			btn_show.transform.DOLocalMove (movePos, 0.1f).SetUpdate (true).SetAutoKill ();
		}
	}
}