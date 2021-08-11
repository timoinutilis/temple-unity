using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public enum Mode
    {
        Start,
        Ingame
    }

    public GameManager gameManager;

    public GameObject startButton;
    public GameObject saveButton;
    public GameObject loadButton;
    public GameObject resumeButton;
    public GameObject quitButton;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMode(Mode mode)
    {
        switch (mode)
        {
            case Mode.Start:
                saveButton.SetActive(false);
                resumeButton.SetActive(false);
                break;
            case Mode.Ingame:
                saveButton.SetActive(true);
                resumeButton.SetActive(true);
                break;
        }
        loadButton.SetActive(gameManager.HasSavedGameState);
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        animator.SetBool("visible", true);
    }

    public void CloseMenu()
    {
        animator.SetBool("visible", false);
    }

    public void StartNewGame()
    {
        gameManager.StartNewGame();
    }

    public void SaveGame()
    {
        gameManager.SaveGameState();
    }

    public void LoadGame()
    {
        gameManager.LoadGameState();
    }

    public void ResumeGame()
    {
        gameManager.SetState(GameManager.State.Playing);
    }

    public void Quit()
    {
        gameManager.Quit();
    }

    public void DidFinishOutAnimation()
    {
        gameObject.SetActive(false);
    }

}
