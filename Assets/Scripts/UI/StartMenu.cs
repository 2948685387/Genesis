using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartMenu : MonoBehaviour
{
    public GameObject panel_home;
    public GameObject panel_loading;
    public GameObject panel_options;
    public Slider slider;
    public Text text;
    public AudioMixer audioMixer;    // 进行控制的Mixer变量


    public void SetMasterVolume(float volume)    // 控制主音量的函数
    {
        audioMixer.SetFloat("Master", volume);
    }

    public void SetBGMVolume(float volume)    // 控制背景音乐音量的函数
    {
        audioMixer.SetFloat("BGM", volume);
    }

    public void SetSoundEffectVolume(float volume)    // 控制音效音量的函数
    {
        audioMixer.SetFloat("SoundEffect", volume);
    }
    public void StartGame()
    {
        StartCoroutine(LoadLevel());
    }
    IEnumerator LoadLevel()
    {
        panel_loading.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            slider.value = operation.progress;
            text.text = operation.progress * 100 + "%";
            if (operation.progress >= 0.9f)
            {
                slider.value = 1;
                text.text = "Press AnyKey to continue";
                if (Input.anyKeyDown) operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void Options()
    {
        Display(panel_home);
        Display(panel_options);
    }
    public void Back()
    {
        Display(panel_home);
        Display(panel_options);
    }
    public void Display(GameObject GO)
    {
        CanvasGroup canvasGroup = GO.GetComponent<CanvasGroup>();
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0: 1;
        canvasGroup.interactable = canvasGroup.alpha>0?true:false;
        canvasGroup.blocksRaycasts = canvasGroup.alpha > 0 ? true : false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
