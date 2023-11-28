    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : Enemy
{
    public Enemy enemyScript;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        StartCoroutine(LevelCheck());
    }

    //Start Setting
    IEnumerator LevelCheck()
    {
        rigid.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(0.5f);

        Enemy enemyLogic = enemyScript.GetComponent<Enemy>();
        level = enemyLogic.level;

        yield return null;
        switch (level)
        {
            case Level.Easy:
                dmg = 8;
                break;

            case Level.Normal:

                break;

            case Level.Hard:
                dmg = 15;
                break;
        }
        yield return new WaitForSeconds(3f);

        Off();
    }
    void Off()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"|| collision.gameObject.tag=="Border"|| collision.gameObject.tag == "Ground")
        {
            gameObject.SetActive(false);
        }
    }
}
