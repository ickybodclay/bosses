using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;

    public LevelManager levelManager;

    private int level = 1;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        levelManager = GetComponent<LevelManager>();
        levelManager.SetupGame();

        InitGame();
    }

    private void OnLevelWasLoaded(int index) {
        level++;

        InitGame();
    }

    void InitGame() {
        levelManager.SetupScene(level);
    }

    void RestartGame() {
        levelManager.SetupGame();
    }
}
