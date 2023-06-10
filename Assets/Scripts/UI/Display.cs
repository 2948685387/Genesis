using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Display : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public string description;
    public TMP_Text text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.text = "";
    }
}
