using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Entity : MonoBehaviour
{
    #region Components

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }

    #endregion
    [FormerlySerializedAs("knockbackDirection")]
    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockBackPower = new Vector2(8, 11);
    [SerializeField] protected Vector2 knockBackOffset = new Vector2(1, 2);
    [SerializeField] protected float knockbackDuration = .1f;
    protected bool isKnocked;
    
    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius = 1.3f;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 1;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = 1.5f;
    [SerializeField] protected LayerMask whatIsGround;
    
    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;
    
    public int knockBackDir { get; private set; }
    
    public System.Action onFlipped;
    
    protected virtual void Awake()
    {
        
    }
    
    protected virtual void Start()
    {
        
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        stats = GetComponentInChildren<CharacterStats>();
        cd = GetComponentInChildren<CapsuleCollider2D>();
    }
    
    protected virtual void Update()
    {
        
    }

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        
    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public virtual void DamageImpact()
    {
        StartCoroutine("HitKnocback");
        
    }

    public virtual void SetupKnockBackDirection(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)
        {
            knockBackDir = -1;
        }
        else
        {
            if (_damageDirection.position.x < transform.position.x)
            {
                knockBackDir = 1;
            }
        }
        
    }

    public void SetupKnockBackPower(Vector2 _knockBackPower)
    {
        knockBackPower = _knockBackPower;
    }

    protected virtual IEnumerator HitKnocback()
    {
        isKnocked = true;
        float xOffset = Random.Range(knockBackOffset.x, knockBackOffset.y);
        if (knockBackPower.x > 0 || knockBackPower.y > 0)
            rb.velocity = new Vector2((knockBackPower.x + xOffset) * knockBackDir, knockBackPower.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
        SetupZeroKnockbavkPower();
    }

    protected virtual void SetupZeroKnockbavkPower()
    {
        
    }

    #region Velocity

    public void SetZeroVelocity()
    {
        if (isKnocked)
            return;
        
        rb.velocity = Vector2.zero;
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
            return;
        
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    

    #endregion
    
    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    

    #endregion
    
    #region Flip
    public virtual void Filp()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
        if (onFlipped != null)
            onFlipped();
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0&& !facingRight)
        {
            Filp();
        }
        else if (_x < 0 && facingRight)
        {
            Filp();
        }
    }

    public virtual void SetupDefaultFacingDir(int _direction)
    {
        facingDir = _direction;

        if (facingDir == -1)
        {
            facingRight = false;
        }
    }

    #endregion



    public virtual void Die()
    {
        
    }
}
