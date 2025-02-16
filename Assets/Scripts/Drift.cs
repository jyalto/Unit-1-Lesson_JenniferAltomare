using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour
{
    public float speed = 5.0f;
    public enum DriftDirection
    {
        LEFT = -1,
        RIGHT = 1
    }
    public DriftDirection driftDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(new Vector3(-1, 0, 0)); 
        transform.Translate(Vector3.right * Time.deltaTime * speed * (int)driftDirection); 

        if (transform.position.x < -80 || transform.position.x > 80)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            player.transform.SetParent(transform); // sets the parent of player to this
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            player.transform.SetParent(null); // sets the parent of player to this
        }
    }
}
