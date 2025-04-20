using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPlayerMovement : MonoBehaviour
{
    public delegate void DropHive(Vector3 pos);
    public static event DropHive DroppedHive;

    public float speed = 6.0f;
    public float rotationSpeed = 60.0f;
    private Rigidbody playerRb = null;
    private float translationValue = 0;
    private float rotateValue = 0;
    private Animator animator;
    private bool dead = false;
    private bool onAlert = false;
    public GameObject lookTarget;
    private Vector3 defaultLookPosition = new Vector3(0, 1, 6);
    private Coroutine smoothLookCoroutine;
    private GameObject currHazard;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        
    }

    void Update()
    {
        if (dead)
        {
            return;
        }
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical");
        float rotation = Input.GetAxis("Horizontal");

        animator.SetFloat("speed", translation);

        translationValue = translation;
        rotateValue = rotation;

        // only when we've picked up and have not dropped the hive
        // will we drop the hive
        if(Input.GetKeyDown(KeyCode.Space) && HivePickUp.pickedUp && !HivePickUp.dropped)
        {
            DroppedHive.Invoke(transform.position);
        }
    }

    private void FixedUpdate()
    {
        if (dead)
        {
            return;
        }
        // rotates the player
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y += rotateValue * rotationSpeed * Time.deltaTime;
        playerRb.MoveRotation(Quaternion.Euler(rot));

        // simply moves the player by however much the player is pressing with respect
        // to the speed parameter. Does not affect gravity.
        Vector3 move = transform.forward * translationValue;
        playerRb.velocity = new Vector3(move.x * speed, playerRb.velocity.y, move.z * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if we run into a hazard and we're not dead yet
        if (collision.collider.CompareTag("Hazard") && dead == false)
        {
            // die and play the died animation
            dead = true;
            animator.SetTrigger("died");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Hazard"))
        {   
            if (smoothLookCoroutine != null)
            {
                StopCoroutine(smoothLookCoroutine);
            }

            onAlert = true;
            animator.SetBool("leftEarStand", onAlert);

            if(currHazard == null)
            {
                currHazard = other.gameObject;
            }
            lookTarget.transform.position = Vector3.Lerp(lookTarget.transform.position, currHazard.transform.position, 0.08f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            currHazard = null;
            smoothLookCoroutine = StartCoroutine("SmoothLookForward");
            onAlert = false;
            animator.SetBool("leftEarStand", onAlert);
        }
    }
    IEnumerator SmoothLookForward()
    {
        Vector3 targetPosition = transform.position + transform.forward * 6;

        while (Vector3.Distance(lookTarget.transform.position, targetPosition) > 0.1f)
        {
            targetPosition = transform.position + transform.forward * 6;
            lookTarget.transform.position = Vector3.Lerp(lookTarget.transform.position, targetPosition, 0.08f);
            yield return new WaitForSeconds(0.05f);
        }

        lookTarget.transform.position = targetPosition;
    }
}
