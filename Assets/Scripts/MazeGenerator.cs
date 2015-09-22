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
}
