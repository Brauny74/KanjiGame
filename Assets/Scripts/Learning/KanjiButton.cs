using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KanjiButton : MonoBehaviour
{

    public Kanji kanjiData;
    public TextMeshProUGUI KanjiText;
    public TextMeshProUGUI MeaningText;
    public Image Mark;

    public void SetData(Kanji TKanjiData){
        kanjiData = TKanjiData;
        
        KanjiText.text = kanjiData.Symbol;
        MeaningText.text = kanjiData.ShortDescription;
    }

    public void SetMark(Sprite NewMark){
        Mark.sprite = NewMark;
    }
}
