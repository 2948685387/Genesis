using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Image healthBar;
    public Image fakeHealthBar;

    public GameObject boss;
    public FireCentipede fireCentipede;

    void Update()
    {
        FindBoss();
        if (fireCentipede != null)
        {
            healthBar.fillAmount = fireCentipede.Hp / fireCentipede.MaxHp;
            if (fakeHealthBar.fillAmount > healthBar.fillAmount)
            {
                fakeHealthBar.fillAmount -= 0.001f;
            }
            else
            {
                fakeHealthBar.fillAmount = healthBar.fillAmount;
            }
        }
    }
    void FindBoss()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss)
        {
            fireCentipede = boss.GetComponent<FireCentipede>();
        }
    }
}
