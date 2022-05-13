using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{   

    public GameObject gameOverMenu;
    public GameObject gameCompleteMenu;
    public static GameManager instance;

    bool gameEnd = false;
    bool gameComplete = false;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameOverMenu.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void EndGame()
    {
        if (!gameEnd)
        {
            Cursor.lockState = CursorLockMode.None;
            gameEnd = true;
            gameOverMenu.SetActive(true);
        }
    }
    public void FinishGame()
    {
        if (!gameComplete)
        {
            Cursor.lockState = CursorLockMode.None;
            gameComplete = true;
            gameCompleteMenu.SetActive(true);
        }
    }


    public void QuitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}

public enum GameState
{
    
}