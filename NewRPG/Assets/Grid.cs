using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour {
	public GridData data;
	public Transform obj;
	Image img;
	void Start () {
		img = gameObject.AddComponent<Image> ();
		if (data.gType == GridType.Wall) {
			img.color = Color.black;
		}
		UIClickHelper.AddClick (gameObject, () => {
			Debug.Log ("click grid");
			Dispatcher.SendProtocalEvent ("clickGrid", this);
		});
	}
	public void RefishColor () {
		if (data.gType == GridType.Wall) {
			img.DOColor (Color.black, 0.3f);
		} else {
			img.DOColor (Color.white, 1f);
		}
	}
	public void SetColor () {
		img.DOColor (Color.green, 0.2f);
	}
}