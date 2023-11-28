using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [Header("파티클")]
    public GameObject ground_EffectPrb;
    public GameObject spawn_EffectPrb;
    public GameObject roar_EffectPrb;
    public GameObject hit_Effect1Prb;
    public GameObject hit_Effect2Prb;
    public GameObject hit_Effect3Prb;

    [Header("팔로우캣&텔레포트 파티클")]
    public GameObject followCatPrb;
    public GameObject teleportPrb;

    [Header("원거리 공격")]
    public GameObject snakeAttackPrb;
    public GameObject flowerAttackPrb;

    public GameObject bossAAttack_1Prb; //원거리 공격

    [Header("낙하물")]
    public GameObject debrisAPrb;
    public GameObject debrisBPrb;
    public GameObject debrisCPrb;

    [Header("몬스터")]
    public GameObject bossAPrb;
    public GameObject elitePrb;

    public GameObject mousePrb;
    public GameObject snakePrb;

    [Header("아이템")]
    public GameObject goldPrb;
    public GameObject itemBoxPrb;


    [Header("코어")]
    public GameObject rollCorePrb;
    public GameObject summonCorePrb;
    public GameObject dropCorePrb;

    public GameObject stateCorePrb;

    GameObject[] ground_Effect;
    GameObject[] spawn_Effect;
    GameObject[] roar_Effect;
    GameObject[] hit_Effect1;
    GameObject[] hit_Effect2;
    GameObject[] hit_Effect3;

    GameObject[] followCat;
    GameObject[] teleport;

    //Attack Object
    GameObject[] snakeAttack;
    GameObject[] flowerAttack;

    GameObject[] bossAAttack_1;

    GameObject[] debrisA;
    GameObject[] debrisB;
    GameObject[] debrisC;


    //Enemy
    GameObject[] bossA;
    GameObject[] elite;

    GameObject[] mouse;
    GameObject[] snake;

    //Item
    GameObject[] gold;

    GameObject[] rollCore;
    GameObject[] summonCore;
    GameObject[] dropCore;

    GameObject[] stateCore;

    GameObject[] itemBox;

    GameObject[] targetPool;

    void Awake()
    {
        ground_Effect = new GameObject[15];
        spawn_Effect = new GameObject[5];
        roar_Effect = new GameObject[5];

        hit_Effect1 = new GameObject[40];
        hit_Effect2 = new GameObject[40];
        hit_Effect3 = new GameObject[40];

        followCat = new GameObject[10];
        teleport = new GameObject[10];

        snakeAttack = new GameObject[15];
        flowerAttack = new GameObject[30];

        bossAAttack_1 = new GameObject[60];

        debrisA = new GameObject[20];
        debrisB= new GameObject[20];
        debrisC = new GameObject[20];

        bossA = new GameObject[2];
        elite = new GameObject[2];

        mouse = new GameObject[10];
        snake = new GameObject[10];

        gold = new GameObject[60];

        rollCore = new GameObject[2];
        summonCore = new GameObject[2];
        dropCore = new GameObject[2];

        stateCore = new GameObject[5];

        itemBox = new GameObject[3];

        Generate();
    }
    void Generate()
    {
        for (int index = 0; index < ground_Effect.Length; index++)
        {
            ground_Effect[index] = Instantiate(ground_EffectPrb);
            ground_Effect[index].SetActive(false);
        }
        for (int index = 0; index < spawn_Effect.Length; index++)
        {
            spawn_Effect[index] = Instantiate(spawn_EffectPrb);
            spawn_Effect[index].SetActive(false);
        }
        for (int index = 0; index < roar_Effect.Length; index++)
        {
            roar_Effect[index] = Instantiate(roar_EffectPrb);
            roar_Effect[index].SetActive(false);
        }  
        for (int index = 0; index < hit_Effect1.Length; index++)
        {
            hit_Effect1[index] = Instantiate(hit_Effect1Prb);
            hit_Effect1[index].SetActive(false);
        } 
        for (int index = 0; index < hit_Effect2.Length; index++)
        {
            hit_Effect2[index] = Instantiate(hit_Effect2Prb);
            hit_Effect2[index].SetActive(false);
        } 
        for (int index = 0; index < hit_Effect3.Length; index++)
        {
            hit_Effect3[index] = Instantiate(hit_Effect3Prb);
            hit_Effect3[index].SetActive(false);
        } 

        for (int index = 0; index < followCat.Length; index++)
        {
            followCat[index] = Instantiate(followCatPrb);
            followCat[index].SetActive(false);
        } 
        for (int index = 0; index < teleport.Length; index++)
        {
            teleport[index] = Instantiate(teleportPrb);
            teleport[index].SetActive(false);
        }

        for (int index=0;index<snakeAttack.Length;index++)
        {
            snakeAttack[index] = Instantiate(snakeAttackPrb);
            snakeAttack[index].SetActive(false);
        }
        for (int index = 0; index < flowerAttack.Length; index++)
        {
            flowerAttack[index] = Instantiate(flowerAttackPrb);
            flowerAttack[index].SetActive(false);
        }

        for (int index = 0; index < bossAAttack_1.Length; index++)
        {
            bossAAttack_1[index] = Instantiate(bossAAttack_1Prb);
            bossAAttack_1[index].SetActive(false);
        }

        for (int index = 0; index < debrisA.Length; index++)
        {
            debrisA[index] = Instantiate(debrisAPrb);
            debrisA[index].SetActive(false);
        }
        for (int index = 0; index < debrisB.Length; index++)
        {
            debrisB[index] = Instantiate(debrisBPrb);
            debrisB[index].SetActive(false);
        }
        for (int index = 0; index < debrisC.Length; index++)
        {
            debrisC[index] = Instantiate(debrisCPrb);
            debrisC[index].SetActive(false);
        }

        for (int index = 0; index < bossA.Length; index++)
        {
            bossA[index] = Instantiate(bossAPrb);
            bossA[index].SetActive(false);
        } 
        for (int index = 0; index < elite.Length; index++)
        {
            elite[index] = Instantiate(elitePrb);
            elite[index].SetActive(false);
        }

        for (int index = 0; index < mouse.Length; index++)
        {
            mouse[index] = Instantiate(mousePrb);
            mouse[index].SetActive(false);
        }
        for (int index = 0; index < snake.Length; index++)
        {
            snake[index] = Instantiate(snakePrb);
            snake[index].SetActive(false);
        }

        for (int index = 0; index < gold.Length; index++)
        {
            gold[index] = Instantiate(goldPrb);
            gold[index].SetActive(false);
        }

        for (int index = 0; index < rollCore.Length; index++)
        {
            rollCore[index] = Instantiate(rollCorePrb);
            rollCore[index].SetActive(false);
        }
        for (int index = 0; index < summonCore.Length; index++)
        {
            summonCore[index] = Instantiate(summonCorePrb);
            summonCore[index].SetActive(false);
        }
        for (int index = 0; index < dropCore.Length; index++)
        {
            dropCore[index] = Instantiate(dropCorePrb);
            dropCore[index].SetActive(false);
        }

        for (int index = 0; index < stateCore.Length; index++)
        {
            stateCore[index] = Instantiate(stateCorePrb);
            stateCore[index].SetActive(false);
        }
        for (int index = 0; index < itemBox.Length; index++)
        {
            itemBox[index] = Instantiate(itemBoxPrb);
            itemBox[index].SetActive(false);
        }
    }
    public GameObject MakeObj(string type)
    {
        switch(type)
        {
            case "ground_Effect":
                targetPool = ground_Effect;
                break;

            case "spawn_Effect":
                targetPool = spawn_Effect;
                break;
            case "roar_Effect":
                targetPool = roar_Effect;
                break;  
            case "hit_Effect1":
                targetPool = hit_Effect1;
                break;    
            case "hit_Effect2":
                targetPool = hit_Effect2;
                break;   
            case "hit_Effect3":
                targetPool = hit_Effect3;
                break;  

            case "followCat":
                targetPool = followCat;
                break; 
            case "teleport":
                targetPool = teleport;
                break;

            case "snakeAttack":
                targetPool = snakeAttack;
                break;
            case "flowerAttack":
                targetPool = flowerAttack;
                break;

            case "bossAAttack_1":
                targetPool = bossAAttack_1;
                break;

            case "debrisA":
                targetPool = debrisA;
                break;
            case "debrisB":
                targetPool = debrisB;
                break;
            case "debrisC":
                targetPool = debrisC;
                break;

            case "bossA":
                targetPool = bossA;
                break;
            case "elite":
                targetPool = elite;
                break;

            case "mouse":
                targetPool = mouse;
                break;
            case "snake":
                targetPool = snake;
                break;

            case "gold":
                targetPool = gold;
                break;

            case "rollCore":
                targetPool = rollCore;
                break;
            case "summonCore":
                targetPool = summonCore;
                break;
            case "dropCore":
                targetPool = dropCore;
                break;

            case "stateCore":
                targetPool = stateCore;
                break;

            case "itemBox":
                targetPool = itemBox;
                break;
        }
        for(int index=0;index<targetPool.Length;index++)
        {
            if(!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                return targetPool[index];
            }
        }
        return null;
    }
}
