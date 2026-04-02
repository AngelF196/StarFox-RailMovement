using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Windows;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Mesh))]
public class Bomb : MonoBehaviour
{
    private Rigidbody rb;
    private SphereCollider blastRadius;
    [SerializeField] private float maxRadius;
    [SerializeField] private float expansionRate;
    [SerializeField] private int damage;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        blastRadius = GetComponent<SphereCollider>();
        blastRadius.radius = 0;
        blastRadius.enabled = false;
        rb.AddForce(transform.forward * 100f, ForceMode.VelocityChange);
        StartCoroutine(TimedExplode());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Explode();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Damageable")
        {
            Debug.Log("There was an enemy to blow up");
            other.gameObject.GetComponent<Damageable>().TakeDamage(damage);
        }
    }

    public void Explode()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        Debug.Log("exploding");
        rb.constraints = RigidbodyConstraints.FreezePosition;
        blastRadius.enabled = true;

        while (blastRadius.radius < maxRadius)
        {
            blastRadius.radius += expansionRate * Time.deltaTime;
            yield return null; // wait one frame
        }

        Destroy(gameObject);
    }

    IEnumerator TimedExplode()
    {
        yield return new WaitForSeconds(4);
        StartCoroutine(Explosion());
    }
}
