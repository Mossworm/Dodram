using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RecipeScript : MonoBehaviour
{
    private int[] randArray;
    private int[] recipeArray;
    public int recipeOrder = 0;

    public GameObject rockCheck;  //레시피 완성 체크용
    public GameObject mushCheck;
    public GameObject treeCheck;

    public GameObject[] ingredientArray;
    // public GameObject Stonecutter;
    // public GameObject Mill;
    // public GameObject Sawmill;

    public GameObject FinalMachine;
    public int rockCnt;     //넣은 개수
    public int mushCnt;
    public int treeCnt;
    public int nowRockNeed;    //필요 개수
    public int nowMushNeed;
    public int nowTreeNeed;
    public int nextRockNeed;
    public int nextMushNeed;
    public int nextTreeNeed;

    public string[] ingredientStringArray;

    public int needNum;
    public List<GameObject> nowRecipe = new List<GameObject>();
    //public TextMeshProUGUI recipeText;
    public TextMeshProUGUI nowMushText;
    public TextMeshProUGUI nowRockText;
    public TextMeshProUGUI nowTreeText;
    public TextMeshProUGUI nextMushText;
    public TextMeshProUGUI nextRockText;
    public TextMeshProUGUI nextTreeText;

    private void Start()
    {
        rockCnt = 0;
        mushCnt = 0;
        treeCnt = 0;
    }

    private void Update()
    {
        RecipeSetting();
        RecipeTextUpdate();
        CountIngre();
    }

    void RecipeTextUpdate()
    {
        //string ingredientlist = "";

        //for (int i = 0; i < randArray.Length; i++)
        //{
        //    ingredientlist += (ingredientStringArray[randArray[i]].ToString() + "\n");
        //}

        //recipeText.text = ingredientlist;

        //mushText.text = FinalMachine.transform.childCount.ToString() + "/" + ingredientStringArray[randArray[0]].ToString();
        //rockText.text = Stonecutter.transform.childCount.ToString() + "/" + ingredientStringArray[randArray[1]].ToString();
        //treeText.text = Sawmill.transform.childCount.ToString() + "/" + ingredientStringArray[randArray[2]].ToString();


        //mushText.text = rockCnt + "/" + ingredientStringArray[randArray[0]].ToString();
        //rockText.text = mushCnt + "/" + ingredientStringArray[randArray[1]].ToString();
        //treeText.text = treeCnt + "/" + ingredientStringArray[randArray[2]].ToString();

        nowMushText.text = mushCnt + "/" + nowMushNeed;
        nowRockText.text = rockCnt + "/" + nowRockNeed;
        nowTreeText.text = treeCnt + "/" + nowTreeNeed;

        nextMushText.text = nextMushNeed.ToString();
        nextRockText.text =  nextRockNeed.ToString();
        nextTreeText.text =  nextTreeNeed.ToString();

        if (rockCnt == nowRockNeed)
        {
            rockCheck.GetComponent<RecipeDawnCheck>().check();
        }
        if (mushCnt == nowMushNeed)
        {
            mushCheck.GetComponent<RecipeDawnCheck>().check();
        }
        if (treeCnt == nowTreeNeed)
        {
            treeCheck.GetComponent<RecipeDawnCheck>().check();
        }
    }

    public void RecipeSetting()
    {
        //randArray = GetRandomInt(needNum, 0, ingredientArray.Length);
        RecipeOrder();

        needNum = nowRockNeed + nowMushNeed + nowTreeNeed;

        nowRecipe.Clear();

        for (int i = 0; i < randArray.Length; i++)
        {
            nowRecipe.Add(ingredientArray[randArray[i]]);
        }

    }

    //public int[] GetRandomInt(int length, int min, int max)
    //{
    //    int[] randArray = new int[length];
    //    bool isSame;

    //    for (int i = 0; i < length; i++)
    //    {
    //        while (true)
    //        {
    //            randArray[i] = Random.Range(min, max);
    //            isSame = false;

    //            for (int j = 0; j < i; j++)
    //            {
    //                if (randArray[j] == randArray[i])
    //                {
    //                    isSame = true;
    //                    break;
    //                }
    //            }
    //            if(!isSame) break;
    //        }
    //    }
    //    return randArray;
    //}

    void RecipeOrder()
    {
        if (recipeOrder == 0)
        {
            randArray = new[] { 0, 3, 6 }; //레시피 용으로 남겨둠

            nowMushNeed = 1;
            nowRockNeed = 1;
            nowTreeNeed = 1;

            nextMushNeed = 1;
            nextRockNeed = 2;
            nextTreeNeed = 1;
        }
        else if (recipeOrder == 1)
        {
            randArray = new[] { 0, 4, 6 };

            nowMushNeed = 1;
            nowRockNeed = 2;
            nowTreeNeed = 1;

            nextMushNeed = 1;
            nextRockNeed = 2;
            nextTreeNeed = 1;
        }
        else if (recipeOrder == 2)
        {
            randArray = new[] { 0, 4, 6 };

            nowMushNeed = 1;
            nowRockNeed = 2;
            nowTreeNeed = 1;

            nextMushNeed = 1;
            nextRockNeed = 2;
            nextTreeNeed = 1;
        }
        else if (recipeOrder == 3)
        {
            randArray = new[] { 0, 4, 6 };

            nowMushNeed = 1;
            nowRockNeed = 2;
            nowTreeNeed = 1;

            nextMushNeed = 1;
            nextRockNeed = 2;
            nextTreeNeed = 2;
        }
        else if (recipeOrder == 4)
        {
            randArray = new[] { 0, 4, 7 };

            nowMushNeed = 1;
            nowRockNeed = 2;
            nowTreeNeed = 2;

            nextMushNeed = 0;
            nextRockNeed = 0;
            nextTreeNeed = 0;
        }


        //if (randArray[0] == 0)
        //{
        //    nowRockNeed = 1;
        //}
        //else if (randArray[0] == 1)
        //{
        //    nowRockNeed = 2;
        //}
        //else if (randArray[0] == 2)
        //{
        //    nowRockNeed = 3;
        //}
        //if (randArray[1] == 3)
        //{
        //    nowMushNeed = 1;
        //}
        //else if (randArray[1] == 4)
        //{
        //    nowMushNeed = 2;
        //}
        //else if (randArray[1] == 5)
        //{
        //    nowMushNeed = 3;
        //}
        //if (randArray[2] == 6)
        //{
        //    nowTreeNeed = 1;
        //}
        //else if (randArray[2] == 7)
        //{
        //    nowTreeNeed = 2;
        //}
        //else if (randArray[2] == 8)
        //{
        //    nowTreeNeed = 3;
        //}
    }

    void CountIngre()
    {
        int rc = 0;
        int mc = 0;
        int tc = 0;
        for (int a = 0; a < FinalMachine.transform.childCount; a++)
        {
            if (FinalMachine.transform.GetChild(a).name == "Stone_P_2")
            {
                rc++;
            }
            else if (FinalMachine.transform.GetChild(a).name == "Mill_P_2")
            {
                mc++;
            }
            else if (FinalMachine.transform.GetChild(a).name == "Wood_P_2")
            {
                tc++;
            }
        }

        rockCnt = rc;
        mushCnt = mc;
        treeCnt = tc;
    }

}