
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
    [SerializeField] private float explosionAnimTime;

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
    public MachineState destState;

    public GameObject ingredient;

    public GameObject[] productionArray;


    public GameObject prfGaugeBar;
    public GameObject canvas;
    private RectTransform gaugeBar;
    public float height = 0.0f;
    private Image nowGaugebar;

    public GameObject breakIcon;
    public Vector3 breakIconPos;
    private RectTransform breakIconRect;


    private void Start()
    {
        explosionAnimTime = 2f;
        _animator = GetComponent<Animator>();
        Max_breakCoolTime = breakCoolTime;

        if (canvas == null)
        {
            canvas = GameObject.Find("ObjectCanvas");
        }

        gaugeBar = Instantiate(prfGaugeBar, canvas.transform).GetComponent<RectTransform>();
        nowGaugebar = gaugeBar.transform.GetChild(0).GetComponent<Image>();

        breakIconRect = Instantiate(breakIcon, canvas.transform).GetComponent<RectTransform>();
        breakIconPos = new Vector3(transform.position.x, transform.position.y + height, 0);

        currentState = MachineState.None;
        destState= MachineState.Destroying; 
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
        ChangeAnimation(prefixString + "_Machine_Idle");
    }

    private void Update()
    {
        GaugeBar();
        BreakCoolDown();

        breakIconRect.position = breakIconPos;

        if (isBreak)
        {
            currentState = MachineState.Breakdown;
            breakIconRect.gameObject.SetActive(true);
        }
        else
        {
            breakIconRect.gameObject.SetActive(false);
        }

        switch (currentState)
        {
            case MachineState.None:
                ChangeAnimation(new String(prefixString + "_Machine_Idle"));
                break;

            case MachineState.Working:
                if (workTime >= craftTime)
                {
                    if (prefixString == "Grass")
                    {
                        ChangeAnimation(prefixString + "_Muchine_Overload");
                    }
                    else
                    {
                        ChangeAnimation(prefixString + "_Machine_Overload");
                    }
                    //workTime = 0;
                    Crafting();
                    //currentState = MachineState.Destroying;
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
                    StartCoroutine(WaitNonLoopAnim());
                    ChildDestroy();
                }
                else
                {
                    if (prefixString == "Grass")
                    {
                        ChangeAnimation(prefixString + "_Muchine_Overload");
                    }
                    else
                    {
                        ChangeAnimation(prefixString + "_Machine_Overload");
                    }
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
    }

    IEnumerator WaitNonLoopAnim()
    {
        yield return new WaitForSeconds(explosionAnimTime);
        workTime = 0;
        saveState = currentState;
        currentState = MachineState.None;

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
            if (this.transform.childCount < 4 && currentState == MachineState.None)
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
        if (currentState == MachineState.None && this.transform.childCount == 2 + 1 + 1) //2:필요한재료개수, 1:스파인스켈레톤, 1:말풍선오브젝트
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
        currentState = MachineState.Destroying;
        saveState = currentState;
        workTime = 0;
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
        ChildDestroy();
    }

    public void ChildDestroy() //자식 삭제
    {
        workTime = 0;
        stopTime = 0;
        currentState = MachineState.None;
        saveState = MachineState.None;
        for (int i = 1; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).name == "wood" || this.transform.GetChild(i).name == "chip" || this.transform.GetChild(i).name == "cobblestone")
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
        }
    }

    public void Breakdown()
    {
        int max = Random.Range(0, 100);
        if (breakCoolTime <= 0 && isBreak == false)
        {
            if (max < per)
            {
                currentState = MachineState.Breakdown;
                isBreak = true;
                if (workTime > 0)
                {
                    breakIconPos = new Vector3(transform.position.x, transform.position.y + height + 0.8f, 0);
                }
                else
                {
                    breakIconPos = new Vector3(transform.position.x, transform.position.y + height, 0);
                }
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