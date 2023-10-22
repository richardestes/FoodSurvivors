using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsPassiveItem : PassiveItem
{
    // Multiplier is converted from % to a decimal fraction by dividing by 100.
    // This is added to 1 to obtain total multiplier to be applied.
    // Ex. Multiplier of 50 
    // 1 + 50/100 = 1.5
    protected override void ApplyModifier()
    {
        playerStats.CurrentMoveSpeed *= 1 + PassiveItemData.Multiplier / 100f;
    }
}
