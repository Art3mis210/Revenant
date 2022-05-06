using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneInBackground : MonoBehaviour
{
    public string SceneToLoad;
    void Start()
    {
        StartCoroutine(LoadYourAsyncScene(SceneToLoad));
    }
    IEnumerator LoadYourAsyncScene(string SceneName)
    {

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneName);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                Debug.Log("SceneLoaded");
                //Wait to you press the space key to activate the Scene
                if (Input.GetKeyDown(KeyCode.Space))
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
