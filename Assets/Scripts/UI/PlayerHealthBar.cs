using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Image healthBars;
    public Image fakeHealthBars;

    PlayerController Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        healthBars.fillAmount = Player.HP / Player.MaxHP;
        if (fakeHealthBars.fillAmount > healthBars.fillAmount)
        {
            fakeHealthBars.fillAmount -= 0.002f;
        }
        else
        {
            fakeHealthBars.fillAmount = healthBars.fillAmount;
        }
    }
}
