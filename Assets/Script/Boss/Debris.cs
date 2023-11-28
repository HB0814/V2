using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    public float dmg;
    public GameManager gameManager;

    void Awake()
    {
        dmg = 10;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            gameObject.SetActive(false);
            gameManager.Ground_Effect(transform.position+Vector3.down*1f);
        }
    }
}
