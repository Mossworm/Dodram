using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.UI;

public class FinalMachineScript : MonoBehaviour
{
    public GameObject recipcheck;
    public GameObject mushcheck;
    public GameObject rockcheck;
    public GameObject treecheck;

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

    public GameObject production;

    public GameObject recipes;
    
    
    public GameObject prfGaugeBar;
    public GameObject canvas;
    private RectTransform gaugeBar;
    public float height = 0.0f;
    private Image nowGaugebar;
    

    void Start()
    {
        gaugeBar = Instantiate(prfGaugeBar, canvas.transform).GetComponent<RectTransform>();
        nowGaugebar = gaugeBar.transform.GetChild(0).GetComponent<Image>();
        state = MachineState.None;
    }
    
    void Update()
    {
        CraftOn();      //모든 재료가 들어갔을 때 바로 실행되게

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
        GameObject playerItem;
        playerItem = hand.transform.GetChild(0).gameObject;
        
        if (this.transform.childCount < recipes.GetComponent<RecipeScript>().needNum && state == MachineState.None) //기계에 들어간 재료가 레시피 재료보다 적은가?
        {
            for (int i = 0; i < recipes.GetComponent<RecipeScript>().nowRecipe.Count; i++) //현재 레시피의 배열(필요재료) 수 만큼 루프를 돌림
            {
                if (playerItem.name == recipes.GetComponent<RecipeScript>().nowRecipe[i].name) //루프를 돌리다가 플레이어가 넣으려 하는 아이템과 현재 필요한 레시피 재료가 같은가?
                {
                    for (int j = 0; j < this.transform.childCount; j++)
                    {
                        int r = 0;
                        int m = 0;
                        int t = 0;
                        if (this.transform.GetChild(j).name == "Stone_P_2")
                        {
                            r++;
                        }
                        else if (this.transform.GetChild(j).name == "Mill_P_2")
                        {
                            m++;
                        }
                        else if (this.transform.GetChild(j).name == "Wood_P_2")
                        {
                            t++;
                        }

                        if (playerItem.name == "Stone_P_2") //넣기 전 재료량을 다 넣었다면 더 넣지 않고 return 한다.
                        {
                            if (r == recipes.GetComponent<RecipeScript>().nowRockNeed)
                            {
                                return;
                            }
                        }
                        else if (playerItem.name == "Mill_P_2")
                        {
                            if (m == recipes.GetComponent<RecipeScript>().nowMushNeed)
                            {
                                return;
                            }
                        }
                        else if (playerItem.name == "Wood_P_2")
                        {
                            if (t == recipes.GetComponent<RecipeScript>().nowTreeNeed)
                            {
                                return;
                            }
                        }
                    }
                    playerItem.transform.SetParent(this.transform); //아니라면 그 아이템을 기계에 넣는다.
                    playerItem.SetActive(false);   
                }
            }
        }
        
    }

    public void CraftOn()   //제작 시작
    {
        if (recipes.GetComponent<RecipeScript>().needNum == this.transform.childCount)
        {
            if (state == MachineState.None)
            {
                for (int i = 0; i < this.transform.childCount; i++)
                { 
                    Destroy(this.transform.GetChild(i).gameObject);
                }
                Invoke("Crafting", craftTime);
                state = MachineState.Working;
            }   
        }
    }

    public void PickUp(GameObject hand) //꺼내기
    {
        if (state == MachineState.Destroying)
        {
            CreateDone(hand);
            recipcheck.GetComponent<RecipeScript>().recipeOrder++;
        }
    }

    public void Crafting()      //제작완성 및 삭제중 상태로 이동
    {
        state = MachineState.Destroying;
        workTime = 0;
    }
    

    public void CreateDone(GameObject hand)    //완성품 배출
    {
        var go =Instantiate(production, Vector2.zero, quaternion.identity);
        
        int index = go.name.IndexOf("(Clone)"); //이름뒤에 클론 붙는거 제거
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
    
    public void ChildDestroy() //자식 삭제 and 레시피 재생성
    {
        for (int i = 0; i < this.transform.childCount; i++)
        { 
            Destroy(this.transform.GetChild(i).gameObject);
        }
        mushcheck.GetComponent<RecipeDawnCheck>().checkInit();
        rockcheck.GetComponent<RecipeDawnCheck>().checkInit();
        treecheck.GetComponent<RecipeDawnCheck>().checkInit();
        recipes.GetComponent<RecipeScript>().RecipeSetting();
    } 
    
}
