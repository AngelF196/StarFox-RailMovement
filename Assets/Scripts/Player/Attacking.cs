using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    public GameObject LazerPrefab;
    public GameObject BombPrefab;

    private GameObject lastBomb;
    public void Bomb()
    {
        if (lastBomb == null)
        {
            lastBomb = Instantiate(BombPrefab, transform.position, Quaternion.LookRotation(transform.forward));
        }
        else lastBomb.GetComponent<Bomb>().Explode();
    }
    public void Shoot()
    {
        Instantiate(LazerPrefab, transform.position, Quaternion.LookRotation(transform.forward));
    }
}
