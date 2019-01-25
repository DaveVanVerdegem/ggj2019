using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangingScript : MonoBehaviour
{

    //public Scene startScene;
    public string startSceneName;

    // Start is called before the first frame update
    void Awake()
    {
        //SceneManager.LoadScene(startScene.name, LoadSceneMode.Single);
        SceneManager.LoadScene(startSceneName, LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
