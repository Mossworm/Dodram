using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine.Serialization;

public class PickUpScript : MonoBehaviour
{
    public Vector2 size;
    public Vector3 boxTransform;
    public LayerMask whatIsLayer;
    public GameObject Hand;
    GameObject changeHold;

    public bool isHold;

    [FormerlySerializedAs("DiggingPer")] public float GaugePer;

    [SerializeField] private GameObject targetObject;
    [SerializeField] private GameObject targetEndObject;

    [SerializeField] private Material whiteMaterial;
    [SerializeField] private Material originalMaterial;

    private KeyCode[] ArrayInteractiveKey = new KeyCode[] { KeyCode.LeftControl, KeyCode.RightControl };
    private KeyCode[] ArrayPickupKey = new KeyCode[] { KeyCode.LeftShift, KeyCode.RightShift };

    [SerializeField] private KeyCode InteractiveKey;
    [SerializeField] private KeyCode PickupKey;

    private PlayerController.Dir dir;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        isHold = false;

        GaugePer = 0.0f;

        animator = GetComponent<Animator>();

        if (this.GetComponent<PlayerController>().isMainPlayer)
        {
            InteractiveKey = ArrayInteractiveKey[0];
            PickupKey = ArrayPickupKey[0];
        }
        else
        {
            InteractiveKey = ArrayInteractiveKey[1];
            PickupKey = ArrayPickupKey[1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        Interactive(); //��ȣ�ۿ�
        if (Hand.transform.childCount != 0)
        {
            isHold = true;
        }
        else
        {
            isHold = false;
        }

        dir = this.gameObject.GetComponent<PlayerController>().direction;

        if (dir == PlayerController.Dir.Down)
        {
            boxTransform = new Vector3(0, -0.12f, 0);
        }
        else if (dir == PlayerController.Dir.Up)
        {
            boxTransform = new Vector3(0, 0.7f, 0);
        }
        else if (dir == PlayerController.Dir.Left)
        {
            boxTransform = new Vector3(-0.56f, 0.4f, 0);
        }
        else if (dir == PlayerController.Dir.Right)
        {
            boxTransform = new Vector3(0.61f, 0.4f, 0);
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + boxTransform, size);
    }

    void Interactive()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position + boxTransform, size, 0, whatIsLayer);

        //---------------------------------------
        //  ���͸��� and äĨ ������ �ʱ�ȭ
        //---------------------------------------
        if (Physics2D.OverlapBox(transform.position + boxTransform, size, 0, whatIsLayer) == null)
        {
            if (targetObject != null)
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


        if (Input.GetKeyDown(PickupKey))
        {
            if (hit != null)
            {
                //---------------------------------------
                //  ��迡 ��� �ֱ� & �����Ű��
                //---------------------------------------

                if (isHold == true) //������ ����ִ� ����
                {
                    if (hit.gameObject.name == "Sawmill") // ���� ��迡 �ֱ�
                    {
                        hit.GetComponent<MachineScript>().SubCount(Hand);
                    }
                    else if (hit.gameObject.name == "Stonecutter") // �� ��迡 �ֱ�
                    {
                        hit.GetComponent<MachineScript>().SubCount(Hand);
                    }
                    else if (hit.gameObject.name == "Mill") // ���� ��迡 �ֱ�
                    {
                        hit.GetComponent<MachineScript>().SubCount(Hand);
                    }
                    else if (hit.gameObject.name == "Last_Machine")  //2������ ��迡 �ֱ�
                    {
                        hit.GetComponent<FinalMachineScript>().SubCount(Hand);
                    }
                    else if (hit.gameObject.name == "House") //���� 2������ ���� �ֱ�
                    {
                        hit.GetComponent<HouseScript>().Building(Hand);
                    }
                }
                else //������ ������� ���� ����
                {
                    //��谡 �������� �� ������ٸ� ����
                    if (hit.gameObject.name == "Sawmill")
                    {
                        hit.GetComponent<MachineScript>().PickUp(Hand);
                    }
                    else if (hit.gameObject.name == "Stonecutter")
                    {
                        hit.GetComponent<MachineScript>().PickUp(Hand);
                    }
                    else if (hit.gameObject.name == "Mill")
                    {
                        hit.GetComponent<MachineScript>().PickUp(Hand);
                    }
                    else if (hit.gameObject.name == "Last_Machine")
                    {
                        if (this.gameObject.name == "Player 1")
                        {
                            hit.GetComponent<FinalMachineScript>().PickUp(Hand);
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(InteractiveKey))
        {
            if (hit != null)
            {
                if (isHold != true)
                {
                    if (hit.gameObject.name == "RockMachineButton")                 //��� ��ư�� ���� �� ��� ����
                    {
                        hit.GetComponent<MachineButtonScript>().MachineRun();
                    }
                    else if (hit.gameObject.name == "MushMachineButton")
                    {
                        hit.GetComponent<MachineButtonScript>().MachineRun();
                    }
                    else if (hit.gameObject.name == "TreeMachineButton")
                    {
                        hit.GetComponent<MachineButtonScript>().MachineRun();
                    }
                    //else if (hit.gameObject.name == "Last_Machine")
                    //{
                    //    hit.GetComponent<FinalMachineScript>().CraftOn();
                    //}
                }
            }
        }

        //---------------------------------------
        //  �ڿ� ĳ��
        //---------------------------------------
        if (Input.GetKey(InteractiveKey))
        {
            if (hit != null)
            {
                if (isHold == true)
                {
                    if (Hand.transform.GetChild(0).name == "Axe") //���� ĳ��
                    {
                        if (hit.CompareTag("Tree"))
                        {
                            GaugePer += (100.0f / animator.GetCurrentAnimatorStateInfo(0).length) * Time.deltaTime;
                            if (GaugePer >= 100.0f)
                            {
                                hit.GetComponent<FarmingObject>().Digging();
                                GaugePer = 0.0f;
                            }
                        }
                    }
                    else if (Hand.transform.GetChild(0).name == "PickAxe") //�� ĳ��
                    {
                        if (hit.CompareTag("Stone"))
                        {
                            GaugePer += (100.0f / animator.GetCurrentAnimatorStateInfo(0).length) * Time.deltaTime;
                            if (GaugePer >= 100)
                            {
                                hit.GetComponent<FarmingObject>().Digging();
                                GaugePer = 0.0f;
                            }
                        }
                    }
                    else if (Hand.transform.GetChild(0).name == "Scythe")   //Ǯ ����
                    {
                        if (hit.CompareTag("Grass"))
                        {
                            GaugePer += (100.0f / animator.GetCurrentAnimatorStateInfo(0).length) * Time.deltaTime;
                            if (GaugePer >= 100)
                            {
                                hit.GetComponent<FarmingObject>().Digging();
                                GaugePer = 0.0f;
                            }
                        }
                    }
                    else if (Hand.transform.GetChild(0).name == "Hammer")   //��� ��ġ��
                    {
                        if (hit.CompareTag("Machine"))
                        {
                            hit.GetComponent<MachineScript>().MachinePix();
                        }
                    }
                }
            }
        }
        else if (Input.GetKeyUp(InteractiveKey))
        {
            GaugePer = 0.0f;
            if (hit.CompareTag("Tree") || hit.CompareTag("Stone") || hit.CompareTag("Grass"))
            {
                hit.GetComponent<FarmingObject>().hp = hit.GetComponent<FarmingObject>().maxhp;
            }
        }


        //---------------------------------------
        //  ������ �Ⱦ�&�ȴٿ�
        //---------------------------------------
        else if (Input.GetKeyDown(PickupKey))
        {
            if (hit != null)
            {
                if (isHold == true)
                {
                    if (hit.CompareTag("tool") || hit.CompareTag("item"))
                    //�ٲٱ�
                    {
                        changeHold = hit.gameObject;

                        //�� ������(����) ��������
                        if (Hand.transform.GetChild(0).gameObject.CompareTag("tool"))
                        {
                            Hand.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        Hand.transform.GetChild(0).gameObject.layer = 6;
                        if (dir == PlayerController.Dir.Down)
                        {
                            Hand.transform.GetChild(0).localPosition = new Vector3(0f, -1.41f, 0f);
                        }
                        else if (dir == PlayerController.Dir.Up)
                        {
                            Hand.transform.GetChild(0).localPosition = new Vector3(0f, -0.37f, 0f);
                        }
                        else if (dir == PlayerController.Dir.Left)
                        {
                            Hand.transform.GetChild(0).localPosition = new Vector3(-0.6f, -1.02f, 0f);
                        }
                        else if (dir == PlayerController.Dir.Right)
                        {
                            Hand.transform.GetChild(0).localPosition = new Vector3(0.6f, -1.02f, 0f);
                        }
                        Hand.transform.DetachChildren();

                        //�ٲ� ������ ���
                        changeHold.transform.SetParent(Hand.transform);
                        changeHold.transform.localPosition = Vector2.zero;
                        changeHold.layer = 0;
                        if (changeHold.CompareTag("tool"))
                        {
                            changeHold.SetActive(false);
                        }
                    }
                }
                else
                {
                    if (hit.CompareTag("tool") || hit.CompareTag("item"))
                    //���
                    {
                        hit.gameObject.transform.SetParent(Hand.transform);
                        hit.transform.localPosition = Vector2.zero;
                        hit.gameObject.layer = 0; //Default
                        if (hit.CompareTag("tool"))
                        {
                            hit.gameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                if (isHold == true)
                {
                    //����
                    if (Hand.transform.GetChild(0).gameObject.CompareTag("tool"))
                    {
                        Hand.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    Hand.transform.GetChild(0).gameObject.layer = 6;

                    if (dir == PlayerController.Dir.Down)
                    {
                        Hand.transform.GetChild(0).localPosition = new Vector3(0f, -1.41f, 0f);
                    }
                    else if (dir == PlayerController.Dir.Up)
                    {
                        Hand.transform.GetChild(0).localPosition = new Vector3(0f, -0.37f, 0f);
                    }
                    else if (dir == PlayerController.Dir.Left)
                    {
                        Hand.transform.GetChild(0).localPosition = new Vector3(-0.6f, -1.02f, 0f);
                    }
                    else if (dir == PlayerController.Dir.Right)
                    {
                        Hand.transform.GetChild(0).localPosition = new Vector3(0.6f, -1.02f, 0f);
                    }

                    Hand.transform.DetachChildren();
                    return;
                }
            }
        }
    }
}