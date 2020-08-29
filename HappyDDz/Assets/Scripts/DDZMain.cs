using System;
using System.Collections;
using DG.Tweening;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class DDZMain : MonoBehaviour {
	private static DDZMain ddzMain;
	public static DDZMain Self {
		get {
			return ddzMain;
		}
	}
	public Transform rootTrans;
	void Awake () {
		ddzMain = this;
		this.UIInit ();
		UIGameOperPanel.Self.ShowGameStart ();
	}

	void Test () {
		RectTransform trans = rootTrans.Find ("Test").GetComponent<RectTransform> ();
		Text text = trans.Find ("Text").GetComponent<Text> ();
		Tools.RunText (trans, text);
	}

	private void UIInit () {
		rootTrans = transform.Find ("root");
		UIUserPanel.Self.Init ();
		UISettingPanel.Self.Init ();
		UIGameMainPanel.Self.Init ();
		UILoginPanel.Self.Init ();
		UIGameOperPanel.Self.Init ();
	}

	void Start () {
		ResourcesManage.LoadAll ();
		GameController.Self.Init ();
	}

	public event UpdateAction updateEvent = null;
	public delegate void UpdateAction ();

	void Update () {
		if (updateEvent != null) {
			updateEvent ();
		}
	}

	public static Transform FindChild (string _str) {
		return ddzMain.rootTrans.Find (_str);
	}

	public void DonlowdImage (string _url, Action<Sprite> _callback = null) {
		StartCoroutine (Coroutine_DonlowdImage (_url, _callback));
	}

	IEnumerator Coroutine_DonlowdImage (string _url, Action<Sprite> _callback = null) {
		WWW www = new WWW (_url);
		yield return www;
		while (!www.isDone) { }
		if (_callback != null && www.texture != null) {
			_callback (ResourcesManage.CreateSprite (www.texture));
		}
	}
}