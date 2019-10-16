using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeishiView : MonoBehaviour
{
    public ProfileMeishiButton MeishiButtonPrefab;
    public MeishiWindow meishiWindow;
    RectTransform rectTransform;
    public RectTransform content;
    public Vector3 StartingPos;
    public float StepHeight = 220;
    float CurrentStepHeight = 0;
    List<MeiShi> meishis;
    List<ProfileMeishiButton> meishiButtons;


    void Awake(){
        rectTransform = GetComponent<RectTransform>();
        ProfileMeishiButton tempButton;
        meishis = GameController.instance.GetMeiShis();
        for(int i = 0; i < meishis.Count; i++){
            if(meishis[i].IsLearnt){
                tempButton = Instantiate(MeishiButtonPrefab, new Vector3(0, 0, 0), rectTransform.rotation, content);
                tempButton.rectTransform.anchoredPosition = new Vector3(StartingPos.x, StartingPos.y - CurrentStepHeight, 0);
                CurrentStepHeight += StepHeight;
                tempButton.Init(meishiWindow, meishis[i]);            
            }
        }
        content.sizeDelta = new Vector2(content.sizeDelta.x, CurrentStepHeight);
    }
}
