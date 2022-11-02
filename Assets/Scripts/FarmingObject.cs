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
    
    
    
    private void Start()
    {
        maxhp = hp;
        gaugeBar = Instantiate(prfGaugeBar, canvas.transform).GetComponent<RectTransform>();
        nowGaugebar = gaugeBar.transform.GetChild(0).GetComponent<Image>();
    }
    
    private void Update()
    {
        GaugeBar();
    }
    
    void GaugeBar()
    {
        Vector3 _gaugeBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        gaugeBar.position = _gaugeBarPos;

        if (hp == maxhp)
        {
            gaugeBar.gameObject.SetActive(false);
        }
        else
        {
            gaugeBar.gameObject.SetActive(true);
        }
        
        nowGaugebar.fillAmount = ((float)hp / (float)maxhp);
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

            Destroy(gaugeBar.gameObject);
            Destroy(gameObject);
    }
}