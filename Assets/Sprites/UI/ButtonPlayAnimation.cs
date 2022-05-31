using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonPlayAnimation : MonoBehaviour
{
    Animator animator;
    public GameObject Loading;
    public GameObject TapToStart;
    public Slider LoadingSlider;
    bool LoadingStatus;
    public List<string> Scene;
    public GameObject ContinueButton;
    public GameObject Buttons;
    void Start()
    {
        animator = GetComponent<Animator>();
        if (PlayerPrefs.GetInt("Mission", 0) == 0)
        {
            ContinueButton.SetActive(false);
            Buttons.transform.localPosition = new Vector3(137f, Buttons.transform.localPosition.y, Buttons.transform.localPosition.z);
        }
    }
    private void Update()
    {
        if(Input.touchCount>0 && !animator.GetBool("Menu"))
        {
            animator.SetBool("Menu", true);
        }
    }
    public void Continue()
    {
        LoadScene(Scene[PlayerPrefs.GetInt("Mission")]);
    }
    public void NewGame()
    {
        LoadScene(Scene[0]);
    }

    public void PlayAnimation(string AnimationName)
    {
        animator.SetBool(AnimationName, true);
    }
    public void CancelAnimation(string AnimationName)
    {
        animator.SetBool(AnimationName, false);
    }
    public void GameObjectToTurnOn(GameObject go)
    {
        go.SetActive(true);
    }
    public void GameObjectToTurnOff(GameObject go)
    {
        go.SetActive(false);
    }
    public void LoadSceneWithoutLoading(string SceneToLoad)
    {
        SceneManager.LoadScene(SceneToLoad);
    }
    public void LoadScene(string SceneToLoad)
    {
        if (LoadingStatus == false)
        {
            LoadingStatus = true;
            Loading.SetActive(true);
            StartCoroutine(LoadYourAsyncScene(SceneToLoad));
        }
    }
    IEnumerator LoadYourAsyncScene(string SceneName)
    {

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneName);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                LoadingSlider.value = 1f;
                //Change the Text to show the Scene is ready
                TapToStart.SetActive(true);
                //Wait to you press the space key to activate the Scene
                if (Input.touchCount>0)
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
            }
            else
            {
                LoadingSlider.value = asyncOperation.progress;
            }
            yield return null;
        }
    }
    public void ChangeGraphics(int newGraphics)
    {
        QualitySettings.SetQualityLevel(newGraphics);
    }
}
