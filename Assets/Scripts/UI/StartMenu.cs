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
    public AudioMixer audioMixer;    // ���п��Ƶ�Mixer����


    public void SetMasterVolume(float volume)    // �����������ĺ���
    {
        audioMixer.SetFloat("Master", volume);
    }

    public void SetBGMVolume(float volume)    // ���Ʊ������������ĺ���
    {
        audioMixer.SetFloat("BGM", volume);
    }

    public void SetSoundEffectVolume(float volume)    // ������Ч�����ĺ���
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
