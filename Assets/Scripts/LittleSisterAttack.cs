using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleSisterAttack : MonoBehaviour
{
    public int damage;
    public float attackTime;

    private SpriteRenderer attackEffect;
    private Animator anim;
    private PolygonCollider2D collider2d;

    // Start is called before the first frame update
    void Start()
    {
        attackEffect = GameObject.FindGameObjectWithTag("AttackEffect").GetComponent<SpriteRenderer>();
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        collider2d = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void Attack()
    {
        // 攻击
        if (Input.GetButtonDown("Attack"))
        {
            collider2d.enabled = true;
            anim.SetTrigger("Attack");
            StartCoroutine(disableHitBox());
        }
    }

    IEnumerator disableHitBox()
    {
        yield return new WaitForSeconds(attackTime);
        collider2d.enabled = false;
    }

    private void ShowAttackEffect()
    {
        attackEffect.enabled = true;
        StartCoroutine(disableAttackEffect());
    }

    IEnumerator disableAttackEffect()
    {
        yield return new WaitForSeconds(attackTime);
        attackEffect.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Grass grass = other.GetComponent<Grass>();

        if (grass != null)
        {
            grass.CutOff();
        }
    }
}
