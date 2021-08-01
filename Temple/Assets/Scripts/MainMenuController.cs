using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public PlayerController player;
    public GameObject startButton;
    public GameObject saveButton;
    public GameObject loadButton;
    public GameObject resumeButton;
    public GameObject quitButton;

    // Start is called before the first frame update
    void Start()
    {
        saveButton.SetActive(false);
        resumeButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame()
    {
        gameObject.SetActive(false);
        player.SetState(PlayerController.State.Playing);
    }

    public void SaveGame()
    {

    }

    public void LoadGame()
    {

    }

    public void ResumeGame()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }

}
