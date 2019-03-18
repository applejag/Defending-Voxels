using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    private BuyTurret[] _turrets;

    private void OnEnable()
    {
        _turrets = GetComponentsInChildren<BuyTurret>();
    }

    public void OnCashChanged(int cash)
    {
        foreach (BuyTurret turret in _turrets)
        {
            turret.UpdateAvailable(cash);
        }
    }
}
