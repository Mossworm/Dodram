using System;
using TMPro;
using UnityEngine;
using System.Collections;
using Spine.Unity;


public class GameManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject settingCanvas;
    public GameObject guideCanvas;

    public GameObject endCanvas;
    public GameObject timer;
    public GameObject highScoreUI;
    public GameObject highScoreText;
    public GameObject currentScoreText;

    [SerializeField] private float playTime;

    private bool isEndflag;

    private GameObject Paper2, Paper1, ScoreUI;
    private GameObject _cutscene_Win, _cutscene_Lose;
    private GameObject _win_Cut, _lose_Cut;
    private GameObject _win_Icon, _lose_Icon;
    private GameObject _winText, _loseText;

    public GameObject endingCanvas;
    public GameObject house;

    // Start is called before the first frame update
    void Start()
    {
        Paper2 = endCanvas.transform.Find("Paper2").gameObject;
        Paper1 = endCanvas.transform.Find("Paper1").gameObject;
        ScoreUI = Paper1.transform.Find("ScoreUI").gameObject;

        _cutscene_Win = Paper2.transform.Find("Cutscene_Win").gameObject;
        _cutscene_Lose = Paper2.transform.Find("Cutscene_Lose").gameObject;
        _win_Cut = Paper1.transform.Find("Win_Cut").gameObject;
        _lose_Cut = Paper1.transform.Find("Lose_Cut").gameObject;
        _win_Icon = Paper1.transform.Find("Win_Icon").gameObject;
        _lose_Icon = Paper1.transform.Find("Lose_Icon").gameObject;
        _winText = ScoreUI.transform.Find("WinText").gameObject;
        _loseText = ScoreUI.transform.Find("LoseText").gameObject;

        isEndflag = false;
        if (settingCanvas == null)
        {
            settingCanvas = GameObject.Find("=====UI=====").transform.Find("SettingCanvas").gameObject;
        }
        if (guideCanvas == null)
        {
            guideCanvas = GameObject.Find("=====UI=====").transform.Find("GuideCanvas").gameObject;
        }
    }

    IEnumerator WaitNonLoopAnim()
    {
        yield return new WaitForSeconds(5f);

        if (!house.GetComponent<HouseScript>().isWin)
        {
            _cutscene_Lose.SetActive(true);
            _cutscene_Win.SetActive(false);
            _lose_Cut.SetActive(true);
            _win_Cut.SetActive(false);
            _lose_Icon.SetActive(true);
            _win_Icon.SetActive(false);
            _loseText.SetActive(true);
            _winText.SetActive(false);

            SoundController.Instance.PlaySFXSound("건축 실패시 결과화면 소리");
        }
        else
        {
            SoundController.Instance.PlaySFXSound("건축 성공시 결과화면에서 나오는 소리");
        }

        endingCanvas.SetActive(false);
        isEndflag = true;

    }

    // Update is called once per frame
    void Update()
    {

        if (isEndflag)
        {
            playTime = timer.GetComponent<UI_Timer>()._currentTime;
            string time = "";

            Time.timeScale = 0f;
            endCanvas.SetActive(true);

            if ((int)playTime / 60 >= 10)
            {
                time += ((int)playTime / 60 + ":");
            }
            else
            {
                time += ("0" + (int)playTime / 60 + ":");
            }

            if ((int)playTime % 60 < 10)
            {
                time += ("0" + (int)playTime % 60);

            }
            else
            {
                time += ((int)playTime % 60);
            }

            currentScoreText.GetComponent<TextMeshProUGUI>().text = time;


            if (!PlayerPrefs.HasKey("highScore"))
            {
                PlayerPrefs.SetFloat("highScore", timer.GetComponent<UI_Timer>()._MAX_TIME);
            }

            float highScoreTime = PlayerPrefs.GetFloat("highScore");

            if (highScoreTime > playTime)
            {
                highScoreUI.SetActive(true);
                PlayerPrefs.SetFloat("highScore", playTime);
                PlayerPrefs.Save();
                highScoreTime = PlayerPrefs.GetFloat("highScore");
            }

            string highScore = "";

            if ((int)highScoreTime / 60 >= 10)
            {
                highScore += ((int)highScoreTime / 60 + ":");
            }
            else
            {
                highScore += ("0" + (int)highScoreTime / 60 + ":");
            }

            if ((int)highScoreTime % 60 < 10)
            {
                highScore += ("0" + (int)highScoreTime % 60);

            }
            else
            {
                highScore += ((int)highScoreTime % 60);
            }

            highScoreText.GetComponent<TextMeshProUGUI>().text = highScore;
            return;
        }




        if (house.GetComponent<HouseScript>().isWin)
        {

            //endingAnim.gameObject.SetActive(true);
            //ChangeAnimation("win");
            //StartCoroutine(WaitNonLoopAnim());

            endingCanvas.SetActive(true);
            StartCoroutine(WaitNonLoopAnim());
        }

        if (timer.GetComponent<UI_Timer>()._currentTime >= timer.GetComponent<UI_Timer>()._MAX_TIME)
        {
            //endingCanvas.SetActive(true);
            //ChangeAnimation("lose");
            //StartCoroutine(WaitNonLoopAnim());

            endingCanvas.SetActive(true);
            StartCoroutine(WaitNonLoopAnim());
        }

      



        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseCanvas.activeSelf || settingCanvas.activeSelf || guideCanvas.activeSelf)
            {
                changeTimeScale();
                pauseCanvas.SetActive(false);
                settingCanvas.SetActive(false);
                guideCanvas.SetActive(false);
            }
            else
            {
                changeTimeScale();
                pauseCanvas.SetActive(true);
            }
        }
    }

    public void changeTimeScale()
    {
        Debug.Log("Time Changed!");
        // if (Time.timeScale == 1f)
        // {
        //     Time.timeScale = 0f;
        // }
        // else
        // {
        //     Time.timeScale = 1f;
        // }

        Time.timeScale = MathF.Abs(1 - Time.timeScale);

        Debug.Log(Time.timeScale);
    }
}
