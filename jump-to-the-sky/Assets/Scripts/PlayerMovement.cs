using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // скорость игрока
    public float dashForce = 10f; // сила рывка

    public Transform groundCheck; // точка, откуда выпускается луч для проверки земли
    public float groundCheckRadius = 0.1f; // радиус луча проверки земли
    public LayerMask groundLayer; // слой, представляющий землю

    private Rigidbody2D rb;
    private Animator animator;
    private JumpForce currentJumpForce = JumpForce.Low; // текущая сила прыжка
    private float jumpPressTime = 0.0f; // время начала нажатия на кнопку Space
    private const float lowJumpTime = 0.2f; // время, которое должно пройти, чтобы прыжок считался низким

    public bool isGrounded; // флаг, указывающий, находится ли игрок на земле
    public bool isDashing; // флаг, указывающий, выполняется ли рывок

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
            else
            {
                currentJumpForce = JumpForce.High; // прыжок считается высоким
            }

            Jump();
        }

        // рывок (dash)
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            Dash();
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
            case JumpForce.High:
                jumpForce = 7f; // сила прыжка при высоком прыжке
                break;
        }

        rb.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
    }

    private void Dash()
    {
        // Apply the dash force in the direction of the player's current movement
        float dashDirection = Mathf.Sign(rb.velocity.x);
        rb.AddForce(new Vector2(dashDirection * dashForce, 0.0f), ForceMode2D.Impulse);

        // Start a coroutine to temporarily disable the dash ability
        StartCoroutine(DisableDash());
    }

    private IEnumerator DisableDash()
    {
        // Disable the ability to dash
        isDashing = true;

        // Wait for a certain duration to complete the dash
        yield return new WaitForSeconds(0.5f);

        // Re-enable the ability to dash
        isDashing = false;
    }

    public enum JumpForce
    {
        Low,
        High
    }
}