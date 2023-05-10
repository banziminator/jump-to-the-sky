using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // скорость игрока

    public Transform groundCheck; // точка, откуда выпускается луч для проверки земли
    public float groundCheckRadius = 0.1f; // радиус луча проверки земли
    public LayerMask groundLayer; // слой, представляющий землю

    private Rigidbody2D rb;
    private Animator animator;
    private JumpForce currentJumpForce = JumpForce.Low; // текущая сила прыжка
    private float jumpPressTime = 0.0f; // время начала нажатия на кнопку Space
    private const float lowJumpTime = 0.2f; // время, которое должно пройти, чтобы прыжок считался низким
    private const float mediumJumpTime = 0.4f; // время, которое должно пройти, чтобы прыжок считался средним

    public bool isGrounded; // флаг, указывающий, находится ли игрок на земле

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // получаем Rigidbody2D объекта
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Проверяем, находится ли игрок на земле
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        // движение влево и вправо
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
        
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.velocity = Vector2.zero;
        }

        // прыжок
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpPressTime = Time.time; // сохраняем время начала нажатия кнопки Space
        }

        if (Input.GetKeyUp(KeyCode.Space) && isGrounded)
        {
            float jumpTime = Time.time - jumpPressTime; // определяем, как долго была зажата кнопка Space
            if (jumpTime < lowJumpTime)
            {
                currentJumpForce = JumpForce.Low; // прыжок считается низким
            }
            else if (jumpTime < mediumJumpTime)
            {
                currentJumpForce = JumpForce.Medium; // прыжок считается средним
            }
            else
            {
                currentJumpForce = JumpForce.High; // прыжок считается дальним
            }

            Jump();
        }

        if (rb.velocity.magnitude > 0)
        {
            GetComponent<PlayerAnimation>().SetRunning(true);
        }
        else
        {
            GetComponent<PlayerAnimation>().SetRunning(false);
        }
    }

    private void Jump()
    {
        float jumpForce = 0.0f;

        switch (currentJumpForce)
        {
            case JumpForce.Low:
                jumpForce = 3f; // сила прыжка при низком прыжке
                break;
            case JumpForce.Medium:
                jumpForce = 5f; // сила прыжка при среднем прыжке
                break;
            case JumpForce.High:
                jumpForce = 7f; // сила прыжка при дальнем прыжке
                break;
        }

        rb.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
    }

public enum JumpForce
{
    Low,
    Medium,
    High
}
}