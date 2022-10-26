using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FarmingObject : MonoBehaviour
{
    public int hp = 3;
    public GameObject dropItem;
    private GameObject prefab_obj;

    private void Update()
    {
        Drop();
    }



    public void Digging()
    {
        //prefab_obj = Instantiate(dropItem);
        //prefab_obj.transform.position = this.transform.position;
        //prefab_obj.name = dropItem.name;

        //Destroy(gameObject);
        hp--;
    }

    public void Drop()
    {
        if (hp == 0)
        {
            prefab_obj = Instantiate(dropItem);
            prefab_obj.transform.position = this.transform.position;
            prefab_obj.name = dropItem.name;

            Destroy(gameObject);
        }
    }
}