using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class CountDownScript : MonoBehaviour
{
    public float countDown;
    public GameObject UI;
    public GameObject count3;
    public GameObject count2;
    public GameObject count1;
    public GameObject gameStart;

    void Start()
    {
        count3.SetActive(false);
        count2.SetActive(false);
        count1.SetActive(false);
        gameStart.SetActive(false);

        countDown = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        CountDown();
    }

    void CountDown()
    {
        if(GameObject.Find("Main Camera").GetComponent<Camera>().enabled == false)
        {
            if (UI.activeSelf == false)
            {
                UI.SetActive(true);
            }
            
            countDown += Time.deltaTime;
            //if (countDown <= 1)
            //{
            //    count3.SetActive(true);
            //}
            //else if (1 <= countDown && countDown <= 2)
            //{
            //    count3.SetActive(false);
            //    count2.SetActive(true);
            //}
            //else if (2 <= countDown && countDown <= 3)
            //{
            //    count2.SetActive(false);
            //    count1.SetActive(true);
            //}
            //else if (3 <= countDown && countDown < 4)
            //{
            //    count1.SetActive(false);
            //    gameStart.SetActive(true);
            //}
            //else if (countDown >= 4)
            //{
                gameStart.SetActive(false);
                countDown = 4f;
                Managers.isReady = true;
                this.GetComponent<CountDownScript>().enabled = false;
            //}
        }
    }
}