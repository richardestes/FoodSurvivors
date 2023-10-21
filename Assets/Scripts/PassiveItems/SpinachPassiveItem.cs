using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinachPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        playerStats.currentMight *= 1 + PassiveItemData.Multiplier / 100f;
    }
}
