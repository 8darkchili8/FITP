using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    public float speed = 10.0f;
    private Vector3 moveDirection;
    // Jump variables
    public float jumpForce = 0.5f;
    private bool jump;
    private bool isGrounded;
    private bool isOnWall;
    public bool isWallJumpAllowed;
    private Vector3 jumpDirection;
    private Rigidbody rigidbody;
    // Explosion variables
    private bool explode;
    public GameObject explosion;
    public float explosionRadius, ExplosionForceOnLight, ExplosionForceOnMedium, ExplosionForceOnHeavy;
    // Camera variables
    public GameObject camera;
    // Layers variables
    private LayerMask propsLayer = 1 << 9;

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
        //// Direction by camera
        moveDirection = (transform.position - camera.transform.position);
        moveDirection.y = 0;
        Vector3 drift = Quaternion.AngleAxis(90, Vector3.up) * moveDirection;
        moveDirection = moveDirection.normalized * moveVertical + (drift * moveHorizontal).normalized;;

        // Jump management
        LayerMask layers = 1 << gameObject.layer;
        layers = ~layers;

        //// Check if player is on the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.2f, layers);
        //// Checks if player can wall jump
        isOnWall = Physics.CheckSphere(transform.position, 0.2f, layers);
        jump = Input.GetButtonDown("Jump");
        if (jump && isGrounded)
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        } 
        else if (jump && isWallJumpAllowed && !isGrounded && isOnWall)
        {
            rigidbody.AddForce(jumpDirection.normalized * jumpForce, ForceMode.Impulse);
            rigidbody.AddForce((Vector3.up * jumpForce)/2, ForceMode.VelocityChange);
        }

        // Explosion management
        explode = Input.GetButtonDown("Explode");
        if (explode)
        {
            GameObject _explosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(_explosion, 3);
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, propsLayer);
            knockBack(colliders);
            colliders = null;
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

    /*
     * Adds knockback to objects in explosion radius
     * colliders : Array of objets in explosion radius
     */
    void knockBack(Collider[] colliders)
    {
        foreach(Collider closeCollider in colliders)
        {
            Debug.DrawLine(transform.position, closeCollider.transform.position, Color.green, 3, false);

            Rigidbody rigid = closeCollider.GetComponent<Rigidbody>();
            if (rigid != null)
            {
                switch (closeCollider.tag)
                {
                    case "Light":
                        rigid.AddExplosionForce(ExplosionForceOnLight, transform.position, explosionRadius, 0.1f, ForceMode.VelocityChange);
                        break;
                    case "Medium":
                        rigid.AddExplosionForce(ExplosionForceOnMedium, transform.position, explosionRadius, 0.1f, ForceMode.VelocityChange);
                        break;
                    case "Heavy":
                        rigid.AddExplosionForce(ExplosionForceOnHeavy, transform.position, explosionRadius, 0.1f, ForceMode.VelocityChange);
                        break;
                    default:
                        break;
                }
            }
            if (closeCollider.name.Contains("Soldier"))
            {
                // TO DO - Set soldier in ragdoll
                Debug.Log("To ragdoll");
            }
        }
    }
}
