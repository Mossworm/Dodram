using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameObject.Find("EndCanvas") != null){
                //SceneManager.LoadScene("StartScene");
            }
            else
            {
                if (pauseCanvas.activeSelf)
                {
                    changeTimeScale();
                    pauseCanvas.SetActive(false);
                }
                else
                {
                    changeTimeScale();
                    pauseCanvas.SetActive(true);
                }
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
