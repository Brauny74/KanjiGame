using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeishiWindow : MonoBehaviour
{
    [Header("Links to Window Elements")]
    public TextMeshProUGUI Word;
    public TextMeshProUGUI Reading;
    public TextMeshProUGUI Meaning;
    public Animator Picture;

    private MeiShi MeishiData;

    public void SetMainInfo(MeiShi newMeishiData){
        MeishiData = newMeishiData;
        Word.text = MeishiData.Word;
        Reading.text = MeishiData.Reading;
        Meaning.text = MeishiData.Meaning;
        Picture.SetBool(MeishiData.ID, true);
    }

    public void HideWindow(){
        Picture.SetBool(MeishiData.ID, false);
        gameObject.SetActive(false);
    }
}
