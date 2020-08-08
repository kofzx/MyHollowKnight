using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LittleSister : MonoBehaviour
{
    public Animator animator;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public PhysicsMaterial2D p1;    // 有摩擦力的
    public PhysicsMaterial2D p2;    // 无摩擦力的
    public GameObject life1;
    public GameObject life2;
    public GameObject life3;
    public GameObject life4;
    public GameObject life5;

    public float runSpeed = 55f;
    public float jumpForce = 700f;
    public float attackTime = 1.0f;
    public float timeInvincible = 0.8f;     // 无敌时间
    public int maxHealth = 5;

    Rigidbody2D rigidbody2d;

    private float xVelocity = 0.0f;
    float horizontal;           // 按键水平移动值
    bool facingRight = true;    // 角色是否面朝右边
    bool jump = false;          // 角色是否处于跳跃中
    bool isGrounded;            // 角色是否在地面上
    bool isInvincible;      // 是否为无敌状态
    float invincibleTimer;  // 无敌状态的计时器
    int currentHealth;          // 当前血量

    const float groundedRadius = .2f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.sharedMaterial = p1;

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        // 走路动画状态
        animator.SetFloat("Speed", Mathf.Abs(horizontal * runSpeed));

        // 跳跃监听
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        // 无敌时间
        if (isInvincible)
        {
            // 开始计时
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }
    }

    private void FixedUpdate()
    {
        CheckGrounded();

        // 走路
        rigidbody2d.velocity = new Vector2(horizontal * runSpeed * Time.fixedDeltaTime, rigidbody2d.velocity.y);

        // 方向矫正
        if (horizontal > 0 && !facingRight)
        {
            Flip();
        } else if (horizontal < 0 && facingRight)
        {
            Flip();
        }

        // 跳跃
        if (isGrounded && jump)
        {
            rigidbody2d.AddForce(new Vector2(0f, jumpForce));
            rigidbody2d.sharedMaterial = p2;

            jump = false;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // 检验角色是否着地
    private void CheckGrounded()
    {
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;

                OnLanded();
            }
        }
    }

    // 着地回调
    private void OnLanded()
    {
        animator.SetBool("IsJumping", false);

        rigidbody2d.sharedMaterial = p1;
    }

    // 角色受到的反作用力
    public void ReactionForce(float v)
    {
        var x = transform.position.x;
        var y = transform.position.y;
        var z = transform.position.z;

        float directionVelocity = facingRight ? -v : v;
        x = Mathf.SmoothDamp(x, directionVelocity * runSpeed * Time.deltaTime, ref xVelocity, 0.05f);
        transform.position = new Vector3(x, y, z);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            //ReactionForce(1f);

            if (isInvincible)
                return;

            // 开启无敌
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        ChangeCharacterLifeSprite(currentHealth);
    }

    private void ChangeCharacterLifeSprite(int currentHealth)
    {
        switch (currentHealth)
        {
            case 5:
                life1.GetComponent<Image>().enabled = true;
                life2.GetComponent<Image>().enabled = true;
                life3.GetComponent<Image>().enabled = true;
                life4.GetComponent<Image>().enabled = true;
                life5.GetComponent<Image>().enabled = true;
                break;
            case 4:
                life1.GetComponent<Image>().enabled = true;
                life2.GetComponent<Image>().enabled = true;
                life3.GetComponent<Image>().enabled = true;
                life4.GetComponent<Image>().enabled = true;
                life5.GetComponent<Image>().enabled = false;
                break;
            case 3:
                life1.GetComponent<Image>().enabled = true;
                life2.GetComponent<Image>().enabled = true;
                life3.GetComponent<Image>().enabled = true;
                life4.GetComponent<Image>().enabled = false;
                life5.GetComponent<Image>().enabled = false;
                break;
            case 2:
                life1.GetComponent<Image>().enabled = true;
                life2.GetComponent<Image>().enabled = true;
                life3.GetComponent<Image>().enabled = false;
                life4.GetComponent<Image>().enabled = false;
                life5.GetComponent<Image>().enabled = false;
                break;
            case 1:
                life1.GetComponent<Image>().enabled = true;
                life2.GetComponent<Image>().enabled = false;
                life3.GetComponent<Image>().enabled = false;
                life4.GetComponent<Image>().enabled = false;
                life5.GetComponent<Image>().enabled = false;
                break;
            case 0:
                life1.GetComponent<Image>().enabled = false;
                life2.GetComponent<Image>().enabled = false;
                life3.GetComponent<Image>().enabled = false;
                life4.GetComponent<Image>().enabled = false;
                life5.GetComponent<Image>().enabled = false;
                break;
        }
    }
}
