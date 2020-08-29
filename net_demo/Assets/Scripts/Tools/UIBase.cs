using System;
using UnityEngine;

public abstract class UIBase<T> : SingleT<T> where T : class, new () {
	public GameObject Obj { get; set; }
	public Transform Trans { get; set; }
	public string Name { get; set; }
	public virtual void Init () {
		this.Load (Name);
	}
	public virtual void Load (string _uiRootName) {
		Name = _uiRootName;
		Obj = Trans.gameObject;
	}
	public virtual void Show () {
		Obj.SetActive (true);
	}
	public virtual void Hide () {
		Obj.SetActive (false);
	}
}