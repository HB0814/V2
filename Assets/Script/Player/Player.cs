using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("�÷��̾� ����")]
    public float curLife;
    public float maxLife;
    public float curCoolDown;
    public float maxCoolDown;

    public float dmg;
    public float jumpPower;
    public float speed;
    
    public bool isStart;

    [Header("�������ͽ� �ھ�")]
    public bool stateCore;

    [Header("���� ����")]
    public float sacrifice;

    [Header("��ų �ھ� ���� ����")]
    public bool CrouchCore;
    public bool RollCore;
    public bool DropCore;
    public bool SummonCore;

    public enum Skill_Core { Crouch, Roll, Drop, Summon }
    public Skill_Core skill_Type;

    [Header("���� ������� ��ų �ھ�")]
    public bool isCrouchCore;
    public bool isRollCore;
    public bool isDropCore;
    public bool isSummonCore;

    [Header("���� �������� ��ų �ھ�")]
    public bool equipCrouch;
    public bool equipRoll;
    public bool equipDrop;
    public bool equipSummon;

    public float equipCount;

    [Header("���� ��ȯ�� ����")]
    public int followCount;
    int maxFollowCount;
    int hpDecrease;

    [Header("���� Ÿ��")]
    public bool isNormal;
    public bool isPower;
    public bool isSharp;
    public bool isMystic;

    public enum Att_Type { Normal, Power, Sharp, Mystic }
    public Att_Type att_Type;

    bool Q_IsSwitch;

    float crouchCoolDown;
    float rollCoolDown;
    float dropCoolDown;
    float summonCoolDown;

    float roll_Speed;

    //���� ������
    float curAttackDelay;
    float maxAttackDelay;

    //��ų ���ӽð�
    float curSkillTime;
    float maxSkillTime;

    //Controller
    float Move_Axis;

    bool isGround;
    bool isJump;
    bool jumping;

    bool isAtt;
    bool speedUp;
    bool speedDown;
    bool isCrouch;
    bool isDash;

    public bool OnSkill;

    //Trigger
    bool isTouchRoom;       //���� �̱���

    [Header("Ʈ����")]
    public bool isElite;
    public bool isBoss;
    public bool clearMap;       //Ŭ���� �� Ʈ���� �۵�(�̱���)

    public bool isCameraMove;

    bool OnElite;
    bool OnBoss;

    bool isHit;

    bool isCheat;

    [Header("������Ʈ")]
    public ObjectManager objectManager;
    public GameManager gameManager;

    [Header("��ƼŬ")]
    public GameObject crouchParticle;
    public GameObject right_DashParticle;
    public GameObject left_DashParticle;
    public GameObject dropParticle;
    public GameObject coreChangeParticle;
    public GameObject healingParticle;

    [Header("����")]
    public BoxCollider2D meleeAttack;
    public BoxCollider2D dropAttack;
    public CircleCollider2D rollAttack;

    [Header("�÷��̾� ����")]
    public AudioSource walkSound;
    public AudioSource jumpSound;
    public AudioSource HitSound_1;
    public AudioSource HitSound_2;
    public AudioSource HitSound_3;
    public AudioSource meleeAttackSound;
    public AudioSource crouchAttackSound;
    public AudioSource rollAttackSound;
    public AudioSource dropAttackSound;
    public AudioSource summonAttackSound;
    public AudioSource healSound;
    public AudioSource weaponChangeSound;

    Animator anim;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;

    void Awake()
    {

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //�ִ�ü��&��üü��
        maxLife = 150;
        curLife = 150;

        //��Ÿ��
        maxCoolDown = 0;
        crouchCoolDown = 0;
        rollCoolDown = 0;
        dropCoolDown = 0;
        summonCoolDown = 0;

        //���ݷ�&�ӵ�&������ �ӵ�
        dmg = 15;
        speed = 5;
        roll_Speed = speed;

        //���ݼӵ�&�ִ뽺ų�ð�
        maxAttackDelay = 0.5f;
        maxSkillTime = 2f;

        //��ȯ�� ����
        maxFollowCount = 3;
        followCount = 0;
        hpDecrease = 25;

        //��ų�ھ� �� ����Ÿ��
        CrouchCore = true;
        skill_Type = Skill_Core.Crouch;
        equipCount = 4;
        isCrouchCore = true;
        equipCrouch = true;
        equipRoll = true;
        equipDrop = true;
        equipSummon = true;
        anim.SetBool("isLieDown", true);

        walkSound.volume = 0.7f;
        jumpSound.volume = 0.7f;
        HitSound_1.volume = 0.7f;
        HitSound_2.volume = 0.7f;
        HitSound_3.volume = 0.7f;
        meleeAttackSound.volume = 0.7f;
        crouchAttackSound.volume = 0.7f;
        rollAttackSound.volume = 0.7f;
        dropAttackSound.volume = 0.7f;
        summonAttackSound.volume = 0.7f;
        healSound.volume = 0.7f;
        weaponChangeSound.volume = 0.7f;
    }
    void Update()
    {
        InPut();

        CoolDown();

        SkillOn();
        if (isCameraMove)
            Stop();
        else
            Move();

        MoveMent();
        Jump();
        StartCoroutine(Attack());

        if (isStart)
        {
            StartCoroutine(Skill());
            StartCoroutine(Summon());
            StartCoroutine(Drop());
        }
        StartCoroutine(Switching_Attack_Type());
        Core_TypeMatch();

        LifeCheck();
        StartCoroutine(heal());
        JCanvas.Instance.HPBarSet(maxLife,curLife);
    }
    //�÷��̾� �̵�
    void MoveMent()
    {
        if (!clearMap && Move_Axis == 1)
        {
            Move_Axis = 0;
        }
        if (isTouchRoom || isCrouch || isCameraMove || isElite || isBoss)
            Move_Axis = 0;

        rigid.velocity = new Vector2(Move_Axis * speed, rigid.velocity.y);
        if (Move_Axis != 0)
        {
            walkSound.enabled = true;
            if (Move_Axis > 0)
            {
                spriteRenderer.flipX = false;
                if (isDash)
                {
                    right_DashParticle.SetActive(true);
                    left_DashParticle.SetActive(false);
                }
                meleeAttack.offset = new Vector2(0.6f, 0);
            }
            else if (Move_Axis < 0)
            {
                if (isDash)
                {
                    right_DashParticle.SetActive(false);
                    left_DashParticle.SetActive(true);
                }
                spriteRenderer.flipX = true;
                meleeAttack.offset = new Vector2(-0.6f, 0);
            }
        }
        else
            walkSound.enabled = false;

    }
    void Stop()
    {
        rigid.velocity = Vector2.zero;
        rigid.gravityScale = 0;
    }
    void Move()
    {
        rigid.gravityScale = 3;
    }

    //���� �� ���� �Ұ�
    void InPut()
    {
        if (!isStart || isCrouch || isCameraMove || isElite || isBoss)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            return;
        }

        Move_Axis = Input.GetAxisRaw("Horizontal"); //�̵�
        isAtt = Input.GetKeyDown(KeyCode.A);          //����
        isJump = Input.GetKeyDown(KeyCode.S);         //����
        Q_IsSwitch = Input.GetKeyDown(KeyCode.Q);   //���� Ÿ�� ����ü���� ���->�Ŀ�->����->�ź�
        isCheat = Input.GetKeyDown(KeyCode.R);      //ġƮŰ= ü�� 100����

    }
    void Input_Skill()
    {
        if (!isStart || isCameraMove || isBoss || isElite)
            return;

        if (!OnSkill)
        {
            OnSkill = Input.GetKeyDown(KeyCode.D);      //��ų
        }
        else
            curCoolDown = 0;
    }
    void Jump()
    {
        if (rigid.velocity.y == 0)
        {
            jumpSound.enabled = false;
            isGround = true;
            jumping = false;
        }
        if (isJump && jumping)
        {
            jumpSound.enabled = false;
            jumpSound.enabled = true;

            anim.SetTrigger("doJump");
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumping = false;
        }
        if (isJump && isGround)
        {
            jumpSound.enabled = true;
            anim.SetTrigger("doJump");
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isGround = false;
            jumping = true;
        }
    }

    IEnumerator Attack()
    {
        curAttackDelay += Time.deltaTime;
        if (curAttackDelay < maxAttackDelay)
            yield break;
        if (isBoss || !isAtt)
            yield break;

        meleeAttackSound.enabled = true;
        meleeAttack.enabled = true;
        anim.SetTrigger("doAttack");
        yield return new WaitForSeconds(0.1f);

        meleeAttack.enabled = false;
        isAtt = false;
        curAttackDelay = 0;

        yield return new WaitForSeconds(0.4f);

        meleeAttackSound.enabled = false;
    }

    //��ų
    IEnumerator Skill()
    {
        switch (skill_Type)
        {
            case Skill_Core.Roll:
                if (OnSkill)
                {
                    isHit = true;
                    isDash = true;
                    Physics2D.IgnoreLayerCollision(6, 7, true);
                    rollAttack.enabled = true;
                    rollAttackSound.enabled = true;


                    if (!speedUp)
                        StartCoroutine(RollingSpeedUp());
                }
                else
                {
                    isHit = false;
                    isDash = false;
                    Physics2D.IgnoreLayerCollision(6, 7, false);
                    right_DashParticle.SetActive(false);
                    left_DashParticle.SetActive(false);

                    rollAttack.enabled = false;
                    rollAttackSound.enabled = false;
                    if (!speedDown)
                        StartCoroutine(RollingSpeedDown());
                }

                break;
            case Skill_Core.Crouch:
                if (OnSkill)
                {
                    isHit = true;
                    isCrouch = true;
                    crouchAttackSound.enabled = true;
                    anim.SetBool("isLieDown", true);
                    crouchParticle.SetActive(true);
                    Physics2D.IgnoreLayerCollision(6, 7, true);
                }
                else
                {
                    isHit = false;
                    isCrouch = false;
                    crouchAttackSound.enabled = false;
                    anim.SetBool("isLieDown", false);
                    crouchParticle.SetActive(false);
                    Physics2D.IgnoreLayerCollision(6, 7, false);
                }
                break;
        }
        yield return null;
    }
    IEnumerator RollingSpeedUp()
    {
        if (speed == roll_Speed* 1.75f)
            yield break;
        if (speed > roll_Speed * 1.75f)
            speed = roll_Speed * 1.75f;

        speedUp = true;
        yield return new WaitForSeconds(0.15f);

        speed = speed + (roll_Speed * 0.15f);
        speedUp = false;
    }
    IEnumerator RollingSpeedDown()
    {
        if (speed == roll_Speed)
            yield break;
        if (speed < roll_Speed)
            speed = roll_Speed;

        speedDown = true;
        yield return new WaitForSeconds(0.1f);

        speed = speed - (roll_Speed * 0.1f);
        speedDown = false;
    }
    IEnumerator Summon()
    {
        if(skill_Type!=Skill_Core.Summon)
            yield break;

        if (curLife <= hpDecrease || followCount >= maxFollowCount)
        yield break;

        if (Input.GetKeyDown(KeyCode.D))
        {
            followCount++;
            gameManager.FollowerSpawn();
            maxLife -= hpDecrease;
            summonAttackSound.enabled = true;
            yield return new WaitForSeconds(2f);

            summonAttackSound.enabled = false;
        }

    }
    IEnumerator Drop()
    {
        if (skill_Type != Skill_Core.Drop)
            yield break;

        if (!jumping)
            yield break;

        if (Input.GetKeyDown(KeyCode.D)&& curCoolDown == maxCoolDown)
        {
            rigid.AddForce(Vector2.down * 20, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.2f);

            dropParticle.SetActive(true);
            dropAttackSound.enabled = true;
            dropAttack.enabled = true;
            yield return new WaitForSeconds(0.2f);

            dropAttack.enabled = false;
            yield return new WaitForSeconds(0.3f);

            dropParticle.SetActive(false);
            dropAttackSound.enabled = false;
        }
        yield return null;
    }

    void SkillOn()
    {
        if (!isStart)
            return;

        if (OnSkill)
        {
            curSkillTime += Time.deltaTime;

            if (curSkillTime > maxSkillTime)
            {
                curSkillTime = 0;
                SkillOff();
            }
        }
        else
            curSkillTime = 0;
    }
    void SkillOff()
    {
        OnSkill = false;
    }
    void CoolDown()
    {
        if (!isStart)
            return;

        curCoolDown += Time.deltaTime;

        if (curCoolDown > maxCoolDown)
            curCoolDown = maxCoolDown;

        if(curCoolDown==maxCoolDown)
            Input_Skill();

        if (Input.GetKeyUp(KeyCode.D))
            OnSkill = false;
    }

    //ü�� Ȯ��
    void LifeCheck()
    {
        if (curLife > maxLife)
            curLife = maxLife;
    }

    //����Ÿ�� ����
    IEnumerator Switching_Attack_Type()
    {
        if (Q_IsSwitch)
        {
            weaponChangeSound.enabled = true;
            coreChangeParticle.SetActive(true);
            int num;
            //4�� �����
            if (equipCount == 4)
            {
                switch (skill_Type)
                {
                    case Skill_Core.Crouch:
                        skill_Type = Skill_Core.Roll;
                        maxCoolDown = rollCoolDown;
                        break;

                    case Skill_Core.Roll:
                        skill_Type = Skill_Core.Drop;
                        maxCoolDown = dropCoolDown;
                        break;
                    case Skill_Core.Drop:
                        skill_Type = Skill_Core.Summon;
                        maxCoolDown = summonCoolDown;
                        break;

                    case Skill_Core.Summon:
                        skill_Type = Skill_Core.Crouch;
                        maxCoolDown = crouchCoolDown;
                        break;
                }
            }
            //3�� �����
            else if (equipCount == 3)
            {
                num = Equip_Core_3();
                switch (num)
                {
                    case 1:
                        switch (skill_Type)
                        {
                            case Skill_Core.Crouch:
                                skill_Type = Skill_Core.Roll;
                                maxCoolDown = rollCoolDown;
                                break;

                            case Skill_Core.Roll:
                                skill_Type = Skill_Core.Drop;
                                maxCoolDown = dropCoolDown;
                                break;

                            case Skill_Core.Drop:
                                skill_Type = Skill_Core.Crouch;
                                maxCoolDown = crouchCoolDown;
                                break;
                        }
                        break;
                    case 2:
                        switch (skill_Type)
                        {
                            case Skill_Core.Crouch:
                                skill_Type = Skill_Core.Roll;
                                maxCoolDown = rollCoolDown;
                                break;

                            case Skill_Core.Roll:
                                skill_Type = Skill_Core.Summon;
                                maxCoolDown = summonCoolDown;
                                break;

                            case Skill_Core.Summon:
                                skill_Type = Skill_Core.Crouch;
                                maxCoolDown = crouchCoolDown;
                                break;
                        }
                        break;
                    case 3:
                        switch (skill_Type)
                        {
                            case Skill_Core.Crouch:
                                skill_Type = Skill_Core.Drop;
                                maxCoolDown = dropCoolDown;
                                break;

                            case Skill_Core.Drop:
                                skill_Type = Skill_Core.Summon;
                                maxCoolDown = summonCoolDown;
                                break;

                            case Skill_Core.Summon:
                                skill_Type = Skill_Core.Crouch;
                                maxCoolDown = crouchCoolDown;
                                break;
                        }
                        break;
                    case 4:
                        switch (skill_Type)
                        {
                            case Skill_Core.Roll:
                                skill_Type = Skill_Core.Drop;
                                maxCoolDown = dropCoolDown;
                                break;

                            case Skill_Core.Drop:
                                skill_Type = Skill_Core.Summon;
                                maxCoolDown = summonCoolDown;
                                break;

                            case Skill_Core.Summon:
                                skill_Type = Skill_Core.Roll;
                                maxCoolDown = rollCoolDown;
                                break;
                        }
                        break;
                }
            }
            //2�� �����
            else if (equipCount == 2)
            {
                num = Equip_Core_2();
                switch (num)
                {
                    case 1:
                        switch (skill_Type)
                        {
                            case Skill_Core.Crouch:
                                skill_Type = Skill_Core.Roll;
                                maxCoolDown = rollCoolDown;
                                break;

                            case Skill_Core.Roll:
                                skill_Type = Skill_Core.Crouch;
                                maxCoolDown = crouchCoolDown;
                                break;
                        }
                        break;
                    case 2:
                        switch (skill_Type)
                        {
                            case Skill_Core.Crouch:
                                skill_Type = Skill_Core.Drop;
                                maxCoolDown = dropCoolDown;
                                break;

                            case Skill_Core.Drop:
                                skill_Type = Skill_Core.Crouch;
                                maxCoolDown = crouchCoolDown;
                                break;
                        }
                        break;
                    case 3:
                        switch (skill_Type)
                        {
                            case Skill_Core.Crouch:
                                skill_Type = Skill_Core.Summon;
                                maxCoolDown = summonCoolDown;
                                break;

                            case Skill_Core.Summon:
                                skill_Type = Skill_Core.Crouch;
                                maxCoolDown = crouchCoolDown;
                                break;
                        }
                        break;
                    case 4:
                        switch (skill_Type)
                        {
                            case Skill_Core.Roll:
                                skill_Type = Skill_Core.Drop;
                                maxCoolDown = dropCoolDown;
                                break;

                            case Skill_Core.Drop:
                                skill_Type = Skill_Core.Roll;
                                maxCoolDown = rollCoolDown;
                                break;
                        }
                        break;
                    case 5:
                        switch (skill_Type)
                        {
                            case Skill_Core.Roll:
                                skill_Type = Skill_Core.Summon;
                                maxCoolDown = summonCoolDown;
                                break;

                            case Skill_Core.Summon:
                                skill_Type = Skill_Core.Roll;
                                maxCoolDown = rollCoolDown;
                                break;
                        }
                        break;
                    case 6:
                        switch (skill_Type)
                        {
                            case Skill_Core.Drop:
                                skill_Type = Skill_Core.Summon;
                                maxCoolDown = summonCoolDown;
                                break;

                            case Skill_Core.Summon:
                                skill_Type = Skill_Core.Drop;
                                maxCoolDown = dropCoolDown;
                                break;
                        }
                        break;
                }
            }
            else if (equipCount == 1)
                yield break;
            yield return new WaitForSeconds(1f);

            coreChangeParticle.SetActive(false);
            weaponChangeSound.enabled = false;
            Q_IsSwitch = false;
        }
    }
    void Core_TypeMatch()
    {
        if(skill_Type== Skill_Core.Crouch)
        {
            isCrouchCore = true;
            isRollCore = false;
            isDropCore = false;
            isSummonCore = false;

            att_Type = Att_Type.Normal;
            isNormal = true;
            isPower = false;
            isSharp = false;
            isMystic = false;
        }
        if (skill_Type == Skill_Core.Roll)
        {
            isCrouchCore = false;
            isRollCore = true;
            isDropCore = false;
            isSummonCore = false;

            att_Type = Att_Type.Power;
            isNormal = false;
            isPower = true;
            isSharp = false;
            isMystic = false;
        }
        if (skill_Type == Skill_Core.Drop)
        {
            isCrouchCore = false;
            isRollCore = false;
            isDropCore = true;
            isSummonCore = false;

            att_Type = Att_Type.Sharp;
            isNormal = false;
            isPower = false;
            isSharp = true;
            isMystic = false;
        }
        if (skill_Type == Skill_Core.Summon)
        {
            isCrouchCore = false;
            isRollCore = false;
            isDropCore = false;
            isSummonCore = true;

            att_Type = Att_Type.Mystic;
            isNormal = false;
            isPower = false;
            isSharp = false;
            isMystic = true;
        }
    }

    int Equip_Core_3()
    {
        if (equipCrouch && equipRoll && equipDrop)
        {
            return 1;
        }
        if (equipCrouch && equipRoll && equipSummon)
        {
            return 2;
        }
        if (equipCrouch && equipDrop && equipSummon)
        {
            return 3;
        }
        if (equipRoll && equipDrop && equipSummon)
        {
            return 4;
        }
        return 0;
    }
    int Equip_Core_2()
    {
        if (equipCrouch && equipRoll)
        {
            return 1;
        }
        if (equipCrouch && equipDrop)
        {
            return 2;
        }
        if (equipCrouch && equipSummon)
        {
            return 3;
        }
        if (equipRoll && equipDrop)
        {
            return 4;
        }
        if (equipRoll && equipSummon)
        {
            return 5;
        }
        if (equipDrop && equipSummon)
        {
            return 6;
        }
        return 0;
    }
    //ġƮ     Life+100
    IEnumerator heal()
    {
        if (!isCheat)
            yield break;

        yield return null;
        curLife += 100;
        maxLife += 100;
        healingParticle.SetActive(true);
        healSound.enabled = true;

        yield return new WaitForSeconds(1f);

        healingParticle.SetActive(false);
        healSound.enabled = false;
        isCheat = false;
    }

    //�ǰ�
    IEnumerator OnHit(float dmg)
    {
        if (isHit)
            yield break;

        int ranSound = Random.Range(0, 3);
        switch(ranSound)
        {
            case 0:
                HitSound_1.enabled = true;
                break;
            case 1:
                HitSound_2.enabled = true;
                break;
            case 2:
                HitSound_3.enabled = true;
                break;
        }

        isHit = true;
        curLife -= dmg;
        ReturnSprite(0.8f);
        anim.SetTrigger("doHit");
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * 4, ForceMode2D.Impulse);

        if (curLife < 0)
        {
            gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.3f);

        HitSound_1.enabled = false;
        HitSound_2.enabled = false;
        HitSound_3.enabled = false;
        isHit = false;
        ReturnSprite(1f);
    }
    void ReturnSprite(float Alpha)
    {
        spriteRenderer.color = new Color(1, 1, 1, Alpha);
    }

    //���� ���� Ʈ����
    IEnumerator EliteStart()
    {
        isElite = true;
        spriteRenderer.flipX = false;
        if (!OnElite)
        {
            gameManager.CreateElite();
            gameManager.OnEliteRoom();
        }
        OnElite = true;
        yield return new WaitForSeconds(2f);

        isElite = false;
    }
    IEnumerator BossStart()
    {
        isBoss = true;
        spriteRenderer.flipX = false;
        if (!OnBoss)
            gameManager.BossRoom();
        OnBoss = true;
        yield return new WaitForSeconds(4f);

        isBoss = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemyLogic = collision.gameObject.GetComponentInParent<Enemy>();

            StartCoroutine(OnHit(enemyLogic.dmg));
        }

        //Item
        if (collision.gameObject.tag == "Item")
        {
            Item itemLogic = collision.gameObject.GetComponent<Item>();
            switch (itemLogic.itemType)
            {
                case Item.ItemType.Coin:
                    sacrifice += 1;
                    break;

                case Item.ItemType.Core:
                    switch (itemLogic.coreType)
                    {
                        case Item.CoreType.State:
                            gameManager.StateCore_Increase();
                            break;
                        case Item.CoreType.Roll:
                            RollCore = true;
                            equipRoll = true;
                            break;
                        case Item.CoreType.Summon:
                            SummonCore = true;
                            equipSummon = true;
                            break;
                        case Item.CoreType.Drop:
                            DropCore = true;
                            equipDrop = true;
                            break; 
                    }
                    break;
            }
            collision.gameObject.SetActive(false);
        }
        //Ground
        if (collision.gameObject.tag == "Ground")
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack")
        {
            EnemyObject enemyLogic = collision.gameObject.GetComponent<EnemyObject>();

            StartCoroutine(OnHit(enemyLogic.dmg));
        }
        if (collision.gameObject.tag == "BossAttack")
        {
            BossAttackObject objectLogic = collision.gameObject.GetComponent<BossAttackObject>();
            switch (objectLogic.Att_type)
            {
                case BossAttackObject.Attack_Type.Melee:
                    StartCoroutine(OnHit(objectLogic.dmg));
                    break;
                case BossAttackObject.Attack_Type.Range:
                    StartCoroutine(OnHit(objectLogic.dmg));
                    break;
            }
        }
        if (collision.gameObject.tag == "Boss")
        {
            Boss bossLogic = collision.gameObject.GetComponentInParent<Boss>();

            StartCoroutine(OnHit(bossLogic.dmg));
        }
        if (collision.gameObject.tag == "EliteMonster")
        {
            EliteEnemy eliteLogic = collision.gameObject.GetComponentInParent<EliteEnemy>();

            StartCoroutine(OnHit(eliteLogic.dmg));
        }
        if (collision.gameObject.tag == "TriggerMap")
        {
            isTouchRoom = true;
        }
        if (collision.gameObject.tag == "BossTrigger" && collision.gameObject.name == "EliteTrigger")
        {
            StartCoroutine(EliteStart());
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "BossTrigger" && collision.gameObject.name == "BossTrigger")
        {
            StartCoroutine(BossStart());
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "Debris")
        {
            Debris debrisLogic = collision.gameObject.GetComponent<Debris>();
            collision.gameObject.SetActive(false);

            StartCoroutine(OnHit(debrisLogic.dmg));
        }

        if(collision.gameObject.name=="EndPortal")
        {
            Debug.Log("EndPortal");
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TriggerMap")
        {
            isTouchRoom = false;
        }
    }
}
