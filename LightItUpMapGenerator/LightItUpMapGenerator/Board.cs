using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightItUpMapGenerator
{
    public class Board
    {

        private ObstacleKind[,] _Obstacles;

        public Board(ObstacleKind[,] obstacles)
        {
            _Obstacles = obstacles;
        }

        public ObstacleKind GetObstacle(int x, int y)
        {
            if (y >= _Obstacles.GetLength(0) || x >= _Obstacles.GetLength(1))
                return ObstacleKind.None;

            return _Obstacles[y, x];
        }


        internal List<Obstacle> GetObstacleList()
        {
            List<Obstacle> result = new List<Obstacle>();
            for (int row = 0; row < _Obstacles.GetLength(0); row++)
                for (int column = 0; column < _Obstacles.GetLength(1); column++)
                {
                    ObstacleKind curObstacle = _Obstacles[row, column];
                    if (curObstacle != ObstacleKind.None)
                        result.Add(new Obstacle(column, row, curObstacle));
                }

            return result;
        }
    }
}
