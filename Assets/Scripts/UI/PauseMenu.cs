using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Reference
    {
        get;
        set;
    }
    GameObject Pause;
    GameObject Dead;
    void Awake()
    {
        Pause = transform.GetChild(0).gameObject;
        Dead = transform.GetChild(0).gameObject;
        Reference = this;
    }
    public void PauseGame()
    {
        Pause.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Pause.SetActive(false);
        Time.timeScale = 1;
    }
    public void PlayerDead()
    {
        Dead.SetActive(true);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
