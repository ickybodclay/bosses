using System.Collections;

public class Maze {
    public enum RoomType {
        WALL,
        ROOM
    }

    private RoomType[][] maze;
    private readonly int WIDTH;
    private readonly int HEIGHT;

    public Maze(int width, int height) {
        maze = MazeGenerator.generate(width, height);
        WIDTH = width;
        HEIGHT = height;
    }

    public RoomType get(int x, int y) {
        return maze[x][y];
    }

    public bool isRoom(int x, int y) {
        return isValid(x, y) && maze[x][y] == RoomType.ROOM;
    }

    public bool isValid(int x, int y) {
        return x >= 0 && x < WIDTH && y >= 0 && y < HEIGHT;
    }

    public bool hasRoomNorth(int x, int y) {
        return isRoom(x, y - 1);
    }

    public bool hasRoomSouth(int x, int y) {
        return isRoom(x, y + 1);
    }

    public bool hasRoomWest(int x, int y) {
        return isRoom(x - 1, y);
    }

    public bool hasRoomEast(int x, int y) {
        return isRoom(x + 1, y);
    }

    public void print() {
        for (int i = 0; i < maze.Length; ++i) {
            string row = "";
            for (int j = 0; j < maze[i].Length; ++j) {
                row +=
                    maze[i][j] == RoomType.ROOM ? 'O' : 
                    maze[i][j] == RoomType.WALL ? 'X' : '?';
            }
            System.Console.Write(row);
        }
    }
}
