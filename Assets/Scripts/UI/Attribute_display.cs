using TMPro;
using UnityEngine;

public class Attribute_display : MonoBehaviour
{
    public TMP_Text TMP_Text;
    public float duration;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (TMP_Text.text != "")
        {
            timer += Time.deltaTime;
            if (timer >= duration)
            {
                TMP_Text.text = "";
                timer = 0f;
            }
        }
    }
}
