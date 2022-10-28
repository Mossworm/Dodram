using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeDawnCheck : MonoBehaviour
{
    public GameObject list;
    public GameObject chk;

    public void check()
    {
        list.SetActive(false);
        chk.SetActive(true);
    }

    public void checkInit()
    {
        list.SetActive(true);
        chk.SetActive(false);
    }

}