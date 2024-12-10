namespace GridNomads;

public partial class MainPage : ContentPage
{
    private const int Rows = 54;
    private const int Columns = 96;
    private const int CellSize = 10;
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
                    Margin = 0.5
                };
                GameGrid.Children.Add(boxView);
                Grid.SetRow(boxView, i);
                Grid.SetColumn(boxView, j);
                cells[(i, j)] = boxView;
            }
        }

        int totalNomads = 20;
        for (int k = 0; k < totalNomads; k++)
        {
            var row = random.Next(Rows);
            var col = random.Next(Columns);
            var color = (k % 2 == 0) ? Colors.Crimson : Colors.DodgerBlue;
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
            UpdateProximityData();
            UpdateNomadViews();
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

    private void UpdateProximityData()
    {
        foreach (var nomad in nomads)
        {
            var neighbors = nomads
                .Where(n => n != nomad)
                .Select(n => new NeighborInfo
                {
                    Direction = GetDirection(nomad, n),
                    Distance = CalculateDistance(nomad, n),
                    Color = n.Color
                })
                .ToList();

            nomad.UpdateNeighbors(neighbors);
        }
    }

    private void UpdateNomadViews()
    {
        foreach (var nomad in nomads)
        {
            var excitementLevel = nomad.ExcitementLevel;
            cells[(nomad.Row, nomad.Column)].BackgroundColor =
                nomad.Color.WithLuminosity(0.5f + 0.3f * excitementLevel); // Adjust brightness based on excitement
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

    private Direction GetDirection(Nomad origin, Nomad target)
    {
        int dRow = (target.Row - origin.Row + Rows) % Rows;
        int dCol = (target.Column - origin.Column + Columns) % Columns;

        if (dRow > Rows / 2) dRow -= Rows;
        if (dCol > Columns / 2) dCol -= Columns;

        return DirectionHelper.GetDirection(dRow, dCol);
    }

    private void UpdateTrailView(Trail trail)
    {
        cells[(trail.Row, trail.Column)].BackgroundColor = trail.GetFadedColor();
    }

    private void ClearNomadView(Nomad nomad)
    {
        cells[(nomad.Row, nomad.Column)].BackgroundColor = Colors.Gray;
    }

    private void UpdateNomadView(Nomad nomad)
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
        }
        trails.RemoveAll(trailsToRemove.Contains);
    }

    private void ClearTrailView(Trail trail)
    {
        cells[(trail.Row, trail.Column)].BackgroundColor = Colors.Gray;
    }
}
