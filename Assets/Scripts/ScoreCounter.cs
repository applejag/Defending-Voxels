using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public int score;
    public int digits = 3;
    [TextArea]
    public string format = "<mspace=1.7em><color=#FFF3>{0}</color>{1}</mspace>";
    [TextArea]
    public string separator = "</mspace><mspace=0.5em>,</mspace><mspace=1.7em>";

    public TMP_Text text;

#if UNITY_EDITOR
    
    private void OnValidate()
    {
        score = Mathf.Clamp(score, 0, (int)Mathf.Pow(10, digits)-1);
        if (text)
            UpdateScoreText();
    }

#endif

    public void AddScore(int add)
    {
        score = Mathf.Clamp(score + add, 0, (int) Mathf.Pow(10, digits) - 1);
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        string digitsWithSeparators =
            ((int) Mathf.Pow(10, digits)).ToString("#,###", CultureInfo.InvariantCulture).Substring(1).Trim(',');

        if (score == 0)
        {
            text.text = string.Format(format, 
                digitsWithSeparators.Replace(",", separator),
                string.Empty);
        }
        else
        {
            string valueWithSeparators = score.ToString("#,###", CultureInfo.InvariantCulture);
            string digitsSubStringed =
                digitsWithSeparators.Substring(0, digitsWithSeparators.Length - valueWithSeparators.Length);

            text.text = string.Format(format,
                digitsSubStringed.Replace(",", separator),
                valueWithSeparators.Replace(",", separator));
        }
    }
}
