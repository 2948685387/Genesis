using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    public bool playerAlive;

    public GameObject boss;
    public bool bossAlive;

    public bool pause;
    public bool gameOver;

    public int indexLevel;

    public Vector2[] playerPos;
    public static GameManager Instance { get; private set; }

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
    }

    // Update is called once per frame
    void Update()
    {
        indexLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public void Pause(bool isPause)
    {
        pause = isPause;
        Time.timeScale = isPause ? 0f : 1f;
        player.GetComponent<PlayerController>().canInput = !isPause;
    }


    public IEnumerator NextLevel(int index)
    {
        UIController.Instance.animator.SetTrigger("In");

        yield return new WaitForSeconds(1);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        asyncOperation.completed += AsyncOperation_completed;
    }

    private void AsyncOperation_completed(AsyncOperation obj)
    {
        UIController.Instance.animator.SetTrigger("Out");
        player.transform.position = playerPos[indexLevel + 1];
        print(player.transform.position);
    }
}
