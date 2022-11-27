using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SpineFarmingObject : MonoBehaviour
{
    public int hp = 3;
    public int maxhp;
    public GameObject dropItem;
    private GameObject prefab_obj;

    public GameObject prfGaugeBar;
    public GameObject canvas;
    [SerializeField]private RectTransform gaugeBar;
    public float height = 0.0f;
    private Image nowGaugebar;
    
    public float regenTime;

    public bool isTarget;

    private Animator _animator;
    [SerializeField] private string currentAnimState;
    [SerializeField] private string prefixString;

    [SerializeField] private float dropAnimTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        dropAnimTime = 0.65f; /*_animator.GetComponent<SpineAnimationBehavior>().exitTime;*/

        maxhp = hp;
        isTarget = false;
        if (canvas == null)
        {
            canvas = GameObject.Find("ObjectCanvas");
        }

        gaugeBar = Instantiate(prfGaugeBar, canvas.transform).GetComponent<RectTransform>();
        nowGaugebar = gaugeBar.transform.GetChild(0).GetComponent<Image>();
        nowGaugebar.color = new Color(143/ 255.0f, 206/ 255.0f, 90/ 255.0f);


        switch (tag)
        {
            case "Grass":
                prefixString = "grass";
                break;
            case "Stone":
                prefixString = "rock";
                break;
            case "Tree":
                prefixString = "tree";
                break;
            default:
                break;
        }

        ChangeAnimation(prefixString + "_Idle");
    }
    
    private void Update()
    {
        if (hp == maxhp)
        {
            ChangeAnimation(prefixString + "_Idle");
        }
        GaugeBar();
    }
    
    void GaugeBar()
    {
        Vector3 _gaugeBarPos = new Vector3(transform.position.x, transform.position.y + height, 0);
        gaugeBar.position = _gaugeBarPos; //오류고쳐야함. 

        if (hp == maxhp && isTarget == false) 
        {
            gaugeBar.gameObject.SetActive(false);
        }
        else if(hp > 0)
        {
            gaugeBar.gameObject.SetActive(true);
        }
        
        nowGaugebar.fillAmount = ((float)(maxhp-hp) / (float)maxhp);
    }

    public void Digging()
    {
        ChangeAnimation(prefixString + "_use");
        hp--;

        if (hp == 0)
        {
            Drop();
            gaugeBar.gameObject.SetActive(false);
        }
    }

    public void Drop()
    {
        ChangeAnimation(prefixString + "_down");
        prefab_obj = Instantiate(dropItem);
        prefab_obj.transform.position = new Vector3(transform.position.x,transform.position.y-0.01f,transform.position.z);
        prefab_obj.name = dropItem.name;
        StartCoroutine(Regen());
    }

    IEnumerator Regen()
    {
        Vector3 tempPosition = this.transform.position;

        yield return new WaitForSeconds(dropAnimTime);
        transform.position = new Vector3(0.0f,-60.0f,0.0f);

        yield return new WaitForSeconds(regenTime);
        isTarget = false;
        transform.position = tempPosition;
        hp = maxhp;
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