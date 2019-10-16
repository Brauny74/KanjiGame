using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowControlButtonBehavior : MonoBehaviour
{

    public bool TurnOnWindow = false;
    public GameObject Window;

    public void ToggleWindow(){
        Window.SetActive(TurnOnWindow);
    }
}
