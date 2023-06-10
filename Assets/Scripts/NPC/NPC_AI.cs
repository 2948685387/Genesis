using UnityEngine;

public enum NPCState
{
    Idle,
    Walk,
}

public class NPC_AI : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float direction;//Ïò×ó-1£»ÏòÓÒ1
    [SerializeField] private float idleTime;
    [SerializeField] private float walkTime;
    [SerializeField] private Animator animator;

    [SerializeField] private float timer;

    public GameObject UI_NPC;
    // Start is called before the first frame update

    public NPCState state;

    void Start()
    {
        state = NPCState.Idle;

        //walkSpeed = 2f;
        //direction = 1f;

        //idleTime = 3f;
        //walkTime = 3f;

        animator = GetComponent<Animator>();

        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case NPCState.Idle:
                IdleProcess();
                break;
            case NPCState.Walk:
                WalkProcess();
                break;
        }

    }

    private void WalkProcess()
    {
        animator.Play("Walk");
        timer += Time.deltaTime;
        transform.Translate(new Vector2(direction, 0) * walkSpeed * Time.deltaTime, Space.World);
        if (timer >= walkTime||UI_NPC.GetComponent<NPC_Interact>().canInteract)
        {
            state = NPCState.Idle;
            timer = 0f;
        }
    }

    private void IdleProcess()
    {
        animator.Play("Idle");
        if (UI_NPC.GetComponent<NPC_Interact>().canInteract==false)
        {
            timer += Time.deltaTime;
            if (timer >= idleTime)
            {
                direction = -direction;
                transform.localScale = new Vector3(direction * 2, 2, 1);
                state = NPCState.Walk;
                timer = 0f;
            }
        }
        
    }
}
