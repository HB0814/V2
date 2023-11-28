using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum Level { Easy, Normal, Hard }
    public Level level;

    public enum Type { None,Mouse ,Snake,Slime, Flower };
    public Type enemyType;

    public enum Def_Type { Normal, Resist, Nimble, Solid }
    public Def_Type type;

    public float maxdistance;
    public float mindistance;
    protected float curAttackDelay;
    protected float maxAttackDelay;
    protected float curAggroTime;
    protected float maxAggroTime;
    public  float dmg;
    protected float nextMove;
    protected float speed;

    protected float dieTime=1f;
    protected float invincibleTime=0.1f;

    public float health;
    public float maxHealth;

    public GameObject player;
    public ObjectManager objectManager;
    public GameManager gameManager;

    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer;
    public Animator anim;

    protected bool isAttack;
    protected bool isHit;
    protected bool isFollow;
    protected bool isAggro;
    protected bool isDie;

    public Vector2 forward;
    protected float dist;


    public void FlipX(SpriteRenderer sprite)
    {
        if (sprite.flipX == false)
            sprite.flipX = true;
        else if (sprite.flipX == true)
            sprite.flipX = false;
    }
    public void Follow(Vector3 Pos,float distance,SpriteRenderer sprite)
    {
        if (player.transform.position.x - Pos.x < -distance)
        {
            forward = new Vector2(-speed, rigid.velocity.y).normalized;
            nextMove = -1;
            Watch(Pos, player.transform.position, sprite);
            isFollow = true;
        }
        else if (player.transform.position.x - Pos.x > distance)
        {
            forward = new Vector2(speed, rigid.velocity.y).normalized;
            nextMove = 1;
            Watch(Pos, player.transform.position, sprite);
            isFollow = true;
        }
    }
    public void Watch(Vector3 Pos, Vector3 target, SpriteRenderer sprite)
    {
        if (target.x - Pos.x > 0)
            sprite.flipX = true;
        if (target.x - Pos.x < 0)
            sprite.flipX = false;
    }
    public void WatchCheck(bool fol, SpriteRenderer sprite, Vector2 watch)
    {
        if (isFollow)
            return;
        if (spriteRenderer.flipX == false)
        { forward = new Vector2(-speed, rigid.velocity.y).normalized; }
        else if (spriteRenderer.flipX == true)
        { forward = new Vector2(speed, rigid.velocity.y).normalized; }
    }
    public void DamageLogic(float dmg)
    {
        Player playerLogic = player.GetComponent<Player>();
        switch (type)
        {
            case Def_Type.Normal:
                switch (playerLogic.att_Type)
                {
                    case Player.Att_Type.Normal:
                        health -= dmg;
                        break;
                    case Player.Att_Type.Power:
                        health = health - (dmg * 1.5f);
                        break;
                    case Player.Att_Type.Sharp:
                        health = health - (dmg * 1.5f);
                        break;
                    case Player.Att_Type.Mystic:
                        health = health - (dmg * 1.5f);
                        break;
                }
                break;
            case Def_Type.Nimble:
                switch (playerLogic.att_Type)
                {
                    case Player.Att_Type.Normal:
                        health = health - (dmg * 0.5f);
                        break;
                    case Player.Att_Type.Power:
                        health = health - (dmg * 0.5f);
                        break;
                    case Player.Att_Type.Sharp:
                        health = health - (dmg * 1.3f);
                        break;
                    case Player.Att_Type.Mystic:
                        health = health - (dmg * 0.8f);
                        break;
                }
                break;
            case Def_Type.Resist:
                switch (playerLogic.att_Type)
                {
                    case Player.Att_Type.Normal:
                        health = health - (dmg * 0.5f);
                        break;
                    case Player.Att_Type.Power:
                        health = health - (dmg * 1.5f);
                        break;
                    case Player.Att_Type.Sharp:
                        health = health - (dmg * 0.5f);
                        break;
                    case Player.Att_Type.Mystic:
                        health = health - (dmg * 0.5f);
                        break;
                }
                break;
            case Def_Type.Solid:
                switch (playerLogic.att_Type)
                {
                    case Player.Att_Type.Normal:
                        health = health - (dmg * 0.5f);
                        break;
                    case Player.Att_Type.Power:
                        health = health - (dmg * 0.8f);
                        break;
                    case Player.Att_Type.Sharp:
                        health = health - (dmg * 0.8f);
                        break;
                    case Player.Att_Type.Mystic:
                        health = health - (dmg * 1.5f);
                        break;
                }
                break;
        }
    }
}
