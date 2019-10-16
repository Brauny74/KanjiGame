using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningKanjiPart : MonoBehaviour
{
    public List<KanjiButton> KanjiButtonList;

    public MainKanjiInfoBehavior MainKanjiInfo;

    [Header("Sprites for marks")]
    public Sprite LearntMark;
    public Sprite NotYetLearntMark;

    private int CurrentKanji; 

    public void Awake(){
        for(int i = 0; i < KanjiButtonList.Count; i++){            
            Kanji iKanji = GameController.instance.GetTodayKanji(i);  
            if(iKanji == null){
                KanjiButtonList[i].gameObject.SetActive(false);
            }else{
                KanjiButtonList[i].SetData(iKanji);    
            }            
        }
        ChangeMainInfo(0);
    }

    public void ChangeMainInfo(int KanjiNumber){
        CurrentKanji = KanjiNumber;
        MainKanjiInfo.SetData(KanjiButtonList[KanjiNumber].kanjiData);
    }

    public void OnEnable(){
        foreach(KanjiButton kb in KanjiButtonList){
            if(kb.kanjiData.IsLearnt)
                kb.SetMark(LearntMark);
            else
                kb.SetMark(NotYetLearntMark);
        }
    }

     public bool LearnKanji(){
        KanjiButtonList[CurrentKanji].kanjiData.IsLearnt = true;
        GameController.instance.GameData.SetKanji(KanjiButtonList[CurrentKanji].kanjiData);
        GameController.instance.SaveGameData();
        KanjiButtonList[CurrentKanji].SetMark(LearntMark);

        bool AllKanjisLearnt = true;
        for(int i = 0; i < KanjiButtonList.Count; i++){
            if(KanjiButtonList[i].gameObject.activeSelf && !KanjiButtonList[i].kanjiData.IsLearnt){
                AllKanjisLearnt = false;
            }
        }

        if(AllKanjisLearnt){
            return true;
        }
        return false;
    }

}
