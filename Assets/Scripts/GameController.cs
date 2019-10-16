using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController instance = null;

    [Header("Game Variables")]
    public float ChanceOfFuriganaChallenge = 0.5f;
    public int AmountOFWordsInPractice = 10;
    public int StartingTemporaryScore = 100;
    public int ScoreToAdd = 10;
    public float TimeStepToLosePoints = 0.5f;
    public int QuantityStepToLosePoints = -1;
    public int AllKanjisForPractice = 5;

    [Header("Assets")]
    public TextAsset DataFile;

    StreamWriter kanjiFileWriter;

    public AllData GameData;

    List<Kanji> TodayKanji;
    List<MeiShi> TodayWords;
    List<string> LearntKanjiIDs;

    void Awake(){
        if(instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);      

        GameData = new AllData();
        LoadGameData();
        LoadTodayKanji();
        LoadTodayWords();
    }

    public void ChangeWordWeight(string id, int weight){
        GameData.SetMeishiWeight(id, weight);
        SaveGameData();
    }

    public int AmountOfKanjiToday(){
        return TodayKanji.Count;
    }

    void LoadTodayKanji(){
        TodayKanji = new List<Kanji>();
        LearntKanjiIDs = new List<string>();
        for(int i = 0; i < GameData.Kanjis.Count; i++){
            if(!GameData.Kanjis[i].IsLearnt){                
                if(TodayKanji.Count < 3){
                    TodayKanji.Add(GameData.Kanjis[i]);    
                }
            }else{
                LearntKanjiIDs.Add(GameData.Kanjis[i].ID);
            }
        }
    }
  
    public List<MeiShi> GetMeiShis(){
        return GameData.Words;
    }

    public List<Kanji> GetKanjis(){
        return GameData.Kanjis;
    }

    public Kanji GetRandomKanji(){
        return GameData.Kanjis[UnityEngine.Random.Range(0, GameData.Kanjis.Count)];
    }

    bool isWordInList(List<MeiShi> L, MeiShi w){
        for(int s = 0; s < L.Count; s++){
            if(L[s].ID == w.ID){
                return true;
            }
        }
        return false;
    }

    void LoadTodayWords(){
        TodayWords = new List<MeiShi>();
        for(int i = 0; i < GameData.Words.Count; i++){
            if(!GameData.Words[i].IsLearnt){
                for(int k = 0; k < TodayKanji.Count; k++){
                    if(GameData.Words[i].isContainingKanji(TodayKanji[k].ID) && !isWordInList(TodayWords, GameData.Words[i])){
                        TodayWords.Add(GameData.Words[i]);
                        continue;
                    }
                }
                /*for(int k = 0; k < LearntKanjiIDs.Count; k++){
                    if(GameData.Words[i].isContainingKanji(LearntKanjiIDs[k])){
                        TodayWords.Add(GameData.Words[i]);
                        continue;
                    }
                }*/
            }
        }
    }

    public Kanji GetKanji(string id){
        for(int k = 0; k < GameData.Kanjis.Count; k++){
            if(GameData.Kanjis[k].ID == id){
                return GameData.Kanjis[k];
            }
        }
        return null;
    }

    public MeiShi GetMeiShi(string id){
        for(int k = 0; k < GameData.Words.Count; k++){
            if(GameData.Words[k].ID == id){
                return GameData.Words[k];
            }
        }
        return null;
    }

    public void createDummyData(){
        Kanji TempKanji = new Kanji();
        MeiShi TempMeishi = new MeiShi();
        AllData DummyData = new AllData();

        TempKanji.ID = "1";
        TempKanji.IsLearnt = false;
        TempKanji.Symbol = "火";
        TempKanji.KunYomi = "hi";
        TempKanji.OnYomi = "ka";
        TempKanji.ShortDescription = "FIRE";
        TempKanji.FullDescription = "hi - fire\n花火(hahabi) - fireworks\nka - fire, tuesday\n火事(kaji) - fire (as incident)\n火山(kazan) - volcano\n火曜日(kayoubi) - tuesday";
        DummyData.Kanjis.Add(TempKanji);

        TempKanji.ID = "2";
        TempKanji.IsLearnt = false;
        TempKanji.Symbol = "山";
        TempKanji.KunYomi = "yama";
        TempKanji.OnYomi = "san";
        TempKanji.ShortDescription = "MOUNTAIN";
        TempKanji.FullDescription = "yama, san - mountain\n火山(kazan) - volcano\n山小屋(yamagoya) - cabin in mountains\n富士山(fujisan) - Fuji Mt.";
        DummyData.Kanjis.Add(TempKanji);

        TempKanji.ID = "3";
        TempKanji.IsLearnt = false;
        TempKanji.Symbol = "花";
        TempKanji.KunYomi = "hana";
        TempKanji.OnYomi = "ka";
        TempKanji.ShortDescription = "FLOWER";
        TempKanji.FullDescription = "hana, ka - flower\n花見(hanami) - flower watching\n花束(hanataba) - bouquet\n花火(hahabi) - fireworks";
        DummyData.Kanjis.Add(TempKanji);

        TempMeishi.ID = "1";
        TempMeishi.IsLearnt = false;
        TempMeishi.CurrentWeight = 10;
        TempMeishi.Word = "花火";
        TempMeishi.Reading = "hanabi";
        TempMeishi.Meaning = "fireworks";
        TempMeishi.KanjiIDs.Add("3");
        TempMeishi.KanjiIDs.Add("1");
        DummyData.Words.Add(TempMeishi);

        TempMeishi.ID = "2";
        TempMeishi.IsLearnt = false;
        TempMeishi.CurrentWeight = 10;
        TempMeishi.Word = "火山";
        TempMeishi.Reading = "kazan";
        TempMeishi.Meaning = "volcano";
        TempMeishi.KanjiIDs.Add("1");
        TempMeishi.KanjiIDs.Add("2");
        DummyData.Words.Add(TempMeishi);

        string json = JsonUtility.ToJson(DummyData);
        SaveKanjiData(json);
    }

    void LoadGameData(){
        try 
        {
            Debug.Log(Application.persistentDataPath);
            if(File.Exists(Application.persistentDataPath + "/LearningData.json")){
                StreamReader kanjiFileReader = new StreamReader(Application.persistentDataPath + "/LearningData.json");
                string json = kanjiFileReader.ReadToEnd();
                GameData = JsonUtility.FromJson<AllData>(json);
            }else{
                string json = Resources.Load<TextAsset>("LearningData").text;
                GameData = JsonUtility.FromJson<AllData>(json);
                SaveGameData();
            }
        }
        catch(Exception e){
            Debug.LogError("Error while reading Data: " + e.Message);
        }
    }

    public void SaveKanjiData(string data){        
        kanjiFileWriter =  new StreamWriter(Application.persistentDataPath + "/LearningData.json", true);
        kanjiFileWriter.Write(data);
        kanjiFileWriter.Close();
        Debug.Log(kanjiFileWriter.ToString());
    }

    public void SaveGameData(){
        string json = JsonUtility.ToJson(GameData);
        kanjiFileWriter =  new StreamWriter(Application.persistentDataPath + "/LearningData.json", false);
        kanjiFileWriter.Write(json);
        kanjiFileWriter.Close();
    }

    public void MoveToScene(string SceneName){
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }
    
    public Kanji GetTodayKanji(int i){
        if(i >= TodayKanji.Count)
            return null;
        else
            return TodayKanji[i];
    }

    public MeiShi GetTodayMeishi(int i){
        if(i >= TodayWords.Count)
            return null;
        else
            return TodayWords[i];
    }

    public int GetAmountOfWords(){
        return TodayWords.Count;
    }

}

[Serializable]
public class Kanji
{
    public string ID;
    public string Symbol;
    public string OnYomi;
    public string KunYomi;
    public string ShortDescription;
    public string FullDescription;
    public bool IsLearnt;

    public override string ToString(){
        return ID + ":" + Symbol + " " + ShortDescription;
    }
}

[Serializable]
public class MeiShi
{
    public string ID;
    public string Word;
    public string Reading;
    public string Meaning;
    public List<string> KanjiIDs;
    public List<string> Syllables;
    public bool IsLearnt;
    public int CurrentWeight;

    public MeiShi(){
        KanjiIDs = new List<string>();
        Syllables = new List<string>();
    }

    public bool isContainingKanji(string KanjiID){
        for(int v = 0; v < KanjiIDs.Count; v++){
            if(KanjiID == KanjiIDs[v]){
                return true;
            }
        }
        return false;
    }

    public override string ToString(){
        return ID + " " + Reading + " " + Meaning;
    }
}

[Serializable]
public class AllData
{
    public List<Kanji> Kanjis;
    public List<MeiShi> Words;
    public AllData(){
        Kanjis = new List<Kanji>();
        Words = new List<MeiShi>();
    }

    public void SetKanji(Kanji NewKanji){
        for(int i = 0; i < Kanjis.Count; i++){
            if(Kanjis[i].ID == NewKanji.ID){
                Kanjis[i] = NewKanji;
            }
        }
    }

    public void SetMeishiWeight(string id, int weight){
        for(int k = 0; k < Words.Count; k++){
            if(Words[k].ID == id){
                if(Words[k].CurrentWeight + weight > 1 && Words[k].CurrentWeight + weight <= 20)
                    Words[k].CurrentWeight += weight;
            }
        }
    }
}