using System;
using TMPro;
using UnityEngine;


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
    
    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        if (endCanvas.activeSelf && !isEndflag)
        {
            isEndflag = true;
            playTime = timer.GetComponent<UI_Timer>()._currentTime;
            string time = "";
            
            time += ((int)playTime / 60 + "분 ");
            time += ((int)playTime % 60 + "초");

            currentScoreText.GetComponent<TextMeshProUGUI>().text = time;


            if (!PlayerPrefs.HasKey("highScore"))
            {
                PlayerPrefs.SetFloat("highScore", timer.GetComponent<UI_Timer>()._MAX_TIME);
            }

            float highScoreTime = PlayerPrefs.GetFloat("highScore");

            if (highScoreTime > playTime)
            {
                highScoreUI.SetActive(true);
                PlayerPrefs.SetFloat("highScore",playTime);
                PlayerPrefs.Save();
                highScoreTime = PlayerPrefs.GetFloat("highScore");
            }
            
            string highScore = "";
            highScore += ((int)highScoreTime / 60 + "분 ");
            highScore += ((int)highScoreTime % 60 + "초");


            highScoreText.GetComponent<TextMeshProUGUI>().text = "최고 랭킹 : " + highScore;
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
        
        Time.timeScale = MathF.Abs(1-Time.timeScale);
        
        Debug.Log(Time.timeScale);
    }
}
