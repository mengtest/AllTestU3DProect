using UnityEngine;
using UnityEngine.EventSystems;

public class UIGameOperPanel : UIBase<UIGameOperPanel> {
    GameObject operRoot;
    GameObject btn_operOut;
    GameObject btn_operPass;
    GameObject btn_operTip;
    GameObject grabRoot;
    GameObject btn_grabPass;
    GameObject btn_grab;
    GameObject ctrlRoot;
    GameObject btn_start;
    GameObject btn_reStart;
    public override void Load (string _uiName) {
        base.Load (_uiName);
        operRoot = trans.Find ("root/operArr").gameObject;
        btn_operOut = trans.Find ("root/operArr/btn_out").gameObject;
        btn_operPass = trans.Find ("root/operArr/btn_pass").gameObject;
        btn_operTip = trans.Find ("root/operArr/btn_tip").gameObject;

        grabRoot = trans.Find ("root/grabArr").gameObject;
        btn_grabPass = trans.Find ("root/grabArr/btn_pass").gameObject;
        btn_grab = trans.Find ("root/grabArr/btn_grab").gameObject;

        ctrlRoot = trans.Find ("root/ctrlArr").gameObject;
        btn_start = trans.Find ("root/ctrlArr/btn_start").gameObject;
        btn_reStart = trans.Find ("root/ctrlArr/btn_reStart").gameObject;

        ctrlRoot.SetActive (false);
        grabRoot.SetActive (false);
        operRoot.SetActive (false);
        BindEvent ();
    }

    private void BindEvent () {
        UIClickHelper.AddClick (btn_operOut, OnClickOutPoker);
        UIClickHelper.AddClick (btn_operPass, OnClickPassOper);
        UIClickHelper.AddClick (btn_operTip, OnClickOperTip);

        UIClickHelper.AddClick (btn_grabPass, () => {
            GrabOper (0);
        });
        UIClickHelper.AddClick (btn_grab, () => {
            GrabOper (1);
        });

        UIClickHelper.AddClick (btn_start, OnClickStartGame);
        UIClickHelper.AddClick (btn_reStart, OnClickReStartGame);
    }

    public override void Init () {
        Name = "UIGameOperPanel";
        base.Init ();
        Hide ();
    }
    
    public override void Hide () {
        InitShow ();
        base.Hide ();
    }

    public override void Show () {
        Hide ();
        base.Show ();
    }

    private void InitShow () {
        ctrlRoot.SetActive (false);
        grabRoot.SetActive (false);
        operRoot.SetActive (false);
    }

    public void ShowOperBtn (bool isShowPass = true) {
        Show ();
        btn_operOut.SetActive (true);
        btn_operPass.SetActive (isShowPass);
        btn_operTip.SetActive (true);
        operRoot.SetActive (true);
    }

    public void ShowGameStart (bool isGameOver = false) {
        Show ();
        btn_start.SetActive (!isGameOver);
        btn_reStart.SetActive (isGameOver);
        ctrlRoot.SetActive (true);
    }

    public void OnClickOutPoker () {

    }
    public void OnClickPassOper () {

    }
    public void OnClickOperTip () {

    }

    public void GrabOper (int _oper) {

    }
    public void OnClickStartGame () {
        GameController.Self.StartGame ();
        ctrlRoot.SetActive (false);
    }
    public void OnClickReStartGame () {
        //ReGame
        ctrlRoot.SetActive (false);
    }
}