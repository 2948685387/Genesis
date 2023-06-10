using UnityEngine;

public enum BossState
{
    FireBall,
    FirePillar,
    Dash,
    Idle,
    BeHit,
    Death,
}
public class FireCentipede : MonoBehaviour
{
    Transform C_tra;
    Transform Mouth;
    Rigidbody2D C_rig;
    Animator C_ani;
    BackGround background;//改变背景图(改成灯光变暗)
    public AudioSource HitAudio;
    public AudioSource AttackAudio;
    public ObjectPool FireballPool;//火球
    public ObjectPool FirePillar;//火柱
    public GameObject Player;//获取到玩家的位置


    BossState state;

    public float MaxHp;
    public float Hp;

    public float MoveDamage;

    public float Speed;

    public float IdleTime;
    public float FirePillarCd;
    public float time;

    public int FireBallAttackTime;
    public int FirePillarAttackTime;

    public bool isHit;
    public bool isDead;

    void Awake()
    {
        C_tra = GetComponent<Transform>();
        C_rig = GetComponent<Rigidbody2D>();
        C_ani = GetComponent<Animator>();
        background = GameObject.Find("BlackCloth").GetComponent<BackGround>();
        Player = GameObject.FindWithTag("Player");
        Mouth = transform.Find("Mouth");

        state = BossState.Idle;

        MaxHp = 500;
        Hp = MaxHp;

        MoveDamage = 20;

        Speed = 10;

        isDead = false;

        IdleTime = 5f;
        FirePillarCd = 10f;
        time = 1f;

        FireBallAttackTime = 2;
        FirePillarAttackTime = 3;
    }
    private void Start()
    {
        GameManager.Instance.bossAlive = true;
    }
    void Update()
    {
        CheckHp();
        switch (state)
        {
            case BossState.FireBall:
                {
                    FireBallAttack();
                    break;
                }
            case BossState.FirePillar:
                {
                    FirePillarAttack();

                    break;
                }
            case BossState.Dash:
                {
                    DashSkill();
                    break;
                }
            case BossState.Idle:
                {
                    IdleProcess();
                    break;
                }
            case BossState.BeHit:
                {
                    BeHitProcess();
                    break;
                }
            case BossState.Death:
                {
                    DeathProcess();
                    break;
                }
        }
    }
    public void FireBallAttack() //吐火球
    {
        C_ani.Play("Attack");
        FirePillarCd -= Time.deltaTime;
        if (FireBallAttackTime <= 0 && !isDead)
        {
            state = BossState.Idle;
        }
        else if (isDead)
        {
            state = BossState.Death;
        }
    }
    public void FireBallCreate() //生成火球(动画帧事件)
    {
        for (int i = 0; i < 3; i++)
        {
            var fireball = FireballPool.GetObject((FireBallAttackTime - 1) + i * 3);
            Vector3 dir = Quaternion.Euler(0, i * 15, 0) * transform.right;
            fireball.transform.SetPositionAndRotation(Mouth.position + dir * 1.0f, Quaternion.Euler(0, 0, i * 10 * C_tra.localScale.x));
        }
        FireBallAttackTime -= 1;
    }
    public void DashSkill()
    {
        if (!isDead)
        {
            Dash();
            IdleTime = 5f;
        }
        else if (isDead)
        {
            state = BossState.Death;
        }
    }
    public void Dash()//冲撞 
    {
        C_ani.Play("Walk");
        C_rig.velocity = new Vector2(C_tra.localScale.x * Speed, C_rig.velocity.y);
    }
    public void FirePillarAttack() //火柱攻击
    {
        C_ani.Play("OtherAttack");
        background.isChange = true;
        FirePillarCd = 10f;
        if (FirePillarAttackTime <= 0 && !isDead)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                state = BossState.Idle;
                time = 1;
            }
        }
        else if (isDead)
        {
            state = BossState.Death;
            background.isChange = false;
        }
    }
    public void CreateFirePillar() //生成火柱（动画帧事件）
    {
        for (int i = 0; i < 3; i++)
        {
            var firePillar = FirePillar.GetObject((FirePillarAttackTime - 1) + i * 3);
            int r = Random.Range(-12, 12);
            firePillar.transform.position = new Vector3(r, 6, 0);
        }
        FirePillarAttackTime -= 1;
    }
    public void IdleProcess()
    {
        C_ani.Play("Idle");
        background.isBack = true;
        FirePillarCd -= Time.deltaTime;
        IdleTime -= Time.deltaTime;
        if (IdleTime > 0)
        {
            FireBallAttackTime = 3;
            FirePillarAttackTime = 3;
        }
        if (IdleTime <= 0 && !isHit && !isDead)
        {
            if (FirePillarCd <= 0 && Hp <= MaxHp / 2)
            {
                state = BossState.FirePillar;
            }
            else
            {
                state = BossState.Dash;
            }
        }
        else if (isHit && !isDead)
        {
            state = BossState.BeHit;
        }
        else if (isDead)
        {
            state = BossState.Death;
        }
    }
    public void BeHitProcess()
    {
        C_ani.Play("BeHit");
        IdleTime -= Time.deltaTime;
        if (!isHit && !isDead)
        {
            state = BossState.Idle;
        }
        else if (isDead)
        {
            state = BossState.Death;
        }
    }
    public void BeHit(float Damage)
    {
        Hp -= Damage;
        isHit = true;
        HitAudio.Play();
    }
    public void BeHitOver()
    {
        isHit = false;
    }
    public void CheckHp()
    {
        if (Hp <= 0)
        {
            isDead = true;
            state = BossState.Death;
        }
    }
    public void DeathProcess()
    {
        gameObject.layer = LayerMask.NameToLayer("Background");
        GameManager.Instance.bossAlive = false;
        C_ani.Play("Death");
    }
    public void PlayAttackAudio()
    {
        AttackAudio.Play();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("AirWall"))
        {
            C_tra.localScale = new Vector3(-C_tra.localScale.x, C_tra.localScale.y, C_tra.localScale.z);
            state = BossState.FireBall;
        }
        else if (collision.collider.CompareTag("Player") && state == BossState.Dash)
        {
            var player = collision.collider.GetComponent<PlayerController>();
            if (C_tra.position.x < Player.transform.position.x)
            {
                player.Hurt(Vector2.right, MoveDamage * player.beHitDamageConversion);
            }
            else if (C_tra.position.x >= Player.transform.position.x)
            {
                player.Hurt(Vector2.left, MoveDamage * player.beHitDamageConversion);
            }
        }
    }
}