using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FarmingObject : MonoBehaviour
{
    public int hp = 3;
    public int maxhp;
    public GameObject dropItem;
    private GameObject prefab_obj;

    public GameObject prfGaugeBar;
    public GameObject canvas;
    private RectTransform gaugeBar;
    public float height = 0.0f;
    private Image nowGaugebar;
    
    public float regenTime;

    public bool isTarget;

    private void Start()
    {
        maxhp = hp;
        isTarget = false;
        if (canvas == null)
        {
            canvas = GameObject.Find("ObjectCanvas");
        }
        
        gaugeBar = Instantiate(prfGaugeBar, canvas.transform).GetComponent<RectTransform>();
        nowGaugebar = gaugeBar.transform.GetChild(0).GetComponent<Image>();
    }
    
    private void Update()
    {
        GaugeBar();
    }
    
    void GaugeBar()
    {
        Vector3 _gaugeBarPos = new Vector3(transform.position.x, transform.position.y + height, 0);
        gaugeBar.position = _gaugeBarPos;

        if (hp == maxhp && isTarget == false) 
        {
            gaugeBar.gameObject.SetActive(false);
        }
        else
        {
            gaugeBar.gameObject.SetActive(true);
        }
        
        nowGaugebar.fillAmount = ((float)(maxhp-hp) / (float)maxhp);
    }

    public void Digging()
    {
        hp--;
        
        if (hp == 0) Drop();
    }

    public void Drop()
    {
        prefab_obj = Instantiate(dropItem);
        prefab_obj.transform.position = this.transform.position;
        prefab_obj.name = dropItem.name;
        StartCoroutine(Regen());
    }

    IEnumerator Regen()
    {
        gaugeBar.gameObject.SetActive(false);
        isTarget = false;
        Vector3 tempPosition = this.transform.position;
        transform.position = new Vector3(0.0f,-60.0f,0.0f);

        yield return new WaitForSeconds(regenTime);

        transform.position = tempPosition;
        hp = maxhp;
    }
    
}