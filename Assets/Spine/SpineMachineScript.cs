using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpineMachineScript : MonoBehaviour
{
    //public GameObject recipecheck;  //레시피 체크용
    public bool isBreak;
    public bool randomON;

    public int per;
    public float craftTime;
    public float destroyTime;
    public float workTime;
    public float stopTime;
    public float breakCoolTime;
    private float Max_breakCoolTime;

    private Animator _animator;
    public string currentAnimState;

    [SerializeField] private string prefixString;

    public enum MachineState
    {
        None,
        Working,
        Destroying,
        Breakdown
    }

    public MachineState currentState;
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
        Max_breakCoolTime = breakCoolTime;

        if (canvas == null)
        {
            canvas = GameObject.Find("ObjectCanvas");
        }

        gaugeBar = Instantiate(prfGaugeBar, canvas.transform).GetComponent<RectTransform>();
        nowGaugebar = gaugeBar.transform.GetChild(0).GetComponent<Image>();
        currentState = MachineState.None;
        saveState = currentState;
        if (randomON == true)
        {
            InvokeRepeating("Breakdown", 1f, 1f);
        }

        switch (ingredient.name)
        {
            case "chip":
                prefixString = "Grass";
                break;
            case "cobblestone":
                prefixString = "Rock";
                break;
            case "wood":
                prefixString = "Tree";
                break;
            default:
                break;
        }
        ChangeAnimation(prefixString+"_Machine_Idle");
    }

    private void Update()
    {
        GaugeBar();
        BreakCoolDown();

        if (isBreak) {
            currentState = MachineState.Breakdown;
        }

        switch (currentState)
        {
            case MachineState.None:
                ChangeAnimation(new String(prefixString + "_Machine_Idle"));
                break;

            case MachineState.Working:
                if (workTime >= craftTime)
                {
                    ChangeAnimation(prefixString + "_Machine_Overload");
                    workTime = 0;
                    currentState = MachineState.Destroying;
                }
                else
                {
                    ChangeAnimation(prefixString + "_Machine_Use");
                    workTime += Time.deltaTime;
                    stopTime = workTime;
                }
                break;

            case MachineState.Destroying:
                if (workTime >= destroyTime)
                {
                    ChangeAnimation(prefixString + "_Machine_Explosion");
                    ChildDestroy();
                    currentState = MachineState.None;
                    workTime = 0;
                }
                else
                {
                    ChangeAnimation(prefixString + "_Machine_Overload");
                    Crafting();
                    workTime += Time.deltaTime;
                    stopTime = workTime;
                }
                break;

            case MachineState.Breakdown:
                {
                    ChangeAnimation(new String(prefixString + "_Machine_Broken"));
                    workTime = stopTime;
                }
                break;

            default:
                break;
        }
            saveState = currentState;    
    }

    void GaugeBar()
    {
        Vector3 _gaugeBarPos = new Vector3(transform.position.x, transform.position.y + height, 0);
        gaugeBar.position = _gaugeBarPos;

        if (currentState == MachineState.None)
        {
            gaugeBar.gameObject.SetActive(false);
        }
        else if (currentState == MachineState.Working || currentState == MachineState.Destroying)
        {
            gaugeBar.gameObject.SetActive(true);
        }

        if (currentState == MachineState.Working)
        {
            nowGaugebar.fillAmount = workTime / craftTime;
            nowGaugebar.color = new Color(38 / 255.0f, 162 / 255.0f, 123 / 255.0f);
        }
        else if (currentState == MachineState.Destroying)
        {
            nowGaugebar.fillAmount = 1 - (workTime / destroyTime);
            nowGaugebar.color = new Color(172 / 255.0f, 67 / 255.0f, 63 / 255.0f);
        }
        else if (currentState == MachineState.Breakdown)
        {
            //SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();
            //sr.material.color = Color.red;
            if (gaugeBar.gameObject.activeSelf == true)
            {
                nowGaugebar.color = Color.gray;
            }
        }

    }

    public void SubCount(GameObject hand)      //기계에 넣기 
    {
        if (isBreak == false)
        {
            if (this.transform.childCount < 3 && currentState == MachineState.None)
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
        if (currentState == MachineState.None && this.transform.childCount == 3)
        {
            //Invoke("Crafting", craftTime);
            currentState = MachineState.Working;
            saveState = currentState;
        }
    }

    public void PickUp(GameObject hand) //꺼내기
    {
        if (isBreak == false)
        {
            if (currentState == MachineState.Destroying)
            {
                CreateDone(hand);
                //recipecheck.GetComponent<RecipeDawnCheck>().check();
            }
        }
    }

    public void Crafting()      //제작완성 및 삭제중 상태로 이동
    {
        //state = MachineState.Destroying;
        saveState = currentState;
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
        currentState = MachineState.None;
        saveState = currentState;
        workTime = 0;
        ChildDestroy();
    }

    public void ChildDestroy() //자식 삭제
    {
        for (int i = 1; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }

    public void Breakdown()
    {
        int max = Random.Range(0, 100);
        if (breakCoolTime <= 0)
        {
            if (max < per)
            {
                currentState = MachineState.Breakdown;
                isBreak = true;
            }
        }
    }


    public void MachinePix()
    {
        currentState = saveState;
        isBreak = false;
        breakCoolTime = Max_breakCoolTime;
    }
    public void BreakCoolDown()
    {
        breakCoolTime -= Time.deltaTime;
        if (breakCoolTime <= 0)
        {
            breakCoolTime = 0;
        }
    }
    void ChangeAnimation(string newAnimState)
    {
        if (currentAnimState == newAnimState)
        {
            return;
        }

        _animator.Play(newAnimState);

        currentAnimState = newAnimState;
    }

}