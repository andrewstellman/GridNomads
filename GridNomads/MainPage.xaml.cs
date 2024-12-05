using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GridNomads
{
    public partial class MainPage : ContentPage
    {
        private const int Rows = 27;
        private const int Columns = 48;
        private const int CellSize = 20;
        private const int UpdateInterval = 250; // milliseconds
        private readonly Dictionary<(int, int), BoxView> cells = new();
        private readonly Random random = new();
        private readonly List<MovableCell> movableCells = new();

        public MainPage()
        {
            InitializeComponent();
            InitializeGrid();
            StartGameLoop();
        }

        private void InitializeGrid()
        {
            GameGrid.RowDefinitions = new RowDefinitionCollection();
            GameGrid.ColumnDefinitions = new ColumnDefinitionCollection();

            for (int i = 0; i < Rows; i++)
                GameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(CellSize) });

            for (int j = 0; j < Columns; j++)
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(CellSize) });

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    var boxView = new BoxView { BackgroundColor = Colors.Gray };
                    GameGrid.Children.Add(boxView);
                    Grid.SetRow(boxView, i);
                    Grid.SetColumn(boxView, j);
                    cells[(i, j)] = boxView;
                }
            }

            for (int k = 0; k < 5; k++)
            {
                var row = random.Next(Rows);
                var col = random.Next(Columns);
                var color = random.Next(2) == 0 ? Colors.Red : Colors.Blue;
                var cell = new MovableCell(row, col, color);
                movableCells.Add(cell);
                UpdateCellView(cell);
            }
        }

        private void StartGameLoop()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(UpdateInterval), () =>
            {
                MoveCells();
                return true; // Repeat timer
            });
        }

        private void MoveCells()
        {
            foreach (var cell in movableCells)
            {
                ClearCellView(cell);
                cell.Move(Rows, Columns, random);
                UpdateCellView(cell);
            }
        }

        private void UpdateCellView(MovableCell cell)
        {
            cells[(cell.Row, cell.Column)].BackgroundColor = cell.Color;
        }

        private void ClearCellView(MovableCell cell)
        {
            cells[(cell.Row, cell.Column)].BackgroundColor = Colors.Gray;
        }
    }

    public class MovableCell
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        public Color Color { get; }

        public MovableCell(int row, int column, Color color)
        {
            Row = row;
            Column = column;
            Color = color;
        }

        public void Move(int maxRows, int maxCols, Random random)
        {
            var directions = new (int dRow, int dCol)[]
            {
                (-1, 0), (-1, 1), (0, 1), (1, 1),
                (1, 0), (1, -1), (0, -1), (-1, -1)
            };

            var (dRow, dCol) = directions[random.Next(directions.Length)];
            Row = (Row + dRow + maxRows) % maxRows;
            Column = (Column + dCol + maxCols) % maxCols;
        }
    }
}
