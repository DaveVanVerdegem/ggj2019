using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class LoadSceneAsync : MonoBehaviour
{
    [Tooltip("Scene name or path")]
    public string scene = "";

    private void OnMouseDown()
    {
        if (!string.IsNullOrEmpty(scene))
            StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
