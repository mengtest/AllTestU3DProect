using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PokerListManage {
    private Transform _rootTrans;
    private PokerPosType _posType;
    private List<Poker> __list = new List<Poker> ();
    public List<Poker> PokerList {
        get {
            return __list;
        }
    }
    public PokerListManage (Transform _root, PokerPosType _type) {
        _rootTrans = _root;
        _posType = _type;
    }
    public void Init () {
        for (int i = 0; i < this.PokerList.Count; i++) {
            this.PokerList[i].Init ();
        }
    }
    public void SetInfo (PokerInfo _info) {
        List<Poker> _list = GetHidePokerList ();
        if (_list.Count == 0) {
            Poker poker = new Poker (_rootTrans);
            this.PokerList.Add (poker);
            _list.Add (poker);
        }
        _list[0].SetPoker (_info);
    }
    public void SetInfo (List<PokerInfo> _info) {
        if (this.PokerList.Count < _info.Count) {
            int count = this.PokerList.Count;
            for (int i = count; i < _info.Count; i++) {
                Poker poker = new Poker (_rootTrans);
                this.PokerList.Add (poker);
            }
        }
        for (int i = 0; i < _info.Count; i++) {
            this.PokerList[i].SetPoker (_info[i]);
        }
        for (int i = _info.Count; i < this.PokerList.Count; i++) {
            this.PokerList[i].Hide ();
        }
        Sort ();
        MovePos ();
    }
    public void SetClickEvent (bool _switch) {
        foreach (Poker poker in this.PokerList) {
            poker.isClickEvent = _switch;
        }
    }
    private List<Poker> GetShowPokerList () {
        List<Poker> _list = new List<Poker> ();
        foreach (Poker poker in this.PokerList) {
            if (poker.info != null) {
                _list.Add (poker);
            }
        }
        return _list;
    }
    private List<Poker> GetHidePokerList () {
        List<Poker> _list = new List<Poker> ();
        foreach (Poker poker in this.PokerList) {
            if (poker.info == null) {
                _list.Add (poker);
            }
        }
        return _list;
    }
    public void Sort () {
        this.PokerList.Sort ((a, b) => {
            if (a.info.PaiVal < b.info.PaiVal) {
                return 1;
            } else if (a.info.PaiVal > b.info.PaiVal) {
                return -1;
            } else if (a.info.House < b.info.House) {
                return -1;
            }
            return 0;
        });
    }
    public void MovePos () {
        List<Poker> _list = GetShowPokerList ();
        float count = _list.Count;
        float offsetX = -15;
        float startX = (count / 2) * offsetX;

        float offsetY = -23;
        float startY = (count / 2) * offsetY;

        for (int i = 0; i < _list.Count; i++) {
            Vector3 movePos = Vector3.zero;
            if (_posType == PokerPosType.Center) {
                movePos = new Vector3 (startX - i * offsetX, 0, 0);
            } else {
                movePos = new Vector3 (0, -(startY - i * offsetY), 0);
            }
            _list[i].trans.DOLocalMove (movePos, 0.5f);
            _list[i].initPos = movePos;
            _list[i].trans.SetSiblingIndex (i);
        }
    }
}