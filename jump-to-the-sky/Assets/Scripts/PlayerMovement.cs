using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // скорость игрока

    private Rigidbody2D rb;
    private Animator animator;
    private JumpForce currentJumpForce = JumpForce.Low; // текущая сила прыжка
    private float jumpPressTime = 0.0f; // время начала нажатия на кнопку Space
    private const float lowJumpTime = 0.2f; // время, которое должно пройти, чтобы прыжок считался низким
    private const float mediumJumpTime = 0.4f; // время, которое должно пройти, чтобы прыжок считался средним

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // получаем Rigidbody2D объекта
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // движение влево и вправо
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);

        // прыжок
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressTime = Time.time; // сохраняем время начала нажатия кнопки Space
        }

        if (Input.GetKeyUp(KeyCode.Space))
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
                jumpForce = 6f; // сила прыжка при среднем прыжке
                break;
            case JumpForce.High:
                jumpForce = 9f; // сила прыжка при дальнем прыжке
                break;
        }

        rb.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
    }
}

public enum JumpForce
{
    Low,
    Medium,
    High
}