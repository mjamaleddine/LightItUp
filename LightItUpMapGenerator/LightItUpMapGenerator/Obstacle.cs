using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightItUpMapGenerator
{
    public class Obstacle
    {

        public int Column { get; private set; }
        public int Row { get; private set; }
        public ObstacleKind Kind { get; private set; }

        public Obstacle(int row, int column, ObstacleKind kind)
        {
            Column = column;
            Row = row;
            Kind = kind;
        }

    }
}
