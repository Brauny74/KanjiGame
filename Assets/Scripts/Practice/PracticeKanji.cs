using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PracticeKanji : MonoBehaviour
{
    public RectTransform rectTransform;
    public TextMeshProUGUI Symbol;
    private Kanji KanjiData;

    //Variables needed to drag objects around
    private float deltaX, deltaY;
    public bool AtPlace = false;
    Touch touch;
    Vector2 touchPos;
    private Vector3 initialPosition;
    private Camera UICamera;

    private bool pointInRect(RectTransform r, Vector2 p){
        return RectTransformUtility.RectangleContainsScreenPoint(r, p, UICamera);
    }

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

    private Slot GetNeededSlotPos(){        
        foreach(Slot slot in PracticeManager.instance.PracticeSlots){
            if((Mathf.Abs(rectTransform.position.x - slot.rectT.position.x)) < 0.5f && (Mathf.Abs(rectTransform.position.y - slot.rectT.position.y) < 0.5f) && slot.KanjiID == KanjiData.ID){
                return slot;
            }
        }        
        return null;
    }

    private void OnEndMove(){
        if(AtPlace)
            return;
        Slot slot = GetNeededSlotPos();
        if(slot != null){
            AtPlace = true;
            rectTransform.position = slot.rectT.position;
            PracticeManager.instance.SetSlot();
        }else{
            rectTransform.position = initialPosition;
        }
    }


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();    
        UICamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();        
    }

    public void SetInfo(Kanji newData){
        initialPosition = rectTransform.position;
        KanjiData = newData;
        Symbol.text = KanjiData.Symbol;
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

    public void Kill(){
        GameObject.Destroy(gameObject);
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
