using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackObject : Enemy
{
    public enum Attack_Type {Melee,Range };
    public Attack_Type Att_type;
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
                switch (Att_type)
                {
                    case Attack_Type.Melee:
                        yield return new WaitForSeconds(1f);
                        Off();
                        break;
                    case Attack_Type.Range:
                        yield return new WaitForSeconds(4f);
                        Off();
                        break;
                }
                break;

            case Level.Normal:

                break;

            case Level.Hard:
                switch (Att_type)
                {
                    case Attack_Type.Melee:
                        yield return new WaitForSeconds(1f);
                        Off();
                        break;
                    case Attack_Type.Range:
                        yield return new WaitForSeconds(4f);
                        Off();
                        break;
                }
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
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Border" || collision.gameObject.tag == "Ground")
        {
            gameObject.SetActive(false);
        }
    }
}