using UnityEngine;

public class UILoginPanel : UIBase<UILoginPanel> {
    public override void Load(string _uiName){
        base.Load(_uiName);
    }
	public override void Init()
	{
		Name = "UILoginPanel";
		base.Init();
	}
}