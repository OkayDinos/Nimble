using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradeController : MonoBehaviour
{
    TradeData mr_Trade01, mr_Trade02, mr_Trade03;
    [SerializeField]
    Image mc_Trade01Give, mc_Trade01Get, mc_Trade02Give, mc_Trade02Get, mc_Trade03Give, mc_Trade03Get;
    [SerializeField]
    TextMeshProUGUI mc_Trade01GiveText, mc_Trade01GetText, mc_Trade02GiveText, mc_Trade02GetText, mc_Trade03GiveText, mc_Trade03GetText;
    [SerializeField]
    Button mc_Trade01Button, mc_Trade02Button, mc_Trade03Button;
    Trade mc_Trade;

    public void SetUp(TradeData ar_Trade01, TradeData ar_Trade02, TradeData ar_Trade03, Trade ac_Trade)
    {
        mc_Trade01Give.sprite = ar_Trade01.TradingInItem.ItemIcon;
        mc_Trade01Get.sprite = ar_Trade01.TradingForItem.ItemIcon;

        mc_Trade02Give.sprite = ar_Trade02.TradingInItem.ItemIcon;
        mc_Trade02Get.sprite = ar_Trade02.TradingForItem.ItemIcon;

        mc_Trade03Give.sprite = ar_Trade03.TradingInItem.ItemIcon;
        mc_Trade03Get.sprite = ar_Trade03.TradingForItem.ItemIcon;

        mc_Trade01GiveText.text = "" + ar_Trade01.TradingInAmount;
        mc_Trade01GetText.text = "" + ar_Trade01.TradingForAmount;

        mc_Trade02GiveText.text = "" + ar_Trade02.TradingInAmount;
        mc_Trade02GetText.text = "" + ar_Trade02.TradingForAmount;

        mc_Trade03GiveText.text = "" + ar_Trade03.TradingInAmount;
        mc_Trade03GetText.text = "" + ar_Trade03.TradingForAmount;

        mc_Trade01Button.interactable = (ItemDatabase.CheckAmount(ar_Trade01.TradingInItem) >= ar_Trade01.TradingInAmount);
        mc_Trade02Button.interactable = (ItemDatabase.CheckAmount(ar_Trade02.TradingInItem) >= ar_Trade02.TradingInAmount);
        mc_Trade03Button.interactable = (ItemDatabase.CheckAmount(ar_Trade03.TradingInItem) >= ar_Trade03.TradingInAmount);

        mr_Trade01 = ar_Trade01;
        mr_Trade02 = ar_Trade02;
        mr_Trade03 = ar_Trade03;

        mc_Trade = ac_Trade;

        this.gameObject.SetActive(true);
    }

    public void ExitTrade()
    {
        mc_Trade.ExitTrade();
        this.gameObject.SetActive(false);
    }

    public void DoTrade(int ai_Trade)
    {
        if(ai_Trade == 1)
        {
            if(ItemDatabase.CheckAndUse(mr_Trade01.TradingInItem, mr_Trade01.TradingInAmount))
            {
                ItemDatabase.AddToInv(mr_Trade01.TradingForItem, mr_Trade01.TradingForAmount);
            }
        }
        else if (ai_Trade == 2)
        {
            if (ItemDatabase.CheckAndUse(mr_Trade02.TradingInItem, mr_Trade02.TradingInAmount))
            {
                ItemDatabase.AddToInv(mr_Trade02.TradingForItem, mr_Trade02.TradingForAmount);
            }
        }
        else if (ai_Trade == 3)
        {
            if (ItemDatabase.CheckAndUse(mr_Trade03.TradingInItem, mr_Trade03.TradingInAmount))
            {
                ItemDatabase.AddToInv(mr_Trade03.TradingForItem, mr_Trade03.TradingForAmount);
            }
        }

        this.gameObject.SetActive(false);
        mc_Trade.EndTrade();
    }
}
