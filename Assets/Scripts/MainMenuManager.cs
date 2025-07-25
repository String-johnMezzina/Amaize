using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string gameScene = "Game"; 

    public GameObject mainMenuPanel;     //main menu ui
    public GameObject instructionsPanel;  //instructions ui

    void Start()
    {
        //main menu shown and instructions hidden at start
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);
    }

    //Play game clicked
    public void PlayGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    //Instructions clicked
    public void ShowInstructions()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        if (instructionsPanel != null)
            instructionsPanel.SetActive(true);
    }

    //menu button on instructions ui clocked
    public void ShowMainMenu()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }
    }

    //Exit clicked
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}