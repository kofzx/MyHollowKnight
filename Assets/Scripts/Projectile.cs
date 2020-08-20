using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float attackDamage;

    Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // 两点之间的向量的分量与目标点相加，即可得到击退效果
            Vector2 difference = other.transform.position - transform.position;
            other.transform.position = new Vector2(
                other.transform.position.x + difference.x,
                other.transform.position.y + difference.y
            );

            other.gameObject.GetComponent<Bee>().TakeDamage(attackDamage);
        }

        Destroy(gameObject);
    }

    // 发射剑气
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }
}
