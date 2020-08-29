using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioSound : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    public static Dictionary<string, AudioClip> dictionarySound = new Dictionary<string, AudioClip>();
    public static List<AudioSource> curSoundList = new List<AudioSource>();
    public static List<AudioSource> curBGMList = new List<AudioSource>();
    public static float soundVlo = 0.5f;
    public static float musicVlo = 1;

    /// <summary>加载资源文件夹下路径文件
    /// </summary>
    /// <param name="_path">路径</param>
    public static void LoadSound(string _path)
    {
        foreach (AudioClip sound in Resources.LoadAll<AudioClip>(_path))
        {
            if (!dictionarySound.ContainsKey(sound.name))
            {
                dictionarySound.Add(sound.name, sound);
            }
        }
    }

    /// <summary>创建声音播放
    /// </summary>
    /// <param name="_fileName">文件名</param>
    /// <param name="isBGM">是否是背景音乐</param>
    /// <param name="loop">是否是重复</param>
    /// <param name="autoPlay">是否是自动播放</param>
    public static void CreateSoundPlay(string _fileName, bool isBGM = false, bool loop = false, bool autoPlay = true)
    {
        //不存在值
        if (!dictionarySound.ContainsKey(_fileName))
        {
            return;
        }
        GameObject go = new GameObject("AudioSound:" + _fileName);
        GameObject trans = GameObject.Find("AudioSound");
        if (trans == null)
        {
            trans = new GameObject("AudioSound");
        }
        go.transform.parent = GameObject.Find("AudioSound").transform;
        go.transform.localPosition = Vector3.zero;
        AudioSource ads = go.AddComponent<AudioSource>();
        ads.clip = dictionarySound[_fileName];
        ads.loop = loop;
        if (isBGM)
        {
            ads.volume = musicVlo;
            curBGMList.Add(ads);
        }
        else
        {
            ads.volume = soundVlo;
            curSoundList.Add(ads);
        }
        if (!loop)
        {
            curSoundList.Remove(ads);
            Destroy(go, ads.clip.length);
        }
        if (autoPlay)
        {
            ads.Play();
        }
    }

    /// <summary>改变音乐音量
    /// </summary>
    /// <param name="_value">改变到的值</param>
    public static void ChangeMusicVolume(float _value)
    {
        musicVlo = _value;
        if (curBGMList == null)
        {
            return;
        }
        for (int i = 0; i < curBGMList.Count; i++)
        {
            curBGMList[i].volume = _value;
        }
    }

    /// <summary>改变音效音量
    /// </summary>
    /// <param name="_value">改变到的值</param>
    public static void ChangeSoundVolume(float _value)
    {
        soundVlo = _value;
        if (curSoundList == null)
        {
            return;
        }
        for (int i = 0; i < curSoundList.Count; i++)
        {
            curSoundList[i].volume = soundVlo;
        }
    }
}
