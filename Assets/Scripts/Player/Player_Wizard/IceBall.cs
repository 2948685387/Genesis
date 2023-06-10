using UnityEngine;

public class IceBall : MonoBehaviour
{
    private AudioSource boom;
    public float moveSpeed;
    public Vector3 direction;
    public float damage;
    public float lifeTime;

    public PlayerController player;
    private void Awake()
    {
        
        lifeTime = 1f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        moveSpeed = 10f;
        direction = new Vector3(player.transform.localScale.x, 0, 0);
        damage = player.rangedAttackDamage * player.attackDamageConversion;
    }
    private void Start()
    {
        boom = GetComponent<AudioSource>();
    }
    private void Update()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(gameObject);
            collision.GetComponent<FSM>().parameter.getHit = true;
            collision.GetComponent<FSM>().parameter.beHitDamage = damage;
            collision.GetComponent<FSM>().parameter.health -= damage;
        }
        else if (collision.CompareTag("Boss"))
        {
            Destroy(gameObject);
            collision.GetComponent<FireCentipede>().isHit = true;
            collision.GetComponent<FireCentipede>().BeHit(damage);
        }
        else if (collision.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
