using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MachineScript : MonoBehaviour
{
    public GameObject recipecheck;  //레시피 체크용

    public float craftTime;
    public float destroyTime;
    public float workTime;

    public enum MachineState
    {
        None,
        Working,
        Destroying
    }

    public MachineState state;

    public GameObject ingredient;

    public GameObject[] productionArray;
    
    
    public GameObject prfGaugeBar;
    public GameObject canvas;
    private RectTransform gaugeBar;
    public float height = 0.0f;
    private Image nowGaugebar;
    
    
    private void Start()
    {
        gaugeBar = Instantiate(prfGaugeBar, canvas.transform).GetComponent<RectTransform>();
        nowGaugebar = gaugeBar.transform.GetChild(0).GetComponent<Image>();
        state = MachineState.None;
    }

    private void Update()
    {
        GaugeBar();
        if (state != MachineState.None)
        {
            workTime += Time.deltaTime;
        }
        
        
        if (state == MachineState.Destroying)
        {
            if (workTime >= destroyTime)
            {
                ChildDestroy();
                state = MachineState.None;
                workTime = 0;
            }
        }
        
    }
    
    void GaugeBar()
    {
        Vector3 _gaugeBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
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
            nowGaugebar.color= Color.white;
        }
        else if (state == MachineState.Destroying)
        {
            nowGaugebar.fillAmount = 1 - (workTime / destroyTime);
            nowGaugebar.color= Color.red;
        }
        
    }

    public void SubCount(GameObject hand)      //기계에 넣기 
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

    public void CraftOn()   //제작 시작
    {
        if (state == MachineState.None && this.transform.childCount != 0)
        {
            Invoke("Crafting", craftTime);
            state = MachineState.Working;
        }
    }

    public void PickUp(GameObject hand) //꺼내기
    {
        if (state == MachineState.Destroying)
        {
            CreateDone(hand);
            recipecheck.GetComponent<RecipeDawnCheck>().check();
        }
    }

    public void Crafting()      //제작완성 및 삭제중 상태로 이동
    {
        state = MachineState.Destroying;
        workTime = 0;
    }
    

    public void CreateDone(GameObject hand)   //완성품 꺼내기
    {
        var go =Instantiate(productionArray[transform.childCount-1], Vector2.zero, quaternion.identity);

        int index = go.name.IndexOf("(Clone)");
        if (index > 0)
        {
            go.name = go.name.Substring(0, index);
        }
        
        go.transform.SetParent(hand.transform);
        go.transform.localPosition = Vector2.zero;
        go.layer = 0;
        state = MachineState.None;
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
     


}