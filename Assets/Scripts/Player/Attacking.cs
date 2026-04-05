using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    public GameObject LazerPrefab;
    public GameObject BombPrefab;

    public void Bomb()
    {
        Bomb bomb = FindObjectOfType<Bomb>();
        if (bomb == null)
        {
            Instantiate(BombPrefab, transform.position, Quaternion.LookRotation(transform.forward));
        }
        else
        {
            bomb.Explode();
        }
    }
    public void Shoot()
    {
        Instantiate(LazerPrefab, transform.position, Quaternion.LookRotation(transform.forward));
    }
}
