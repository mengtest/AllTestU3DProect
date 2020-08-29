using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillController : MonoBehaviour 
{

    public float SkillCDTime;
    public float SkillContinueTime;
    bool isFree = false;
    Image SkillImage;
    float T_SkillTime;
    float T_SkillContinueTime;

	// Use this for initialization
	void Start ()
    {
        gameController = GameObject.Find("GameController").GetComponent<FxxkFruit>();
        SkillImage = GetComponent<Image>();
        T_SkillTime = SkillCDTime;
        T_SkillContinueTime = SkillContinueTime;
	}

    FxxkFruit gameController;
	// Update is called once per frame
	void Update ()
    {
        ///游戏没有开始
        if (!gameController.IsGameStart || gameController.IsGameOver)
        {
            SkillCDTime = T_SkillTime;
            SkillContinueTime = T_SkillContinueTime;
            isFree = false;
        }

        if (isFree)
        {
            SkillCDTime = (SkillCDTime - Time.deltaTime) <= 0 ? T_SkillTime : SkillCDTime - Time.deltaTime;
            SkillImage.fillAmount = SkillCDTime == T_SkillTime ? 1 : 1 - SkillCDTime / T_SkillTime;
            isFree = SkillCDTime == T_SkillTime ? false : true;
            SkillContinueTime = (SkillContinueTime - Time.deltaTime) <= 0 ? 0 : SkillContinueTime - Time.deltaTime;
            SkillContinueTime = isFree ? SkillContinueTime : T_SkillContinueTime;
        }


        if (SkillContinueTime < T_SkillContinueTime && SkillContinueTime != 0)
        {
            if (this.gameObject.name == "LockPower")
            {
                gameController.ShiftPower = 1;
            }
            if (this.gameObject.name == "LenssenRandom")
            {
                gameController.IsOpenLine = true;
            }
        }
        else
        {
            if (this.gameObject.name == "LenssenRandom")
            {
                gameController.IsOpenLine = false;
            }
        }


	}
    public void OnSkillFree()
    {
        if (gameController.IsGameStart)
        {
            isFree = true;
        }
    }

}
