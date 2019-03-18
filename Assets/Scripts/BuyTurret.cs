using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BuyTurret : MonoBehaviour
{
    [Header("Format: {0}")] public string turretName;

    [Header("Format: {1}")] public int price = 100;

    [Header("Format: {2}")]
    public Color colorAvailable = Color.white;
    public Color colorTooPricy = new Color(1, 1, 1, 0.1f);

    [TextArea] [Header("Price format")] public string format = "{0}\n<color={2}><size=3>{1}</size></color>";

    [Header("Material colors")]
    [ColorUsage(true, false)]
    public Color materialAvailableAlbedo = new Color(0.8f, 0.8f, 0.8f, 1);
    [ColorUsage(false, true)]
    public Color materialAvailableEmission = new Color(0, 0.9883373f, 0.9883373f, 1);
    [ColorUsage(true, false)]
    public Color materialTooPricyAlbedo = new Color(0.8f, 0.8f, 0.8f, 1);
    [ColorUsage(false, true)]
    public Color materialTooPricyEmission = new Color(0, 0.9883373f, 0.9883373f, 1);

    public Renderer[] renders;

    [Space]

    public float availableFadeSpeed = 0.5f;
    public TMP_Text text;

    [Space]

    public bool available;

    private float _availableFade;

    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

#if UNITY_EDITOR
    
    private void OnValidate()
    {
        if (text)
            UpdateText(available ? colorAvailable : colorTooPricy);
    }

#endif

    public void UpdateAvailable(int cash)
    {
        if (cash >= price)
        {
            available = true;
            enabled = true;
        }
    }

    private void Update()
    {
        float targetFade = available ? 1 : 0;
        _availableFade = Mathf.MoveTowards(_availableFade, targetFade, Time.deltaTime / availableFadeSpeed);
        if (Mathf.Approximately(targetFade, _availableFade))
        {
            _availableFade = targetFade;
            enabled = false;
        }

        Color color = Color.Lerp(colorTooPricy, colorAvailable, _availableFade);
        UpdateText(color);

        Color albedo = Color.Lerp(materialTooPricyAlbedo, materialAvailableAlbedo, _availableFade);
        Color emission = Color.Lerp(materialTooPricyEmission, materialAvailableEmission, _availableFade);
        UpdateMaterial(albedo, emission);
    }

    private void UpdateText(Color color)
    {
        text.text = string.Format(format,
                    turretName,
                    price,
                    ColorUtility.ToHtmlStringRGBA(color));
    }

    private void UpdateMaterial(Color albedoColor, Color emissionColor)
    {
        if (renders.Length == 0)
            return;

        Material material = renders.First().material;

        foreach (Renderer ren in renders.Skip(1))
        {
            ren.material = material;
        }

        material.color = albedoColor;
        material.SetColor(EmissionColor, emissionColor);
    }
}