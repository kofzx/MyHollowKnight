using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleSisterAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage;
    public LittleSister littleSister;
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

        if (other.gameObject.tag == "Enemy")
        {
            ShowAttackEffect();
            littleSister.ReactionForce(1f);

            // 两点之间的向量的分量与目标点相加，即可得到击退效果
            Vector2 difference = other.transform.position - transform.position;
            other.transform.position = new Vector2(
                other.transform.position.x + difference.x,    
                other.transform.position.y + difference.y    
            );

            if (other.gameObject.GetComponent<Bee>() != null)
            {
                other.gameObject.GetComponent<Bee>().TakeDamage(attackDamage);
            }
            if (other.gameObject.GetComponent<NormalMonster>() != null)
            {
                other.gameObject.GetComponent<NormalMonster>().TakeDamage(attackDamage);
            }
        }
    }
}
