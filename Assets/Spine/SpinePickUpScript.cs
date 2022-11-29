using UnityEngine;
//using UnityEditor.PackageManager;
using UnityEngine.Serialization;

public class SpinePickUpScript : MonoBehaviour
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

    [SerializeField] private Shader whiteShader;
    [SerializeField] private Shader originalShader;

    private KeyCode[] ArrayInteractiveKey = new KeyCode[] { KeyCode.LeftControl, KeyCode.RightControl };
    private KeyCode[] ArrayPickupKey = new KeyCode[] { KeyCode.LeftShift, KeyCode.RightShift };

    public KeyCode InteractiveKey;
    public KeyCode PickupKey;

    private SpinePlayerController.Dir dir;

    private Animator animator;

    [SerializeField] private bool isTool;

    // Start is called before the first frame update
    void Start()
    {
        isHold = false;

        GaugePer = 0.0f;

        animator = GetComponent<Animator>();

        if (this.GetComponent<SpinePlayerController>().isMainPlayer)
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
        Interactive(); //상호작용
        if (Hand.transform.childCount != 0)
        {
            isHold = true;
            if (Hand.transform.GetChild(0) != null)
            {
                Hand.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
        else
        {
            isHold = false;
        }

        dir = this.gameObject.GetComponent<SpinePlayerController>().direction;

        if (dir == SpinePlayerController.Dir.Down)
        {
            boxTransform = new Vector3(0, -0.12f, 0);
        }
        else if (dir == SpinePlayerController.Dir.Up)
        {
            boxTransform = new Vector3(0, 0.6f, 0);
        }
        else if (dir == SpinePlayerController.Dir.Left)
        {
            boxTransform = new Vector3(-0.56f, 0.4f, 0);
        }
        else if (dir == SpinePlayerController.Dir.Right)
        {
            boxTransform = new Vector3(0.61f, 0.4f, 0);
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + boxTransform, size);
    }

    void ChangeShader(GameObject gameObject, Shader shader)
    {
        MeshRenderer meshRenderer = gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
        Material[] materials = meshRenderer.materials;
        foreach (var Material in materials)
        {
            Material.shader = shader;
        }
        Debug.Log("Shader Changed!"); //젭알.... .. .
    }
    void Interactive()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + boxTransform, size, 0, whatIsLayer);
        Collider2D hit = null;
        isTool = false;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag("tool"))
            {
                isTool = true;
                hit = hits[i];
            }
        }

        for (int i = 0; i < hits.Length; i++)
        {
            if (isTool)
            {
                if (hits[i].CompareTag("tool"))
                {
                    if (Vector2.Distance(this.transform.position, hit.transform.position) >
                        Vector2.Distance(this.transform.position, hits[i].transform.position))
                    {
                        hit = hits[i];
                    }
                }
            }
            else
            {
                if (hit == null)
                {
                    hit = hits[0];
                }

                if (Vector2.Distance(this.transform.position, hit.transform.position) > Vector2.Distance(this.transform.position, hits[i].transform.position))
                {
                    hit = hits[i];
                }
            }
        }

        // Collider2D hit = Physics2D.OverlapBox(transform.position + boxTransform, size, 0, whatIsLayer);

        //---------------------------------------
        //  머터리얼 and 채칩 게이지 초기화
        //---------------------------------------
        if (Physics2D.OverlapBox(transform.position + boxTransform, size, 0, whatIsLayer) == null)
        {
            if (targetObject != null)
            {
                if (targetObject.CompareTag("Machine")
                    || targetObject.CompareTag("Stone")
                    || targetObject.CompareTag("Tree")
                    || targetObject.CompareTag("Grass")
                    )
                {
                    ChangeShader(targetObject, originalShader);
                }
                else
                {
                    targetObject.GetComponent<SpriteRenderer>().material = originalMaterial;
                }
                targetObject = null;
            }
            if (targetEndObject != null)
            {
                if (targetEndObject.CompareTag("Machine")
                    || targetEndObject.CompareTag("Stone")
                    || targetEndObject.CompareTag("Tree")
                    || targetEndObject.CompareTag("Grass")
                    )
                {
                    ChangeShader(targetEndObject, originalShader);
                }
                else
                {
                    targetEndObject.GetComponent<SpriteRenderer>().material = originalMaterial;
                }
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
                if (targetObject.CompareTag("Machine")
                    || targetObject.CompareTag("Stone")
                    || targetObject.CompareTag("Tree")
                    || targetObject.CompareTag("Grass")
                    )
                {
                    ChangeShader(targetObject, whiteShader);
                }
                else
                {
                    targetObject.GetComponent<SpriteRenderer>().material = whiteMaterial;
                }
            }

            if (targetEndObject != null)
            {
                if (targetEndObject.CompareTag("Machine")
                    || targetEndObject.CompareTag("Stone")
                    || targetEndObject.CompareTag("Tree")
                    || targetEndObject.CompareTag("Grass")
                    )
                {
                    ChangeShader(targetEndObject, originalShader);
                }
                else
                {
                    targetEndObject.GetComponent<SpriteRenderer>().material = originalMaterial;
                }
            }
            GaugePer = 0.0f;
        }


        if (Input.GetKeyDown(PickupKey))
        {
            if (hit != null)
            {
                //---------------------------------------
                //  기계에 재료 넣기 & 실행시키기
                //---------------------------------------

                if (isHold == true) // 기계에 넣기
                {
                    if (hit.gameObject.name == "SpineSawmill") // 나무 기계에 넣기
                    {
                        hit.GetComponent<SpineMachineScript>().SubCount(Hand);
                    }
                    else if (hit.gameObject.name == "SpineStonecutter") // 돌 기계에 넣기
                    {
                        hit.GetComponent<SpineMachineScript>().SubCount(Hand);
                    }
                    else if (hit.gameObject.name == "SpineMill") // 버섯 기계에 넣기
                    {
                        hit.GetComponent<SpineMachineScript>().SubCount(Hand);
                    }
                    else if (hit.gameObject.name == "Spine_Last_Machine")  //2차가공 기계에 넣기
                    {
                        Debug.Log("2차가공 실행");
                        hit.GetComponent<SpineFinalMachineScript>().SubCount(Hand);
                    }
                    else if (hit.gameObject.name == "House") //집에 2차가공 물건 넣기
                    {
                        hit.GetComponent<HouseScript>().Building(Hand);
                    }
                }
                else //물건을 들고있지 않은 상태
                {
                    //기계가 아이템을 다 만들었다면 꺼냄
                    if (hit.gameObject.name == "SpineSawmill")
                    {
                        hit.GetComponent<SpineMachineScript>().PickUp(Hand);
                    }
                    else if (hit.gameObject.name == "SpineStonecutter")
                    {
                        hit.GetComponent<SpineMachineScript>().PickUp(Hand);
                    }
                    else if (hit.gameObject.name == "SpineMill")
                    {
                        hit.GetComponent<SpineMachineScript>().PickUp(Hand);
                    }
                    else if (hit.gameObject.name == "Spine_Last_Machine")
                    {
                        hit.GetComponent<SpineFinalMachineScript>().PickUp(Hand);
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
                    if (hit.gameObject.name == "RockMachineButton")  //기계 버튼을 누를 시 기계 가동
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
        //  자원 캐기
        //---------------------------------------
        if (Input.GetKey(InteractiveKey))
        {
            if (hit != null)
            {
                if (isHold == true)
                {
                    if (Hand.transform.GetChild(0).name == "Axe") //나무 캐기
                    {
                        if (hit.CompareTag("Tree"))
                        {
                            hit.GetComponent<SpineFarmingObject>().isTarget = true;
                            GaugePer += (100.0f / animator.GetCurrentAnimatorStateInfo(0).length * 1.5f) * Time.deltaTime;
                            if (GaugePer >= 100.0f)
                            {
                                SoundController.Instance.PlaySFXSound("채집2");
                                hit.GetComponent<SpineFarmingObject>().Digging();
                                GaugePer = 0.1f;
                            }
                        }
                    }
                    else if (Hand.transform.GetChild(0).name == "PickAxe") //돌 캐기
                    {
                        if (hit.CompareTag("Stone"))
                        {
                            hit.GetComponent<SpineFarmingObject>().isTarget = true;
                            GaugePer += (100.0f / animator.GetCurrentAnimatorClipInfo(0).Length * 1.5f) * Time.deltaTime;
                            if (GaugePer >= 100)
                            {
                                SoundController.Instance.PlaySFXSound("채집2");
                                hit.GetComponent<SpineFarmingObject>().Digging();
                                GaugePer = 0.1f;
                            }
                        }
                    }
                    else if (Hand.transform.GetChild(0).name == "Scythe")   //풀 베기
                    {
                        if (hit.CompareTag("Grass"))
                        {
                            hit.GetComponent<SpineFarmingObject>().isTarget = true;
                            GaugePer += (100.0f / animator.GetCurrentAnimatorClipInfo(0).Length * 1.5f) * Time.deltaTime;
                            if (GaugePer >= 100)
                            {
                                SoundController.Instance.PlaySFXSound("채집2");
                                hit.GetComponent<SpineFarmingObject>().Digging();
                                GaugePer = 0.1f;
                            }
                        }
                    }
                    else if (Hand.transform.GetChild(0).name == "Hammer")   //기계 고치기
                    {
                        if (hit.CompareTag("Machine"))
                        {
                            GaugePer += (100.0f / animator.GetCurrentAnimatorClipInfo(0).Length * 1.5f) * Time.deltaTime;
                            if (GaugePer >= 100)
                            {
                                SoundController.Instance.PlaySFXSound("채집2");
                                hit.GetComponent<SpineMachineScript>().MachinePix();
                                GaugePer = 0.1f;
                            }

                            //hit.GetComponent<MachineScript>().MachinePix();
                        }
                    }
                }
            }
        }

        if (Input.GetKeyUp(InteractiveKey) && hit != null)
        {
            GaugePer = 0.0f;
            if (hit.CompareTag("Tree") || hit.CompareTag("Stone") || hit.CompareTag("Grass"))
            {
                hit.GetComponent<SpineFarmingObject>().hp = hit.GetComponent<SpineFarmingObject>().maxhp;
                hit.GetComponent<SpineFarmingObject>().isTarget = false;
            }
        }


        //---------------------------------------
        //  아이템 픽업&픽다운
        //---------------------------------------
        else if (Input.GetKeyDown(PickupKey))
        {
            if (hit != null)
            {
                if (isHold == true)
                {
                    if (hit.CompareTag("tool") || hit.CompareTag("item"))
                    //바꾸기
                    {
                        changeHold = hit.gameObject;

                        //든 아이템(도구) 내려놓기
                        if (Hand.transform.GetChild(0).gameObject.CompareTag("tool"))
                        {
                            Hand.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        Hand.transform.GetChild(0).gameObject.layer = 6;
                        Hand.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
                        Hand.transform.GetChild(0).position = boxTransform + transform.position + new Vector3(0f, -0.3f, 0f);
                        Hand.transform.DetachChildren();

                        //바꿀 아이템 들기
                        changeHold.transform.SetParent(Hand.transform);
                        changeHold.transform.localPosition = Vector2.zero;
                        changeHold.layer = 0;
                        if (changeHold.CompareTag("tool"))
                        {
                            changeHold.SetActive(false);
                        }
                        
                        SoundController.Instance.PlaySFXSound("들기");
                    }
                }
                else
                {
                    if (hit.CompareTag("tool") || hit.CompareTag("item"))
                    //들기
                    {
                        hit.gameObject.transform.SetParent(Hand.transform);
                        hit.transform.localPosition = Vector2.zero;
                        hit.gameObject.layer = 0; //Default
                        if (hit.CompareTag("tool"))
                        {
                            hit.gameObject.SetActive(false);
                        }
                        SoundController.Instance.PlaySFXSound("들기");
                    }
                }
            }
            else
            {
                if (isHold == true)
                {
                    // 놓기
                    if (Hand.transform.GetChild(0).gameObject.CompareTag("tool"))
                    {
                        Hand.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    Hand.transform.GetChild(0).gameObject.layer = 6;
                    Hand.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
                    Hand.transform.GetChild(0).position = boxTransform + transform.position + new Vector3(0f, -0.3f, 0f);
                    Hand.transform.DetachChildren();
                    SoundController.Instance.PlaySFXSound("들기");
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (this.GetComponent<SpinePlayerController>().isMainPlayer)
            {
                this.transform.position = new Vector3(-2.88f, 3.75f, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (!this.GetComponent<SpinePlayerController>().isMainPlayer)
            {
                this.transform.position = new Vector3(15f, 0, 0);
            }
        }
    }
}