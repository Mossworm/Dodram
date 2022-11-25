using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpineMachineScript : MonoBehaviour
{
    public GameObject recipecheck;  //������ üũ��
    public bool isBreak;
    public bool randomON;

    public int per;
    public float craftTime;
    public float destroyTime;
    public float workTime;
    public float stopTime;

    private Animator _animator;
    public string currentState;

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
        _animator = GetComponent<Animator>();
        gaugeBar = Instantiate(prfGaugeBar, canvas.transform).GetComponent<RectTransform>();
        nowGaugebar = gaugeBar.transform.GetChild(0).GetComponent<Image>();
        state = MachineState.None;
        saveState = state;
        if (randomON == true)
        {
            InvokeRepeating("Breakdown", 1f, 1f);
        }
        ChangeAnimation("Grass_Machine_Idle");
    }

    private void Update()
    {
        GaugeBar();
        if (isBreak == false)
        {
            if (state == MachineState.Working)
            {
                ChangeAnimation("Grass_Machine_Use");
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
            nowGaugebar.color = Color.white;
        }
        else if (state == MachineState.Destroying)
        {
            nowGaugebar.fillAmount = 1 - (workTime / destroyTime);
            nowGaugebar.color = Color.red;
        }
        else if (state == MachineState.Breakdown)
        {
            nowGaugebar.color = Color.gray;
        }

    }

    public void SubCount(GameObject hand)      //��迡 �ֱ� 
    {
        if (isBreak == false)
        {
            if (this.transform.childCount < productionArray.Length && state == MachineState.None)
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

    public void CraftOn()   //���� ����
    {
        if (state == MachineState.None && this.transform.childCount != 0)
        {
            //Invoke("Crafting", craftTime);
            state = MachineState.Working;
            saveState = state;
        }
    }

    public void PickUp(GameObject hand) //������
    {
        if (isBreak == false)
        {
            if (state == MachineState.Destroying)
            {
                CreateDone(hand);
                recipecheck.GetComponent<RecipeDawnCheck>().check();
            }
        }
    }

    public void Crafting()      //���ۿϼ� �� ������ ���·� �̵�
    {
            //state = MachineState.Destroying;
            saveState = state;
            //workTime = 0;
    }


    public void CreateDone(GameObject hand)   //�ϼ�ǰ ������
    {
        var go = Instantiate(productionArray[transform.childCount - 1], Vector2.zero, quaternion.identity);

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

    public void ChildDestroy() //�ڽ� ����
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }

    public void Breakdown()
    {
        int max = Random.Range(0, 100);
        if (max < per)
        {
            state = MachineState.Breakdown;
            isBreak = true;
        }
    }


    public void MachinePix()
    {
        state = saveState;
        isBreak = false;
    }

    void ChangeAnimation(string newState)
    {
        if (currentState == newState)
        {
            return;
        }

        _animator.Play(newState);

        currentState = newState;
    }

}