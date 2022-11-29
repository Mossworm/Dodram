using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class MachineButtonScript : MonoBehaviour
{
    SpriteRenderer thisImg;
    public Sprite downImg;
    public Sprite upImg;
    public float pushCheck;

    public GameObject machine;

    private void Start()
    {
        pushCheck = 1;
        thisImg = this.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        pushCheck += Time.deltaTime;
        thisImg.sprite = upImg;

        if(pushCheck < 0.1)
        {
            thisImg.sprite = downImg;
        }
        else
        {
            pushCheck = 0.1f;
        }
    }

    public void MachineRun()
    {
        buttonDown();
        machine.GetComponent<SpineMachineScript>().CraftOn();
    }

    public void buttonDown()
    {
        SoundController.Instance.PlaySFXSound("버튼");
        pushCheck = 0;
    }
}