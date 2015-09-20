using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

    public int columns = 8;
    public int rows = 8;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] doorTiles;

    private Transform levelHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitializeList() {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; ++x) {
            for (int y = 1; y < rows - 1; ++y) {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void LevelSetup() {
        levelHolder = new GameObject("Level").transform;

        for (int x = -1; x < columns + 1; ++x) {
            for (int y = -1; y < rows + 1; ++y) {
                GameObject tile = floorTiles[Random.Range(0, floorTiles.Length)];
                Quaternion rotation = Quaternion.identity;
                if (x == -1 || x == columns || y == -1 || y == rows) {
                    if (x == -1 && y == rows / 2) {
                        tile = doorTiles[Random.Range(0, doorTiles.Length)];
                    }
                    else if (x == columns && y == rows / 2) {
                        tile = doorTiles[Random.Range(0, doorTiles.Length)];
                    }
                    else if (x == columns / 2 && y == -1) {
                        tile = doorTiles[Random.Range(0, doorTiles.Length)];
                    }
                    else if (x == columns / 2 && y == rows) {
                        tile = doorTiles[Random.Range(0, doorTiles.Length)];
                    }
                    else {
                        tile = wallTiles[Random.Range(0, wallTiles.Length)];
                    }
                }

                GameObject tileInstance = Instantiate(tile, new Vector3(x, y, 0f), rotation) as GameObject;
                tileInstance.transform.SetParent(levelHolder);
            }
        }
    }

    void MazeSetup() {
        // TODO generate maze for current game
    }

    public void SetupGame() {
        MazeSetup();
    }

    public void SetupScene(int level) {
        LevelSetup();
        InitializeList();
    }
}
