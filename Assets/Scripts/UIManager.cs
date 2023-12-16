using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject[] ui_screens;

    private void Start()
    {
        uiSettings(0);
    }

    public void OpenScoreTable()
    {
        uiSettings(1);
    }
    public void CloseScoreTable()
    {
        uiSettings(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    



    public void uiSettings(int open_screen)
    {
        for (int i = 0; i < ui_screens.Length; i++)
        {
            if(i == open_screen)
            {
                ui_screens[i].SetActive(true);
            }
            else
            {
                ui_screens[i].SetActive(false);
            }
        }
    }


}
