using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfileMeishiButton : MonoBehaviour
{
    private MeishiWindow window;

    public RectTransform rectTransform;
    public TextMeshProUGUI Word;
    public TextMeshProUGUI Desc;
    private MeiShi MeishiData;
    

    void Awake(){
        rectTransform = GetComponent<RectTransform>();              
    }

    public void Init(MeishiWindow newWindow, MeiShi data){
        window = newWindow;
        MeishiData = data;
        Word.text = MeishiData.Word;
        Desc.text = MeishiData.Meaning;
    }

    public void ShowWindow(){
        window.gameObject.SetActive(true);
        window.SetMainInfo(MeishiData);
    }
}
