using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControls : MonoBehaviour
{   
    // Fuck Boilerplate, Doing this for expermintation, otherwise would use public for everything that does not use Range

    [SerializeField, Range(0, 100)]
    float SteerClamp = 10;

    [SerializeField, Range(0, 180)]
    float resetRotation = 30;

    [SerializeField]
    float SteerAmount;

    [SerializeField]
    float Speed;

    [SerializeField]
    float MinusVelocity;

    [SerializeField]
    float MaxVelocity;

    [SerializeField] 
    float BoostAmount;

    [SerializeField, Range(0.1f, 10f)]
    float BoostTime = 1f;

    [SerializeField, Range(0.1f, 10f)]
    float BoostWaitTime = 1f;

    [SerializeField, Range(0.1f, 10f)]
    float ResetTime = 3f;

    [SerializeField, Range(0.1f, 10f)]
    float ResetDuration = 3f;

    [SerializeField, Range(0.1f, 10f)]
    float respawnTime = 3f;

    [SerializeField]
    float TimeScale = 1f;

    [SerializeField]
    WheelCollider Left;

    [SerializeField]
    WheelCollider Right;

    public Transform playerHead;

    Rigidbody rb;
    HingeJoint headJoint;
    Transform currentCheckpoint;
    Vector3 playerHeadPos;
    JointSpring jSpring;
    Quaternion ogRotQ;

    float timer;
    float turnTimer;
    float breakForce;
    float breakTorque;


    private void Start () {

        Time.timeScale = TimeScale;

        Mathf.Clamp(Right.steerAngle, -SteerClamp, SteerClamp);
        Mathf.Clamp(Left.steerAngle, -SteerClamp, SteerClamp);

        rb = gameObject.GetComponent<Rigidbody>();
        headJoint = gameObject.GetComponent<HingeJoint>();
        playerHead = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        

        var HJ = headJoint;
        jSpring = HJ.spring;
        breakForce = HJ.breakForce;
        breakTorque = HJ.breakTorque;

        playerHeadPos = transform.position - playerHead.position;
        ogRotQ = transform.rotation;

    }

    private void Update () {
        Left.steerAngle = Input.GetAxisRaw("Horizontal") * SteerAmount;
        Right.steerAngle = Input.GetAxisRaw("Horizontal") * SteerAmount;
        timer += Time.deltaTime;
        bool isRotated = (transform.localRotation.eulerAngles.y >= resetRotation && transform.localRotation.eulerAngles.y <= (360f - resetRotation));
        //Debug.Log(isRotated);

        if (isRotated) {
            turnTimer += Time.deltaTime;
            if(turnTimer >= ResetTime) {
                StartCoroutine("Reset");
                turnTimer = 0f;
            }
        }

        else{
            // Backwards
            if (Input.GetKey(KeyCode.Space) && rb.velocity != Vector3.zero){
                rb.velocity -= new Vector3(MinusVelocity, MinusVelocity, MinusVelocity) * Time.deltaTime;
            }

            // Boost
            if (Input.GetKeyDown(KeyCode.E) && timer >= BoostWaitTime) {
                StartCoroutine("Boost");
                timer = 0f;
            }
        }
        if(headJoint == null) {
            Debug.Log("Head Null");
        }
        
    }

    IEnumerator Boost () {


        for(float t = 0f; t <= BoostTime; t += Time.deltaTime) {
            // Debug.Log("Boostin");
            rb.velocity += transform.forward * BoostAmount * Time.deltaTime;
            yield return null;
        }

    }

    IEnumerator Reset () {
        rb.useGravity = false;
        float ogRotY = transform.rotation.y;
        float rsetDiv = ResetDuration / 2;

        for(float t = 0f; t <= ResetDuration; t += Time.deltaTime) {
            if(t <= rsetDiv) {
                transform.Translate(Vector3.up * Time.deltaTime);
            } else{
                float rotTime = (t - rsetDiv) / rsetDiv;
                //Debug.Log(rotTime);
                transform.Rotate(new Vector3(0f, -Mathf.SmoothStep(ogRotY, 0f, rotTime ), 0f));
            }
            yield return null;
        }
        rb.useGravity = true;

    }

    private void OnJointBreak (float breakForce) {
        Debug.Log("Joint Break");
        StartCoroutine("Respawn");
    }

    IEnumerator Respawn () {
        for(float t = 0f; t <= respawnTime; t += Time.deltaTime) {
            yield return null;
        }
        //Add the velocity null and rotation here, so the objects wont explode and make nothing work
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.rotation = ogRotQ;
        transform.position = currentCheckpoint.position;

        var hRB =  playerHead.GetComponent<Rigidbody>();
        hRB.velocity = Vector3.zero;
        hRB.angularVelocity = Vector3.zero;
        playerHead.rotation = ogRotQ;
        playerHead.position = transform.position - playerHeadPos;

        AddHinge();
    }

    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag ("Checkpoint")) {
            currentCheckpoint = other.gameObject.transform;
        }
    }



    private void AddHinge () {
        headJoint = gameObject.AddComponent(typeof(HingeJoint)) as HingeJoint; 
        
        headJoint.connectedBody = playerHead.GetComponent<Rigidbody>();

        headJoint.useSpring = true;
        // jSpring.targetPosition = 180f;
        headJoint.spring = jSpring;
        headJoint.breakForce = breakForce;
        headJoint.breakTorque = breakTorque;
        headJoint.enableCollision = true;
    }

}
