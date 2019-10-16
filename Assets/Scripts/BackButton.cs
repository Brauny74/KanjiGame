using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void Start(){
        Debug.Log(Application.persistentDataPath);
    }

    public void Back(){
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
