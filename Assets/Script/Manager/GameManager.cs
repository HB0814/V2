using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Camera
    float delay;
    float Move_smoothTime;
    public float viewHeight_Y;
    public float viewHeight_X;
    public  float harf_ViewHeaight_Y;
    public float harf_ViewHeaight_X;

    [Header("상하좌우 카메라 타겟")]
    public Transform bossCamera;

    public Transform[] target;
    bool touchingCamera;
    bool smoothCamera;

    Vector3 velocity = Vector3.zero;

    bool isTopMap;
    bool isBottomMap;
    bool isRightMap;
    bool isLeftMap;

    //ShakeCamera
    Vector3 myVec;

    public List<CoreInf> coreInformation;
    public int StateCoreIndex;
    
    public Vector3[] eliteSkill_Spot;

    [Header("[스폰 지점]{원거리}{근거리}{아이템박스}{낙하물}{보스}")]
    public Transform[] Range_SpawnPoint;
    public Transform[] Shot_SpawnPoint;
    public Transform[] Item_SpawnPoint;
    public Transform[] Drop_SpawnPoint;
    public Transform eliteSpawnPoint;
    public Transform bossSpawnPoint;

    [Header("보스맵 투명벽")]
    public BoxCollider2D[] elite_Border;
    public BoxCollider2D[] boss_Border;
    [Header("보스맵 트리거")]
    public GameObject eliteMonster_Trigger;
    public GameObject boss_Trigger;


    [Header("플레이어")]
    public GameObject player;
    [Header("오브젝트 매니저")]
    public ObjectManager objectManager;
    [Header("몬스터스크립트")]
    public Enemy enemyScript;

    void Awake()
    {
        coreInformation = new List<CoreInf>();
        StateCoreIndex = 0;

        smoothCamera = false;
        touchingCamera = false;

        delay = 0.01f;  //거리가 0.01까지 타겟지점으로 카메라 이동
        Move_smoothTime = 0.2f; //카메라 속도

        //카메라 사이즈
        harf_ViewHeaight_Y = Camera.main.orthographicSize;
        harf_ViewHeaight_X = Camera.main.orthographicSize * 16 / 9;
        viewHeight_Y = harf_ViewHeaight_Y * 2;
        viewHeight_X = harf_ViewHeaight_X * 2;

        //이동 카메라 좌표
        target[0].transform.localPosition = new Vector3(0,viewHeight_Y+delay,-1);
        target[1].transform.localPosition = new Vector3(0, -viewHeight_Y + delay, -1);
        target[2].transform.localPosition = new Vector3(viewHeight_X+delay,0, -1);
        target[3].transform.localPosition = new Vector3(-viewHeight_X + delay, 0, -1);

        //EliteSkillSpot
        eliteSkill_Spot = new Vector3[3];

        SpawnEnemy();//레벨 지정 전까지 임시 스폰
    }
    void Update()
    {
        if (touchingCamera)
        {
            TouchCamera();
            NextMap();
        }
        if(smoothCamera)
        {
            SmoothCamera();
        }
    }
    //카메라 이동
    void TouchCamera()
    {
        if (player.transform.position.y + harf_ViewHeaight_Y > target[0].position.y)
        {
            isTopMap = true;
        }
        else if (player.transform.position.y - harf_ViewHeaight_Y < target[1].position.y)
        {
            isBottomMap = true;
        }
        else if (player.transform.position.x + harf_ViewHeaight_X > target[2].position.x)
        {
            isRightMap = true;
        }
        if (player.transform.position.x - harf_ViewHeaight_X < target[3].position.x)
        {
            isLeftMap = true;
        }
    }       //카메라 트리거
    void NextMap()
    {
        Player playerLogic = player.GetComponent<Player>();
        if (isTopMap)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position,
                target[0].position, ref velocity, Move_smoothTime);

            if (Vector3.Distance(target[0].position, Camera.main.transform.position) < delay)
            {
                isTopMap = false;
                for (int index = 0; index < 4; index++)
                {
                    target[index].position = target[index].position + new Vector3(0, viewHeight_Y - delay, 0);
                }
            }
        }
        if (isBottomMap)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position,
                target[1].position, ref velocity, Move_smoothTime);

            if (Vector3.Distance(target[1].position, Camera.main.transform.position) < delay)
            {
                isBottomMap = false;
                for (int index = 0; index < 4; index++)
                {
                    target[index].position = target[index].position + new Vector3(0, -(viewHeight_Y - delay), 0);
                }
            }
        }
        if (isRightMap)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position,
                target[2].position, ref velocity, Move_smoothTime);

            playerLogic.isCameraMove = true;
            if (Vector3.Distance(target[2].position, Camera.main.transform.position) < delay)
            {
                isRightMap = false;
                for (int index = 0; index < 4; index++)
                {
                    playerLogic.isCameraMove = false;

                    target[index].position = target[index].position + new Vector3(viewHeight_X - delay, 0, 0);
                }
            }
        }
        if (isLeftMap)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position,
                target[3].position, ref velocity, Move_smoothTime);

            playerLogic.isCameraMove = true;
            if (Vector3.Distance(target[3].position, Camera.main.transform.position) < delay)
            {
                isLeftMap = false;
                for (int index = 0; index < 4; index++)
                {
                    playerLogic.isCameraMove = false;

                    target[index].position = target[index].position + new Vector3(-(viewHeight_X - delay), 0, 0);
                }
            }
        }
    }           //다음맵으로 카메라 이동
    void SmoothCamera()
    {
        smoothCamera = true;
        Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y, -1);

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPos, 1 * Time.deltaTime) ;   
    }
    public void SmoothCameraOn()
    {
        Invoke("SmoothCamera", 7f);
    }

    //카메라 흔들기
    public void ShakeCam(float power, float shakeTime)
    {
        StartCoroutine(CameraShake(power, shakeTime));
    }
    IEnumerator CameraShake(float power, float time)
    {
        myVec = Camera.main.transform.position;
        float timer = 0;
        while (timer < time)
        {
            Camera.main.transform.position = (Vector3)UnityEngine.Random.insideUnitSphere * power + myVec;

            timer += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.position = myVec;
    }

    //UI
    public void Easy()
    {
        enemyScript.level = Enemy.Level.Easy;
    }
    public void Normal()
    {
        enemyScript.level = Enemy.Level.Easy; //Normal선택시 Easy난이도 설정
    }
    public void Hard()
    {
        enemyScript.level = Enemy.Level.Hard;
    }
    void SpawnEnemy()
    {
        CreateMonsterA();
        CreateMonsterB();
        CreateItemBox();
    }

    public void GameReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GameQuit()
    {
        Application.Quit();
    }

    public void StateCore_Increase()
    {
        CoreInf coreInfo = new CoreInf();

        coreInfo.increaseDamage = Random.Range(10, 21);
        coreInfo.increaseHealth = Random.Range(30, 51);

        coreInformation.Add(coreInfo);

        PrintInfo();
        StateCoreIndex++;
    }
    public void PrintInfo()
    {
        Debug.Log($"스탯코어[{StateCoreIndex+1}] 현재 갯수:"+ (StateCoreIndex+1));
        Debug.Log($"스탯코어[{StateCoreIndex}] +공격력 능력치:" + coreInformation[StateCoreIndex].increaseDamage);
        Debug.Log($"스탯코어[{StateCoreIndex}] +체력 능력치:" + coreInformation[StateCoreIndex].increaseHealth);
    }
    public void EquipCore()
    {
        Player playerLogic = player.GetComponent<Player>();
        if (playerLogic.equipCount == 3)
            return;

    }

    //이펙트
    public void Ground_Effect(Vector3 target)
    {
        GameObject ground_Effect = objectManager.MakeObj("ground_Effect");
        ParticleSystem tround_EffectLogic = ground_Effect.GetComponent<ParticleSystem>();
        tround_EffectLogic.Play();
        ground_Effect.transform.position = target;
    }
    public void Spawn_Effect(Vector3 target,float time)
    {
        GameObject spawn_Effect = objectManager.MakeObj("spawn_Effect");
        ParticleSystem spawn_EffectLogic = spawn_Effect.GetComponent<ParticleSystem>();
        spawn_EffectLogic.Play();
        spawn_Effect.GetComponent<Effect>().time = time;

        spawn_Effect.transform.position = target+ Vector3.up * 3;
        
    }
    public void Roar_Effect(Vector3 target,float time)
    {
        GameObject roar_Effect = objectManager.MakeObj("roar_Effect");
        ParticleSystem roar_EffectLogic = roar_Effect.GetComponent<ParticleSystem>();
        roar_Effect.GetComponent<Effect>().time = time;
        roar_Effect.transform.position = target;
        roar_EffectLogic.Play();
    }
    public void Hit_Effect1(Vector3 target, float time)
    {
        GameObject hit_Effect = objectManager.MakeObj("hit_Effect1");
        ParticleSystem hit_EffectLogic = hit_Effect.GetComponent<ParticleSystem>();
        hit_Effect.GetComponent<Effect>().time = time;
        hit_Effect.transform.position = target;
        hit_EffectLogic.Play();
    }
    public void Hit_Effect2(Vector3 target, float time)
    {
        GameObject hit_Effect = objectManager.MakeObj("hit_Effect2");
        ParticleSystem hit_EffectLogic = hit_Effect.GetComponent<ParticleSystem>();
        hit_Effect.GetComponent<Effect>().time = time;
        hit_Effect.transform.position = target;
        hit_EffectLogic.Play();
    }
    public void Hit_Effect3(Vector3 target, float time)
    {
        GameObject hit_Effect = objectManager.MakeObj("hit_Effect3");
        ParticleSystem hit_EffectLogic = hit_Effect.GetComponent<ParticleSystem>();
        hit_Effect.GetComponent<Effect>().time = time;
        hit_Effect.transform.position = target;
        hit_EffectLogic.Play();
    }

    //몬스터 및 아이템박스 보스 스폰
    public void CreateBoss()
    {

        GameObject boss = objectManager.MakeObj("bossA");
        boss.transform.position = bossSpawnPoint.position;

        Boss bossLogic = boss.GetComponent<Boss>();
        bossLogic.player = player;
        bossLogic.objectManager = objectManager;
        bossLogic.enemyScript = enemyScript;
        bossLogic.Trigger = boss_Trigger;
        bossLogic.gameManager = this;

    }
    public void CreateElite()
    {
        GameObject elite = objectManager.MakeObj("elite");
        elite.transform.position = eliteSpawnPoint.position;

        EliteEnemy eliteLogic = elite.GetComponent<EliteEnemy>();
        eliteLogic.player = player;
        eliteLogic.objectManager = objectManager;
        eliteLogic.enemyScript = enemyScript;
        eliteLogic.Trigger = boss_Trigger;
        eliteLogic.gameManager = this;
        
        eliteLogic.point = eliteSkill_Spot;

    }
    void CreateMonsterA()
    {
        for (int index = 0; index < Shot_SpawnPoint.Length; index++)
        {
            GameObject mouse = objectManager.MakeObj("mouse");
            mouse.transform.position = Shot_SpawnPoint[index].position;


            MeleeEnemy mouseLogic = mouse.GetComponent<MeleeEnemy>();

            mouseLogic.point = Shot_SpawnPoint[index];
            mouseLogic.player = player;
            mouseLogic.objectManager = objectManager;
            mouseLogic.enemyScript = enemyScript;
            mouseLogic.gameManager = this;
        }
    }
    void CreateMonsterB()
    {
        for (int index = 0; index < Range_SpawnPoint.Length; index++)
        {
            GameObject snake = objectManager.MakeObj("snake");
            snake.transform.position = Range_SpawnPoint[index].position;

            RangeEnemy snakeLogic = snake.GetComponent<RangeEnemy>();
            snakeLogic.player = player;
            snakeLogic.objectManager = objectManager;
            snakeLogic.enemyScript = enemyScript;
            snakeLogic.gameManager = this;
        }
    }
    void CreateItemBox()
    {
        for (int index = 0; index < Item_SpawnPoint.Length; index++)
        {
            GameObject itemBox = objectManager.MakeObj("itemBox");
            itemBox.transform.position = Item_SpawnPoint[index].position;

            ItemBox ItemBoxLogic = itemBox.GetComponent<ItemBox>();
            SpriteRenderer sprite = itemBox.GetComponent<SpriteRenderer>();
            ItemBoxLogic.objectManager = objectManager;

            if (itemBox.transform.position.x > 0)
                sprite.flipX = true;

        }
    }
    public void EliteMonsterSkill()
    {
        eliteSkill_Spot[0] =new Vector3(player.transform.position.x+2,-7);
        eliteSkill_Spot[1] = new Vector3(player.transform.position.x, -7);
        eliteSkill_Spot[2] = new Vector3(player.transform.position.x-2, -7);
    }

    //보스전 낙하물 및 몬스터 스폰
    public void DropDebris()
    {
        int ranCount = Random.Range(3, 6);

        for (int index = 0; index < ranCount; index++)
        {
            int ranObj = Random.Range(0, 3);
            int ranPoint = Random.Range(0, Drop_SpawnPoint.Length);

            string type = null;
            switch (ranObj)
            {
                case 0:
                    type = "debrisA";
                    break;
                case 1:
                    type = "debrisB";
                    break;
                case 2:
                    type = "debrisC";
                    break;
            }

            GameObject debris = objectManager.MakeObj(type);
            Debris debrisLogic = debris.GetComponent<Debris>();
            debris.transform.position = Drop_SpawnPoint[ranPoint].position;

            debrisLogic.gameManager = this;
        }
    }
    public void SpawnMouse()
    {
        for (int index = 0; index < 2; index++)
        {
            int ranPoint = Random.Range(0, Drop_SpawnPoint.Length);
            GameObject mouse = objectManager.MakeObj("mouse");

            mouse.transform.position = Drop_SpawnPoint[ranPoint].position;
            MeleeEnemy mouseLogic = mouse.GetComponent<MeleeEnemy>();

            mouseLogic.player = player;
            mouseLogic.objectManager = objectManager;
            mouseLogic.enemyScript = enemyScript;
            mouseLogic.gameManager = this;
        }
    }

    //소환수 스폰
    public void FollowerSpawn()
    {
        Player playerLogic = player.GetComponent<Player>();
        GameObject followCat = objectManager.MakeObj("followCat");
        followCat.transform.position = playerLogic.transform.position + Vector3.up * 2;
        StartCoroutine(TeleportParticle(followCat.transform));

        FollowCat followCatLogic = followCat.GetComponent<FollowCat>();
        followCatLogic.player = player;
        followCatLogic.gameManager = this;
        followCatLogic.objectManager = objectManager;
        followCatLogic.parent = player.transform;
        followCatLogic.attack = followCat.GetComponentInChildren<BoxCollider2D>();

    }
    public IEnumerator TeleportParticle(Transform target)
    {
        GameObject teleport_Effect = objectManager.MakeObj("teleport");
        ParticleSystem teleportLogic = teleport_Effect.GetComponent<ParticleSystem>();
        teleport_Effect.transform.position = target.position;
        teleportLogic.Play();
        yield return new WaitForSeconds(2f);

        teleport_Effect.SetActive(false);
    }

    //Trigger
    public void BossRoom()
    {
        Camera.main.transform.position = bossCamera.position;
        touchingCamera = true;
        smoothCamera = false;

        for (int index = 0; index < boss_Border.Length; index++)
        {
            boss_Border[index].enabled = true;
        }

    }
    public void OnEliteRoom()
    {
        for (int index = 0; index < elite_Border.Length; index++)
        {
            elite_Border[index].enabled = true;
        }
    }
    public void OffEliteRoom()
    {
        for (int index = 0; index < elite_Border.Length; index++)
        {
            elite_Border[index].enabled = false;
        }
    }

    //몬스터 다 잡을시 클리어 조건 발생(미구현)
    public void MonsterKillCheck()
    {
        Player playerLogic = player.GetComponent<Player>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
            playerLogic.clearMap = true;
    }

}