using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public GameObject explosion;
    public float explosionRadius, explosionForce;


    private void OnCollisionEnter(Collision collision)
    {
        GameObject _explosion = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(_explosion, 3);
    }
}
