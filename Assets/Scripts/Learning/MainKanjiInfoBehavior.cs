using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainKanjiInfoBehavior : MonoBehaviour
{
    public Kanji kanjiData;
    public TextMeshProUGUI KanjiText;
    public TextMeshProUGUI OnYomi;
    public TextMeshProUGUI KunYomi;
    public TextMeshProUGUI Description;

    public void SetData(Kanji TKanjiData){
        kanjiData = TKanjiData;
        UpdateInfo();
    }

    void UpdateInfo(){
        KanjiText.text = kanjiData.Symbol;
        OnYomi.text = kanjiData.OnYomi;
        KunYomi.text = kanjiData.KunYomi;
        Description.text = kanjiData.FullDescription;
    }
}
