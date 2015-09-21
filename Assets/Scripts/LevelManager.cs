using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

    public int columns = 8;
    public int rows = 8;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] doorTiles;

    private Transform levelHolder;

    private Maze currentMaze;
    //private int roomX;
    //private int roomY;

    public void SetupGame(Maze currentMaze) {
        this.currentMaze = currentMaze;
    }

    public void SetupScene(int roomX, int roomY) {
        LevelSetup(roomX, roomY);
    }

    private void LevelSetup(int roomX, int roomY) {
        levelHolder = new GameObject("Level").transform;
        //this.roomX = roomX;
        //this.roomY = roomY;

        for (int x = -1; x < columns + 1; ++x) {
            for (int y = -1; y < rows + 1; ++y) {
                GameObject tile = floorTiles[Random.Range(0, floorTiles.Length)];
                int destinationRoomX = -1;
                int destinationRoomY = -1;
                if (x == -1 || x == columns || y == -1 || y == rows) {
                    if (currentMaze.hasRoomWest(roomX, roomY) && x == -1 && y == rows / 2) { // left
                        tile = doorTiles[Random.Range(0, doorTiles.Length)];
                        destinationRoomX = roomX - 1;
                        destinationRoomY = roomY;
                    }
                    else if (currentMaze.hasRoomEast(roomX, roomY) && x == columns && y == rows / 2) { // right
                        tile = doorTiles[Random.Range(0, doorTiles.Length)];
                        destinationRoomX = roomX + 1;
                        destinationRoomY = roomY;
                    }
                    else if (currentMaze.hasRoomNorth(roomX, roomY) && x == columns / 2 && y == -1) { // top
                        tile = doorTiles[Random.Range(0, doorTiles.Length)];
                        destinationRoomX = roomX;
                        destinationRoomY = roomY - 1;
                    }
                    else if (currentMaze.hasRoomSouth(roomX, roomY) && x == columns / 2 && y == rows) { // bottom
                        tile = doorTiles[Random.Range(0, doorTiles.Length)];
                        destinationRoomX = roomX;
                        destinationRoomY = roomY + 1;
                    }
                    else {
                        tile = wallTiles[Random.Range(0, wallTiles.Length)];
                    }
                }

                GameObject tileInstance = Instantiate(tile, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                tileInstance.transform.SetParent(levelHolder);
                if (tileInstance.GetComponent<Door>() != null) {
                    Door door = tileInstance.GetComponent<Door>();
                    door.destinationRoomX = destinationRoomX;
                    door.destinationRoomY = destinationRoomY;
                }
            }
        }
    }
}
