using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class CountScript : MonoBehaviour
{
    SpineMachineScript SMS;

    public TextMesh count;
    public GameObject countObj;
    public GameObject objImg;

    public GameObject Machine;

    // Start is called before the first frame update
    void Start()
    {
        objImg.SetActive(false);
        SMS = Machine.GetComponent<SpineMachineScript>();
    }

    // Update is called once per frame
    void Update()
    {
        IngreCount();
        if (SMS.currentState == SMS.destState)
        {
            countObj.SetActive(false);
            objImg.SetActive(true);
        }
        else
        {
            countObj.SetActive(true);
            objImg.SetActive(false);
        }
    }

    public void IngreCount()
    {
        float cnt = Machine.transform.childCount - 2;
        count.text = cnt.ToString() + "/2";
    }
}
