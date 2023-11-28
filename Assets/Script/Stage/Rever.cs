using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rever : MonoBehaviour
{
    public bool isRever;
    public bool isStop;

    public Transform[] target;

    public GameObject player;
    public GameObject Cage;
    public GameObject rever;
    public BoxCollider2D R_door;
    public BoxCollider2D L_door;

    Rever reverLogic;
    void Awake()
    {
        reverLogic = rever.GetComponent<Rever>();
    }
    void FixedUpdate()
    {
        Move();
    }

    //Move& Stop
    IEnumerator Stop()
    {
        isStop = true;
        reverLogic.isStop = isStop;
        yield return new WaitForSeconds(1f);

        isStop = false;
        reverLogic.isStop = isStop;
        R_door.enabled = true;
        L_door.enabled = true;
        yield return new WaitForSeconds(0.5f);
    }
    void Move()
    {
        if (isStop)
            return;
        if (isRever)
            Cage.transform.position = Vector2.MoveTowards(Cage.transform.position, target[0].position, 0.03f);
        else if(!isRever)
            Cage.transform.position = Vector2.MoveTowards(Cage.transform.position, target[1].position, 0.03f);

        if(Cage.transform.position==target[0].position)
            R_door.enabled = false;
        if (Cage.transform.position == target[1].position)
            L_door.enabled = false;
    }

    //Open Door
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="PlayerAttack")
        {
            if (!isRever)
            {
                isRever = true;
                reverLogic.isRever = isRever;
                L_door.enabled = false;
            }
            else
            {
                isRever = false;
                reverLogic.isRever = isRever;
                R_door.enabled = false;
            }
            StartCoroutine (Stop());
        }
    }
}
