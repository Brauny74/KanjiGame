using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slot : MonoBehaviour
{
    public TextMeshProUGUI Reading;

    public RectTransform rectT;

    public string KanjiID;

    void Start(){
        rectT = GetComponent<RectTransform>();
    }

    public void SetInfo(string Syllable, string TID){
        KanjiID = TID;
        Reading.text = Syllable;
    }

}
