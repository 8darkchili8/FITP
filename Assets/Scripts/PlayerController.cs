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

    private bool explode;
    public GameObject explosion;
    public float explosionRadius, explosionForce;

    private Rigidbody rigidbody;
    public GameObject camera;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        jumpDirection = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement management
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Direction by camera
        moveDirection = (transform.position - camera.transform.position);
        moveDirection.y = 0;
        Vector3 drift = Quaternion.AngleAxis(90, Vector3.up) * moveDirection;
        moveDirection = moveDirection.normalized * moveVertical + (drift * moveHorizontal).normalized;
        // Direction by keys
        //moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);



        // Jump management
        LayerMask layers = 1 << gameObject.layer;
        layers = ~layers;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.2f, layers);
        isOnWall = Physics.CheckSphere(transform.position, 0.2f, layers);
        jump = Input.GetButtonDown("Jump");
        if (jump && isGrounded)
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        } 
        else if (jump && !isGrounded && isOnWall)
        {
            rigidbody.AddForce(jumpDirection.normalized * jumpForce, ForceMode.Impulse);
            rigidbody.AddForce((Vector3.up * jumpForce)/2, ForceMode.VelocityChange);
            Debug.Log("Jump : " + (jumpDirection.normalized * jumpForce));
        }



        // Explosion management
        explode = Input.GetButtonDown("Explode");
        if (explode)
        {
            GameObject _explosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(_explosion, 3);
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            knockBack(colliders);
            colliders = null;
            //Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rigidbody.AddForce(moveDirection * speed);
    }

    private void OnCollisionStay(Collision collision)
    {
        jumpDirection = Vector3.zero;
        foreach (ContactPoint c in collision.contacts)
        {
            jumpDirection += c.normal;
        }
    }

    void knockBack(Collider[] colliders)
    {
        foreach(Collider closeCollider in colliders)
        {
            //Debug.Log(closeCollider.name);
            Rigidbody rigid = closeCollider.GetComponent<Rigidbody>();
            if (rigid != null)
            {
                rigid.AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.1f);
            }
        }
    }
}
