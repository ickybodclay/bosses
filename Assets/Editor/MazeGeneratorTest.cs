using NUnit.Framework;

public class MazeGeneratorTest {
    [Test]
    public void TestMazeGenerator() {
        Maze testMaze = new Maze(10, 10);
        testMaze.print();
    }
}
