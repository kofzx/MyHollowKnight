using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Transform target;
    [SerializeField] private float maxHp;
    public float hp;

    [Header("Hurt")]
    private SpriteRenderer sr;
    public float hurtLength;    // 效果持续多久
    private float hurtCounter;  // 计数器


    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();

        if (hurtCounter <= 0)
        {
            sr.material.SetFloat("_FlashAmount", 0);
        } else
        {
            hurtCounter -= Time.deltaTime;
        }
    }

    private void FollowPlayer()
    {
        // 怪物跟随角色
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(float _amount)
    {
        hp -= _amount;
        HurtShader();
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    // 受伤的改变底色效果
    private void HurtShader()
    {
        // 利用Shader去改变底色
        sr.material.SetFloat("_FlashAmount", 1);
        hurtCounter = hurtLength;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        LittleSister littleSister = other.gameObject.GetComponent<LittleSister>();

        if (littleSister != null)
        {
            littleSister.ChangeHealth(-1);
        }
    }
}
