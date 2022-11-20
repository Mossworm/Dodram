using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneScript : MonoBehaviour
{
    [SerializeField] private int currentImage;
    
    public GameObject[] CutScenes;

    public float cutSceneSpeed;
    private float speed;

    [SerializeField]private bool isCutEnd;

    private float currentTime;

    public GameObject nextButton;
    public GameObject nextCutScene;

    public bool EndSceneManager;

    public GameObject sceneChanger;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2;
        currentImage = -1;
        isCutEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentImage < CutScenes.Length-1)
        {
            currentTime += Time.deltaTime;
            
            if (currentTime >= cutSceneSpeed)
            {
                currentImage++;
                CutScenes[currentImage].SetActive(true);
                currentTime = 0;
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                cutSceneSpeed = 0.0f;
            }
        }
        
        if (currentImage == CutScenes.Length - 1)
        {
            if (!isCutEnd)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= 1.0f)
                {
                    nextButton.SetActive(true);   
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isCutEnd = true;
                nextButton.SetActive(false);
                currentTime = 0;
            }

            if (isCutEnd != true) return;
            if (EndSceneManager) //마지막 캔버스라면 다음씬으로 이동
            {
                if (currentTime != 0) return;
                currentTime += Time.deltaTime;
                sceneChanger.GetComponent<SceneManagement>().SceneChanger();
            }
            else //아니라면 현재 이미지 비활성화 + 다음 캔버스 호출
            {
                foreach (var t in CutScenes)
                {
                    t.SetActive(false);
                }  
                nextCutScene.SetActive(true);
            }
        }
    }
}
