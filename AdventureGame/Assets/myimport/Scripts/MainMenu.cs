using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //[SerializeField]
    //private GameObject MainMenuUI;
    //[SerializeField]
    //private GameObject CreateCharacterMenu;


    public void LoadCrossingstbdScene()
    {
        SceneManager.LoadScene("Crossingstbdbow");
    }
    public void LoadCrossingPortScene()
    {
        SceneManager.LoadScene("Crossingportbow");
    }
    public void LoadHeadonScene()
    {
        SceneManager.LoadScene("Headon");
    }
    public void LoadDevScene()
    {
        SceneManager.LoadScene("Dev");
    }
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
