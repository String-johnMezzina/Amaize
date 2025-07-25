using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject GameOverWin;
    public GameObject GameOverLose;
    public string menuScreen = "Menu"; //Main menu scene name

    private GameObject player;
    private GameObject mainCamera;
    private MazeSpawner mazeSpawner;

    void Start()
    {
        //UI canvases are hidden at start
        if (GameOverWin != null)
            GameOverWin.SetActive(false);

        if (GameOverLose != null)
            GameOverLose.SetActive(false);

        mazeSpawner = Object.FindFirstObjectByType<MazeSpawner>();
    }

    public void ShowWinScreen()
    {
        Debug.Log("Player won - showing win screen!");

        //Show win canvas
        if (GameOverWin != null)
        {
            GameOverWin.SetActive(true);

            //Unlock cursor
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;

            //Pause the game
            Time.timeScale = 0f;
        }
    }

    public void ShowLoseScreen()
    {
        Debug.Log("Player lost - showing lose screen!");

        //Show lose canvas
        if (GameOverLose != null)
        {
            GameOverLose.SetActive(true);

            //Unlock cursor
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;

            //Pause the game
            Time.timeScale = 0f;
        }       
    }

    public void ReturnToMainMenu()
    {
        //Reset time scale and go to main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}