using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject player;
    public GameObject playerHealthBar;
    public GameObject[] playerAttributes;

    public GameObject boss;
    public GameObject bossHealthBar;

    public GameObject failure;
    public GameObject victory;

    public GameObject options;
    public bool optionsDisplay;

    public Animator animator;
    public static UIController Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        optionsDisplay = false;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBarDisplay();
        Failure();
        Victory();
        Options();
    }

    private void Options()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionsDisplay = !optionsDisplay;
            GameManager.Instance.Pause(optionsDisplay);
        }
        options.SetActive(optionsDisplay);
    }

    void Failure()
    {
        failure.SetActive(!GameManager.Instance.playerAlive);
    }

    public void TryAgain()
    {
        if (player)
        {
            player.GetComponent<PlayerController>().Wake();
        }
    }

    void Victory()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss)
        {
            victory.SetActive(!GameManager.Instance.bossAlive);
        }

    }

    void HealthBarDisplay()
    {
        playerHealthBar.SetActive(GameManager.Instance.playerAlive);
        bossHealthBar.SetActive(GameManager.Instance.bossAlive);
    }
}
