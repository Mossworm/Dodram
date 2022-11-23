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
    public int recipeOrder = 0;

    public GameObject rockCheck;  //레시피 완성 체크용
    public GameObject mushCheck;
    public GameObject treeCheck;

    public GameObject[] ingredientArray;
    public GameObject Stonecutter;
    public GameObject Mill;
    public GameObject Sawmill;

    public GameObject FinalMachine;
    public int rockCnt;     //넣은 개수
    public int mushCnt;
    public int treeCnt;
    public int rockNeed;    //필요 개수
    public int mushNeed;
    public int treeNeed;

    public string[] ingredientStringArray;

    public int needNum;
    public List<GameObject> nowRecipe = new List<GameObject>();
    //public TextMeshProUGUI recipeText;
    public TextMeshProUGUI mushText;
    public TextMeshProUGUI rockText;
    public TextMeshProUGUI treeText;

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

        rockText.text = rockCnt + "/" + rockNeed;
        mushText.text = mushCnt + "/" + mushNeed;
        treeText.text = treeCnt + "/" + treeNeed;

        if (rockCnt == rockNeed)
        {
            rockCheck.GetComponent<RecipeDawnCheck>().check();
        }
        if (mushCnt == mushNeed)
        {
            mushCheck.GetComponent<RecipeDawnCheck>().check();
        }
        if (treeCnt == treeNeed)
        {
            treeCheck.GetComponent<RecipeDawnCheck>().check();
        }
    }

    public void RecipeSetting()
    {
        //randArray = GetRandomInt(needNum, 0, ingredientArray.Length);
        RecipeOrder();

        needNum = rockNeed + mushNeed + treeNeed;

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
            randArray = new[] { 0, 3, 6 };
        }
        else if (recipeOrder == 1)
        {
            randArray = new[] { 2, 4, 6 };
        }
        else if (recipeOrder == 2)
        {
            randArray = new[] { 2, 5, 7 };
        }


        if (randArray[0] == 0)
        {
            rockNeed = 1;
        }
        else if (randArray[0] == 1)
        {
            rockNeed = 2;
        }
        else if (randArray[0] == 2)
        {
            rockNeed = 3;
        }
        if (randArray[1] == 3)
        {
            mushNeed = 1;
        }
        else if (randArray[1] == 4)
        {
            mushNeed = 2;
        }
        else if (randArray[1] == 5)
        {
            mushNeed = 3;
        }
        if (randArray[2] == 6)
        {
            treeNeed = 1;
        }
        else if (randArray[2] == 7)
        {
            treeNeed = 2;
        }
        else if (randArray[2] == 8)
        {
            treeNeed = 3;
        }
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