using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagsMapElement : MonoBehaviour
{
    [SerializeField]
    PlayerLocation me_ThisLocation;

    public void DoUpdateElement(PlayerLocation ae_CurrentLocation)
    {
        GetComponent<Image>().enabled = (ae_CurrentLocation == me_ThisLocation);
    }
}
