using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;

    public LevelManager levelManager;
    public int RoomX {
        get;
        set;
    }
    public int RoomY {
        get;
        set;
    }
    public Door.Spawn Spawn {
        get;
        set;
    }

    private Maze currentMaze;
    public Maze CurrentMaze {
        get {
            return currentMaze;
        }
    }

    private static readonly int MAZE_WIDTH = 3;
    private static readonly int MAZE_HEIGHT = 3;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        levelManager = GetComponent<LevelManager>();

        InitGame();
        SetupRoom();
    }

    private void OnLevelWasLoaded(int index) {
        SetupRoom();
    }

    void SetupRoom() {
        levelManager.SetupScene(RoomX, RoomY);
    }

    void InitGame() {
        RoomX = 0;
        RoomY = 0;
        currentMaze = new Maze(MAZE_WIDTH, MAZE_HEIGHT);
        levelManager.SetupGame(currentMaze);
        Spawn = Door.Spawn.CENTER;
    }

}
