using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody2D _rigidBody { get; set; }
    private CapsuleCollider2D _collider { get; set; }
    private Animator _animator { get; set; }

    [SerializeField]
    private float jumpHight;
    [SerializeField]
    private bool isGrounded;
    private AudioManager audioManager;

   

    [SerializeField] float fallMulitplier;
    [SerializeField] float lowJumpMulitplier;
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
        audioManager.PlaySound("Run");
    }


    private void Update()
    {
        if (isGrounded)
        {
           
                if (!GameManager.Instance.GameOver)
                {
                    audioManager.PlaySound("Run");
                }
            
            if (Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

    }
    private void FixedUpdate()
    {
        JumpPhysics();
    }


    private void Jump()
    {
        _rigidBody.AddForce(new Vector2(0, jumpHight), ForceMode2D.Impulse);
        _animator.SetTrigger("Jump");
        audioManager.PlaySound("Jump");
    }
    void JumpPhysics()
    {

        if (_rigidBody.velocity.y < 0)
            _rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMulitplier - 1) * Time.deltaTime;
        else if (_rigidBody.velocity.y > 0)
            if ((!Input.GetMouseButton(0) && !Input.GetKey(KeyCode.Space)))
            _rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMulitplier - 1) * Time.deltaTime;
    }

    #region GroundCheck
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isGrounded = true;
            _animator.SetBool("IsGrounded", true);
            audioManager.PlaySound("Land");
        }
        else if (collision.gameObject.layer == 7)
        {
            GameManager.Instance.AddScore(5);
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
            if (collision.gameObject.layer == 6)
            {
                isGrounded = false;
                _animator.SetBool("IsGrounded", false);
            }


            }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == 6)
        {
            foreach (ContactPoint2D Point in collision.contacts)
            {
                if (Point.point.x >= transform.position.x + (_collider.bounds.size.x / 2))
                {
                    GameManager.Instance.GameOverEvent.Invoke();
                    break;
                }
            } 
        }
        else if(collision.gameObject.layer == 7)
        {
            GameManager.Instance.AddScore(5);
            Destroy(collision.gameObject);
        }
        
    }
    public void OnGameOver()
    {
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _animator.SetTrigger("GameOver");
        audioManager.PlaySound("Death");
    }
}
