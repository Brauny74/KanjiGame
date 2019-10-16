using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KanjiWindow : MonoBehaviour
{
    [Header("Links to Window Elements")]
    public TextMeshProUGUI Symbol;
    public TextMeshProUGUI OnYomi;
    public TextMeshProUGUI KunYomi;
    public TextMeshProUGUI ShortDesc;
    public TextMeshProUGUI LongDesc;

    private Kanji KanjiData;

    public void SetMainInfo(Kanji newKanjiData){
        KanjiData = newKanjiData;
        Symbol.text = KanjiData.Symbol;
        OnYomi.text = KanjiData.OnYomi;
        KunYomi.text = KanjiData.KunYomi;
        ShortDesc.text = KanjiData.ShortDescription;
        LongDesc.text = KanjiData.FullDescription;
    }

    public void HideWindow(){
        gameObject.SetActive(false);
    }
}