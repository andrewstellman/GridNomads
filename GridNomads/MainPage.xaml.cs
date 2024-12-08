namespace GridNomads;

public partial class MainPage : ContentPage
{
    private const int Rows = 54; // Doubled
    private const int Columns = 96; // Doubled
    private const int CellSize = 10; // Halved to maintain grid size
    private const int UpdateInterval = 250; // milliseconds
    private readonly Dictionary<(int, int), BoxView> cells = new();
    private readonly Random random = new();
    private readonly List<Nomad> nomads = new();
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
            var nomad = new Nomad(row, col, color);
            nomads.Add(nomad);
            UpdateNomadView(nomad);
        }
    }

    private void StartGameLoop()
    {
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(UpdateInterval), () =>
        {
            MoveNomads();
            UpdateTrails();
            CheckProximityAndHighlight();
            return true; // Repeat timer
        });
    }

    private void MoveNomads()
    {
        foreach (var nomad in nomads)
        {
            var previousPosition = (nomad.Row, nomad.Column);
            ClearNomadView(nomad);

            nomad.Move(Rows, Columns, random);
            UpdateNomadView(nomad);

            var trail = new Trail(previousPosition.Item1, previousPosition.Item2, nomad.Color);
            trails.Add(trail);
            UpdateTrailView(trail);
        }
    }

    private void CheckProximityAndHighlight()
    {
        foreach (var nomad in nomads)
        {
            ClearHighlight(nomad);
        }

        for (int i = 0; i < nomads.Count; i++)
        {
            var nomadA = nomads[i];
            Nomad? nearestNeighbor = null;
            double minDistance = double.MaxValue;

            for (int j = 0; j < nomads.Count; j++)
            {
                if (i == j) continue;
                var nomadB = nomads[j];
                double distance = CalculateDistance(nomadA, nomadB);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestNeighbor = nomadB;
                }
            }

            if (nearestNeighbor != null && minDistance < 5)
            {
                HighlightNomad(nomadA);
                HighlightNomad(nearestNeighbor);
            }
        }
    }

    private double CalculateDistance(Nomad nomadA, Nomad nomadB)
    {
        int dRow = Math.Abs(nomadA.Row - nomadB.Row);
        int dCol = Math.Abs(nomadA.Column - nomadB.Column);

        dRow = Math.Min(dRow, Rows - dRow);
        dCol = Math.Min(dCol, Columns - dCol);

        return Math.Sqrt(dRow * dRow + dCol * dCol);
    }

    private void HighlightNomad(Nomad nomad)
    {
        cells[(nomad.Row, nomad.Column)].BackgroundColor = nomad.Color.WithLuminosity(0.8f);
    }

    private void ClearHighlight(Nomad nomad)
    {
        cells[(nomad.Row, nomad.Column)].BackgroundColor = nomad.Color;
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

    private void UpdateNomadView(Nomad nomad)
    {
        cells[(nomad.Row, nomad.Column)].BackgroundColor = nomad.Color;
    }

    private void ClearNomadView(Nomad nomad)
    {
        cells[(nomad.Row, nomad.Column)].BackgroundColor = Colors.Gray;
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
