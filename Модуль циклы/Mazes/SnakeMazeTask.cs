// Вставьте сюда финальное содержимое файла SnakeMazeTask.cs

namespace Mazes;

public static class SnakeMazeTask
{   
    public static void LookingForWayOut(Robot robot, int width, int height)
    {
        int LengthWey = (height - 1 - 1) / 2 + 1;
        for (int q = 0; q < LengthWey; q++)
        {
            if ((q % 2) == 0)
            {
                DoStepFromRightToLeft(robot, width, Direction.Right);   
            }
            else if ((LengthWey - q > 1) && (q % 2 == 1))
            {
				Turning(robot, width, height);

                robot.MoveTo(Direction.Down); 
                robot.MoveTo(Direction.Down); 
            }
            else
            {
                Turning(robot, width, height);   
            }
        }
    }

    public static void Turning(Robot robot, int width, int height)
    {

        robot.MoveTo(Direction.Down);
        robot.MoveTo(Direction.Down);

        DoStepFromRightToLeft(robot, width, Direction.Left);
    }
    
    public static void DoStepFromRightToLeft(Robot robot, int width, Direction dir)
    {
        for (int q = 0; q < width - 2 - 1; q++)
        {
            robot.MoveTo(dir);
        }
    }
}
