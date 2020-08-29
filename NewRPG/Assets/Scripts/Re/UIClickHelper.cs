using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum AnimeType
{
    None,
    Scale,
    OffSet,
    Both,
}

public class UIClickHelper :MonoBehaviour
{
    public static void AddClick(GameObject _go,Action<GameObject,PointerEventData> _fun,AnimeType _type = AnimeType.Scale,string _isClickSoundName = "")
    {
        UIEventListener.Get(_go).onClick = (go,e)=>
        {
            if (_fun != null)
            {
                PlaySound(_go,_isClickSoundName);
                _fun(go,e);
            }
        };
        if (_type != AnimeType.None)
        {
            AddAnime(_go);
        }
    }
    public static void AddClick(GameObject _go,Action _fun,AnimeType _type = AnimeType.Scale,string _isClickSoundName = "")
    {
        AddClick(_go,(g,d)=>{
            _fun();
        },_type,_isClickSoundName);
    }

    public static void AddBtnClick(GameObject _go, Action _fun, string _isClickSoundName = "")
    {
        Button btn = _go.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() =>
            {
                PlaySound(_go,_isClickSoundName);
                _fun();
            });
        }
        else
        {
            Debug.LogErrorFormat("预知物没有button组件！", _go.name);
        }
    }
    public static void PlaySound(GameObject _go,string _soundName)
    {
        // Debug.Log("播放声音~！");
    }

    public static void AddAnime(GameObject _go)
    {
        Tween tween = null;
        UIEventListener go = UIEventListener.Get(_go);
        go.aniClickDown = ()=>
        {
            tween = _go.transform.DOScale(1.1f,0.1f).SetUpdate(true);
        };
        go.aniClickUp = ()=>
        {
            if (tween != null)
            {
                tween.Kill();
            }
            go.transform.DOScale(1,0.1f).SetUpdate(true);
        };
    }
}