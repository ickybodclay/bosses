using System.Collections;

public class Maze {
    public enum RoomType {
        WALL,
        ROOM
    }

    private RoomType[][] maze;

    public Maze(int width, int height) {
        maze = MazeGenerator.generate(width, height);
    }

    public RoomType get(int x, int y) {
        return maze[x][y];
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
