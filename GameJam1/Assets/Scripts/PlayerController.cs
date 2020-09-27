using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public GameObject door1;
    public GameObject door2;
    private Animator Door1Animator;
    private Animator Door2Animator;
    [SerializeField] private LayerMask groundMask;
    private Rigidbody2D rb;
    private PolygonCollider2D pc;
    public Transform respawnPoint1;
    public Transform respawnPoint2;
    public float speed = 5f;
    private float jumpVelo = 6f;
    private bool onLadder = false;
    private bool isJumping = false;
    private bool respawn = false;
    private bool level2 = false;
    private bool door1opened = false;
    private bool door2opened = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PolygonCollider2D>();
        Door1Animator = door1.GetComponent<Animator>();
        Door2Animator = door2.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        float hIn = Input.GetAxis("Horizontal");
        float vIn = Input.GetAxis("Vertical");
        transform.position = transform.position + new Vector3(hIn * speed * Time.deltaTime, 0, 0);

        animator.SetFloat("Speed", Mathf.Abs(hIn * speed * Time.deltaTime));

        Vector3 localScale = transform.localScale;
        if(hIn > 0 && !(transform.localScale.x > 0)) {
            localScale.x *= -1;
            transform.localScale = localScale;
        }
        if(hIn < 0 && !(transform.localScale.x < 0)) {
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        if(respawn == true) {
            if(level2 == false) {
                transform.position = new Vector3(respawnPoint1.position.x,respawnPoint1.position.y, respawnPoint1.position.z);
            } else {
                transform.position = new Vector3(respawnPoint2.position.x, respawnPoint2.position.y, respawnPoint2.position.x);
            }
            respawn = false;
        }

        if((IsGrounded() || onLadder) && (Input.GetKeyDown(KeyCode.Space))) {
            isJumping = true;
            rb.velocity = Vector2.up * jumpVelo;
        }

        if(onLadder == true) {
            if(!isJumping) rb.velocity = new Vector2(0,0);
            transform.position = transform.position + new Vector3(0, vIn * speed * Time.deltaTime, 0);

            rb.gravityScale = 0;

            if(isJumping) {
                onLadder = false;
            }
        }

    if(onLadder == false) {
        rb.gravityScale = 1;
        animator.SetBool("IsClimbing", false);
    }
        if(IsGrounded() && Input.GetKeyDown(KeyCode.DownArrow)) {
            animator.SetBool("IsCrouching", true);
        }
        if(IsGrounded() && Input.GetKeyUp(KeyCode.DownArrow)) {
            animator.SetBool("IsCrouching", false);
        }
    }
    private bool IsGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(pc.bounds.center, Vector2.down, 1f, groundMask);
        isJumping = false;
        return raycastHit2D.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if("OpenDoor1".Equals(col.gameObject.tag)) {
            Debug.Log("Door open triggered");
            door1opened = true;
            Door1Animator.SetBool("DoorOpens1", true);
        }

        if("Door1".Equals(col.gameObject.tag) && door1opened == true) {
            transform.position = new Vector3(respawnPoint2.position.x, respawnPoint2.position.y, respawnPoint2.position.z);
            level2 = true;
        }

        if("OpenDoor2".Equals(col.gameObject.tag)) {
            Debug.Log("Door Open triggered");
            door2opened = true;
            Door2Animator.SetBool("DoorOpens2", true);
        }

        if("Door2".Equals(col.gameObject.tag) && door2opened == true) {
            Destroy(this.gameObject);
        }

        if("Ladder".Equals(col.gameObject.tag)) {
            Debug.Log("Ladder Climbing");
            onLadder = true;
            animator.SetBool("IsClimbing", true);
        }

        if("Bullet".Equals(col.gameObject.tag)) {
            respawn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if("Ladder".Equals(col.gameObject.tag)) {
            onLadder = false;
        }
    }
}
