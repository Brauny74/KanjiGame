using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PracticeManager : MonoBehaviour
{
    public static PracticeManager instance;
    [Header("Prefabs")]
    public PracticeKanji PrefabKanji;
    public Slot PrefabSlot;  
    [Header("GUI")]
    public RectTransform SlotsPart;
    public RectTransform KanjisPart; 
    public TextMeshProUGUI Reading;
    public TextMeshProUGUI Meaning;
    public Animator WordPicture; 
    public TextMeshProUGUI ScoreText;

    [Header("Slot Positions")]
    public float SlotY = 0.0f;
    public float KanjiY = 0.0f;

    private int CurrentWord = 0;
    private List<MeiShi> words;
    private int Score;

    private List<PracticeKanji> PracticeKanjis;
    public List<Slot> PracticeSlots;
    private int SlotsAtTheirPlace;

    private enum Mode {None, Furigana, Picture};
    private Mode CurrentMode;

    private enum Stage{ None, PreGuess, Guessed };
    private Stage CurrentStage;

    List<Kanji> ShuffleKanjiList(List<Kanji> L){
        int c = L.Count;
        int lt = L.Count - 1;
        for(int j = 0; j < lt; ++j){
            int r = UnityEngine.Random.Range(j, c);
            Kanji tmp = L[j];
            L[j] = L[r];
            L[r] = tmp;
        }
        return L;
    }

    List<MeiShi> ShuffleMeishiList(List<MeiShi> L){
        int c = L.Count;
        int lt = L.Count - 1;
        for(int j = 0; j < lt; ++j){
            int r = UnityEngine.Random.Range(j, c);
            MeiShi tmp = L[j];
            L[j] = L[r];
            L[r] = tmp;
        }
        return L;
    }

    void GetWords(){      
        words = new List<MeiShi>();  
        List<MeiShi> wordsT = ShuffleMeishiList(GameController.instance.GetMeiShis());
        if(wordsT.Count > GameController.instance.AmountOFWordsInPractice){
            for(int i = 0; i < GameController.instance.AmountOFWordsInPractice; i++){
                words.Add(wordsT[i]);
            }
        }else{
            words = wordsT;
        }
    }

    void SetScore(int N){
        Score = N;
        ScoreText.text = Score.ToString();
    }

    void AddScore(int D){
        Score += D;
        ScoreText.text = Score.ToString();
    }

    void InitMainInfo(){
        GetWords();
        CurrentWord = 0;
        SetScore(GameController.instance.StartingTemporaryScore);
        SetMainInfo();
        InvokeRepeating("RepeatRemovingScore", 0.0f, GameController.instance.TimeStepToLosePoints);
    }

    void RepeatRemovingScore(){
        if(Score > 0 && CurrentStage == Stage.PreGuess){
            AddScore(GameController.instance.QuantityStepToLosePoints);
            if(Score == 0){
                GameController.instance.ChangeWordWeight(words[CurrentWord].ID, 1);
            }
        }
    }

    void SetMainInfo(){        
        WordPicture.SetBool(words[CurrentWord].ID, false);            
        CurrentWord++;
        if(CurrentWord < words.Count){
            CurrentStage = Stage.PreGuess;
            if(PracticeKanjis != null){
                for(int i = 0; i < PracticeKanjis.Count; i++){
                    PracticeKanjis[i].Kill();
                }
                PracticeKanjis.Clear();
            }
            if(PracticeSlots != null){
                for(int i = 0; i < PracticeSlots.Count; i++){
                    Destroy(PracticeSlots[i].gameObject);
                }
                PracticeSlots.Clear();
            }
            PracticeKanjis = new List<PracticeKanji>();
            PracticeSlots = new List<Slot>();
            SlotsAtTheirPlace = 0;

            float R = Random.Range(0.0f, 1.0f);
            Debug.Log(R);
            if(R < GameController.instance.ChanceOfFuriganaChallenge){
                CurrentMode = Mode.Furigana;
            }else{
                CurrentMode = Mode.Picture;
            }

            Reading.text = words[CurrentWord].Reading;
            Meaning.text = words[CurrentWord].Meaning;
            float StartingPosition = SlotsPart.rect.width / (words[CurrentWord].Syllables.Count * 2);
            float StepWidth = SlotsPart.rect.width / words[CurrentWord].Syllables.Count; 
            if(CurrentMode == Mode.Picture){
                WordPicture.gameObject.SetActive(true);
                WordPicture.SetBool(words[CurrentWord].ID, true);
                Reading.gameObject.SetActive(false);
            }else{
                WordPicture.gameObject.SetActive(false);
                Reading.gameObject.SetActive(true);
            }

            List<Kanji> MixKanjis = new List<Kanji>();
            Slot PS;
            for(int i = 0; i < words[CurrentWord].KanjiIDs.Count; i++){                
                Kanji TK = GameController.instance.GetKanji(words[CurrentWord].KanjiIDs[i]);
                MixKanjis.Add(TK);
                PS = Instantiate(PrefabSlot, new Vector3(0, 0, 100), SlotsPart.rotation, SlotsPart);
                PS.rectT.anchoredPosition = new Vector3(StartingPosition + StepWidth * i, SlotY, 0.0f);
                PS.SetInfo(words[CurrentWord].Syllables[i], TK.ID);
                PS.Reading.gameObject.SetActive(false);
                PracticeSlots.Add(PS);
            }

            StartingPosition = KanjisPart.rect.width / (GameController.instance.AllKanjisForPractice * 2);
            StepWidth = KanjisPart.rect.width / GameController.instance.AllKanjisForPractice;                         
            for(int i = 0; i < GameController.instance.AllKanjisForPractice - words[CurrentWord].KanjiIDs.Count ; i++){                
                MixKanjis.Add(GameController.instance.GetRandomKanji());                
            }
            MixKanjis = ShuffleKanjiList(MixKanjis);
            int k = 0; // this is kanji's position per line
            float StepHeight = 0.0f;
            for(int i = 0; i < MixKanjis.Count; i++){                
                PracticeKanji PK = Instantiate(PrefabKanji, new Vector3(0, 0, 100), KanjisPart.rotation, KanjisPart);
                if(StartingPosition + StepWidth * k >= KanjisPart.rect.width){                    
                    k = 0;                    
                    StepHeight += PK.rectTransform.rect.height + 5.0f;
                }           
                PK.rectTransform.anchoredPosition = new Vector3(StartingPosition + StepWidth * k, KanjiY - StepHeight, 0.0f);
                PK.SetInfo(MixKanjis[i]);
                PracticeKanjis.Add(PK);
                k++;
            }

        }else{
            GameController.instance.MoveToScene("MainMenu");
        }
    }

    void Awake(){
        if(instance == null)
            instance = this;
        else
            GameObject.Destroy(this);
    }

    void Start()
    {
        InitMainInfo();
    }

    void Update()
    {
        if(Application.platform == RuntimePlatform.Android){
            if(Input.GetTouch(0).phase == TouchPhase.Ended){
                if(CurrentStage == Stage.Guessed){
                    SetMainInfo();
                }
            }
        }
        if(Application.platform == RuntimePlatform.WindowsEditor){
            if(Input.GetMouseButtonDown(0)){
                if(CurrentStage == Stage.Guessed){
                    SetMainInfo();
                }
            }
        }
    }

    public void Back(){
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void SetSlot(){
        SlotsAtTheirPlace++;
        if(SlotsAtTheirPlace == PracticeSlots.Count){
            AddScore(GameController.instance.ScoreToAdd);
            CurrentStage = Stage.Guessed;
            WordPicture.gameObject.SetActive(true);
            WordPicture.SetBool(words[CurrentWord].ID, true);
            Reading.gameObject.SetActive(true);
        }
    }

    
    
}