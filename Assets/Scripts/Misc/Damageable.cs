using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Mesh))]

public class Damageable : MonoBehaviour
{
    [SerializeField] private int health = 3;
    private Rigidbody rb;
    private CapsuleCollider col;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        rb.isKinematic = true;
    }

    public void TakeDamage(int damageval)
    {
        health -= damageval;

        if (health <= 0) Die();
    }

    private void Die()
    {
        LevelTracker.Instance.IncrementPoints(1);
        Destroy(this.gameObject);
    }

}
