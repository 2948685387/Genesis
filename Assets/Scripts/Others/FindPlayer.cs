using Cinemachine;
using UnityEngine;

public class FindPlayer : MonoBehaviour
{
    public GameObject player;
    public CinemachineVirtualCamera cinemachine;
    private void Awake()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            cinemachine.Follow = player.transform;
    }

}
