using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class UpFloating : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    public float damage;

    void Update()
    {
        if (damage > 0)
        {
            damageText.text = $"-{damage}";
            if (damageText.fontSize < 0.3f)
            {
                damageText.transform.Translate(Vector2.up * Time.deltaTime);
                damageText.fontSize += 0.005f;
            }
            else
            {
                damageText.transform.localPosition = Vector2.up;
                damageText.fontSize = 0.1f;
                damage = 0;
                damageText.text = "";
            }
        }
    }
}
