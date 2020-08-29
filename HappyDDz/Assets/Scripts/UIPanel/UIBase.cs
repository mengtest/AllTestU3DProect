using System;
using UnityEngine;

public abstract class UIBase<T> : SingleT<T>,IObjBase where T : class, new () {
    public GameObject obj { get; set; }
    public Transform trans { get; set; }
    public string Name { get; set; }
    public virtual void Init (){
        this.Load(Name);
    }
    public virtual void Load (string _uiRootName) {
        Name = _uiRootName;
        trans = Tools.UIRoot.Find (_uiRootName);
        obj = trans.gameObject;
    }
    public virtual void Show () {
        obj.SetActive (true);
    }
    public virtual void Hide () {
        obj.SetActive (false);
    }
}