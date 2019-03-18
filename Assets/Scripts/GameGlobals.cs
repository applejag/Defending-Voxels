using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGlobals : MonoBehaviour
{
    public static GenericValueCounter HealthCounter => _instance.healthCounter;
    public static GenericValueCounter ScoreCounter => _instance.scoreCounter;

    public GenericValueCounter healthCounter;
    public GenericValueCounter scoreCounter;

    private static GameGlobals _instance;

    private void OnEnable()
    {
        _instance = this;
    }
}
