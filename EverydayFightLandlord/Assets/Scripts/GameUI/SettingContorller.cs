using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class SettingContorller : MonoBehaviour
{
    Slider soundSlider;
    Slider musicSlider;
    Button setting_Reset;
    Text soundVol_Text;
    Text musicVol_Text;
	// Use this for initialization
	void Start ()
    {
        setting_Reset = GameObject.Find("Setting_Reset").GetComponent<Button>();
        soundSlider = GameObject.Find("SoundVol_Slider").GetComponent<Slider>();
        musicSlider = GameObject.Find("MusicVol_Slider").GetComponent<Slider>();
        soundVol_Text = GameObject.Find("SoundVol_Text").GetComponent<Text>();
        musicVol_Text = GameObject.Find("MusicVol_Text").GetComponent<Text>();
        soundSlider.onValueChanged.AddListener(ChangeSoundVol);
        musicSlider.onValueChanged.AddListener(ChangeMusicVol);
        setting_Reset.onClick.AddListener(Reset);
        musicSlider.value = AudioSound.musicVlo;
        soundSlider.value = AudioSound.soundVlo;
        musicVol_Text.text = (int)(musicSlider.value * 100) + "%";
        soundVol_Text.text = (int)(soundSlider.value * 100) + "%";
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void SettingEvent()
    {
        transform.DOScale(Vector3.zero, 0.25f);
    }

    public void ChangeMusicVol(float _value)
    {
        AudioSound.ChangeMusicVolume(_value);
        musicVol_Text.text = (int)(_value * 100) + "%";
    }
    public void ChangeSoundVol(float _value)
    {
        AudioSound.ChangeSoundVolume(_value);
        soundVol_Text.text = (int)(_value * 100) + "%";
    }

    public void Reset()
    {
        ChangeMusicVol(1);
        ChangeSoundVol(1);
        musicSlider.value = AudioSound.musicVlo;
        soundSlider.value = AudioSound.soundVlo;
    }
}