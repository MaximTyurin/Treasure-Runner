using System.Collections;
using UnityEngine;
using SAP2D;

public class Enemy : MonoBehaviour
{
    public GameObject HaveTreasure;
    public SAP2DAgent SAP2DAgent;
    public Rigidbody2D Rb;

    public bool IsClimbingOnLadder;
    public bool IsClimbingOnRope;
    public bool IsFalling;
    public bool IsGrounded;
    public bool CanTakeTreasure;

    [SerializeField] private GameObject _pointExitFromPit;
    [SerializeField] private GameObject _fakeBrick;
    [SerializeField] private Treasure _treasure;
    [SerializeField] private float _timeToStayInPit = 5f;
    
    private Animator _animator;
    private SceneController _sceneController;
    private GridManager _gridManager;

    private float _gravityOnGround = 1f;
    private float _gravityInFalling = 10f;

    private void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        SAP2DAgent = GetComponent<SAP2DAgent>();
        _sceneController = FindObjectOfType<SceneController>();
        _gridManager = FindObjectOfType<GridManager>();
        CanTakeTreasure = true;
        IsFalling = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            _animator.SetTrigger("isAttacking");
            _sceneController.Restartlevel();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Brick brick))
        {
            IsClimbingOnRope = false;
            IsGrounded = true;
            IsFalling = false;
            Rb.bodyType = RigidbodyType2D.Dynamic;
            if (IsClimbingOnLadder == false)
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

            if (IsClimbingOnLadder == false && IsFalling == false && IsClimbingOnRope == false && IsGrounded == true)
            {
                IsGrounded = false;
                IsFalling = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out CenterOfBrick centerOfBrick))
        {
            StartCoroutine(TimeToLiveFakeBrick());
            transform.position = centerOfBrick.transform.position;
            SAP2DAgent.CanMove = false;
            StartCoroutine(TimeToStayInPit());
            if (CanTakeTreasure == false)
            {
                Vector3 treasurePos = _pointExitFromPit.transform.position;
                Instantiate(_treasure, treasurePos, Quaternion.identity);
                CanTakeTreasure = true;
                HaveTreasure.SetActive(false);
            }
        }
        float ladderCorrectoin = 0.2f;
        if (collision.gameObject.CompareTag("TopLadder") && transform.position.y < collision.gameObject.transform.position.y - ladderCorrectoin)
        {
            transform.position = collision.gameObject.transform.position;
        }

        if (collision.gameObject.CompareTag("BotLadder") && transform.position.y > collision.gameObject.transform.position.y + ladderCorrectoin)
        {
            transform.position = collision.gameObject.transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out CenterOfBrick centerOfBrick))
        {
            SAP2DAgent.CanMove = false;
            transform.position = centerOfBrick.transform.position;
            IsFalling = true;
            IsGrounded = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out CenterOfBrick centerOfBrick))
        {
            SAP2DAgent.CanMove = true;
            IsFalling = false;
        }
    }

    private IEnumerator TimeToStayInPit()
    {
        Vector3 exit = _pointExitFromPit.transform.position;
        yield return new WaitForSeconds(_timeToStayInPit);
        transform.position = exit;
        Rb.bodyType = RigidbodyType2D.Kinematic;
        SAP2DAgent.CanMove = true;
    }

    private IEnumerator TimeToLiveFakeBrick()
    {
        Vector3 fakeBrickPos = _pointExitFromPit.transform.position;
        GameObject fakeBrick = Instantiate(_fakeBrick, fakeBrickPos, Quaternion.identity);
        _gridManager.DestroyFakeBrick(fakeBrick);
        yield return new WaitForSeconds(_gridManager.CoolDownRestoreBricksProp);
    }

    private void FixedUpdate()
    {
        if (IsFalling && IsGrounded == false && IsClimbingOnLadder == false && IsClimbingOnRope == false)
        {
            Rb.gravityScale = _gravityInFalling;
            Rb.velocity = Vector2.zero;
            SAP2DAgent.CanMove = false;
            _animator.SetBool("isRunning", false);
        }

        if (IsGrounded || IsClimbingOnLadder || IsClimbingOnRope)
        {
            SAP2DAgent.CanMove = true;
            SAP2DAgent.enabled = true;
            _animator.SetBool("isRunning", true);
        }
    }
}