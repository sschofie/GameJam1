using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private LayerMask groundMask;
    private Rigidbody2D rb;
    private PolygonCollider2D pc;
    public float speed = 5f;
    private float jumpVelo = 6f;
    private int collectibleCounter = 0;
    public bool isHidden = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        //if(!(screenPoint.x < 0) && !(screenPoint.x > 1)) {
        float hIn = Input.GetAxis("Horizontal");
        transform.position = transform.position + new Vector3(hIn * speed * Time.deltaTime, 0, 0);

        animator.SetFloat("Speed", Mathf.Abs(hIn * speed * Time.deltaTime));
        //}
        Vector3 localScale = transform.localScale;
        if(hIn == 1 && !(transform.localScale.x > 0)) {
            localScale.x *= -1;
            transform.localScale = localScale;
            //Debug.Log("Facing Right");
        }
        if(hIn == -1 && !(transform.localScale.x < 0)) {
            //Debug.Log("Facing Left");
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        if(IsGrounded() && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))) {
            rb.velocity = Vector2.up * jumpVelo;
            //animator.SetBool("IsJumping", true);
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
        //Debug.Log(raycastHit2D.collider);
        return raycastHit2D.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if("Collectible".Equals(col.gameObject.tag)) {
            Destroy(col.gameObject);
            collectibleCounter++;
            //Debug.Log(collectibleCounter);
        }
    }
}
