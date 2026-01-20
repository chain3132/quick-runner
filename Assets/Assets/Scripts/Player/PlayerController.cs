using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float lerpSpeed = 10f;
    public float targetX;

    
    public float jumpForce = 7f;
    public float gravity = -20f;
    public float slideDuration = 0.5f;
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
    }
    private void Update()
    {
        Vector3 pos = rb.position;;
        pos.x = Mathf.Lerp(pos.x, targetX, lerpSpeed * Time.deltaTime);
        
        verticalVelocity += gravity * Time.deltaTime;
        pos.y += verticalVelocity * Time.deltaTime;
    
        // Ground
        if (pos.y <= trackHeight)
        {
            pos.y = trackHeight;
            verticalVelocity = 0f;
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
            verticalVelocity += gravity * 1.5f * Time.deltaTime;

            if (IsGrounded())
            {
                longJumping = false;
                CharacterManager.Instance.OnLongJumpLanded();
            }
        }

        rb.MovePosition(pos);
    }
    public void ExecuteLongJump()
    {
        verticalVelocity = jumpForce * 1.8f; 
        longJumping = true;
    }
    public void Jump()
    {
        Debug.Log("Jump");
        if (IsGrounded() && !isSliding)
        {
            verticalVelocity = jumpForce;
            animator.SetTrigger("Jump");
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
        }
    }

    bool IsGrounded()
    {
        return transform.position.y <= trackHeight + 0.01f;
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
    

    
}