using UnityEngine;

public class FirePillar : MonoBehaviour
{
    Animator ani;
    AudioSource music;
    public float Speed;
    public float Damage;
    public bool IsBoom;

    public ObjectPool pool;
    void Awake()
    {
        ani = GetComponent<Animator>();
        music = GetComponent<AudioSource>();

        Speed = 5;
        Damage = 15f;
        IsBoom = false;

        pool = transform.parent.GetComponent<ObjectPool>();
    }


    void Update()
    {
        if (!IsBoom)
        {
            Move();
        }
    }
    public void Move()
    {
        transform.position += Speed * Time.deltaTime * Vector3.down;
    }
    public void Destroy()
    {
        IsBoom = false;
        pool.ReturnObject(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            IsBoom = true;
            transform.position = new Vector3(transform.position.x, -3.5f, 0);
            ani.Play("boom");
            music.Play();
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("boom"))
            {
                music.Play();
            }
        }
        else if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerController>();
            IsBoom = true;
            transform.position = new Vector3(transform.position.x, -3.5f, 0);
            ani.Play("boom");
            music.Play();
            if (transform.position.x < collision.transform.position.x)
            {
                player.Hurt(Vector2.right, Damage*player.beHitDamageConversion);
            }
            else if (transform.position.x >= collision.transform.position.x)
            {
                player.Hurt(Vector2.left, Damage*player.beHitDamageConversion);
            }
        }
    }
}
