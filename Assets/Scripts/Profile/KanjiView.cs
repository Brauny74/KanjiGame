using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KanjiView : MonoBehaviour
{
    
    public KanjiWindow kanjiWindow;
    public ProfileKanjiButton KanjiButtonPrefab;
    public Vector3 StartingPos;
    public float StepWidth = 200;
    public float StepHeight = 200;
    public RectTransform content;
    List<Kanji> kanjis;
    List<ProfileKanjiButton> kanjiButtons;
    RectTransform rectTransform;


    int rowPos = 0;
    int linePos = -1;
    void Awake(){
        rectTransform = GetComponent<RectTransform>();
        ProfileKanjiButton tempButton;
        kanjis = GameController.instance.GetKanjis();
        for(int i = 0; i < kanjis.Count; i++){            
            if(kanjis[i].IsLearnt){
                tempButton = Instantiate(KanjiButtonPrefab, new Vector3(0, 0, 0), rectTransform.rotation, content);
                tempButton.Init(kanjiWindow, kanjis[i]);
                if(linePos >= 4){                    
                    linePos = 0;
                    rowPos++;
                }else{
                    linePos++;
                }
                tempButton.rectTransform.anchoredPosition = new Vector3(StartingPos.x + StepWidth * linePos, StartingPos.y - StepHeight * rowPos, 0);                
            }
        }
    }
}