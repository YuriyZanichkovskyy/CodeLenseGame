using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using CodeLenseGame.Annotations;
using CodeLenseGame.Enums;

namespace CodeLenseGame
{
    using Vector = Tuple<int, int>;

    public class Game2048 : INotifyPropertyChanged
    {
        public const int GridSize = 4;
        public const int InitialCount = 2;

        private readonly CellModel[,] _cellsGrid = new CellModel[GridSize, GridSize];
        private readonly Random _random = new Random((int) DateTime.Now.Ticks);
        public ObservableCollection<CellModel> Cells { get; private set; }

        public GameState State
        {
            get { return _state; }
            private set
            {
                if (value == _state) return;
                _state = value;
                OnPropertyChanged();
            }
        }

        public int Score
        {
            get { return _score; }
            private set
            {
                if (value == _score) return;
                _score = value;
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty IsGameGridProperty = DependencyProperty.RegisterAttached(
            "IsGameGrid", typeof (bool), typeof (Game2048), new FrameworkPropertyMetadata(default(bool), IsGameGridPropertyChanged));

        private static void IsGameGridPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var gridPanel = d as Grid;
            if (gridPanel != null)
            {
                for (int i = 0; i < GridSize; i++)
                {
                    gridPanel.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star)});
                    gridPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                }
            }
        }

        public static void SetIsGameGrid(DependencyObject element, bool value)
        {
            element.SetValue(IsGameGridProperty, value);
        }

        public static bool GetIsGameGrid(DependencyObject element)
        {
            return (bool) element.GetValue(IsGameGridProperty);
        }

        private readonly IDictionary<MoveDirection, Tuple<int, int>> _vectors = new Dictionary<MoveDirection, Tuple<int, int>>()
        {
            { MoveDirection.Up, new Vector(0, -1)},
            { MoveDirection.Right, new Vector(1, 0)},
            { MoveDirection.Down, new Vector(0, 1)},
            { MoveDirection.Left, new Vector(-1, 0)}
        };

        private GameState _state;
        private int _score;

        public Game2048()
        {
            Cells = new ObservableCollection<CellModel>();
            State = GameState.InProgress;

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    this[i, j] = new CellModel(i, j);
                }
            }

            for (int i = 0; i < InitialCount; i++)
            {
                AddRandomCell(); 
            }
        }

        public void PerformMove(MoveDirection direction)
        {
            if (State != GameState.InProgress)
                return;

            bool moved = false;
            var vector = _vectors[direction];

            foreach (var xCoord in CellsRange(direction))
            {
                foreach (var yCoord in CellsRange(direction))
                {
                    var cell = this[xCoord, yCoord];
                    if(cell.IsEmpty)
                        continue;

                    int x = xCoord;
                    int y = yCoord;
                    CellModel neighbourCell = null;
                    while (IsWithinBounds(x + vector.Item1, y + vector.Item2))
                    {
                        if (!_cellsGrid[x + vector.Item1, y + vector.Item2].IsEmpty)
                        {
                            neighbourCell = _cellsGrid[x + vector.Item1, y + vector.Item2];
                            break;
                        }
                        x = x + vector.Item1;
                        y = y + vector.Item2;
                    }

                    if (neighbourCell != null && neighbourCell.Value == cell.Value)
                    {
                        moved = true;
                        cell.Value *= 2;
                        Score += cell.Value;
                        MoveCell(cell, neighbourCell.X, neighbourCell.Y, mergedWith:neighbourCell);

                        if (cell.Value == 2048)
                        {
                            State = GameState.Game2048;
                            return;
                        }
                    }
                    else
                    {
                        if (this[x, y] != cell)
                        {
                            moved = true;
                            MoveCell(cell, x, y); 
                        }
                    } 
                }
            }

            if (!moved || !AddRandomCell())
            {
                if (!HasAvailableMoves())
                {
                    State = GameState.GameOver;
                    Debug.WriteLine("GameOver");
                }
            }
        }

        private static bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < GridSize && y >= 0 && y < GridSize;
        }

        private IEnumerable<int> CellsRange(MoveDirection moveDirection)
        {
            var cells = Enumerable.Range(0, GridSize);
            if (moveDirection == MoveDirection.Right || moveDirection == MoveDirection.Down)
            {
                cells = cells.Reverse();
            }

            return cells;
        }

        private void MoveCell(CellModel cell, int x, int y, CellModel mergedWith = null)
        {
            if(!this[x, y].IsEmpty && this[x,y] != mergedWith)
                throw new Exception("Couldn't move to non empty cell");

            if (this[x, y] != cell)
            {
                var emptyCell = new CellModel(cell.X, cell.Y);
                _cellsGrid[cell.X, cell.Y] = emptyCell;
                Cells.Add(emptyCell);
                Cells.Remove(_cellsGrid[x, y]);
                _cellsGrid[x, y] = cell;
                cell.X = x;
                cell.Y = y;
            }
        }

        private bool HasAvailableMoves()
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    var currentCell = _cellsGrid[i, j];
                    if (currentCell.IsEmpty)
                        return true;

                    foreach (var moveDirection in (MoveDirection[])Enum.GetValues(typeof(MoveDirection)))
                    {
                        var movementVector = _vectors[moveDirection];
                        int x = i + movementVector.Item1;
                        int y = j + movementVector.Item2;
                        if (x >= 0 && x < GridSize && y >= 0 && y < GridSize)
                        {
                            var neigbourCell = _cellsGrid[x, y];
                            if (neigbourCell.IsEmpty)
                                return true;
                            if (neigbourCell.Value == currentCell.Value)
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool AddRandomCell()
        {
            var cells = GetEmptyCells();
            if (cells.Length == 0)
                return false;

            Tuple<int, int> cell;
            if (cells.Length == 1)
            {
                cell = cells[0];
            }
            else
            {
                var cellIndex = _random.Next(0, cells.Length - 1);
                cell = cells[cellIndex];
            }

            this[cell.Item1, cell.Item2] = new CellModel(GetRandomValue(), cell.Item1, cell.Item2);

            return true;
        }

        private CellModel this[int i, int j]
        {
            get { return _cellsGrid[i, j]; }
            set
            {
                if (_cellsGrid[i, j] != null)
                {
                    Cells.Remove(_cellsGrid[i, j]);
                }
                Cells.Add(value);
                _cellsGrid[i, j] = value;
            } 
        }

        private Tuple<int, int>[] GetEmptyCells()
        {
            var emptyCells = new List<Tuple<int, int>>(GridSize * GridSize);
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    if (_cellsGrid[i, j].IsEmpty)
                    {
                        emptyCells.Add(new Tuple<int, int>(i, j));
                    }                   
                }
            }

            return emptyCells.ToArray();
        }

        private int GetRandomValue()
        {
            return _random.NextDouble() < 0.9 ? 2 : 4;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}