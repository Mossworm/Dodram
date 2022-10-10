using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float dashPower;
    private Rigidbody2D _characterRigidbody;
    private Vector2 _movement;

    public bool MainPlayer;

    Animator _animator;
    string _animationState = "AnimationState";

    enum States
    {
        Right = 1,
        Left = 2,
        Up = 3,
        Down = 4,
        Idle = 5
    }

    public enum Dir
    {
        Right = 1,
        Left = 2,
        Up = 3,
        Down = 4
    }
    
    public Dir direction;

    void Start()
    {
        // // 구독 신청! KeyAction이 Invoke 되면 호출할 함수! (중복을 막기위해 빼준 후 추가)
        // Managers.Input.KeyAction -= OnKeyboard;
        // Managers.Input.KeyAction += OnKeyboard;

        _animator = GetComponent<Animator>();
        _characterRigidbody = GetComponent<Rigidbody2D>();
        direction = Dir.Down;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            _characterRigidbody.AddForce(_characterRigidbody.velocity * dashPower, ForceMode2D.Impulse);
            return;
        }

        if (MainPlayer)
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

    private void UpdateState()
    {
        if (_movement.x > 0)
        {
            _animator.SetInteger(_animationState, (int)States.Right);
            direction = Dir.Right;
        }
        else if (_movement.x < 0)
        {
            _animator.SetInteger(_animationState, (int)States.Left);
            direction = Dir.Left;
        }
        else if (_movement.y > 0)
        {
            _animator.SetInteger(_animationState, (int)States.Up);
            direction = Dir.Up;
        }
        else if (_movement.y < 0)
        {
            _animator.SetInteger(_animationState, (int)States.Down);
            direction = Dir.Down;
        }
        else
        {
            _animator.SetInteger(_animationState, (int)States.Idle);
        }
    }

}