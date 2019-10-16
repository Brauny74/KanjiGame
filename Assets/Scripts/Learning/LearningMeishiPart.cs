using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LearningMeishiPart : MonoBehaviour
{
    public static LearningMeishiPart instance;
    public MeiShi MeishiData;
    public Animator ImageAnimator;
    public RectTransform HintPart;
    public RectTransform KanjiPart;
    public Slot slotPrefab;
    public KanjiChoice kanjiPrefab;
    public List<KanjiChoice> KanjiChoices;
    public List<Slot> Slots;

    public TextMeshProUGUI WordText;
    public Image WordImage;

    int currentWord = 0;

    private string previousId = "0";

    private bool WordGuessed = false;

    [Header("Slot Positions")]
    public float SlotY = 0.0f;
    public float KanjiY = 0.0f;
    
    public void Awake(){
        instance = this;
        SetInfo(GameController.instance.GetTodayMeishi(currentWord));
    }

    List<Kanji> ShuffleKanjiList(List<Kanji> L){
        int c = L.Count;
        int lt = L.Count - 1;
        for(int j = 0; j < lt; ++j){
            int r = Random.Range(j, c);
            Kanji tmp = L[j];
            L[j] = L[r];
            L[r] = tmp;
        }
        return L;
    }

    public void SetInfo(MeiShi newWord){        
        Debug.Log(newWord);
        //Clean the GUI for the word
        for(int i = 0; i < Slots.Count; i++){
            Destroy(Slots[i].gameObject);
        }
        Slots.Clear();
        for(int i = 0; i < KanjiChoices.Count; i++){
            Destroy(KanjiChoices[i].gameObject);
        }
        KanjiChoices.Clear();
        WordImage.gameObject.SetActive(false);
        WordText.text = "";
        //Set it for the new word
        if(MeishiData != null)
            previousId = MeishiData.ID;
        MeishiData = newWord;
        float StartingPosition = HintPart.rect.width / (MeishiData.Syllables.Count * 2);
        float StepWidth = HintPart.rect.width / MeishiData.Syllables.Count;              
        Slot TSlot;
        for(int i = 0; i < MeishiData.Syllables.Count; i++){                     
            TSlot = Instantiate(slotPrefab, Vector3.zero, HintPart.rotation, HintPart);
            TSlot.rectT.anchoredPosition = new Vector3(StartingPosition + StepWidth * i, SlotY, 0.0f);
            TSlot.SetInfo(MeishiData.Syllables[i], MeishiData.KanjiIDs[i]);
            Slots.Add(TSlot);
        }        
        KanjiChoice TKanji;
        List<Kanji> TListK = new List<Kanji>();
        foreach(string KID in MeishiData.KanjiIDs){
            TListK.Add(GameController.instance.GetKanji(KID));
        }
        TListK = ShuffleKanjiList(TListK);
        for(int i = 0; i < TListK.Count; i++){
            TKanji = Instantiate(kanjiPrefab, Vector3.zero, KanjiPart.rotation, KanjiPart);
            TKanji.rectTransform.anchoredPosition = new Vector3(StartingPosition + StepWidth * i, KanjiY, 0.0f);   
            TKanji.SetInfo(TListK[i]);
            KanjiChoices.Add(TKanji);
        }
    }

    Touch touch;
    Vector2 touchPos;
    void AllKanjisSet(){        
        WordImage.gameObject.SetActive(true);
        WordText.text = MeishiData.Meaning;
        ImageAnimator.SetBool(previousId, false);
        ImageAnimator.SetBool(MeishiData.ID, true);        
        GameController.instance.GetMeiShi(GameController.instance.GetTodayMeishi(currentWord).ID).IsLearnt = true;
        GameController.instance.SaveGameData();
        WordGuessed = true;
    }

    void Update(){
        if(Application.platform == RuntimePlatform.Android){
            if(WordGuessed){
                touch = Input.GetTouch(0);
                touchPos = touch.position;
                if(touch.phase == TouchPhase.Ended){
                    WordGuessed = false;
                    NextWord();
                }
            }
        }
        if(Application.platform == RuntimePlatform.WindowsEditor){
            if(Input.GetMouseButtonDown(0)){
                if(WordGuessed){
                    WordGuessed = false;                
                    NextWord();                       
                }   
            }
        }
    }

    public void SlotSet(){
        bool AllSet = true;
        for(int i = 0; i < KanjiChoices.Count; i++){
            if(!KanjiChoices[i].AtPlace)
                AllSet = false;
        }
        if(AllSet){
            AllKanjisSet();
        }
    }

    public void NextWord(){
        currentWord++;
        if(currentWord < GameController.instance.GetAmountOfWords()){
            SetInfo(GameController.instance.GetTodayMeishi(currentWord));
        }else{
            GameController.instance.MoveToScene("MainMenu");
        }
    }

    public Slot FindSlotById(string id){
        for(int m = 0; m < Slots.Count; m++){
            if(Slots[m].KanjiID == id){
                return Slots[m];
            }
        }
        return null;
    }
}
