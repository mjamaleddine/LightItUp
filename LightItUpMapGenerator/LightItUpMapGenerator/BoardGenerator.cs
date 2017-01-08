using LightItUpMapGenerator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightItUpMapGenerator
{
    public class BoardGenerator
    {

        private BoardSettingsViewModel _BoardSettings;

        public BoardGenerator(BoardSettingsViewModel boardSettings)
        {
            if (boardSettings == null)
                throw new ArgumentNullException("boardSettings", "boardSettings is null.");

            _BoardSettings = boardSettings;
        }

        public Board Generate()
        {
            int obstacleCount = _BoardSettings.ObstacleCount;
            Random rnd = new Random();

            List<Obstacle> obstacleList = new List<Obstacle>();
            for (int i = 0; i < obstacleCount; i++)
            {
                int column = 0;
                int row = 0;
                do
                {
                    column = rnd.Next(_BoardSettings.Columns);
                    row = rnd.Next(_BoardSettings.Rows);
                } while (obstacleList.Any(cur => cur.Column == column && cur.Row == row));

                ObstacleKind kind = ObstacleKind.Wall;
                obstacleList.Add(new Obstacle(column, row, kind));
            }

            ObstacleKind[,] obstacles = new ObstacleKind[_BoardSettings.Rows, _BoardSettings.Columns];
            for (int row = 0; row < _BoardSettings.Rows; row++)
                for (int column = 0; column < _BoardSettings.Columns; column++)
                {
                    var obstacle = obstacleList.FirstOrDefault(cur => cur.Column == column && cur.Row == row);
                    if (obstacle != null)
                        obstacles[row, column] = obstacle.Kind;
                }

            return new Board(obstacles);
        }


        public object Obstacle { get; set; }
    }
}
