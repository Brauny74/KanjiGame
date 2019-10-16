using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LearningManager : MonoBehaviour
{   
    
    public LearningKanjiPart KanjiPart;
    public LearningMeishiPart MeishiPart;


    public void Awake(){        
        if(GameController.instance.AmountOfKanjiToday() > 0){
            MeishiPart.gameObject.SetActive(false);
            KanjiPart.gameObject.SetActive(true);
        }else{
            MeishiPart.gameObject.SetActive(true);
            KanjiPart.gameObject.SetActive(false);
        }
    }

    public void Back(){
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void Learn(){
        if(KanjiPart.LearnKanji()){
            KanjiPart.gameObject.SetActive(false);
            MeishiPart.gameObject.SetActive(true);
        }
    }

}
