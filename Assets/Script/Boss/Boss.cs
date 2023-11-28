using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public enum Boss_Type { BossA, BossB, BossC }
    public Boss_Type bossType;

    public int patternIndex;
    public int deathHitCount;
    public int attackDamage;

    float rageGage;

    float frontPos;

    bool StateCore;
    bool SkillCore;

    //BossA
    public float rollingSpeed;
    public float PatternDelay;
    public float curShotDelay;
    public float maxShotDelay;

    float normalScatchRatio;

    float rageScatchRatio;
    float rageRollingRatio;

    float doReadyRoll;

    bool isRolling;

    bool isRolling_1;
    bool isRolling_2;

    float scaleCount;
    bool isScalesFire;

    bool isStart;
    bool isAppear;

    public bool rageState;

    [Header("공격")]
    public CircleCollider2D Rolling;
    public BoxCollider2D EarthQuake;
    public BoxCollider2D Roar;

    [Header("사운드")]
    public AudioSource hitSound;
    public AudioSource dieSound;
    public AudioSource rollSound;
    public AudioSource roarSound;
    public AudioSource earthQuakeSound;

    public Enemy enemyScript;
    public GameObject Trigger;
    public GameObject roarReadyParticle;

    IEnumerator roll;
    IEnumerator roar;
    IEnumerator jump;
    
    void Awake()
    {
        hitSound.volume = 0.5f;
        dieSound.volume = 0.5f;
        rollSound.volume = 0.5f;
        roarSound.volume = 0.5f;
        earthQuakeSound.volume = 0.5f;

        hitSound.enabled = false;
        dieSound.enabled = false;
        rollSound.enabled = false;
        roarSound.enabled = false;
        earthQuakeSound.enabled = false;
        
        PatternDelay = 1.45f;

        normalScatchRatio = 7;
        rageScatchRatio = 4;
        rageRollingRatio = 7;
        doReadyRoll = 5;

        rollingSpeed = 25;
        speed = 1;
        maxShotDelay = 0.5f;

        isDie = false;

        type = Def_Type.Resist;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        roll = RollAttack();
        roar = RoarAttack();
        jump = JumpAttack();
    }
    void OnEnable()
    {
        StartCoroutine(LevelCheck());
    }
    void FixedUpdate()
    {
        if (!isDie)
        {
            dist = Vector2.Distance(player.transform.position, transform.position);

            Follow();
            //BossA
            {
                //Moving
                if (!isAttack && isStart)
                {
                    Move();
                    anim.SetBool("isWalk", true);
                }
                //ScaleShot
                else if (isScalesFire)
                {
                    curShotDelay += Time.deltaTime;
                    ScalesAttack();
                }
                if (rageState)
                    RageSprite();

            }
        }
        else if(isDie)
        {
            StopCoroutine(roll);
            StopCoroutine(roar);
            StopCoroutine(jump);
            DieSprite();
        }

        if (Trigger.activeSelf == false&&!isAppear)
            StartCoroutine(Appear());
    }

    //Start Setting
    public IEnumerator Appear()
    {
        isAppear = true;
        anim.SetTrigger("Roar");
        gameManager.ShakeCam(1f,1f);
        yield return new WaitForSeconds(1f);

        gameManager.DropDebris();
        yield return new WaitForSeconds(2f);

        isStart = true;

        StartCoroutine(Think());
    }
    IEnumerator LevelCheck()
    {
        yield return new WaitForSeconds(0.5f);

        Enemy enemyLogic = enemyScript.GetComponent<Enemy>();
        level = enemyLogic.level;

        switch (bossType)
        {
            case Boss_Type.BossA:

                break;
            case Boss_Type.BossB:

                break;
            case Boss_Type.BossC:

                break;
        }
        switch (level)
        {
            case Level.Easy:
                health = 300;
                maxHealth = 300;
                dmg = 15;
                rageGage = 70;
                scaleCount = 9;
                break;

            case Level.Normal:

                break;

            case Level.Hard:
                health = 700;
                maxHealth = 700;
                dmg = 20;
                rageGage = 70;
                scaleCount = 13;
                break;

        }
    }

    //Move
    void Move()
    {
        rigid.velocity = new Vector2(frontPos * speed, rigid.velocity.y);
    }
    void Follow()
    {
        if (player.transform.position.x - transform.position.x < 0)
        {
            frontPos = -speed;
            spriteRenderer.flipX = false;
        }
        else if (player.transform.position.x - transform.position.x >= 0)
        {
            frontPos = speed;
            spriteRenderer.flipX = true;
        }
    }
    void RandomPattern()
    {
        int ranPattern = Random.Range(0, 10);
        if (!rageState)
        {
            if (ranPattern < normalScatchRatio) //70%
                patternIndex = 0;
            else                                //30%
                patternIndex = 1;
        }
        else
        {
            if (ranPattern < rageScatchRatio)         //40%
                patternIndex = 0;
            else if(ranPattern< rageRollingRatio)       //30%
                patternIndex = 1;
            else                        //30%
                patternIndex = 2;
        }
    }
    IEnumerator Think()
    {
        if (isDie)
            yield break;

        isAttack = false;

        rigid.velocity = Vector2.zero;

        if (!rageState)
            ReturnSprite(1f);
        //Rage Mode
        if (health <= maxHealth * (rageGage / 100f) && !rageState)
        {
            isAttack = true;
            rageState = true;
            yield return new WaitForSeconds(3);

            Rage();
            StartCoroutine(Think());
            yield break;
        }
        RandomPattern();
        switch(patternIndex)
        {
            case 0:     //Roar&ReThink
                if(dist<5)
                {
                    yield return new WaitForSeconds(PatternDelay);
                    StartCoroutine(RoarAttack());
                }
                else
                {
                    yield return new WaitForSeconds(PatternDelay);
                    StartCoroutine(Think());
                }
                break;
            case 1:     //Roll
                StartCoroutine(RollAttack());
                break;
            case 2:     //Jump
                StartCoroutine(JumpAttack());
                break;
            default:
                StartCoroutine(Think());
                break;
        }
    }

    //Rolling Attack
    IEnumerator RollAttack()
    {
        float rollDmg = dmg;
        yield return null;

        dmg = rollDmg * 1.2f;
        isRolling = true;
        isAttack = true;
        rollSound.enabled = true;
        anim.SetTrigger("StartRoll");
        rigid.AddForce(Vector2.right * frontPos * -doReadyRoll + Vector2.up * doReadyRoll, ForceMode2D.Impulse);

        int ranRoll = Random.Range(0, 2);
        yield return new WaitForSeconds(PatternDelay);

        Rolling.enabled = true;
        if (ranRoll == 0)
            isRolling_1 = true;

        if (ranRoll == 1)
            isRolling_2 = true;

        rigid.AddForce(Vector2.right * frontPos * rollingSpeed, ForceMode2D.Impulse);
        yield return new WaitForSeconds(PatternDelay);

        anim.SetTrigger("EndRoll");
        rollSound.enabled = false;
        dmg = rollDmg;
        Stun();
        Rolling.enabled = false;
        yield return new WaitForSeconds(PatternDelay * 2);

        StartCoroutine(Think());
    }

    //Scratch Attack
    IEnumerator RoarAttack()
    {
        isAttack = true;
        roarReadyParticle.SetActive(true);
        anim.SetBool("isWalk", false);
        roarSound.enabled = true;
        yield return new WaitForSeconds(0.5f);

        roarSound.volume = 1f;
        yield return new WaitForSeconds(PatternDelay);

        roarReadyParticle.SetActive(false);
        anim.SetTrigger("Roar");
        gameManager.Roar_Effect(transform.position,0.5f);
        Roar.enabled = true;

        yield return new WaitForSeconds(0.1f);

        Roar.enabled = false;
        yield return new WaitForSeconds(PatternDelay);

        roarSound.enabled = false;

        StartCoroutine(Think());
    }

    //Jump Attack
    IEnumerator JumpAttack()
    {
        isAttack = true;
        float Quakedmg=15;
        float curdmg=dmg;
        dmg = Quakedmg;
        yield return new WaitForSeconds(1f);

        anim.SetTrigger("doJump");
        rigid.AddForce(Vector2.up * 30, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);

        earthQuakeSound.enabled = true;
        anim.SetTrigger("doLand");
        gameManager.DropDebris();
        gameManager.ShakeCam(1f,1f);
        EarthQuake.enabled = true;
        yield return null;

        EarthQuake.enabled = false;
        dmg = curdmg;

        yield return new WaitForSeconds(0.4f);

        earthQuakeSound.enabled = false;
        StartCoroutine(Think());
    }

    //Scales Attack
    void ScalesAttack()
    {
        if (curShotDelay < maxShotDelay)
            return;

        int ran = Random.Range(0, 10);

        if (ran < 3)   //30%
        {
            for (float index = 0; index < scaleCount; index++)
            {
                GameObject scales = objectManager.MakeObj("bossAAttack_1");
                BossAttackObject scaleLogic = scales.GetComponent<BossAttackObject>();
                scaleLogic.enemyScript = enemyScript;
                scaleLogic.dmg = dmg * 0.7f;
                scaleLogic.Att_type = BossAttackObject.Attack_Type.Range;

                scales.transform.position = transform.position+Vector3.down*1f;
                Rigidbody2D S_rigid = scales.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * index / (scaleCount-1)),
                    Mathf.Sin(Mathf.PI * index / (scaleCount - 1)));

                S_rigid.transform.Rotate(Vector3.forward *180 * (index/((scaleCount-1))));
                S_rigid.AddForce(dirVec * 12, ForceMode2D.Impulse);
            }
        }
        curShotDelay = 0;
    }

    //Speed*1.25
    void Rage()
    {
        isScalesFire = true;
        isAttack = false;
        rageState = true;
        PatternDelay = 1.3f;
        speed *= 1.25f ;
    }

    //Rolling Stun
    void Stun()
    {
        isRolling = false;

        isRolling_1 = false;
        isRolling_2 = false;

        rigid.velocity = Vector2.zero;
        transform.rotation = Quaternion.identity;

        ReturnSprite(0.7f);
    }

    //Hit
    IEnumerator OnHit(float dmg)
    {
        if (isHit)
            yield break;


        hitSound.enabled = true;
        if (health > 0)
        {
            isHit = true;
            if (!rageState)
                DamageLogic(dmg);
            else if(rageState)
                DamageLogic(dmg*2);
            ReturnSprite(0.4f);

            //Rolling Animation
            if (!isRolling)
            {
                anim.SetTrigger("doHit");
            }
            yield return new WaitForSeconds(0.3f);

            hitSound.enabled = false;
            isHit = false;
            ReturnSprite(1f);
        }
        if (health <= 0)
        {
            //Dead Animation
            if (!isDie)
            {
                anim.SetTrigger("doDie");
                dieSound.enabled = true;
                isDie = true;
                for(int index=0;index<21;index++)
                {

                    Vector3 ranVec_1 = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
                    Vector3 ranVec_2 = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
                    Vector3 ranVec_3 = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
                    float ranPow = Random.Range(0.5f, 1.5f);
                    gameManager.ShakeCam(ranPow, 0.09f);
                    gameManager.Hit_Effect1(transform.position + ranVec_1, 0.5f);
                    gameManager.Hit_Effect2(transform.position + ranVec_2, 0.5f);
                    gameManager.Hit_Effect3(transform.position + ranVec_3, 0.5f);

                    yield return new WaitForSeconds(0.1f);
                }
            }

            ReturnSprite(0.3f);
            deathHitCount++;
            
            yield return new WaitForSeconds(0.2f);

            isHit = false;
            //Item
            int num = 3;
            if (StateCore || SkillCore)
                num = 2;
            if (StateCore && SkillCore)
                num = 1;

            int ranItem = Random.Range(0, num);
            string itemType = null;
            if ((deathHitCount == 3 || deathHitCount == 4) && !SkillCore)
                ranItem = 1;
            if ((deathHitCount == 3 || deathHitCount == 4) && !StateCore)
                ranItem = 2;

            switch (ranItem)
            {
                case 0:
                    itemType = "gold";
                    break;
                case 1:
                    int ranSkillCore = Random.Range(0, 3);
                    SkillCore = true;
                    switch (ranSkillCore)
                    {
                        case 0:
                            itemType = "rollCore";
                            break;
                        case 1:
                            itemType = "summonCore";
                            break;
                        case 2:
                            itemType = "dropCore";
                            break;
                    }
                    break;
                case 2:
                    StateCore = true;
                    itemType = "stateCore";
                    break;
            }
            if (deathHitCount <= 4 && itemType == "gold")
            {
                for (int index = 0; index < 10; index++)
                {
                    int ranPower = Random.Range(12, 18);
                    Vector2 ranVec = new Vector2(Random.Range(-1f, 1f), 0);
                    GameObject Item = objectManager.MakeObj(itemType);
                    Item.transform.position = transform.position;

                    Rigidbody2D Item_Rigid = Item.GetComponent<Rigidbody2D>();
                    Item_Rigid.AddForce(ranVec * 2 + Vector2.up * ranPower, ForceMode2D.Impulse);
                }
            }
            if (deathHitCount <= 4 && itemType != "gold")
            {
                Vector2 ranVec = new Vector2(Random.Range(-1f, 1f), 0);
                GameObject Item = objectManager.MakeObj(itemType);
                Item.transform.position = transform.position;

                Rigidbody2D Item_Rigid = Item.GetComponent<Rigidbody2D>();
                Item_Rigid.AddForce(ranVec * 2 + Vector2.up * 4, ForceMode2D.Impulse);
            }
        }
    }
    void ReturnSprite(float Alpha)
    {
        spriteRenderer.color = new Color(0.4f, 0.4f, 0.4f, Alpha);
    }

    //Sprite
    void RageSprite()
    {
        spriteRenderer.color = new Color(1, 0.5f, 0.5f, 1);
    }
    void DieSprite()
    {
        ReturnSprite(0.5f);
    }

    //Tigger
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDie)
            return;
        if(collision.gameObject.tag=="Border")
        {
            if (isRolling_1)
            {
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                gameManager.SpawnMouse();
            }
            if (isRolling_2)
            {
                rigid.velocity = Vector2.zero;
                rigid.AddForce(Vector2.right * frontPos * rollingSpeed, ForceMode2D.Impulse);
                gameManager.SpawnMouse();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        int ranHit = Random.Range(0, 3);
        //플레이어의 무기에 공격당했을 때 
        if (collision.gameObject.tag == "PlayerAttack")
        {
            Player playerLogic = player.GetComponent<Player>();
            StartCoroutine(OnHit(playerLogic.dmg));
            switch(ranHit)
            {
                case 0:
                    gameManager.Hit_Effect1(collision.bounds.ClosestPoint(transform.position), 0.5f);
                    break;
                case 1:
                    gameManager.Hit_Effect2(collision.bounds.ClosestPoint(transform.position), 0.5f);
                    break;
                case 2:
                    gameManager.Hit_Effect3(collision.bounds.ClosestPoint(transform.position), 0.5f);
                    break;
            }
        }
        if (collision.gameObject.tag == "Follow")
        {
            Player playerLogic = player.GetComponent<Player>();
            StartCoroutine(OnHit(playerLogic.dmg*0.3f));
            gameManager.Hit_Effect1(collision.bounds.ClosestPoint(transform.position), 0.5f);
            switch (ranHit)
            {
                case 0:
                    gameManager.Hit_Effect1(collision.bounds.ClosestPoint(transform.position), 0.5f);
                    break;
                case 1:
                    gameManager.Hit_Effect2(collision.bounds.ClosestPoint(transform.position), 0.5f);
                    break;
                case 2:
                    gameManager.Hit_Effect3(collision.bounds.ClosestPoint(transform.position), 0.5f);
                    break;
            }
        }
    }
}