using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image healthBars;
    public Image fakeHealthBars;

    public Canvas canvas;
    public FSM enemy;

    public UpFloating damageText;

    void Update()
    {
        canvas.gameObject.transform.localScale = enemy.transform.localScale;
        if (enemy != null&&enemy.parameter.getHit)
        {
            healthBars.fillAmount = enemy.parameter.health / enemy.parameter.MaxHP;
            if (healthBars.fillAmount == 0)
            {
                Destroy(gameObject);
            }

            if (fakeHealthBars.fillAmount > healthBars.fillAmount)
            {
                fakeHealthBars.fillAmount -= 0.01f;
            }
            else
            {
                fakeHealthBars.fillAmount = healthBars.fillAmount;
            }
        }
    }
    public void Text_Display()
    {
        damageText.damage = enemy.parameter.beHitDamage;
    }
}
