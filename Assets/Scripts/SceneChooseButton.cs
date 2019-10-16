using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChooseButton : MonoBehaviour
{
    public void ChangeScene(string SceneName){
        GameController.instance.MoveToScene(SceneName);
    }
}
