using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestController : MonoBehaviour
{
    public TextMeshProUGUI TestText;
    public Camera UICamera;

    void Update(){
        Vector3 mousePos = Input.mousePosition;//UICamera.ScreenToWorldPoint(Input.mousePosition);
        TestText.text = mousePos.x.ToString() + ":" + mousePos.y.ToString();
    }
}
