using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public LevelManager levelManager;
    public AudioClip phase1Music;
    public AudioClip phase2Music;

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
    public readonly string[] blueprint = new string[] {
        "XOX",
        "OOO",
        "XOX",
        "XXX",
        "XOX"
    };

    public int PlayerHealth {
        get;
        set;
    }

    private Dictionary<Boss.Type, int> bossHealthMap;

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
        if(Application.loadedLevelName == "Game")
            SetupRoom();
    }

    void SetupRoom() {
        levelManager.SetupScene(RoomX, RoomY);
    }

    void InitGame() {
        RoomX = 1;
        RoomY = 1;
        currentMaze = new Maze(blueprint);
        levelManager.SetupGame(currentMaze);
        Spawn = Door.Spawn.CENTER;
        PlayerHealth = 5;

        bossHealthMap = new Dictionary<Boss.Type, int>();
        bossHealthMap.Add(Boss.Type.FIRE, 1);
        bossHealthMap.Add(Boss.Type.WATER, 1);
        bossHealthMap.Add(Boss.Type.EARTH, 1);
        bossHealthMap.Add(Boss.Type.AIR, 1);
        bossHealthMap.Add(Boss.Type.RAINBOW, 3);

        SoundManager.instance.musicSource.clip = phase1Music;
        SoundManager.instance.musicSource.Play();
    }

    public int GetBossHealth(Boss.Type type) {
        return bossHealthMap[type];
    }

    public void DamageBoss(Boss.Type type, int damage) {
        bossHealthMap[type] -= damage;
    }

    public bool IsBossAlive(Boss.Type type) {
        return bossHealthMap[type] > 0;
    }

    public bool ShouldSpawnRainbowDragon() {
        return 
            bossHealthMap[Boss.Type.FIRE] <= 0 &&
            bossHealthMap[Boss.Type.WATER] <= 0 &&
            bossHealthMap[Boss.Type.EARTH] <= 0 &&
            bossHealthMap[Boss.Type.AIR] <= 0 &&
            bossHealthMap[Boss.Type.RAINBOW] > 0;
    }

    public void ResetGame() {
        RoomX = 1;
        RoomY = 1;
        Spawn = Door.Spawn.CENTER;
        PlayerHealth = 5;

        bossHealthMap[Boss.Type.FIRE] = 1;
        bossHealthMap[Boss.Type.WATER] = 1;
        bossHealthMap[Boss.Type.EARTH] = 1;
        bossHealthMap[Boss.Type.AIR] = 1;
        bossHealthMap[Boss.Type.RAINBOW] = 3;

        SoundManager.instance.musicSource.clip = phase1Music;
        SoundManager.instance.musicSource.Play();

        Application.LoadLevel(Application.loadedLevel);
    }
}
