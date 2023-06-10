using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float value;
    public string description;

    // Start is called before the first frame update
    void Start()
    {
        value = 1.2f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().attackDamageConversion *= value;
            collision.gameObject.GetComponentInChildren<TMP_Text>().text = description;
            Destroy(gameObject);
            UIController.Instance.playerAttributes[1].SetActive(true);

        }
    }
}
