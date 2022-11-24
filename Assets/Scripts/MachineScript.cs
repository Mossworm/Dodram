using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MachineScript : MonoBehaviour
{
    public GameObject recipecheck;  //레시피 체크용
    public bool isBreak;
    public bool randomON;

    public int per;
    public float craftTime;
    public float destroyTime;
    public float workTime;
    public float stopTime;
    public float breakCoolTime;

    public enum MachineState
    {
        None,
        Working,
        Destroying,
        Breakdown
    }

    public MachineState state;
    public MachineState saveState;

    public GameObject ingredient;

    public GameObject[] productionArray;


    public GameObject prfGaugeBar;
    public GameObject canvas;
    private RectTransform gaugeBar;
    public float height = 0.0f;
    private Image nowGaugebar;


    private void Start()
    {
        breakCoolTime = 5f;
        gaugeBar = Instantiate(prfGaugeBar, canvas.transform).GetComponent<RectTransform>();
        nowGaugebar = gaugeBar.transform.GetChild(0).GetComponent<Image>();
        state = MachineState.None;
        saveState = state;
        if (randomON == true)
        {
            InvokeRepeating("Breakdown", 1f, 1f);
        }
    }

    private void Update()
    {
        GaugeBar();
        BreakCoolDown();
        if (isBreak == false)
        {
            if (state == MachineState.Working)
            {
                workTime += Time.deltaTime;
                stopTime = workTime;
            }
            else if (state == MachineState.Destroying)
            {
                Crafting();
                workTime += Time.deltaTime;
                stopTime = workTime;
            }
            else if(state == MachineState.Breakdown)
            {
                workTime = stopTime;
            }

            if(state == MachineState.Working)
            {
                if (workTime >= craftTime)
                {
                    workTime = 0;
                    state = MachineState.Destroying;
                }
            }
            else if (state == MachineState.Destroying)
            {
                if (workTime >= destroyTime)
                {
                    ChildDestroy();
                    state = MachineState.None;
                    saveState = state;
                    workTime = 0;
                }
            }
        }
    }

    void GaugeBar()
    {
        Vector3 _gaugeBarPos = new Vector3(transform.position.x, transform.position.y + height, 0);
        gaugeBar.position = _gaugeBarPos;

        if (state == MachineState.None)
        {
            gaugeBar.gameObject.SetActive(false);
        }
        else
        {
            gaugeBar.gameObject.SetActive(true);
        }

        if (state == MachineState.Working)
        {
            nowGaugebar.fillAmount = workTime / craftTime;
            nowGaugebar.color = new Color(38, 162, 123);
        }
        else if (state == MachineState.Destroying)
        {
            nowGaugebar.fillAmount = 1 - (workTime / destroyTime);
            nowGaugebar.color = new Color(172, 67, 63);
        }
        else if (state == MachineState.Breakdown)
        {
            nowGaugebar.color = Color.gray;
        }

    }

    public void SubCount(GameObject hand)      //기계에 넣기 
    {
        if (isBreak == false)
        {
            if (this.transform.childCount < 2 && state == MachineState.None)
            {
                GameObject playerItem;
                playerItem = hand.transform.GetChild(0).gameObject;
                if (ingredient.name == playerItem.name)
                {
                    playerItem.transform.SetParent(this.transform);
                    playerItem.SetActive(false);
                }
            }
        }
    }

    public void CraftOn()   //제작 시작
    {
        if (state == MachineState.None && this.transform.childCount == 2)
        {
            //Invoke("Crafting", craftTime);
            state = MachineState.Working;
            saveState = state;
        }
    }

    public void PickUp(GameObject hand) //꺼내기
    {
        if (isBreak == false)
        {
            if (state == MachineState.Destroying)
            {
                CreateDone(hand);
                //recipecheck.GetComponent<RecipeDawnCheck>().check();
            }
        }
    }

    public void Crafting()      //제작완성 및 삭제중 상태로 이동
    {
            //state = MachineState.Destroying;
            saveState = state;
            //workTime = 0;
    }


    public void CreateDone(GameObject hand)   //완성품 꺼내기
    {
        var go = Instantiate(productionArray[1], Vector2.zero, quaternion.identity);

        int index = go.name.IndexOf("(Clone)");
        if (index > 0)
        {
            go.name = go.name.Substring(0, index);
        }

        go.transform.SetParent(hand.transform);
        go.transform.localPosition = Vector2.zero;
        go.layer = 0;
        state = MachineState.None;
        saveState = state;
        workTime = 0;
        ChildDestroy();
    }

    public void ChildDestroy() //자식 삭제
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }

    public void Breakdown()
    {
        int max = Random.Range(0, 100);
        if(breakCoolTime <= 0)
        {
            if (max < per)
            {
                state = MachineState.Breakdown;
                isBreak = true;
            }
        }
    }


    public void MachinePix()
    {
        state = saveState;
        isBreak = false;
        breakCoolTime = 10f;
    }
    public void BreakCoolDown()
    {
        breakCoolTime -= Time.deltaTime;
        if(breakCoolTime <= 0)
        {
            breakCoolTime = 0;
        }
    }
}