using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour
{
    public bool IsGrounded;
    public bool IsClimbingOnLadder;
    public bool IsFalling;
    public bool IsStayOnHead;
    public bool IsClimbingOnRope;

    [SerializeField] private Transform _pointOfRay;
    [SerializeField] private Transform _rightRay;
    [SerializeField] private Transform _leftRay;
    [SerializeField] private LayerMask _ropeMask;
    [SerializeField] private LayerMask _maskCenterOfPit;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Text _textCountTreasure;
    [SerializeField] private int _countTreasure;
    [SerializeField] private float _lengthRay = 1f;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _coolDown = 1.5f;

    private Animator _animator;
    private PlayerInput _playerInput;

    private int _totalTreasures;
    private float _gravityInFalling = 20f;
    private float _gravityOnGround = 1f;
    private string _animIsAttaking = "isAttaking";
    private string _animIsRunning = "isRunning";
    private string _animIsClimbing = "isClimbing";
    private string _animIsClimbIdle = "isClimbIdle";
    private bool _canMove;
    private bool _canDestroy;
    private bool _checkNextPit;
    private bool _checkEnemyInPit;

    public Rigidbody2D Rb { get; set; }

    public void DestroyBricks(float direction)
    {
        if (direction > 0 && _canDestroy)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            _animator.SetTrigger(_animIsAttaking);
            StartCoroutine(CooldownMoveAndDestroy());
        }
        else if (direction < 0 && _canDestroy)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            _animator.SetTrigger(_animIsAttaking);
            StartCoroutine(CooldownMoveAndDestroy());
        }
    }

    public void Move(Vector2 moveInput)
    {
        if (moveInput.x != 0 && IsClimbingOnRope == false)
        {
            _animator.SetBool(_animIsRunning, true);
            _animator.SetBool(_animIsClimbing, false);
            if (moveInput.x < 0)
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            if (moveInput.x > 0)
                transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveInput.x != 0 && IsClimbingOnRope)
        {
            _animator.SetBool(_animIsClimbing, true);
            _animator.SetBool(_animIsClimbIdle, false);
            if (moveInput.x < 0)
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            if (moveInput.x > 0)
                transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveInput.x == 0 && IsClimbingOnRope)
        {
            _animator.SetBool(_animIsClimbIdle, true);
            _animator.SetBool(_animIsClimbing, true);
        }
        else
        {
            _animator.SetBool(_animIsRunning, false);
            _animator.SetBool(_animIsClimbing, false);
            _animator.SetBool(_animIsClimbIdle, false);
        }

        if (moveInput.y == 0 && IsClimbingOnLadder == false)
            Rb.velocity = moveInput * _speed;
        else if (IsClimbingOnLadder)
            Rb.velocity = moveInput * _speed;
        else if (moveInput.y != 0 && IsFalling)
            Rb.velocity = moveInput;

        if (moveInput.y == -1 && IsClimbingOnRope)
        {
            _animator.SetBool(_animIsClimbIdle, false);
            IsClimbingOnRope = false;
            IsFalling = true;
        }
    }

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Enable();
     }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        _totalTreasures = FindObjectsOfType<Treasure>().Length;
        _animator = GetComponent<Animator>();
        _canMove = true;
        _canDestroy = true;
        _textCountTreasure.text = $"{_countTreasure} / {_totalTreasures}";
    }

    private void Update()
    {
        _checkNextPit = CheckPitAfterGoingOnHeadEnemy();
        _checkEnemyInPit = WalkOnEnemyHead();
        if (_checkEnemyInPit)
        {
            Rb.bodyType = RigidbodyType2D.Kinematic;
            IsStayOnHead = true;
            IsFalling = false;
        }
        else
        {
            Rb.bodyType = RigidbodyType2D.Dynamic;
            IsStayOnHead = false;
        }

        if(_checkNextPit && _checkEnemyInPit == false)
        {
            IsGrounded = false;
            IsFalling = true;
        }
    }

    private void FixedUpdate()
     {
        Vector2 moveInput = _playerInput.Player.Move.ReadValue<Vector2>();
        if(_canMove)
            Move(moveInput);
        if (IsFalling && IsGrounded == false && IsClimbingOnLadder == false && IsClimbingOnRope == false)
        {
            Rb.gravityScale = _gravityInFalling;
            OnDisable();
        }
        else
            OnEnable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Treasure treasure))
        {
            _countTreasure++;
            _textCountTreasure.text = $"{_countTreasure} / {_totalTreasures}";
            Destroy(collision.gameObject);
            if (_countTreasure == _totalTreasures)
                _gameManager.CreateFinishLadder();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Brick brick))
        {
            IsClimbingOnRope = false;
            IsGrounded = true;
            IsFalling = false;
            if(IsClimbingOnLadder == false)
                Rb.gravityScale = _gravityOnGround;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Brick brick))
        {
            if (IsClimbingOnLadder == true || IsFalling == true)
            {
                IsGrounded = false;
            }

            if (IsClimbingOnLadder == false && IsFalling == false && IsGrounded == true && IsStayOnHead == false)
            {
                IsGrounded = false;
                IsFalling = true;
            }
        }
    }

    private bool WalkOnEnemyHead()
    {
        bool value = false;
        float castDist = _lengthRay;
        string enemy = "Enemy";
        string enemyClone = "Enemy(Clone)";

        RaycastHit2D rightHit = Physics2D.Raycast(_rightRay.position, Vector2.down, castDist, _ropeMask);
        if(rightHit.collider != null)
        {
            if (rightHit.collider.name == enemy || rightHit.collider.name == enemyClone)
            {
                value = true;
            }
            else
                value = false;
        }

        return value;
    }

    private bool CheckPitAfterGoingOnHeadEnemy()
    {
        bool value = false;
        string centerClone = "Center(Clone)";
        float castDist = _lengthRay;
        RaycastHit2D checkHit = Physics2D.Raycast(transform.position, Vector2.down, castDist, _maskCenterOfPit);
        if (checkHit.collider != null)
        {
            if (checkHit.collider.name == centerClone)
            {
                value = true;
            }
            else
                value = false;
        }

        return value;
    }

    private IEnumerator CooldownMoveAndDestroy()
    {
        Rb.velocity = Vector2.zero;
        _animator.SetBool(_animIsRunning, false);
        _canMove = false;
        _canDestroy = false;
        yield return new WaitForSeconds(_coolDown);
        _canMove = true;
        _canDestroy = true;
    }
}
