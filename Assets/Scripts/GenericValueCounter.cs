using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GenericValueCounter : MonoBehaviour
{
    [FormerlySerializedAs("score")]
    public int value;
    public int digits = 3;
    [TextArea]
    public string format = "<mspace=1.7em><color=#FFF3>{0}</color>{1}</mspace>";
    [TextArea]
    public string separator = "</mspace><mspace=0.5em>,</mspace><mspace=1.7em>";

    public TMP_Text text;

    [Space]
    [FormerlySerializedAs("scoreChanged")]
    public ValueChangedEvent valueChanged;

#if UNITY_EDITOR
    
    private void OnValidate()
    {
        value = Mathf.Clamp(value, 0, (int)Mathf.Pow(10, digits)-1);
        if (text)
            UpdateValueText();
    }

#endif

    public void ChangeValue(int delta)
    {
        value = Mathf.Clamp(value + delta, 0, (int)Mathf.Pow(10, digits) - 1);
        valueChanged.Invoke(value);
        UpdateValueText();
    }

    public void UpdateValueText()
    {
        string digitsWithSeparators =
            ((int) Mathf.Pow(10, digits)).ToString("#,###", CultureInfo.InvariantCulture).Substring(1).Trim(',');

        if (value == 0)
        {
            text.text = string.Format(format, 
                digitsWithSeparators.Replace(",", separator),
                string.Empty);
        }
        else
        {
            string valueWithSeparators = value.ToString("#,###", CultureInfo.InvariantCulture);
            string digitsSubStringed =
                digitsWithSeparators.Substring(0, digitsWithSeparators.Length - valueWithSeparators.Length);

            text.text = string.Format(format,
                digitsSubStringed.Replace(",", separator),
                valueWithSeparators.Replace(",", separator));
        }
    }

    [Serializable]
    public class ValueChangedEvent : UnityEvent<int>
    {

    }
}
