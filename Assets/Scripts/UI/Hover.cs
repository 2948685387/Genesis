using UnityEngine;
using UnityEngine.EventSystems;

public class Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.localScale += Vector3.one;
        PlayOnTouch();
    }
    
    private void OnDisable()
    {
        gameObject.transform.localScale = Vector3.one;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localScale = Vector3.one;
    }
    public void PlayOnClick()
    {
        audioSource.PlayOneShot(audioClips[1]);
    }
    public void PlayOnTouch()
    {
        audioSource.PlayOneShot(audioClips[0]);
    }
}
