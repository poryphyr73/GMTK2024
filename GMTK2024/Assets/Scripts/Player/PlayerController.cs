using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float maxHorizontalVelocity, timeToMaxSpeed, decelerationSpeed, maxJumpTime, jumpSpeed, fallSpeed, maxCoyoteTime, wallJumpForce, wallJumpTime, deathFreezeTime, deathFallTime = 1, deathArc;
    float moveTime, moveDirection, globalVelocityX;
    bool coyoteTimeExpired, jumped, died;
    Rigidbody2D playerBody;
    LayerMask floor = 1 << 6;
    SpriteRenderer spriteRenderer;
    Animator anim;

    private void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (!died)
        {
            HandleMovement();
            HandleJump();
            HandleTexture();
        }
    }

    private void FixedUpdate()
    {
        playerBody.velocity = new Vector2(globalVelocityX, playerBody.velocity.y);
    }

    private void HandleTexture()
    {
        float velocityX = (float) System.Math.Round(playerBody.velocity.x, 2);
        float velocityY = (float) System.Math.Round(playerBody.velocity.y, 2);
        anim.SetFloat("yVelocity", velocityY);
        anim.SetFloat("xSpeed", Mathf.Abs(velocityX));
        if (velocityX != 0) spriteRenderer.flipX = velocityX > 0;

        float z = Mathf.Lerp(0, 10, 
                    Mathf.Clamp(Mathf.Abs(velocityY), 0, 5));

        Quaternion r = Quaternion.Euler(0f, 0f, z * Mathf.Sign(velocityY) * (spriteRenderer.flipX ? 1 : -1));
            anim.transform.rotation = r;
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput != 0)
        {
            moveTime = Mathf.Min(timeToMaxSpeed, moveTime + Time.deltaTime);
            moveDirection = horizontalInput;
        }
        else moveTime = Mathf.Max(0, moveTime - Time.deltaTime * decelerationSpeed);
        float accelerationProgress = moveTime / timeToMaxSpeed;

        float velocityX = Mathf.Lerp(0, maxHorizontalVelocity * moveDirection, accelerationProgress);

        globalVelocityX = velocityX;
    }

    private void HandleJump()
    {
        if (!(HasFloorCollision() || coyoteTimeExpired)) StartCoroutine(CheckCoyoteTime());

        if (Input.GetButtonDown("Jump"))
        {
            if (CanJump()) StartCoroutine(Jump(0));
            else
            {
                float f = HasWallCollision();
                if (f == 0) return;

                StartCoroutine(Jump(f));
            }

            jumped = true;
            coyoteTimeExpired = true;
        }
    }

    IEnumerator Jump(float horizontalInfluence)
    {
        playerBody.gravityScale = 0;

        if (horizontalInfluence != 0) StartCoroutine(WallKick(horizontalInfluence));

        float t = 0f;
        while(t < maxJumpTime && Input.GetButton("Jump"))
        {
            t += Time.deltaTime;
            playerBody.velocity = new Vector2(playerBody.velocity.x, Mathf.Lerp(jumpSpeed, 0, t / maxJumpTime));
            yield return null;
        }

        playerBody.gravityScale = fallSpeed;
    }

    IEnumerator WallKick(float horizontalInfluence)
    {
        
        float t = wallJumpTime;
        while(t > 0)
        {
            globalVelocityX += Mathf.Lerp(0, wallJumpForce, t / wallJumpTime) * horizontalInfluence;
            t -= Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator CheckCoyoteTime()
    {
        float t = 0f;
        while (t < maxCoyoteTime)
        {
            if (jumped) break;
            t += Time.deltaTime;
            yield return null;
        }

        coyoteTimeExpired = true;
    }

    IEnumerator EndGameCoroutine(Vector3 newPos)
    {
        died = true;
        FindObjectOfType<AudioManager>().Play("Death");
        Camera.main.GetComponent<CameraController>().enabled = false;
        FindObjectOfType<StartScreen>(true).gameObject.SetActive(false);
        playerBody.gravityScale = 0;
        globalVelocityX = 0;
        playerBody.velocity = Vector3.zero;
        transform.position = newPos;
        HandleTexture();
        
        //Play Sound
        GetComponent<Collider2D>().enabled = false;
        
        yield return new WaitForSeconds(deathFreezeTime);

        transform.Rotate(0f, 0f, 180f);

        float t = 0;
        playerBody.gravityScale = -deathArc;

        while(t < deathFallTime)
        {
            playerBody.gravityScale = Mathf.Lerp(-deathArc, deathArc, t / deathFallTime);
            t += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(FindObjectOfType<GameOverScreen>().FadeInText());
    }

    public void EndGame()
    {
        EndGame(transform.position);
    }

    public void EndGame(Vector3 newPos)
    {
        StopAllCoroutines();
        StartCoroutine(EndGameCoroutine(newPos));
    }

    bool CanJump()
    {
        return !jumped && (HasFloorCollision() || !coyoteTimeExpired);
    }

    bool HasFloorCollision()
    {
        Vector3 offset = new Vector3(transform.localScale.x / 2, 0);
        float distance = transform.localScale.y / 2 + 0.1f;

        RaycastHit2D rightHit = Physics2D.Raycast(transform.position + offset, Vector2.down, distance, floor);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position - offset, Vector2.down, distance, floor);

        var a = rightHit.collider != null;
        var b = leftHit.collider != null;

        return a || b;
    }

    float HasWallCollision()
    {
        float distance = transform.localScale.x / 2 + 0.1f;

        RaycastHit2D rightSideHit = Physics2D.Raycast(transform.position, Vector2.right, distance, floor);
        RaycastHit2D leftSideHit = Physics2D.Raycast(transform.position, Vector2.left, distance, floor);

        var a = rightSideHit.collider != null;
        var b = leftSideHit.collider != null;

        if (a == b) return 0;
        return a ? -1 : 1;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (HasFloorCollision())
        {
            coyoteTimeExpired = false;
            jumped = false;
        }
    }
}


