using UnityEngine;

public class FireBall : MonoBehaviour
{
    GameObject Boss;
    Animator animator;
    Collider2D collider2d;

    Vector3 dir;

    public float Speed;

    public float Damage;

    public float LifeTime;

    public ObjectPool pool;
    private void Awake()
    {
        pool = transform.parent.GetComponent<ObjectPool>();
    }
    void Start()
    {
        Boss = GameObject.Find("Boss");
        animator = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();

        dir = transform.localScale;

        Speed = 5;

        Damage = 8f;

    }
    private void OnEnable()
    {
        LifeTime = 5f;
    }
    void Update()
    {
        Move();
        LifeTime -= Time.deltaTime;
        if (LifeTime <= 0 || Boss.GetComponent<FireCentipede>().isDead)
        {
            pool.ReturnObject(gameObject);
        }
    }
    public void Move()
    {
        if (Boss.transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-dir.x, dir.y, dir.z);
            transform.position += Speed * Time.deltaTime * -transform.right;
        }
        else if (Boss.transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-dir.x, dir.y, dir.z);
            transform.position += Speed * Time.deltaTime * transform.right;
        }
    }
    public void CloseCollider()
    {
        collider2d.enabled = false;
    }
    public void Destroy()
    {
        collider2d.enabled = true;
        pool.ReturnObject(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerController>();
            animator.Play("Hit");
            if (transform.position.x < collision.transform.position.x)
            {
                player.Hurt(Vector2.right, Damage * player.beHitDamageConversion);
            }
            else if (transform.position.x >= collision.transform.position.x)
            {
                player.Hurt(Vector2.left, Damage*player.beHitDamageConversion);
            }

        }
        else if (collision.CompareTag("Ground"))
        {
            animator.Play("Hit");
        }
    }
}
