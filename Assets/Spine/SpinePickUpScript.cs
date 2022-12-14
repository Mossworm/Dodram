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
        Interactive(); //μνΈμμ©
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
        Debug.Log("Shader Changed!"); //μ ­μ.... .. .
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
        //  λ¨Έν°λ¦¬μΌ and μ±μΉ© κ²μ΄μ§ μ΄κΈ°ν
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
                //  κΈ°κ³μ μ¬λ£ λ£κΈ° & μ€νμν€κΈ°
                //---------------------------------------

                if (isHold == true) // κΈ°κ³μ λ£κΈ°
                {
                    if (hit.gameObject.name == "SpineSawmill") // λλ¬΄ κΈ°κ³μ λ£κΈ°
                    {
                        hit.GetComponent<SpineMachineScript>().SubCount(Hand);
                    }
                    else if (hit.gameObject.name == "SpineStonecutter") // λ κΈ°κ³μ λ£κΈ°
                    {
                        hit.GetComponent<SpineMachineScript>().SubCount(Hand);
                    }
                    else if (hit.gameObject.name == "SpineMill") // λ²μ― κΈ°κ³μ λ£κΈ°
                    {
                        hit.GetComponent<SpineMachineScript>().SubCount(Hand);
                    }
                    else if (hit.gameObject.name == "Spine_Last_Machine")  //2μ°¨κ°κ³΅ κΈ°κ³μ λ£κΈ°
                    {
                        Debug.Log("2μ°¨κ°κ³΅ μ€ν");
                        hit.GetComponent<SpineFinalMachineScript>().SubCount(Hand);
                    }
                    else if (hit.gameObject.name == "House") //μ§μ 2μ°¨κ°κ³΅ λ¬Όκ±΄ λ£κΈ°
                    {
                        hit.GetComponent<HouseScript>().Building(Hand);
                    }
                }
                else //λ¬Όκ±΄μ λ€κ³ μμ§ μμ μν
                {
                    //κΈ°κ³κ° μμ΄νμ λ€ λ§λ€μλ€λ©΄ κΊΌλ
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
                    if (hit.gameObject.name == "RockMachineButton")  //κΈ°κ³ λ²νΌμ λλ₯Ό μ κΈ°κ³ κ°λ
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
        //  μμ μΊκΈ°
        //---------------------------------------
        if (Input.GetKey(InteractiveKey))
        {
            if (hit != null)
            {
                if (isHold == true)
                {
                    if (Hand.transform.GetChild(0).name == "Axe") //λλ¬΄ μΊκΈ°
                    {
                        if (hit.CompareTag("Tree"))
                        {
                            hit.GetComponent<SpineFarmingObject>().isTarget = true;
                            GaugePer += (100.0f / animator.GetCurrentAnimatorStateInfo(0).length * 1.5f) * Time.deltaTime;
                            if (GaugePer >= 100.0f)
                            {
                                SoundController.Instance.PlaySFXSound("μ±μ§2");
                                hit.GetComponent<SpineFarmingObject>().Digging();
                                GaugePer = 0.1f;
                            }
                        }
                    }
                    else if (Hand.transform.GetChild(0).name == "PickAxe") //λ μΊκΈ°
                    {
                        if (hit.CompareTag("Stone"))
                        {
                            hit.GetComponent<SpineFarmingObject>().isTarget = true;
                            GaugePer += (100.0f / animator.GetCurrentAnimatorClipInfo(0).Length * 1.5f) * Time.deltaTime;
                            if (GaugePer >= 100)
                            {
                                SoundController.Instance.PlaySFXSound("μ±μ§2");
                                hit.GetComponent<SpineFarmingObject>().Digging();
                                GaugePer = 0.1f;
                            }
                        }
                    }
                    else if (Hand.transform.GetChild(0).name == "Scythe")   //ν λ² κΈ°
                    {
                        if (hit.CompareTag("Grass"))
                        {
                            hit.GetComponent<SpineFarmingObject>().isTarget = true;
                            GaugePer += (100.0f / animator.GetCurrentAnimatorClipInfo(0).Length * 1.5f) * Time.deltaTime;
                            if (GaugePer >= 100)
                            {
                                SoundController.Instance.PlaySFXSound("μ±μ§2");
                                hit.GetComponent<SpineFarmingObject>().Digging();
                                GaugePer = 0.1f;
                            }
                        }
                    }
                    else if (Hand.transform.GetChild(0).name == "Hammer")   //κΈ°κ³ κ³ μΉκΈ°
                    {
                        if (hit.CompareTag("Machine"))
                        {
                            GaugePer += (100.0f / animator.GetCurrentAnimatorClipInfo(0).Length * 1.5f) * Time.deltaTime;
                            if (GaugePer >= 100)
                            {
                                SoundController.Instance.PlaySFXSound("μ±μ§2");
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
        //  μμ΄ν ν½μ&ν½λ€μ΄
        //---------------------------------------
        else if (Input.GetKeyDown(PickupKey))
        {
            if (hit != null)
            {
                if (isHold == true)
                {
                    if (hit.CompareTag("tool") || hit.CompareTag("item"))
                    //λ°κΎΈκΈ°
                    {
                        changeHold = hit.gameObject;

                        //λ  μμ΄ν(λκ΅¬) λ΄λ €λκΈ°
                        if (Hand.transform.GetChild(0).gameObject.CompareTag("tool"))
                        {
                            Hand.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        Hand.transform.GetChild(0).gameObject.layer = 6;
                        Hand.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
                        Hand.transform.GetChild(0).position = boxTransform + transform.position + new Vector3(0f, -0.3f, 0f);
                        Hand.transform.DetachChildren();

                        //λ°κΏ μμ΄ν λ€κΈ°
                        changeHold.transform.SetParent(Hand.transform);
                        changeHold.transform.localPosition = Vector3.zero;
                        changeHold.layer = 0;
                        if (changeHold.CompareTag("tool"))
                        {
                            changeHold.SetActive(false);
                        }
                        
                        SoundController.Instance.PlaySFXSound("λ€κΈ°");
                    }
                }
                else
                {
                    if (hit.CompareTag("tool") || hit.CompareTag("item"))
                    //λ€κΈ°
                    {
                        hit.gameObject.transform.SetParent(Hand.transform);
                        hit.transform.localPosition = Vector3.zero;
                        hit.gameObject.layer = 0; //Default
                        if (hit.CompareTag("tool"))
                        {
                            hit.gameObject.SetActive(false);
                        }
                        SoundController.Instance.PlaySFXSound("λ€κΈ°");
                    }
                }
            }
            else
            {
                if (isHold == true)
                {
                    // λκΈ°
                    if (Hand.transform.GetChild(0).gameObject.CompareTag("tool"))
                    {
                        Hand.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    Hand.transform.GetChild(0).gameObject.layer = 6;
                    Hand.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
                    Hand.transform.GetChild(0).position = boxTransform + transform.position + new Vector3(0f, -0.3f, 0f);
                    Hand.transform.DetachChildren();
                    SoundController.Instance.PlaySFXSound("λ€κΈ°");
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