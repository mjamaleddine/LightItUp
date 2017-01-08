using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightItUpMapGenerator.ViewModels
{
    public class BoardSettingsViewModel : ViewModel
    {

        private int _ObstacleCoverage;
        private int _ObstacleCount;
        private int _CellCount;
        private int _Columns;
        private int _Rows;
        public int Rows
        {
            get
            {
                return _Rows;
            }
            set
            {
                _Rows = value;
                CalculateBoard();

                FirePropertyChanged("Rows");
            }
        }
        public int Columns
        {
            get
            {
                return _Columns;
            }
            set
            {
                _Columns = value;
                CalculateBoard();

                FirePropertyChanged("Columns");
            }
        }
        public int CellCount
        {
            get
            {
                return _CellCount;
            }
            private set
            {
                _CellCount = value;
                FirePropertyChanged("CellCount");
            }
        }
        public int ObstacleCount
        {
            get
            {
                return _ObstacleCount;
            }
            set
            {
                _ObstacleCount = value;
                CalculateBoard();
                CalculateObstacleCoverage();

                FirePropertyChanged("ObstacleCount");
            }
        }

        public int ObstacleCoverage
        {
            get
            {
                return _ObstacleCoverage;
            }
            set
            {
                _ObstacleCoverage = value;
                CalculateBoard();
                CalculateObstacleCount();

                FirePropertyChanged("ObstacleCoverage");
            }
        }

        public BoardSettingsViewModel()
        {
            Columns = 10;
            Rows = 10;
        }

        private void CalculateObstacleCount()
        {
            _ObstacleCount = (int)(CellCount * ObstacleCoverage / 100);
            FirePropertyChanged("ObstacleCount");
        }

        private void CalculateObstacleCoverage()
        {
            if (CellCount == 0)
                _ObstacleCount = 0;
            else
                _ObstacleCoverage = (int)((float)ObstacleCount / (float)CellCount * 100);

            FirePropertyChanged("ObstacleCoverage");
        }

        private void CalculateBoard()
        {
            CellCount = Columns * Rows;
            if (ObstacleCoverage > 0)
                CalculateObstacleCount();
            else
                CalculateObstacleCoverage();
        }

    }
}
