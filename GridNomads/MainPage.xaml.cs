namespace GridNomads;

public partial class MainPage : ContentPage
{
    private const int Rows = 54; // Doubled
    private const int Columns = 96; // Doubled
    private const int CellSize = 10; // Halved to maintain grid size
    private const int UpdateInterval = 250; // milliseconds
    private readonly Dictionary<(int, int), BoxView> cells = new();
    private readonly Random random = new();
    private readonly List<MovableCell> movableCells = new();
    private readonly List<Trail> trails = new();

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
                var boxView = new BoxView
                {
                    BackgroundColor = Colors.Gray,
                    Margin = 0.5 // Adds thin grid lines
                };
                GameGrid.Children.Add(boxView);
                Grid.SetRow(boxView, i);
                Grid.SetColumn(boxView, j);
                cells[(i, j)] = boxView;
            }
        }

        // Add four times as many nomads
        int totalNomads = 20; // 4 times the original 5
        for (int k = 0; k < totalNomads; k++)
        {
            var row = random.Next(Rows);
            var col = random.Next(Columns);
            var color = (k % 2 == 0) ? Colors.Crimson : Colors.DodgerBlue; // Alternate red and blue
            var cell = new MovableCell(row, col, color);
            movableCells.Add(cell);
            UpdateCellView(cell);
        }
    }

    private void StartGameLoop()
    {
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(UpdateInterval), () =>
        {
            MoveCells();
            UpdateTrails();
            CheckProximityAndHighlight();
            return true; // Repeat timer
        });
    }

    private void MoveCells()
    {
        foreach (var cell in movableCells)
        {
            var previousPosition = (cell.Row, cell.Column);
            ClearCellView(cell);

            cell.Move(Rows, Columns, random);
            UpdateCellView(cell);

            var trail = new Trail(previousPosition.Item1, previousPosition.Item2, cell.Color);
            trails.Add(trail);
            UpdateTrailView(trail);
        }
    }

    private void CheckProximityAndHighlight()
    {
        foreach (var cell in movableCells)
        {
            ClearHighlight(cell);
        }

        for (int i = 0; i < movableCells.Count; i++)
        {
            var cellA = movableCells[i];
            MovableCell? nearestNeighbor = null;
            double minDistance = double.MaxValue;

            for (int j = 0; j < movableCells.Count; j++)
            {
                if (i == j) continue;
                var cellB = movableCells[j];
                double distance = CalculateDistance(cellA, cellB);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestNeighbor = cellB;
                }
            }

            if (nearestNeighbor != null && minDistance < 5)
            {
                HighlightNomad(cellA);
                HighlightNomad(nearestNeighbor);
            }
        }
    }

    private double CalculateDistance(MovableCell cellA, MovableCell cellB)
    {
        int dRow = Math.Abs(cellA.Row - cellB.Row);
        int dCol = Math.Abs(cellA.Column - cellB.Column);

        dRow = Math.Min(dRow, Rows - dRow);
        dCol = Math.Min(dCol, Columns - dCol);

        return Math.Sqrt(dRow * dRow + dCol * dCol);
    }

    private void HighlightNomad(MovableCell cell)
    {
        cells[(cell.Row, cell.Column)].BackgroundColor = cell.Color.WithLuminosity(0.8f);
    }

    private void ClearHighlight(MovableCell cell)
    {
        cells[(cell.Row, cell.Column)].BackgroundColor = cell.Color;
    }

    private void UpdateTrails()
    {
        var trailsToRemove = new List<Trail>();
        foreach (var trail in trails)
        {
            trail.Fade();
            if (trail.Opacity <= 0)
            {
                trailsToRemove.Add(trail);
                ClearTrailView(trail);
            }
            else
            {
                UpdateTrailView(trail);
            }
        }
        trails.RemoveAll(trailsToRemove.Contains);
    }

    private void UpdateCellView(MovableCell cell)
    {
        cells[(cell.Row, cell.Column)].BackgroundColor = cell.Color;
    }

    private void ClearCellView(MovableCell cell)
    {
        cells[(cell.Row, cell.Column)].BackgroundColor = Colors.Gray;
    }

    private void UpdateTrailView(Trail trail)
    {
        cells[(trail.Row, trail.Column)].BackgroundColor = trail.GetFadedColor();
    }

    private void ClearTrailView(Trail trail)
    {
        cells[(trail.Row, trail.Column)].BackgroundColor = Colors.Gray;
    }
}
