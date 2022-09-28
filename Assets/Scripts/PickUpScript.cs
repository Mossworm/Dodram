using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Serialization;

public class PickUpScript : MonoBehaviour
{
    public Vector2 size;
    public Vector3 boxTransform;
    public LayerMask whatIsLayer;
    public GameObject Hand;

    public bool isHold;

    [FormerlySerializedAs("DiggingPer")] public float GaugePer;

    public GameObject prfGaugeBar;
    public GameObject canvas;

    private RectTransform gaugeBar;
    public float height = 0.0f;
    private Image nowGaugebar;

    public float diggingSpd = 20.0f;

    [SerializeField] private GameObject targetObject;
    [SerializeField] private GameObject targetEndObject;

    [SerializeField] private Material whiteMaterial;
    [SerializeField] private Material originalMaterial;

    // Start is called before the first frame update
    void Start()
    {
        isHold = false;
        GaugePer = 0.0f;
        gaugeBar = Instantiate(prfGaugeBar, canvas.transform).GetComponent<RectTransform>();
        nowGaugebar = gaugeBar.transform.GetChild(0).GetComponent<Image>();
    }
    
    // Update is called once per frame
    void Update()
    {
        Interactive(); //��ȣ�ۿ�

        Vector3 _gaugeBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        gaugeBar.position = _gaugeBarPos;

        if (GaugePer <= 0)
        {
            gaugeBar.gameObject.SetActive(false);
        }
        else
        {
            gaugeBar.gameObject.SetActive(true);
        }

        nowGaugebar.fillAmount = GaugePer / 100.0f;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + boxTransform, size);
    }

    void Interactive()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position+ boxTransform, size, 0, whatIsLayer);

        if (Physics2D.OverlapBox(transform.position+ boxTransform, size, 0, whatIsLayer) == null)
        {
            if(targetObject != null) 
            {
                targetObject.GetComponent<SpriteRenderer>().material = originalMaterial;
                targetObject = null;
            }
            if (targetEndObject != null)
            {
                targetEndObject.GetComponent<SpriteRenderer>().material = originalMaterial;
                targetEndObject = null;
            }
            GaugePer = 0.0f;
        }
        else if (targetObject != hit.gameObject)
        {
            targetEndObject = targetObject;
            targetObject = hit.gameObject;
            
            if (targetObject != null)
            {
                targetObject.GetComponent<SpriteRenderer>().material = whiteMaterial;
            }

            if (targetEndObject != null)
            {
                targetEndObject.GetComponent<SpriteRenderer>().material = originalMaterial;
            }
            GaugePer = 0.0f;
        }
        
        

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isHold == true)
            {
                Hand.transform.GetChild(0).gameObject.layer = 6;
                Hand.transform.DetachChildren();
                isHold = false;
                return;
            }
        }

        if (hit != null)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (hit.CompareTag("tool") || hit.CompareTag("item"))
                {
                    hit.gameObject.transform.SetParent(Hand.transform);
                    hit.transform.localPosition = Vector2.zero;
                    hit.gameObject.layer = 0; //Default
                    isHold = true;
                }
            }
            
            if (isHold == true)
            {
                if (Hand.transform.GetChild(0).name == "Axe")
                {
                    if (hit.CompareTag("Tree"))
                    {
                        if (Input.GetKey(KeyCode.LeftControl))
                        {
                            GaugePer += diggingSpd * Time.deltaTime;
                            if (GaugePer >= 100)
                            {
                                hit.GetComponent<FarmingObject>().Digging();
                                GaugePer = 0.0f;
                            }
                        }
                        else
                        {
                            GaugePer = 0.0f;
                        }
                    }
                }
                
                if (Hand.transform.GetChild(0).name == "PickAxe")
                {
                    if (hit.CompareTag("Stone"))
                    {
                        if (Input.GetKey(KeyCode.LeftControl))
                        {
                            GaugePer += diggingSpd * Time.deltaTime;
                            if (GaugePer >= 100)
                            {
                                hit.GetComponent<FarmingObject>().Digging();
                                GaugePer = 0.0f;
                            }
                        }
                        else
                        {
                            GaugePer = 0.0f;
                        }
                    }

                }
                if (Hand.transform.GetChild(0).name == "Scythe")
                {
                    if (hit.CompareTag("Grass"))
                    {
                        if (Input.GetKey(KeyCode.LeftControl))
                        {
                            GaugePer += diggingSpd * Time.deltaTime;
                            if (GaugePer >= 100)
                            {
                                hit.GetComponent<FarmingObject>().Digging();
                                GaugePer = 0.0f;
                            }
                        }
                        else
                        {
                            GaugePer = 0.0f;
                        }
                    }

                }   
            }

        }
    }
}