using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    public int indexLevel;

    private void Start()
    {
        indexLevel = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(GameManager.Instance.NextLevel(indexLevel+1));
        }
    }
}
