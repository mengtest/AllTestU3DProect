using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventListener : EventTrigger {
	private bool isLockClick = false;
	public Action aniClickDown = null; //动画按下
	public Action aniClickUp = null; //动画抬起
	public Action soundClick = null; //点击音效
	public Action<GameObject, PointerEventData> onClickDown = null;
	public Action<GameObject, PointerEventData> onClickUp = null;
	public Action<GameObject, PointerEventData> onClick = null;
	public Action<GameObject> onEnter = null;
	public Action<GameObject> onExit = null;
	public Action<GameObject> onInitializePotentialDrag = null;
	public Action<GameObject, PointerEventData> onBeginDrag = null;
	public Action<GameObject, Vector3, PointerEventData> onDrag = null;
	public Action<GameObject, PointerEventData> onEndDrag = null;
	public Action<GameObject, PointerEventData> onDrop = null;
	public Action<GameObject> onSelect = null;
	public Action<GameObject> onUpdateSelected = null;
	public Action<GameObject> onDeselect = null;
	public Action<GameObject> onSubmit = null;
	public Action<GameObject> onCancel = null;
	public Action<GameObject> onScroll = null;
	public Action<GameObject, Vector2> onMove = null;

	/// <summary>
	/// 设定点击锁定
	/// </summary>
	/// <param name="_bool">true：上锁  false：解锁</param>
	public void SetClickLock (bool _bool) {
		isLockClick = _bool;
	}
	public override void OnPointerDown (PointerEventData eventData) {
		if (eventData.button != PointerEventData.InputButton.Left) return;
		if (isLockClick) return;
		if (onClickDown != null) onClickDown (gameObject, eventData);
		if (aniClickDown != null) aniClickDown ();
	}
	public override void OnPointerUp (PointerEventData eventData) {
		if (eventData.button != PointerEventData.InputButton.Left) return;
		if (isLockClick) return;
		if (onClickUp != null) onClickUp (gameObject, eventData);
		if (aniClickUp != null) aniClickUp ();
	}
	public override void OnPointerClick (PointerEventData eventData) {
		if (eventData.button != PointerEventData.InputButton.Left) return;
		if (isLockClick) return;
		if (onClick != null) onClick (gameObject, eventData);
		if (soundClick != null) soundClick ();
	}
	public override void OnPointerEnter (PointerEventData eventData) {
		if (onEnter != null) onEnter (gameObject);
	}
	public override void OnPointerExit (PointerEventData eventData) {
		if (onExit != null) onExit (gameObject);
	}
	public override void OnInitializePotentialDrag (PointerEventData eventData) {
		if (onInitializePotentialDrag != null) onInitializePotentialDrag (gameObject);
	}
	public override void OnBeginDrag (PointerEventData eventData) {
		if (onBeginDrag != null) onBeginDrag (gameObject, eventData);
	}
	public override void OnDrag (PointerEventData eventData) {
		if (onDrag != null) onDrag (gameObject, eventData.position, eventData);
	}
	public override void OnEndDrag (PointerEventData eventData) {
		if (onEndDrag != null) onEndDrag (gameObject, eventData);
	}
	public override void OnDrop (PointerEventData eventData) {
		if (onDrop != null) onDrop (gameObject, eventData);
	}
	public override void OnSelect (BaseEventData eventData) {
		if (onSelect != null) onSelect (gameObject);
	}
	public override void OnUpdateSelected (BaseEventData eventData) {
		if (onUpdateSelected != null) onUpdateSelected (gameObject);
	}
	public override void OnDeselect (BaseEventData eventData) {
		if (onDeselect != null) onDeselect (gameObject);
	}
	public override void OnSubmit (BaseEventData eventData) {
		if (onSubmit != null) onSubmit (gameObject);
	}
	public override void OnCancel (BaseEventData eventData) {
		if (onCancel != null) onCancel (gameObject);
	}
	public override void OnScroll (PointerEventData eventData) {
		if (onScroll != null) onScroll (gameObject);
	}
	public override void OnMove (AxisEventData eventData) {
		if (onMove != null) onMove (gameObject, eventData.moveVector);
	}
	public static UIEventListener Get (GameObject go) {
		UIEventListener listener = go.GetComponent<UIEventListener> ();
		if (listener == null) listener = go.AddComponent<UIEventListener> ();
		return listener;
	}
}