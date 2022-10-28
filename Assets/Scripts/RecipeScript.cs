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

    public GameObject[] ingredientArray;
    public GameObject Stonecutter;
    public GameObject Mill;
    public GameObject Sawmill;

    public string[] ingredientStringArray;

    public int needNum;
    public List<GameObject> nowRecipe = new List<GameObject>();
    //public TextMeshProUGUI recipeText;
    public TextMeshProUGUI mushText;
    public TextMeshProUGUI rockText;
    public TextMeshProUGUI treeText;

    private void Start()
    {
        
    }

    private void Update()
    {
        RecipeSetting();
        RecipeTextUpdate();
    }

    void RecipeTextUpdate()
    {
        //string ingredientlist = "";
        
        //for (int i = 0; i < randArray.Length; i++)
        //{
        //    ingredientlist += (ingredientStringArray[randArray[i]].ToString() + "\n");
        //}

        //recipeText.text = ingredientlist;

        mushText.text = Mill.transform.childCount.ToString() + "/" + ingredientStringArray[randArray[0]].ToString();
        rockText.text = Stonecutter.transform.childCount.ToString() + "/" + ingredientStringArray[randArray[1]].ToString();
        treeText.text = Sawmill.transform.childCount.ToString() + "/" + ingredientStringArray[randArray[2]].ToString();
    }

    public void RecipeSetting()
    {
        //randArray = GetRandomInt(needNum, 0, ingredientArray.Length);
        RecipeOrder();

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
    }

}