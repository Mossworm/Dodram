using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Babi_car : MonoBehaviour
{
    
    public RectTransform rect;
    public GameObject timer;
    [SerializeField] float startPosition_x;
    [SerializeField] float endPosition_x;
    private float origin_x;


    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        origin_x = rect.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float value = timer.GetComponent<Image>().fillAmount;

        float current_xScale = (startPosition_x - endPosition_x);
        float current_x = current_xScale * value;

        rect.position = new Vector3(origin_x-current_x, rect.transform.position.y, rect.transform.position.z);
    }
}

