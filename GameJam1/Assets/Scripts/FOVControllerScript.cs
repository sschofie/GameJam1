using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVControllerScript : MonoBehaviour
{
    public Transform player;
    public Rigidbody2D bullet;
    public float modifier;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col) {
        if("Player".Equals(col.gameObject.tag)) {
            Debug.Log(transform.position - player.position);
            Rigidbody2D b = Instantiate(bullet, transform.position, Quaternion.identity);
            b.velocity = (player.position - transform.position) * modifier; 
        }
    }
}
