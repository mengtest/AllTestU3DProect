using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.Main;

public class Loading : MonoBehaviour {

    Text loadTime;
    Slider slider;
	// Use this for initialization
	void Start () 
    {
        slider = GameObject.Find("Slider").GetComponent<Slider>();
        loadTime = GameObject.Find("LoadTime").GetComponent<Text>();
        StartCoroutine(LoadScenes());
	}

    AsyncOperation ao;
    IEnumerator LoadScenes()
    {
        ao = SceneManager.LoadSceneAsync("Main");
        //加载全部资源
        ResourcesManage.LoadAll();
        //加载头像
        ResourcesManage.LoadHeadImages();
        //加载音效
        AudioSound.LoadSound("sound/man");
        ///加载全部扑克
        PokerManage.LoadPoker();
        yield return ao;
    }


	// Update is called once per frame
	void Update () 
    {
        if (ao != null)
        {
            slider.value = ao.progress / 0.9f;
            loadTime.text = (ao.progress / 0.9f) * 100 + "%";
        }
	}
}
