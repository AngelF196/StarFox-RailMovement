using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{

    private Rigidbody rb;
    [SerializeField] private int damage;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 100f, ForceMode.VelocityChange);
        StartCoroutine(Die());
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Damageable")
        {
            collision.gameObject.GetComponent<Damageable>().TakeDamage(damage);
        }

        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Lazer")
        {
            Destroy(transform.parent.gameObject);
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(4);
        Destroy(transform.parent.gameObject);
    }
}