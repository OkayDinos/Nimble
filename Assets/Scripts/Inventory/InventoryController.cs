// This script belongs to Okay Dinos, used for the Nimble Project.
// This script controlls inventory UI functions.
// Author(s): Morgan Finney.
// Date created: Mar 2022.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

enum InventoryOption { None, Wheel, Kist, Journal }
enum ActiveWheel { Kist, Journal, Close };

public class InventoryController : MonoBehaviour
{
    public static InventoryController mc_Instance;

    InventoryOption me_CurrentOption = InventoryOption.None;
    [SerializeField]
    GameObject mg_WheelUI, mg_KistUI, mg_JournalUI;

    [SerializeField]
    List<ItemData> mr_AllGameItems;
    [SerializeField]
    List<InventorySlot> mc_AllInventorySlots;

    [SerializeField]
    Image mc_KistFill, mc_JournalFill, mc_CloseFill;

    [SerializeField]
    Color m4_ColorNormal, m4_ColorActive;

    [SerializeField]
    WheelSprite ma_Kist, ma_Journal, ma_Close;

    [SerializeField]
    Image mc_KistIcon, mc_JornalIcon, mc_CloseIcon;

    ActiveWheel me_ActiveWheel;

    [SerializeField]
    TextMeshProUGUI mc_ItemNAme, mc_ItemDescription;
    [SerializeField]
    Image mc_OldPhoto;

    [SerializeField]
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    void Awake()
    {
        if (mc_Instance != null)
            Destroy(mc_Instance);
        mc_Instance = this;
    }

    private void Start()
    {

            //Fetch the Raycaster from the GameObject (the Canvas)
            //m_Raycaster = GetComponent<GraphicRaycaster>();
            //Fetch the Event System from the Scene
            m_EventSystem = GetComponent<EventSystem>();

        ItemData lr_RND = mr_AllGameItems[UnityEngine.Random.Range(0, mr_AllGameItems.Count)];

        mc_ItemNAme.text = lr_RND.ItemName;
        mc_ItemDescription.text = lr_RND.ItemDescription;
        mc_OldPhoto.sprite = lr_RND.HistoricalImage;

    }

    void SetLayer()
    {
        switch (me_CurrentOption)
        {
            case InventoryOption.None:
                mg_WheelUI.SetActive(false);
                mg_KistUI.SetActive(false);
                mg_JournalUI.SetActive(false);
                GameManager.SetCurrentGameState(GameState.PlayerNormal);
                break;
            case InventoryOption.Wheel:
                mg_WheelUI.SetActive(true);
                mg_KistUI.SetActive(false);
                mg_JournalUI.SetActive(false);
                GameManager.SetCurrentGameState(GameState.InventoryOpen);
                break;
            case InventoryOption.Kist:
                mg_WheelUI.SetActive(false);
                mg_KistUI.SetActive(true);
                mg_JournalUI.SetActive(false);
                GameManager.SetCurrentGameState(GameState.InventoryOpen);
                break;
            case InventoryOption.Journal:
                mg_WheelUI.SetActive(false);
                mg_KistUI.SetActive(false);
                mg_JournalUI.SetActive(true);
                mg_JournalUI.SendMessage("DoUpdate");
                GameManager.SetCurrentGameState(GameState.InventoryOpen);
                break;
            default:
                break;
        }
    }

    void OnOpenInventory()
    {
        if(GameManager.GetCurrentGameState() == GameState.PlayerNormal)
            DoOpenInventory();
        else if(GameManager.GetCurrentGameState() == GameState.InventoryOpen)
        {
            me_CurrentOption = InventoryOption.None;
            SetLayer();
        }
    }

    void OnOpenMap()
    {
        if (GameManager.GetCurrentGameState() == GameState.PlayerNormal)
        {
            SetJournal();
            JournalController.mc_JournalControllerInstance.SendMessage("SetPage", 1);
        }
        else if (GameManager.GetCurrentGameState() == GameState.InventoryOpen)
        {
            me_CurrentOption = InventoryOption.None;
            SetLayer();
        }
    }

    void DoOpenInventory()
    {
        if (me_CurrentOption == InventoryOption.None)
        {
            me_CurrentOption = InventoryOption.Wheel;
        }
        else
        {
            me_CurrentOption = InventoryOption.None;
        }

        SetLayer();
    }

    void SetKist()
    {
        me_CurrentOption = InventoryOption.Kist;
        SetLayer();
        SortInventory();
    }

    void SetJournal()
    {
        me_CurrentOption = InventoryOption.Journal;
        SetLayer();
    }

    void SortInventory()
    {
        List<ItemData> lr_ItemsHas = new List<ItemData>();
        List<ItemData> lr_ItemsNotHas = new List<ItemData>();

        foreach (ItemData ar_Item in mr_AllGameItems)
        {
            if (ItemDatabase.CheckHas(ar_Item))
            {
                lr_ItemsHas.Add(ar_Item);
            }
            else
            {
                lr_ItemsNotHas.Add(ar_Item);
            }
        }

        lr_ItemsHas.AddRange(lr_ItemsNotHas);

        for (int i = 0; i < lr_ItemsHas.Count; i++)
        {
            mc_AllInventorySlots[i].SetIcon(lr_ItemsHas[i].ItemIcon);
            mc_AllInventorySlots[i].SetAmount(ItemDatabase.CheckAmount(lr_ItemsHas[i]));
            mc_AllInventorySlots[i].SetItem(lr_ItemsHas[i]);
        }
    }

    void OnClick()
    {
        switch (me_CurrentOption)
        {
            case InventoryOption.None:
                break;
            case InventoryOption.Wheel:

                switch (me_ActiveWheel)
                {
                    case ActiveWheel.Kist:
                        SetKist();
                        break;
                    case ActiveWheel.Journal:
                        SetJournal();
                        break;
                    case ActiveWheel.Close:
                        DoOpenInventory();
                        break;
                    default:
                        break;
                }

                break;
            case InventoryOption.Kist:
                break;
            case InventoryOption.Journal:
                break;
            default:
                break;
        }
    }

    void OnAim(InputValue ar_InputValue)
    {
        Vector2 l2_Delta = ar_InputValue.Get<Vector2>();

        switch (me_CurrentOption)
        {
            case InventoryOption.None:
                break;
            case InventoryOption.Wheel:
                DoWheelUI(l2_Delta);
                break;
            case InventoryOption.Kist:
                DoKistAim(l2_Delta);
                break;
            case InventoryOption.Journal:
                break;
            default:
                break;
        }
    }

    void OnCloseInventory()
    {
        if (GameManager.GetCurrentGameState() == GameState.InventoryOpen)
        {
            me_CurrentOption = InventoryOption.None;
            SetLayer();
        }
    }

    void DoKistAim(Vector2 a2_InputVector)
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = a2_InputVector;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<InventorySlot>())
            {
                Debug.Log("Hit " + result.gameObject.GetComponent<InventorySlot>().GetID());
                ItemData lr_ItemData = result.gameObject.GetComponent<InventorySlot>().GetItemData();

                if (lr_ItemData)
                {

                    mc_ItemNAme.text = lr_ItemData.ItemName;
                    mc_ItemDescription.text = lr_ItemData.ItemDescription;
                    mc_OldPhoto.sprite = lr_ItemData.HistoricalImage;
                }
            }
        }
    }


    void DoWheelUI(Vector2 a2_InputVector)
    {
        if ((a2_InputVector.x >= -0.01 && a2_InputVector.x <= 0.01 && a2_InputVector.y >= -0.01 && a2_InputVector.y <= 0.01))
        {
            return;
        }


        //*WorkAround
        if (!(a2_InputVector.x >= -1 && a2_InputVector.x <= 1 && a2_InputVector.y >= -1 && a2_InputVector.y <= 1))
        {
            a2_InputVector = new Vector2(
            (float)(a2_InputVector.x - (float)(Screen.width / 2f)),
            (float)(a2_InputVector.y - (float)(Screen.height / 2f))
            );
        }



        float lf_Angle = Mathf.Atan2(a2_InputVector.x, a2_InputVector.y) * Mathf.Rad2Deg;

        if (lf_Angle < 0f)
            lf_Angle += 360;

        if (lf_Angle >= 0f && lf_Angle < 120f)
        {
            me_ActiveWheel = ActiveWheel.Journal;

            mc_JournalFill.color = m4_ColorActive;
            mc_CloseFill.color = m4_ColorNormal;
            mc_KistFill.color = m4_ColorNormal;

            mc_JornalIcon.sprite = ma_Journal.active;
            mc_CloseIcon.sprite = ma_Close.normal;
            mc_KistIcon.sprite = ma_Kist.normal;
        }
        else if (lf_Angle >= 120f && lf_Angle < 240f)
        {
            me_ActiveWheel = ActiveWheel.Close;

            mc_JournalFill.color = m4_ColorNormal;
            mc_CloseFill.color = m4_ColorActive;
            mc_KistFill.color = m4_ColorNormal;

            mc_JornalIcon.sprite = ma_Journal.normal;
            mc_CloseIcon.sprite = ma_Close.active;
            mc_KistIcon.sprite = ma_Kist.normal;
        }
        else if (lf_Angle >= 240f && lf_Angle < 360f)
        {
            me_ActiveWheel = ActiveWheel.Kist;

            mc_JournalFill.color = m4_ColorNormal;
            mc_CloseFill.color = m4_ColorNormal;
            mc_KistFill.color = m4_ColorActive;

            mc_JornalIcon.sprite = ma_Journal.normal;
            mc_CloseIcon.sprite = ma_Close.normal;
            mc_KistIcon.sprite = ma_Kist.active;
        }
    }
}

[Serializable]
public class WheelSprite
{
    public Sprite normal;
    public Sprite active;
}