using UnityEngine;

public class MiniMap : MonoBehaviour {
    private Maze maze;
    private int roomX;
    private int roomY;

    private GameObject[][] mazeMap;

	// Use this for initialization
	void Start () {
        maze = GameManager.instance.CurrentMaze;
        roomX = GameManager.instance.RoomX;
        roomY = GameManager.instance.RoomY;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
