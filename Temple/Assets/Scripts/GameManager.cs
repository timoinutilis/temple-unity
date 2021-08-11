using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        Menu,
        Playing
    }

    public PlayerController player;
    public MainMenuController mainMenu;

    private State state = State.Menu;
    private string gameStatePath;
    private GameState initialGameState;

    void Awake()
    {
        gameStatePath = Application.persistentDataPath + "/gamestate";
    }

    public void Start()
    {
        initialGameState = new GameState();
        player.WriteGameState(initialGameState);

        mainMenu.SetMode(MainMenuController.Mode.Start);
        SetState(State.Menu);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == State.Playing)
            {
                mainMenu.SetMode(MainMenuController.Mode.Ingame);
                SetState(State.Menu);
            }
            else
            {
                SetState(State.Playing);
            }
        }
    }

    public void SetState(State value)
    {
        state = value;
        switch (state)
        {
            case State.Menu:
                player.SetState(PlayerController.State.Paused);
                mainMenu.OpenMenu();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case State.Playing:
                player.SetState(PlayerController.State.Playing);
                mainMenu.CloseMenu();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
        }
    }

    public void StartNewGame()
    {
        player.ReadGameState(initialGameState);
        SetState(State.Playing);
    }

    public bool HasSavedGameState
    {
        get
        {
            return File.Exists(gameStatePath);
        }
    }

    public void LoadGameState()
    {
        if (HasSavedGameState)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(gameStatePath, FileMode.Open);
            GameState gameState = (GameState)bf.Deserialize(file);
            file.Close();

            player.ReadGameState(gameState);

            SetState(State.Playing);
        }
    }

    public void SaveGameState()
    {
        GameState gameState = new GameState();

        player.WriteGameState(gameState);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(gameStatePath);
        bf.Serialize(file, gameState);
        file.Close();
    }

    public void Quit()
    {
        Application.Quit();
    }

}
