public class MazeGenerator {
    public static Maze.Room[][] generate(int width, int height) {
        Maze.Room[][] maze = new Maze.Room[width][];
        for (int i = 0; i < width; ++i) {
            maze[i] = new Maze.Room[height];
            for (int j = 0; j < height; ++j) {
                maze[i][j] = new Maze.Room(Maze.RoomType.ROOM);
            }
        }
        return maze;
    }

    public static Maze.Room[][] generate(string[] layout) {
        int width = layout[0].Length;
        int height = layout.Length;
        Maze.Room[][] maze = new Maze.Room[height][];
        for (int i = 0; i < height; ++i) {
            maze[i] = new Maze.Room[width];
            for (int j = 0; j < width; ++j) {
                maze[i][j] = new Maze.Room(layout[i][j] == 'O' ? Maze.RoomType.ROOM : Maze.RoomType.WALL);
            }
        }
        return maze;
    }
}
