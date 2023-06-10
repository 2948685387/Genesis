using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bread : MonoBehaviour
{
    public float value;
    public string description;

    // Start is called before the first frame update
    void Start()
    {
        value = 10f;
    }

    // Update is called once per frame

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().MaxHP += value;
            collision.gameObject.GetComponent<PlayerController>().HP += value;

            collision.gameObject.GetComponentInChildren<TMP_Text>().text = description;
            Destroy(gameObject);
            UIController.Instance.playerAttributes[2].SetActive(true);

        }
    }
}
