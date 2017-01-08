using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightItUpMapGenerator.ViewModels
{
    public class BoardViewModel : ViewModel
    {

        public Board Board { get; private set; }
        /// <summary>
        /// TODO remove! just for testing!
        /// </summary>
        public List<Obstacle> TempObstacleList { get; set; }

        public BoardViewModel(Board board)
        {
            Board = board;

            TempObstacleList = Board.GetObstacleList();
        }

    }
}
