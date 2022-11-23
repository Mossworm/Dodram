using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Babi_car : MonoBehaviour
{
    public GameObject Timer;
    public GameObject BabiCar;
    float _MAX_TIME;

    [SerializeField] RectTransform rect;
    [SerializeField] Vector3 timerPosition;
    [SerializeField] Vector3 startPosition;
    [SerializeField] Vector3 babiPosition;


    // Start is called before the first frame update
    void Start()
    {
        _MAX_TIME = Timer.GetComponent<UI_Timer>()._MAX_TIME;
        rect = Timer.GetComponent<RectTransform>();
        timerPosition = Timer.transform.localPosition;
        startPosition = new Vector3(timerPosition.x + (rect.sizeDelta.x / 2) -10, timerPosition.y, timerPosition.z);
        babiPosition = BabiCar.transform.localPosition;
        babiPosition = startPosition;
        //BabiCar.transform.localPosition = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //    babiPosition = BabiCar.transform.localPosition;
        if (babiPosition.x <= (timerPosition.x - rect.sizeDelta.x / 2)){
            return;
        }
        babiPosition = new Vector3(babiPosition.x - rect.sizeDelta.x / _MAX_TIME, babiPosition.y, babiPosition.z);
        BabiCar.transform.localPosition = babiPosition;
    }
}

