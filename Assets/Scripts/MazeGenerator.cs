public class MazeGenerator {
    public static Maze.RoomType[][] generate(int width, int height) {
        Maze.RoomType[][] maze = new Maze.RoomType[width][];
        for (int i = 0; i < width; ++i) {
            maze[i] = new Maze.RoomType[height];
            for (int j = 0; j < height; ++j) {
                maze[i][j] = Maze.RoomType.ROOM;
            }
        }
        return maze;
    }
}
