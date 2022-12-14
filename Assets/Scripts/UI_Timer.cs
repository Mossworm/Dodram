using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UI_Timer : MonoBehaviour
{
    public float _MAX_TIME;
    public float _currentTime;
    // public float width;
    // public Vector2 currentSize;
    // public float endWidth;

    public RectTransform rect;
    
    public GameObject endCanvas;
    public GameObject Timer;
    private float originX;
    

    void Start()
    {
        _currentTime = 0;
        Timer.GetComponent<Image>().fillAmount = (_currentTime / _MAX_TIME);
        originX = rect.localPosition.x;

        //rect = GetComponent<RectTransform>();
        //rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);
    }


    void Update()
    {
        if (!Managers.isReady)
        {
            return;
        }
        
        if (_currentTime >= _MAX_TIME && Time.timeScale != 0f)
        {
            //Time.timeScale = 0f;
            //endCanvas.SetActive(true);
        }
        else
        {
            _currentTime += Time.deltaTime;
            float amount = (_currentTime / _MAX_TIME);
            Timer.GetComponent<Image>().fillAmount = amount;
            float carRect = amount;
            rect.localPosition = new Vector3(originX-(696-(-577))*amount, rect.localPosition.y, rect.localPosition.z);




            //currentSize = new Vector2((width-endWidth) * (_currentTime / _MAX_TIME)+endWidth, rect.sizeDelta.y);
            //rect.sizeDelta = currentSize;    
        }
    }
}
