using UnityEngine;

public class Options : MonoBehaviour
{
    public GameObject backButton;
    public GameObject tipsButton;
    public bool tipsDisplay;
    [SerializeField] private GameObject tips;
    public GameObject quitButton;
    // Start is called before the first frame update
    void Start()
    {
        tipsDisplay = false;
    }


    public void Back()
    {
        gameObject.SetActive(false);
        UIController.Instance.optionsDisplay = false;
        GameManager.Instance.Pause(false);
    }

    public void Tips()
    {
        tipsDisplay = !tipsDisplay;
        tips.SetActive(tipsDisplay);
        backButton.SetActive(!tipsDisplay);
        tipsButton.SetActive(!tipsDisplay);
        quitButton.SetActive(!tipsDisplay);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        tipsDisplay = false;
        tips.SetActive(tipsDisplay);
        backButton.SetActive(!tipsDisplay);
        tipsButton.SetActive(!tipsDisplay);
        quitButton.SetActive(!tipsDisplay);
    }
}
