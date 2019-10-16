using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfileKanjiButton : MonoBehaviour
{
    public Kanji KanjiData;
    private KanjiWindow window;
    public RectTransform rectTransform;
    public TextMeshProUGUI Symbol;
    public TextMeshProUGUI Meaning;

    void Awake(){
        rectTransform = GetComponent<RectTransform>();              
    }

    public void Init(KanjiWindow newWindow, Kanji data){
        window = newWindow;
        KanjiData = data;
        Symbol.text = KanjiData.Symbol;
        Meaning.text = KanjiData.ShortDescription;
    }

    public void ShowWindow(){
        window.gameObject.SetActive(true);
        window.SetMainInfo(KanjiData);        
    }
}
