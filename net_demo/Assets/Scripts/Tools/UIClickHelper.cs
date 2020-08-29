using System;
using UnityEngine;
using DG.Tweening;

public class UIClickHelper :MonoBehaviour
{
    public static void AddClick(GameObject _go,Action _fun,AnimeType _type = AnimeType.Scale,bool _isPlayClickSound = true)
    {
        UIEventListener.Get(_go).onClick = (go,e)=>
        {
            if (_fun != null)
            {
                _fun();
            }
        };
        if (_isPlayClickSound)
        {
            AddPlaySound(_go);
        }
        if (_type != AnimeType.None)
        {
            AddAnime(_go);
        }
    }
    public static void AddPlaySound(GameObject _go)
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