using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleSister : MonoBehaviour
{
    public Animator animator;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public PhysicsMaterial2D p1;    // 有摩擦力的
    public PhysicsMaterial2D p2;    // 无摩擦力的

    public float runSpeed = 55f;
    public float jumpForce = 700f;
    public float attackTime = 1.0f;

    Rigidbody2D rigidbody2d;

    float horizontal;           // 按键水平移动值
    bool facingRight = true;    // 角色是否面朝右边
    bool jump = false;          // 角色是否处于跳跃中
    bool isGrounded;            // 角色是否在地面上

    const float groundedRadius = .2f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.sharedMaterial = p1;
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
    }

    private void FixedUpdate()
    {
        checkGrounded();

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
    private void checkGrounded()
    {
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;

                onLanded();
            }
        }
    }

    // 着地以后的事
    private void onLanded()
    {
        animator.SetBool("IsJumping", false);

        rigidbody2d.sharedMaterial = p1;
    }
}
