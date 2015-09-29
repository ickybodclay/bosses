using UnityEngine;

public class LevelManager : MonoBehaviour {

    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] doorTiles;

    private Transform levelHolder;

    private int columns = 11;
    private int rows = 8;
    private Maze currentMaze;
    private GameObject currentBoss;

    public void SetupGame(Maze currentMaze) {
        this.currentMaze = currentMaze;
    }

    public void SetupScene(int roomX, int roomY) {
        LevelSetup(roomX, roomY);
        GameObject.Find("A*").GetComponent<AstarPath>().Scan();
    }

    private void LevelSetup(int roomX, int roomY) {
        levelHolder = new GameObject("Level").transform;

        currentBoss = GameObject.Find("Boss");
        if (roomX == 1 && roomY == 1 && !GameManager.instance.ShouldSpawnRainbowDragon()) {
            Destroy(currentBoss);
            currentBoss = null;
        }

        for (int x = -1; x < columns + 1; ++x) {
            for (int y = -1; y < rows + 1; ++y) {
                GameObject tile = floorTiles[Random.Range(0, floorTiles.Length)];
                int destinationRoomX = -1;
                int destinationRoomY = -1;
                float colliderOffsetX = 0f;
                float colliderOffsetY = 0f;
                Door.Spawn destinationSpawn = Door.Spawn.TOP;
                if (x == -1 || x == columns || y == -1 || y == rows) {
                    if (currentMaze.hasRoomWest(roomX, roomY) && x == -1 && y == rows / 2) { // left door
                        tile = doorTiles[Random.Range(0, doorTiles.Length)];
                        destinationRoomX = roomX - 1;
                        destinationRoomY = roomY;
                        destinationSpawn = Door.Spawn.RIGHT;
                        colliderOffsetX = -0.5f;
                    }
                    else if (currentMaze.hasRoomEast(roomX, roomY) && x == columns && y == rows / 2) { // right door
                        tile = doorTiles[Random.Range(0, doorTiles.Length)];
                        destinationRoomX = roomX + 1;
                        destinationRoomY = roomY;
                        destinationSpawn = Door.Spawn.LEFT;
                        colliderOffsetX = 0.5f;
                    }
                    else if (currentMaze.hasRoomNorth(roomX, roomY) && x == columns / 2 && y == -1) { // top door 
                        tile = doorTiles[Random.Range(0, doorTiles.Length)];
                        destinationRoomX = roomX;
                        destinationRoomY = roomY - 1;
                        destinationSpawn = Door.Spawn.BOTTOM;
                        colliderOffsetY = -0.5f;
                    }
                    else if (currentMaze.hasRoomSouth(roomX, roomY) && x == columns / 2 && y == rows) { // bottom door
                        tile = doorTiles[Random.Range(0, doorTiles.Length)];
                        destinationRoomX = roomX;
                        destinationRoomY = roomY + 1;
                        destinationSpawn = Door.Spawn.TOP;
                        colliderOffsetY = 0.5f;
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
                    door.destinationSpawn = destinationSpawn;
                    tileInstance.GetComponent<BoxCollider2D>().offset = new Vector2(colliderOffsetX, colliderOffsetY);
                }
            }
        }
    }
}
