using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : Enemy
{
    public int Count;
    public int pattern;
    public float Att_Distance;
    public Enemy enemyScript;

    float dis;
    void Awake()
    {
        maxAttackDelay = 2f;
        anim = GetComponent<Animator>();
    }
    void OnEnable()
    {
        StartCoroutine(LevelCheck());
    }
    IEnumerator LevelCheck()
    {
        yield return new WaitForSeconds(0.5f);

        Enemy enemyLogic = enemyScript.GetComponent<Enemy>();
        level = enemyLogic.level;

        yield return null;
        switch (enemyType)
        {
            case Type.Snake:
                maxHealth = 30;
                health = 30;
                isDie = false;
                isFollow = false;
                isAggro = false;
                isHit = false;

                break;
            case Type.Flower:
                maxHealth = 30;
                health = 30;
                isDie = false;
                isFollow = false;
                isAggro = false;
                isHit = false;

                break;
        }
    }
    void FixedUpdate()
    {
        Distance();
        //AroundAttack();
    }
    void Distance()
    {
        dis =Vector2.Distance(transform.position, player.transform.position);
    }
    void AroundAttack()
    {
        curAttackDelay += Time.deltaTime;
        if (dis > Att_Distance)
            return;

        if (curAttackDelay < maxAttackDelay)
            return;
        pattern = pattern % 2 == 0 ? pattern + 1 :0;
        anim.SetTrigger("doAttack");
        if (pattern == 0)
        {
            for (float index = 0; index < Count; index++)
            {
                GameObject flowerAttack = objectManager.MakeObj("flowerAttack");

                flowerAttack.transform.position = transform.position + Vector3.down * 0.5f;
                Rigidbody2D F_rigid = flowerAttack.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * index / (Count - 1)),
                    Mathf.Sin(-Mathf.PI * index / (Count - 1)));

                F_rigid.transform.Rotate(Vector3.forward * index * 45);
                F_rigid.AddForce(dirVec * 5, ForceMode2D.Impulse);
            }
        }
        else if (pattern == 1)
        {
            for (float index = 0; index < Count+1; index++)
            {
                GameObject flowerAttack = objectManager.MakeObj("flowerAttack");

                flowerAttack.transform.position = transform.position + Vector3.down * 0.5f;
                Rigidbody2D F_rigid = flowerAttack.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * index / (Count)),
                    Mathf.Sin(-Mathf.PI * index / (Count)));

                F_rigid.transform.Rotate(Vector3.forward * index * 45);
                F_rigid.AddForce(dirVec * 5, ForceMode2D.Impulse);
            }
        }
        curAttackDelay = 0;
    }
    void Attack()
    {
        curAttackDelay += Time.deltaTime;

        GameObject flowerAttack = objectManager.MakeObj("flowerAttack");
        EnemyObject flowerAttackLogic = flowerAttack.GetComponent<EnemyObject>();
        flowerAttackLogic.enemyScript = enemyScript;
        flowerAttack.transform.position = transform.position;

        Rigidbody2D S_rigid = flowerAttack.GetComponent<Rigidbody2D>();
        SpriteRenderer sprite = flowerAttack.GetComponent<SpriteRenderer>();
        Vector2 dirPos = (player.transform.position - flowerAttack.transform.position).normalized;

        S_rigid.AddForce(dirPos * 5, ForceMode2D.Impulse);

        curAttackDelay = 0;
    }
    IEnumerator OnHit(float dmg)
    {
        if (isHit)
            yield break;

        isHit = true;

        DamageLogic(dmg);
        ReturnSprite(0.4f);

        if (health < 0)
        {
            isDie = true;
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.5f);

        isHit = false;
        ReturnSprite(1f);
    }
    void ReturnSprite(float Alpha)
    {
        spriteRenderer.color = new Color(0.4f, 0.4f, 0.4f, Alpha);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어의 무기에 공격당했을 때 
        if (collision.gameObject.tag == "PlayerAttack")
        {
            Player playerLogic = player.GetComponent<Player>();
            StartCoroutine(OnHit(playerLogic.dmg));
        }
    }
}
