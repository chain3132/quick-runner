using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float lerpSpeed = 10f;
    public float targetX;
    public bool isPlayerDie = false;
    public AfterImageSpawner afterImage;

    public float jumpForce = 7f;
    public float gravity = -20f;
    public float slideDuration = 0.4f;
    public float trackHeight = 0f;
    
    private float verticalVelocity;
    private bool isSliding;
    private float slideTimer;
    
    private CapsuleCollider col;
    private float originalHeight;
    private Vector3 originalCenter;
    private Animator animator;
    bool longJumping;
    float airTime;
    private Rigidbody rb;
    private bool forceCentering;
    [Header("Fast Fall")]
    public float fastFallSpeed = -35f;
    private bool isFastFalling;
    public bool failLongJump = false;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        animator = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        originalHeight = col.height;
        originalCenter = col.center;
        
        col.center = new Vector3(0, originalHeight * 0.5f, 0);
        var pos = transform.position;
        pos.y = trackHeight;
        transform.position = pos;
        rb.constraints = RigidbodyConstraints.FreezeRotation
                         | RigidbodyConstraints.FreezePositionY;
        rb.useGravity = false;
    }
    private void Update()
    {
        
        if (GameManager.Instance.isGameOver)
        {
            
            return;
        }
        float dx = Mathf.Abs(transform.position.x - targetX);

        if (dx < 0.5f)
        {
            afterImage.StopAfterImage();
        }
        if (failLongJump)
        {
            verticalVelocity = fastFallSpeed ;
            
        }
        Vector3 pos = rb.position;;
        if (forceCentering)
        {
            pos.x = Mathf.MoveTowards(
                pos.x,
                targetX,
                lerpSpeed * 10 * Time.deltaTime
            );

            if (Mathf.Abs(pos.x - targetX) < 0.01f)
            {
                pos.x = targetX;
                forceCentering = false;
            }
        }
        else
        {
            pos.x = Mathf.Lerp(pos.x, targetX, lerpSpeed * Time.deltaTime);
        }        
        verticalVelocity += gravity * Time.deltaTime;
        pos.y += verticalVelocity * Time.deltaTime;
    
        // Ground
        if (pos.y <= trackHeight && !isPlayerDie && !failLongJump )
        {
            pos.y = trackHeight;
            verticalVelocity = 0f;
            isFastFalling = false;
            if (CharacterManager.Instance.state == SyncJumpState.None)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation
                                 | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            }
            
        }
        
        // Slide timer
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
            {
                isSliding = false;
                RestoreCollider();
            }
        }
    
        transform.position = pos;
        if (longJumping)
        {
            verticalVelocity += Mathf.Abs( 2f * Time.deltaTime);
            
        }
        if (IsGrounded() )
        {
            Vector3 v = rb.linearVelocity;
            v.y = 0f;
            rb.linearVelocity = v;
        }
        rb.MovePosition(pos);
    }
   
    public void MoveToX(float x)
    {
        targetX = x;
        forceCentering = true;
    }
    public void FastFall()
    {
        if (!IsGrounded())
        {
            verticalVelocity = fastFallSpeed;
            isFastFalling = true;
        }
    }
    public void ExecuteLongJump()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        verticalVelocity = jumpForce * 1.9f; 

        longJumping = true;
        
    }

    
    public void EndLongJump()
    {
        longJumping = false;
    }
    public void Jump()
    {
        if (IsGrounded() && !isSliding)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation| RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;;

            verticalVelocity = jumpForce;
            animator.SetTrigger("Jump");
            audioManager.PlaySFX(audioManager.jumpSFX);
        }
    }
    public void PlayPrayAnimation()
    {
        if (!this.isPlayerDie)
        {
            animator.SetTrigger("Pray");
        }
    }
    public void Slide()
    {
        if (IsGrounded() && !isSliding)
        {
            isSliding = true;
            slideTimer = slideDuration;
            ShrinkCollider();
            animator.SetTrigger("Slide");
            audioManager.PlaySFX(audioManager.slideSFX);
        }
    }

    public bool IsGrounded()
    {
        return Mathf.Abs(transform.position.y - trackHeight) < 0.02f;
        
    }

    void ShrinkCollider()
    {
        col.height = originalHeight * 0.5f;
        col.center = originalCenter * 0.5f;
        Debug.Log("slide");
    }

    void RestoreCollider()
    {
        col.height = originalHeight;
        col.center = originalCenter;
    }

    public void Dead()
    {
        animator.SetTrigger("Dead");
    }
    public void ResetAfterLongJump()
    {
        verticalVelocity = 0f;
        longJumping = false;
        isFastFalling = false;

        Vector3 pos = transform.position;
        pos.y = trackHeight;
        transform.position = pos;

        rb.linearVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation
                         | RigidbodyConstraints.FreezePositionY
                         | RigidbodyConstraints.FreezePositionZ;
    }

    
}