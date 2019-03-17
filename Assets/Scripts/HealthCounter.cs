using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthCounter : MonoBehaviour
{
    public int health = 500;
    public int digits = 3;
    [TextArea]
    public string format = "<mspace=1.7em>HEALTH: <color=#FFF3>{0}</color>{1}</mspace>";

    public TMP_Text text;

    private void OnValidate()
    {
        health = Mathf.Clamp(health, 0, (int)Mathf.Pow(10, digits)-1);
        if (text)
            UpdateHealthText();
    }

    public void UpdateHealthText()
    {
        if (health == 0)
        {
            text.text = string.Format(format, new string('0', digits), string.Empty);
        }
        else
        {
            text.text = string.Format(format, new string('0', digits - health.ToString().Length), health);
        }
    }
}
