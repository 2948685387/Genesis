using System;
using UnityEngine;

public enum PlayerSound
{
    Move, Jump, Dodge, Melee, Ranged,
}

public class PlayerController : MonoBehaviour
{
    public GameObject UI_Player;
    public PlayerSound Sound;

    [Header("攻击补偿速度")]
    [SerializeField] private float meleeSpeed;//轻攻击补偿速度

    [SerializeField] private float rangedSpeed;//重攻击补偿速度

    [Header("攻击设置")]
    [Tooltip("当前的攻击类型，0：没有攻击；1：近战攻击；2：远程攻击"), Range(0, 2)]
    public int attackType;//当前的攻击类型，0：没有攻击；1：近战攻击；2：远程攻击

    [Range(0f, 10f)] public float meleeAttackDamage;//轻攻击伤害 10
    [Range(20f, 30f)] public float rangedAttackDamage;//重攻击伤害 20
    [SerializeField] private GameObject iceBall;
    [SerializeField] private Transform iceBallFirePoint;

    [Header("角色属性")]
    [Tooltip("血量上限"), Range(0f, 1000f)] public float MaxHP;//血量上限,初始值100

    [Range(0f, 1000f)] public float HP;//当前血量
    private float beHitSpeed;//被击退速度
    [Range(0f, 10f)] public float moveSpeed;//移动速度
    [Range(5f, 20f)] public float jumpSpeed;//跳跃速度
    [SerializeField, Tooltip("可以进行几段跳跃"), Range(1, 3)] private int jumpMax;//最大跳跃数
    [SerializeField, Tooltip("无敌帧")] private float defendTime;//无敌时间(无敌帧)
    [SerializeField, Tooltip("无敌帧计时器")] private float defendTimer;
    public float beHitDamageConversion;//被击中的伤害转换率
    public float attackDamageConversion;//攻击的伤害转换率

    [Header("角色状态")]
    public bool canInput;//是否能输入

    public bool jumpPressed;//跳跃键是否按下

    public bool isGround;//判断是否正在地面
    public bool isJump;//判断是否正在跳跃
    public int jumpStep;//当前跳跃数
    public bool isFall;//是否在下落
    public bool isDodge;//是否在闪避中
    public bool isAttack;//是否在攻击中
    public bool isHit;//是否处于被攻击状态
    public bool isDefend;//是否处于无敌状态
    public bool alive;//是否存活

    [Header("角色闪避设置")]
    [SerializeField, Range(5f, 20f)] private float dodgeSpeed;//闪避速度

    [SerializeField] private float dodgeTimer;//闪避计时器
    [SerializeField] private float dodgeCd;//闪避Cd

    [Header("地面检测")]
    [SerializeField] private Vector3 checkPoint_G;//用于地面检测
    [SerializeField] private LayerMask StandingLayer;//图层遮罩
    [Header("面朝向检测")]
    [SerializeField] private Vector3 checkPoint_Face;//用于面前检测
    [SerializeField] private LayerMask FacingLayer;//图层遮罩

    private Rigidbody2D playerBody;//角色刚体
    private Animator animator;//角色动画
    private BoxCollider2D playerCollider;//角色碰撞器
    private AudioSource playerAudioSource;//角色声音

    [SerializeField] private SpriteRenderer[] spriteRenderers;

    [SerializeField] private AudioClip[] playerAudioClip;

    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerAudioSource = GetComponent<AudioSource>();

        meleeSpeed = 1f;
        rangedSpeed = -1f;

        MaxHP = 100f;
        HP = MaxHP;

        attackType = 0;
        meleeAttackDamage = 10f;
        rangedAttackDamage = 20f;
        attackDamageConversion = 1f;
        beHitDamageConversion = 1f;

        moveSpeed = 3f;
        jumpSpeed = 5f;
        beHitSpeed = 3f;
        dodgeSpeed = 7f;

        isJump = false;
        jumpStep = 0;
        isDodge = false;
        isAttack = false;
        canInput = true;
        alive = true;

        jumpMax = 2;

        dodgeCd = 2f;
        dodgeTimer = dodgeCd;

        defendTime = 3f;
        defendTimer = defendTime;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        animator.SetBool("alive", alive);
        GameManager.Instance.playerAlive = true;
    }

    private void Update()
    {
        animator.SetFloat("Horizontal", playerBody.velocity.x);
        animator.SetFloat("Vertical", playerBody.velocity.y);

        if (playerBody.velocity.y < 0)
            isFall = true;
        else isFall = false;

        if (Input.GetButtonDown("Jump") && jumpStep < jumpMax)
        {
            if (!canInput || isHit || isAttack || isDodge) return;
            jumpPressed = true;
        }

        Attack();
        Dodge();
        Defend();
        Death();
        //Debug.Log(HP);
    }

    private void FixedUpdate()
    {
        CheckGround();
        Move();
        Jump();
    }

    private void Jump()
    {
        if (jumpPressed && isGround)
        {
            isJump = true;
            jumpStep++;
            animator.SetInteger("jumpStep", jumpStep);
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpSpeed);
            animator.SetTrigger("Jump");
            playerAudioSource.PlayOneShot(playerAudioClip[(int)PlayerSound.Jump]);
            jumpPressed = false;
        }
        else if (!isGround && jumpPressed && jumpStep < jumpMax)
        {
            jumpStep++;
            animator.SetInteger("jumpStep", jumpStep);
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpSpeed);
            animator.SetTrigger("Jump");
            playerAudioSource.PlayOneShot(playerAudioClip[(int)PlayerSound.Jump]);
            jumpPressed = false;
        }
    }

    private void CheckGround()
    {
        isGround = Physics2D.OverlapCircle(transform.position + new Vector3(checkPoint_G.x, checkPoint_G.y, 0), checkPoint_G.z, StandingLayer);
        animator.SetBool("isGround", isGround);
        if (isGround)
        {
            isJump = false;
            jumpStep = 0;
            animator.SetInteger("jumpStep", jumpStep);
            isFall = false;
            animator.SetBool("isFall", isFall);
        }
    }

    public void Footstep()
    {
        playerAudioSource.PlayOneShot(playerAudioClip[(int)PlayerSound.Move], 0.5f);
    }

    private void Move()
    {
        if (!canInput || isHit || isAttack || isDodge) return;
        bool canMove = !Physics2D.OverlapBox(transform.position + 
            new Vector3(transform.localScale.x * checkPoint_Face.x, checkPoint_Face.y, 0), new(checkPoint_Face.z, 0.8f), 0, FacingLayer);
        float h = Input.GetAxisRaw("Horizontal");
        if (canMove)
        {
            playerBody.velocity = new Vector2(h * moveSpeed, playerBody.velocity.y);
        }

        if (h != 0)
        {
            transform.localScale = new Vector3(h, 1, 1);
            UI_Player.transform.localScale = new Vector3(h, 1, 1);
        }
    }

    private void Dodge()
    {
        dodgeTimer -= Time.deltaTime;
        if (!canInput || isHit || isAttack) return;
        if (Input.GetButtonDown("Dodge") && dodgeTimer <= 0)
        {
            dodgeTimer = dodgeCd;
            canInput = false;
            isDodge = true;
            animator.SetBool("isDodge", isDodge);
            animator.SetTrigger("Dodge");
            playerBody.velocity = new Vector2(transform.localScale.x * dodgeSpeed, 0);
            print(playerBody.velocity);
            playerBody.gravityScale = 0;
            gameObject.layer = LayerMask.NameToLayer("Flashing");
            playerAudioSource.PlayOneShot(playerAudioClip[(int)PlayerSound.Dodge]);
        }
    }

    public void DodgeOver()
    {
        isDodge = false;
        animator.SetBool("isDodge", isDodge);
        playerBody.gravityScale = 1;
        canInput = true;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void Attack()
    {
        if (!canInput) return;
        if (Input.GetButtonDown("MeleeAttack") && !isAttack)
        {
            canInput = false;
            playerBody.velocity = Vector2.zero;
            isAttack = true;
            animator.SetBool("isAttack", isAttack);
            attackType = 1;
            animator.SetInteger("attackType", attackType);
            animator.SetTrigger("MeleeAttack");
            playerBody.velocity = new Vector2(transform.localScale.x * meleeSpeed, playerBody.velocity.y);
            playerAudioSource.PlayOneShot(playerAudioClip[(int)PlayerSound.Melee], 0.5f);
            playerAudioSource.PlayOneShot(playerAudioClip[(int)PlayerSound.Jump], .5f);
        }
        if (Input.GetButtonDown("RangedAttack") && !isAttack)
        {
            canInput = false;
            playerBody.velocity = Vector2.zero;
            isAttack = true;
            animator.SetBool("isAttack", isAttack);
            attackType = 2;
            animator.SetInteger("attackType", attackType);
            animator.SetTrigger("RangedAttack");
            playerBody.velocity = new Vector2(transform.localScale.x * rangedSpeed, playerBody.velocity.y);
        }
    }

    public void IceBall()
    {
        Instantiate(iceBall, iceBallFirePoint.position, Quaternion.identity);
        playerBody.velocity = new Vector2(transform.localScale.x * rangedSpeed, playerBody.velocity.y);
        playerAudioSource.PlayOneShot(playerAudioClip[(int)PlayerSound.Ranged], 0.7f);
    }

    public void AttackOver()
    {
        canInput = true;
        isAttack = false;
        animator.SetBool("isAttack", isAttack);
        attackType = 0;
        animator.SetInteger("attackType", attackType);
    }

    public void Hurt(Vector2 Dir, float damage)
    {
        if (alive && !isDefend)
        {
            isHit = true;
            animator.SetBool("isHit", isHit);
            isDefend = true;
            animator.SetBool("isDefend", isDefend);
            canInput = false;
            playerBody.velocity = Dir * beHitSpeed;
            HP -= damage;
        }
    }

    public void HurtOver()
    {
        canInput = true;
        isHit = false;
        animator.SetBool("isHit", isHit);
    }

    public void Defend()
    {
        if (isDefend)
        {
            defendTimer -= Time.deltaTime;
            if (defendTimer > 0)
            {
                gameObject.layer = LayerMask.NameToLayer("Flashing");
                float flash = defendTimer % 0.5f;
                foreach (var t in spriteRenderers)
                    t.enabled = flash > 0.15f;
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Player");
                isDefend = false;
                animator.SetBool("isDefend", isDefend);
                defendTimer = defendTime;
                foreach (var t in spriteRenderers)
                    t.enabled = true;
            }
        }
    }

    public void Death()
    {
        if (HP <= 0)
        {
            isHit = false;
            animator.SetBool("isHit", isHit);
            isDefend = false;
            animator.SetBool("isDefend", isDefend);
            isAttack = false;
            animator.SetBool("isAttack", isAttack);
            alive = false;
            animator.SetBool("alive", alive);

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
                {
                    GameManager.Instance.playerAlive = false;
                    GameManager.Instance.Pause(true);
                }
            }
        }
    }

    public void Wake()
    {
        canInput = true;
        alive = true;
        animator.SetBool("alive", alive);
        HP = MaxHP;
        GameManager.Instance.playerAlive = true;
        GameManager.Instance.Pause(false);
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<FSM>();
            enemy.parameter.getHit = true;
            enemy.parameter.beHitDamage = meleeAttackDamage * attackDamageConversion;
            enemy.parameter.health -= enemy.parameter.beHitDamage;
        }
        if (other.gameObject.CompareTag("Boss"))
        {
            other.GetComponent<FireCentipede>().isHit = true;
            other.GetComponent<FireCentipede>().BeHit(meleeAttackDamage * attackDamageConversion);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(checkPoint_G.x, checkPoint_G.y, 0), checkPoint_G.z);
        Gizmos.DrawWireCube(transform.position + new Vector3(transform.localScale.x * checkPoint_Face.x, checkPoint_Face.y, 0), new(checkPoint_Face.z, 0.8f));
    }
}