using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    private Vector3 moveDirection;

    public float jumpForce = 0.5f;
    private bool jump;
    private bool isGrounded;
    private bool isOnWall;
    private Vector3 jumpDirection;

    private Rigidbody rigidbody;
    public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        jumpDirection = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        jump = Input.GetButton("Jump");

        // Direction by camera
        moveDirection = (transform.position - camera.transform.position);
        moveDirection.y = 0;
        Vector3 drift = Quaternion.AngleAxis(90, Vector3.up) * moveDirection;

        // Direction by camera
        moveDirection = moveDirection.normalized * moveVertical + (drift * moveHorizontal).normalized;
        // Direction by keys
        //moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);
    }
    void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        rigidbody.AddForce(moveDirection * speed);
    }

    private void Jump()
    {
        LayerMask layers = 1 << gameObject.layer;
        layers = ~layers;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1, layers);
        isOnWall = Physics.CheckSphere(transform.position, 1, layers);

        if (jump && isGrounded)
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            Debug.Log("Jump : " + (Vector3.up * jumpForce));
        }
        else if (jump && !isGrounded && isOnWall)
        {
            rigidbody.AddForce(jumpDirection.normalized * jumpForce, ForceMode.Impulse);
            Debug.Log("Jump : " + (jumpDirection.normalized * jumpForce));
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        jumpDirection = Vector3.zero;
        foreach (ContactPoint c in collision.contacts)
        {
            jumpDirection += c.normal;
        }
    }
}
