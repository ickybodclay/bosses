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

        RoomX = 0;
        RoomY = 0;
        currentMaze = new Maze(MAZE_WIDTH, MAZE_HEIGHT);
        levelManager.SetupGame(currentMaze);

        InitGame();
    }

    private void OnLevelWasLoaded(int index) {
        InitGame();
    }

    void InitGame() {
        levelManager.SetupScene(RoomX, RoomY);
    }

    void RestartGame() {
        currentMaze = new Maze(MAZE_WIDTH, MAZE_HEIGHT);
        levelManager.SetupGame(currentMaze);
    }
}
