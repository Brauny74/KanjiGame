using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KanjiChoice : MonoBehaviour
{
    public TextMeshProUGUI KanjiText;
    public Kanji KanjiData;

    private Camera UICamera;

    private Vector2 initialPosition;
    public RectTransform rectTransform;
    
    private float deltaX, deltaY;
    public bool AtPlace = false;   

    private Slot slot;

    private void Start(){
        UICamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        initialPosition = rectTransform.anchoredPosition;                
        slot = LearningMeishiPart.instance.FindSlotById(KanjiData.ID);
    }

    public void SetInfo(Kanji k){
        KanjiData = k;
        KanjiText.text = KanjiData.Symbol;
    }

    Touch touch;
    Vector2 touchPos;
    private void OnPress(){
        if(AtPlace)
            return;
        Vector3 mousePos = Input.mousePosition;
        if (Application.platform == RuntimePlatform.WindowsEditor){
            deltaX = mousePos.x - rectTransform.anchoredPosition.x;
            deltaY = mousePos.y - rectTransform.anchoredPosition.y;
        }
        if(Application.platform == RuntimePlatform.Android){
            deltaX = touchPos.x - rectTransform.anchoredPosition.x;
            deltaY = touchPos.y - rectTransform.anchoredPosition.y;
        }
    }

    private bool pointInRect(RectTransform r, Vector2 p){
        return RectTransformUtility.RectangleContainsScreenPoint(r, p, UICamera);
    }

    private void OnMove(){
        if(AtPlace)
            return;
        Vector3 mousePos = Input.mousePosition;
        if (Application.platform == RuntimePlatform.WindowsEditor){
            rectTransform.anchoredPosition = new Vector2(mousePos.x - deltaX, mousePos.y - deltaY);
        }
        if(Application.platform == RuntimePlatform.Android){
            if(pointInRect(rectTransform, touchPos)){
                rectTransform.anchoredPosition = new Vector2(touchPos.x - deltaX, touchPos.y - deltaY);
            }
        }
    }

    private bool IsCloseToNeededSlot(){
        if((Mathf.Abs(rectTransform.position.x - slot.rectT.position.x)) < 0.5f && (Mathf.Abs(rectTransform.position.y - slot.rectT.position.y) < 0.5f)){
            return true;
        }
        return false;
    }

    private void OnEndMove(){
        if(AtPlace)
            return;
        if(IsCloseToNeededSlot()){
            AtPlace = true;
            rectTransform.position = slot.rectT.position;
            LearningMeishiPart.instance.SlotSet();
        }else{
            rectTransform.anchoredPosition = initialPosition;
        }
    }

    private void Update(){
        if(Application.platform == RuntimePlatform.Android){
            touch = Input.GetTouch(0);
            touchPos = touch.position;
            switch(touch.phase){
                case TouchPhase.Began:
                    OnPress();
                    break;
                case TouchPhase.Moved:
                    OnMove();
                    break;
                case TouchPhase.Ended:
                    OnEndMove();
                    break;
            }            
        }
    }

    void OnMouseDown(){
        OnPress();
    }
    void OnMouseDrag(){
        OnMove();
    }
    void OnMouseUp(){
        OnEndMove();
    }
}
