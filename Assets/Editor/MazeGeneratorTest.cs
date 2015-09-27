using NUnit.Framework;

public class MazeGeneratorTest {
    [Test]
    public void TestMazeGenerator() {
        Maze testMaze10x10 = new Maze(10, 10);
        testMaze10x10.print();

        Maze testMaze1x3 = new Maze(1, 3);
        testMaze1x3.print();

        Maze testMaze4x2 = new Maze(4, 2);
        testMaze4x2.print();
    }

    public readonly string[] blueprint = new string[] {
        "XOX",
        "OOO",
        "XOX",
        "XXX",
        "XOX"
    };

    [Test]
    public void TestMazeLayout() {
        Maze testMaze = new Maze(blueprint);
        testMaze.print();
    }
}
