using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nimble/Trade")]
public class TradeData : ScriptableObject
{
    public ItemData TradingInItem;
    public int TradingInAmount;
    public ItemData TradingForItem;
    public int TradingForAmount;
}

