using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy
{
    public int cointCount;

    public float curShotDelay;

    public float maxShotDelay;
    public Enemy enemyScript;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        speed = 2;

        forward = new Vector2(speed, rigid.velocity.y).normalized;
        isAggro = false;
        maxAttackDelay = 3;
        maxdistance = 8;
        mindistance = 2;

    }
    void OnEnable()
    {
        StartCoroutine(LevelCheck());
    }
    LayerMask groundLayer;

    void FixedUpdate()
    {
        Aggro();
        WallCheck();
        WatchCheck();
        Move();

        if (isDie)
            rigid.velocity = Vector2.zero;
    }
    //Start Setting
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
        }
        switch (level)
        {
            case Level.Easy:
                break;

            case Level.Normal:

                break;

            case Level.Hard:
                break;
        }
    }

    //Move
    void Move()
    {
        //거리가 maxdistance이하인 경우 플레이어를 따라와 공격
        if (isAggro&&!isDie)
        {
            Follow(transform.position, mindistance, spriteRenderer);

            rigid.velocity = forward * speed;
            curAttackDelay += Time.deltaTime;
            if (curAttackDelay > maxAttackDelay && isAggro)
            {
                Attack();
            }
        }
        else
        {
            rigid.velocity = forward * speed;
            curAttackDelay = 0;
        }
    }
    void WallCheck()
    {
        if (isAggro)
            return;
        groundLayer = LayerMask.GetMask("Ground");

        Vector2 frontVec= new Vector2(rigid.position.x+ nextMove,rigid.position.y);
        RaycastHit2D hit = Physics2D.Raycast(frontVec, Vector3.right* nextMove, 1.2f, groundLayer);

        if(hit)
        {
            FlipX(spriteRenderer);
        }
        Debug.DrawRay(frontVec, Vector3.right*1.2f*nextMove, Color.yellow);
    }
    void Aggro()
    {
        dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist < maxdistance)
        {
            isAggro = true;
        }
        else
        {
            isAggro = false;
            curAttackDelay = 0;
        }

    }
    void WatchCheck()
    {
        if (isFollow)
            return;
        if (spriteRenderer.flipX == false)
        { 
            forward = new Vector2(-speed, rigid.velocity.y).normalized;
            nextMove = -1;
         }
        else if (spriteRenderer.flipX == true)
        { 
            forward = new Vector2(speed, rigid.velocity.y).normalized;
            nextMove = 1;
        }
    }

    //Attack
    void Attack()
    {
        curAttackDelay += Time.deltaTime;

        GameObject snakeShot = objectManager.MakeObj("snakeAttack");
        EnemyObject snakeShotLogic = snakeShot.GetComponent<EnemyObject>();
        snakeShotLogic.enemyScript = enemyScript;
        snakeShot.transform.position = transform.position;

        Rigidbody2D S_rigid = snakeShot.GetComponent<Rigidbody2D>();
        SpriteRenderer sprite = snakeShot.GetComponent<SpriteRenderer>();
        Vector2 dirPos = (player.transform.position - snakeShot.transform.position).normalized;


        if(dirPos.x< 0 )
        {
            sprite.flipX = false;
            if (dirPos.y>0)
            {
                S_rigid.transform.Rotate(Vector3.back*(Mathf.Cos(Mathf.PI/2 * dirPos.x)*90));
            }
            else
            {
                S_rigid.transform.Rotate(Vector3.forward * (Mathf.Cos(Mathf.PI / 2 * dirPos.x) * 90));
            }   
        }
        else
        {
            sprite.flipX = true;
            if (dirPos.y < 0)
            {
                S_rigid.transform.Rotate(Vector3.back * (Mathf.Cos(Mathf.PI / 2 * dirPos.x) * 90));
            }
            else
            {
                S_rigid.transform.Rotate(Vector3.forward * (Mathf.Cos(Mathf.PI / 2 * dirPos.x) * 90));
            }
        }
        S_rigid.AddForce(dirPos * 5, ForceMode2D.Impulse);

        curAttackDelay = 0;
    }
    IEnumerator OnHit (float dmg)
    {
        if (isHit)
            yield break;

        isHit = true;
        DamageLogic(dmg);
        ReturnSprite(0.4f);

        if (health < 0)
        {
            isDie = true;
            yield return new WaitForSeconds(dieTime);

            gameObject.SetActive(false);
            anim.SetTrigger("doDead");
            gameManager.MonsterKillCheck();
        }
        yield return new WaitForSeconds(invincibleTime);

        isHit = false;
        ReturnSprite(1f);
    }
    void ReturnSprite(float Alpha)
    {
        spriteRenderer.color = new Color(0.2f, 0.2f, 0.2f, Alpha);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어의 무기에 공격당했을 때 
        if (collision.gameObject.tag == "PlayerAttack")
        {
            Player playerLogic = player.GetComponent<Player>();
            StartCoroutine(OnHit(playerLogic.dmg));

            //X좌표 넉백
            if (collision.gameObject.transform.position.x - transform.position.x < 0)
                rigid.AddForce(-forward*2,ForceMode2D.Impulse);
            else if (collision.gameObject.transform.position.x - transform.position.x > 0)
                rigid.AddForce(-forward * 2, ForceMode2D.Impulse);

        }
    }
}
