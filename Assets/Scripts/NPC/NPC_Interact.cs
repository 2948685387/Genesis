using UnityEngine;

public class NPC_Interact : MonoBehaviour
{
    public bool canInteract;
    public GameObject tip;
    public bool dialogDisplay;
    public GameObject dialog;

    public GameObject present;
    public Transform npc;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        dialogDisplay = false;
        canInteract = false;
        npc = transform.parent;
    }
    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(npc.localScale.x / 2, 1, 1);
        if (canInteract)
        {
            if (!dialogDisplay)
                if (Input.GetKeyDown(KeyCode.W))
                {
                    tip.SetActive(false);
                    dialog.SetActive(true);
                    dialogDisplay = true;
                    canInteract = false;
                    Instantiate(present, transform.position, Quaternion.identity);
                }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canInteract = !dialogDisplay;
        tip.SetActive(!dialogDisplay);
        if (collision.CompareTag("Player")&&canInteract)
        {
            target = collision.gameObject.transform;
            if ((target.position - npc.position).x < 0)
            {
                npc.localScale = new Vector2(-2, 2);
            }
            else
            {
                npc.localScale = new Vector2(2, 2);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        tip.SetActive(false);
        canInteract = false;
    }
}
