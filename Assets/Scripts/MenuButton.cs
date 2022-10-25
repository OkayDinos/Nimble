using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [SerializeField]
    MainMenu mc_MainMenu;

    [SerializeField]
    MenuTextMode me_Mode;

    public void OnSelect(BaseEventData eventData)
    {
        mc_MainMenu.SendMessage("SetMenuTextMode", me_Mode);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mc_MainMenu.SendMessage("SetMenuTextMode", me_Mode);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
