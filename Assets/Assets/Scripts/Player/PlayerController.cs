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

    void Start()
    {
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
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, targetX, lerpSpeed * Time.deltaTime);
        transform.position = pos;
        
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
    }

    public void Jump()
    {
        Debug.Log("Jump");
        if (IsGrounded() && !isSliding)
            verticalVelocity = jumpForce;
    }

    public void Slide()
    {
        if (IsGrounded() && !isSliding)
        {
            isSliding = true;
            slideTimer = slideDuration;
            ShrinkCollider();
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