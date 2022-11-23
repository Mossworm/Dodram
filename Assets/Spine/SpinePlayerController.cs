using MonsterLove.StateMachine;
using System.Collections;
using UnityEngine;

public class SpinePlayerController : MonoBehaviour
{

    [SerializeField] private float speed;
    private Rigidbody2D _characterRigidbody;
    private Vector2 _movement;

    public bool isMainPlayer;

    private Animator _animator;
    public string currentState;
    private string _toolName;

    public SpinePickUpScript pickUpScript;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 0.2f;

    [SerializeField] private TrailRenderer tr;

    private KeyCode[] ArrayDashKey = new KeyCode[] { KeyCode.LeftAlt, KeyCode.RightAlt };
    [SerializeField] private KeyCode DashKey;
    [SerializeField] private bool canPickUp = true;
    public bool pickAnimEnd { get; set; } = false;




    private enum States
    {
        Idle,
        Walk,
        Work,
        Dash,
        Pick

    }

    StateMachine<States, StateDriverUnity> FSM;

    public enum Dir
    {
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    public Dir direction;

    void ChangeAnimation(string newState)
    {
        if (currentState == newState)
        {
            return;
        }

        _animator.Play(newState);

        currentState = newState;
    }

    bool IsInputMoveKey()
    {
        if (isMainPlayer)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                return true;
            }
        }
        else
        {
            if (Input.GetAxisRaw("Horizontal2") != 0 || Input.GetAxisRaw("Vertical2") != 0)
            {
                return true;
            }
        }
        return false;
    }

    private void Awake()
    {
        FSM = new StateMachine<States, StateDriverUnity>(this);
        FSM.ChangeState(States.Idle);
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _characterRigidbody = GetComponent<Rigidbody2D>();
        direction = Dir.Down;

        if (isMainPlayer)
        {
            DashKey = ArrayDashKey[0];
        }
        else
        {
            DashKey = ArrayDashKey[1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Managers.isReady)
        {
            return;
        }
        //대쉬중엔 아래코드 실행안함
        if (isDashing)
        {
            return;
        }

        UpdateDir();
        FSM.Driver.Update.Invoke();
        if (Input.GetKeyDown(DashKey) && canDash)
        {
            StartCoroutine(Dash());
        }

    }

    private void FixedUpdate()
    {
        if (!Managers.isReady)
        {
            return;
        }
        if (isDashing)
        {
            _characterRigidbody.velocity = _movement.normalized * dashingPower;
            return;
        }

        if (isMainPlayer)
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            _movement.x = Input.GetAxisRaw("Horizontal2");
            _movement.y = Input.GetAxisRaw("Vertical2");
        }

        _movement.Normalize();
        _characterRigidbody.velocity = _movement * speed;
    }

    private void UpdateDir()
    {
        if (_movement.x > 0)
        {
            direction = Dir.Right;
        }
        else if (_movement.x < 0)
        {
            direction = Dir.Left;
        }
        else if (_movement.y > 0)
        {
            direction = Dir.Up;
        }
        else if (_movement.y < 0)
        {
            direction = Dir.Down;
        }
    }

    //대쉬 구현
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }

    //----------------------------------------------------------------------------------
    //  FSM 구현
    //----------------------------------------------------------------------------------

    /**************************************** Idle ************************************/
    #region Idle
    void Idle_Enter()
    {
        _movement.x = 0f;
        _movement.y = 0f;
    }

    void Idle_Update()
    {

        if (IsInputMoveKey())
        {
            FSM.ChangeState(States.Walk);
        }

        if (Input.GetKey(pickUpScript.InteractiveKey) && pickUpScript.isHold
            && pickUpScript.Hand.transform.GetChild(0).CompareTag("tool"))
        {

            FSM.ChangeState(States.Work);
        }

        if (Input.GetKeyDown(pickUpScript.PickupKey)&&canPickUp && 
            pickUpScript.isHold &&
            pickUpScript.Hand.transform.GetChild(0).CompareTag("item"))
        {
            FSM.ChangeState(States.Pick);
        }

        if (pickUpScript.isHold)
        {
            _toolName = pickUpScript.Hand.transform.GetChild(0).name;

            switch (direction)
            {
                case Dir.Up:
                    if (_toolName == "PickAxe")
                        ChangeAnimation("Player_Idle_Pickaxe_Up");

                    else if (_toolName == "Axe")
                        ChangeAnimation("Player_Idle_Axe_Up");

                    else if (_toolName == "Scythe")
                        ChangeAnimation("Player_Idle_Shovel_Up");

                    else if (_toolName == "Hammer")
                        ChangeAnimation("Player_Idle_Hammer_Up");
                    else
                        ChangeAnimation("Player_P_Idle_Up");
                    break;

                case Dir.Down:
                    if (_toolName == "PickAxe")
                        ChangeAnimation("Player_Idle_Pickaxe_Down");

                    else if (_toolName == "Axe")
                        ChangeAnimation("Player_Idle_Axe_Down");

                    else if (_toolName == "Scythe")
                        ChangeAnimation("Player_Idle_Shovel_Down");

                    else if (_toolName == "Hammer")
                        ChangeAnimation("Player_Idle_Hammer_Down");
                    else
                        ChangeAnimation("Player_P_Idle_Down");
                    break;

                case Dir.Left:
                    if (_toolName == "PickAxe")
                        ChangeAnimation("Player_Idle_Pickaxe_Left");

                    else if (_toolName == "Axe")
                        ChangeAnimation("Player_Idle_Axe_Left");

                    else if (_toolName == "Scythe")
                        ChangeAnimation("Player_Idle_Shovel_Left");

                    else if (_toolName == "Hammer")
                        ChangeAnimation("Player_Idle_Hammer_Left");
                    else
                        ChangeAnimation("Player_P_Idle_Left");
                    break;

                case Dir.Right:
                    if (_toolName == "PickAxe")
                        ChangeAnimation("Player_Idle_Pickaxe_Right");

                    else if (_toolName == "Axe")
                        ChangeAnimation("Player_Idle_Axe_Right");

                    else if (_toolName == "Scythe")
                        ChangeAnimation("Player_Idle_Shovel_Right");

                    else if (_toolName == "Hammer")
                        ChangeAnimation("Player_Idle_Hammer_Right");
                    else
                        ChangeAnimation("Player_P_Idle_Right");
                    break;
            }
        }
        else
        {
            canPickUp = true;
            switch (direction)
            {
                case Dir.Up:
                    ChangeAnimation("Player_Idle_Up");
                    break;
                case Dir.Down:
                    ChangeAnimation("Player_Idle_Down");
                    break;
                case Dir.Left:
                    ChangeAnimation("Player_Idle_Left"); ;
                    break;
                case Dir.Right:
                    ChangeAnimation("Player_Idle_Right");
                    break;
            }
        }



    }
    #endregion


    /**************************************** Walk ************************************/
    #region Walk
    protected virtual void Walk_Enter()
    {

    }

    protected void Walk_Update()
    {
        if (Input.GetKeyDown(DashKey) && canDash)
        {
            FSM.ChangeState(States.Dash);
        }

        if (!IsInputMoveKey())
        {
            FSM.ChangeState(States.Idle);
        }

        if (Input.GetKey(pickUpScript.InteractiveKey)&&pickUpScript.isHold
            &&pickUpScript.Hand.transform.GetChild(0).CompareTag("tool"))
        {
            FSM.ChangeState(States.Work);
        }

        if (pickUpScript.isHold)
        {
            _toolName = pickUpScript.Hand.transform.GetChild(0).name;

            switch (direction)
            {
                case Dir.Up:
                    if (_toolName == "PickAxe")
                        ChangeAnimation("Player_Walk_Pickaxe_Up");

                    else if (_toolName == "Axe")
                        ChangeAnimation("Player_Walk_Axe_Up");

                    else if (_toolName == "Scythe")
                        ChangeAnimation("Player_Walk_Shovel_Up");

                    else if (_toolName == "Hammer")
                        ChangeAnimation("Player_Walk_Hammer_Up");
                    else
                        ChangeAnimation("Player_P_Walk_Up");
                    break;

                case Dir.Down:
                    if (_toolName == "PickAxe")
                        ChangeAnimation("Player_Walk_Pickaxe_Down");

                    else if (_toolName == "Axe")
                        ChangeAnimation("Player_Walk_Axe_Down");

                    else if (_toolName == "Scythe")
                        ChangeAnimation("Player_Walk_Shovel_Down");

                    else if (_toolName == "Hammer")
                        ChangeAnimation("Player_Walk_Hammer_Down");
                    else
                        ChangeAnimation("Player_P_Walk_Down");
                    break;

                case Dir.Left:
                    if (_toolName == "PickAxe")
                        ChangeAnimation("Player_Walk_Pickaxe_Left");

                    else if (_toolName == "Axe")
                        ChangeAnimation("Player_Walk_Axe_Left");

                    else if (_toolName == "Scythe")
                        ChangeAnimation("Player_Walk_Shovel_Left");

                    else if (_toolName == "Hammer")
                        ChangeAnimation("Player_Walk_Hammer_Left");
                    else
                        ChangeAnimation("Player_P_Walk_Left");
                    break;

                case Dir.Right:
                    if (_toolName == "PickAxe")
                        ChangeAnimation("Player_Walk_Pickaxe_Right");

                    else if (_toolName == "Axe")
                        ChangeAnimation("Player_Walk_Axe_Right");

                    else if (_toolName == "Scythe")
                        ChangeAnimation("Player_Walk_Shovel_Right");

                    else if (_toolName == "Hammer")
                        ChangeAnimation("Player_Walk_Hammer_Right");
                    else
                        ChangeAnimation("Player_P_Walk_Right");
                    break;
            }
        }
        else
        {
            switch (direction)
            {
                case Dir.Up:
                    ChangeAnimation("Player_Walk_Up");
                    break;
                case Dir.Down:
                    ChangeAnimation("Player_Walk_Down");
                    break;
                case Dir.Left:
                    ChangeAnimation("Player_Walk_Left");
                    break;
                case Dir.Right:
                    ChangeAnimation("Player_Walk_Right");
                    break;
            }
        }

        //if (isMainPlayer)
        //{
        //    _movement.x = Input.GetAxisRaw("Horizontal");
        //    _movement.y = Input.GetAxisRaw("Vertical");
        //}
        //else
        //{
        //    _movement.x = Input.GetAxisRaw("Horizontal2");
        //    _movement.y = Input.GetAxisRaw("Vertical2");
        //}
    }
    #endregion

    /**************************************** Work ************************************/
    #region Work
    protected virtual void Work_Enter()
    {
        _movement.x = 0.0f;
        _movement.y = 0.0f;
    }

    protected virtual void Work_Update()
    {
        if (Input.GetKeyUp(pickUpScript.InteractiveKey))
        {
            FSM.ChangeState(States.Idle);
        }

        //if (IsInputMoveKey())
        //{
        //    FSM.ChangeState(States.Walk);
        //}

        string _toolName = pickUpScript.Hand.transform.GetChild(0).name;

        switch (direction)
        {
            case Dir.Up:
                if (_toolName == "PickAxe")
                    ChangeAnimation("Use_Pickaxe_Up");
                if (_toolName == "Axe")
                    ChangeAnimation("Use_Axe_Up");
                if (_toolName == "Scythe")
                    ChangeAnimation("Use_Shovel_Up");
                if (_toolName == "Hammer")
                    ChangeAnimation("Use_Hammer_Up");
                break;
            case Dir.Down:
                if (_toolName == "PickAxe")
                    ChangeAnimation("Use_Pickaxe_Down");
                if (_toolName == "Axe")
                    ChangeAnimation("Use_Axe_Down");
                if (_toolName == "Scythe")
                    ChangeAnimation("Use_Shovel_Down");
                if (_toolName == "Hammer")
                    ChangeAnimation("Use_Hammer_Down");
                break;
            case Dir.Left:
                if (_toolName == "PickAxe")
                    ChangeAnimation("Use_Pickaxe_Left");
                if (_toolName == "Axe")
                    ChangeAnimation("Use_Axe_Left");
                if (_toolName == "Scythe")
                    ChangeAnimation("Use_Shovel_Left");
                if (_toolName == "Hammer")
                    ChangeAnimation("Use_Hammer_Left");
                break;
            case Dir.Right:
                if (_toolName == "PickAxe")
                    ChangeAnimation("Use_Pickaxe_Right");
                if (_toolName == "Axe")
                    ChangeAnimation("Use_Axe_Right");
                if (_toolName == "Scythe")
                    ChangeAnimation("Use_Shovel_Right");
                if (_toolName == "Hammer")
                    ChangeAnimation("Use_Hammer_Right");
                break;
        }
    }
    #endregion


    #region Dash
    protected virtual void Dash_Enter()
    {
        if (pickUpScript.isHold)
        {
            _toolName = pickUpScript.Hand.transform.GetChild(0).name;

            switch (direction)
            {
                case Dir.Up:
                    if (_toolName == "PickAxe")
                        ChangeAnimation("Run_Pickaxe_Up");

                    else if (_toolName == "Axe")
                        ChangeAnimation("Run_Axe_Up");

                    else if (_toolName == "Scythe")
                        ChangeAnimation("Run_Shovel_Up");

                    else if (_toolName == "Hammer")
                        ChangeAnimation("Run_Hammer_Up");
                    else
                        ChangeAnimation("P_Run_Up");
                    break;

                case Dir.Down:
                    if (_toolName == "PickAxe")
                        ChangeAnimation("Run_Pickaxe_Down");

                    else if (_toolName == "Axe")
                        ChangeAnimation("Run_Axe_Down");

                    else if (_toolName == "Scythe")
                        ChangeAnimation("Run_Shovel_Down");

                    else if (_toolName == "Hammer")
                        ChangeAnimation("Run_Hammer_Down");
                    else
                        ChangeAnimation("P_Run_Down");
                    break;

                case Dir.Left:
                    if (_toolName == "PickAxe")
                        ChangeAnimation("Run_Pickaxe_Left");

                    else if (_toolName == "Axe")
                        ChangeAnimation("Run_Axe_Left");

                    else if (_toolName == "Scythe")
                        ChangeAnimation("Run_Shovel_Left");

                    else if (_toolName == "Hammer")
                        ChangeAnimation("Run_Hammer_Left");
                    else
                        ChangeAnimation("P_Run_Left");
                    break;

                case Dir.Right:
                    if (_toolName == "PickAxe")
                        ChangeAnimation("Run_Pickaxe_Right");

                    else if (_toolName == "Axe")
                        ChangeAnimation("Run_Axe_Right");

                    else if (_toolName == "Scythe")
                        ChangeAnimation("Run_Shovel_Right");

                    else if (_toolName == "Hammer")
                        ChangeAnimation("Run_Hammer_Right");
                    else
                        ChangeAnimation("P_Run_Right");
                    break;
            }
        }
        else
        {
            switch (direction)
            {
                case Dir.Up:
                    ChangeAnimation("Run_Up");
                    break;
                case Dir.Down:
                    ChangeAnimation("Run_Down");
                    break;
                case Dir.Left:
                    ChangeAnimation("Run_Left");
                    break;
                case Dir.Right:
                    ChangeAnimation("Run_Right");
                    break;
            }
        }
    }
    protected virtual void Dash_Update()
    {
        if (IsInputMoveKey())
        {
            FSM.ChangeState(States.Walk);
        }
        if (!isDashing)
        {
            FSM.ChangeState(States.Idle);
        }

    }
    #endregion

    protected virtual void Pick_Enter()
    {
        canPickUp = false;
        pickAnimEnd=false;
    }

    protected virtual void Pick_Update()
    {
        if (IsInputMoveKey())
        {
            FSM.ChangeState(States.Walk);
        }

        if ((Input.GetKeyDown(pickUpScript.PickupKey) && !canPickUp)|| pickAnimEnd)
        {
            FSM.ChangeState(States.Idle);
        }

        switch (direction)
        {
            case Dir.Up:
                ChangeAnimation("P_Up");
                break;
            case Dir.Down:
                ChangeAnimation("P_Down");
                break;
            case Dir.Left:
                ChangeAnimation("P_Left"); ;
                break;
            case Dir.Right:
                ChangeAnimation("P_Right");
                break;
        }
    }
}