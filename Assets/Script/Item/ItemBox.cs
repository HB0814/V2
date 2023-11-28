using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public int ranHitCount;

    bool isHit;
    public int HitCount;
    public ObjectManager objectManager;
    SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    void OnEnable()
    {
        HitCount = 0;
        ranHitCount = Random.Range(1, 4);
    }

    IEnumerator OnHit()
    {
        if (isHit)
            yield break;

        isHit = true;
        sprite.color = new Color(1, 1, 1, 0.4f);
        //Item
        int ranItem= Random.Range(0,3);
        int ranCore = Random.Range(0, 3); 
        if (HitCount == ranHitCount)
        {
            switch (ranCore)
            {
                case 0:
                    GameObject damage_Core = objectManager.MakeObj("damage_Core");
                    damage_Core.transform.position = transform.position;

                    Rigidbody2D D_rigid = damage_Core.GetComponent<Rigidbody2D>();
                    D_rigid.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(1.2f);

                    gameObject.SetActive(false);
                    break;
                case 1:
                    GameObject speed_Core = objectManager.MakeObj("speed_Core");
                    speed_Core.transform.position = transform.position;

                    Rigidbody2D S_rigid = speed_Core.GetComponent<Rigidbody2D>();
                    S_rigid.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(1.2f);

                    gameObject.SetActive(false);
                    break;
                case 2:
                    GameObject health_Core = objectManager.MakeObj("health_Core");
                    health_Core.transform.position = transform.position;

                    Rigidbody2D H_rigid = health_Core.GetComponent<Rigidbody2D>();
                    H_rigid.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(1.2f);

                    gameObject.SetActive(false);
                    break;
            }
            yield break;
        }
        if (HitCount < ranHitCount)
        {
            for (int index = 0; index < 5; index++)
            {
                Vector2 ranVec = new Vector2(Random.Range(-1f, 1f), 0);
                switch (ranItem)
                {
                    case 0:
                        GameObject b_Coin = objectManager.MakeObj("bronze");
                        b_Coin.transform.position = transform.position;

                        Rigidbody2D b_Rigid = b_Coin.GetComponent<Rigidbody2D>();
                        b_Rigid.AddForce(ranVec * 2 + Vector2.up * 4, ForceMode2D.Impulse);

                        break;
                    case 1:
                        GameObject s_Coin = objectManager.MakeObj("silver");
                        s_Coin.transform.position = transform.position;

                        Rigidbody2D s_Rigid = s_Coin.GetComponent<Rigidbody2D>();
                        s_Rigid.AddForce(ranVec * 2 + Vector2.up * 4, ForceMode2D.Impulse);

                        break;
                    case 2:
                        GameObject g_Coin = objectManager.MakeObj("gold");
                        g_Coin.transform.position = transform.position;

                        Rigidbody2D g_Rigid = g_Coin.GetComponent<Rigidbody2D>();
                        g_Rigid.AddForce(ranVec * 2 + Vector2.up * 4, ForceMode2D.Impulse);

                        break;
                }
            }
            HitCount++;
            yield return new WaitForSeconds(0.5f);
            sprite.color = new Color(1, 1, 0, 1);
            isHit = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어의 무기에 공격당했을 때 
        if (collision.gameObject.tag == "PlayerAttack")
        {
            StartCoroutine(OnHit());
        }
    }
}
