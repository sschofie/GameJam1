using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerScript : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    public PolygonCollider2D pc;
    public float speed = -5f;
    public bool isRightFacing;
    public Transform distChecker;
    // Start is called before the first frame update
    void Start()
    {
        //pc = GetComponent<PolygonCollider2D>();
        if(isRightFacing) {
            speed = speed *-1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0, 0);
        RaycastHit2D raycastHit2DL = Physics2D.Raycast(pc.bounds.center, Vector2.left, 1f, groundMask);
        RaycastHit2D raycastHit2DR = Physics2D.Raycast(pc.bounds.center, Vector2.right, 1f, groundMask);
        RaycastHit2D raycastHit2DD = Physics2D.Raycast(distChecker.position, Vector2.down, 2f, groundMask);
        if((raycastHit2DL.collider != null && !isRightFacing) || (raycastHit2DR.collider != null) && isRightFacing || (raycastHit2DD.collider == null)) {
            speed *= -1;

            Vector3 angles = transform.eulerAngles;
            if(isRightFacing) {
                angles.y = 0;
            } else {
                angles.y = 180;
            }
            transform.eulerAngles = angles;
            isRightFacing = !isRightFacing;
        }
    }
    private bool IsGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(pc.bounds.center, Vector2.left, 1f, groundMask);
        //Debug.Log(raycastHit2D.collider);
        return raycastHit2D.collider != null;
    }

    
}
