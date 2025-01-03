﻿namespace GridNomads;

public partial class MainPage : ContentPage
{
    private const int Rows = 54;
    private const int Columns = 96;
    private const int CellSize = 10;
    private const int UpdateInterval = 250; // Nomad movement interval
    private const int TrailUpdateInterval = 50; // Trail fading interval
    private readonly Dictionary<(int, int), BoxView> trailCells = new();
    private readonly Dictionary<(int, int), BoxView> nomadCells = new();
    private readonly Random random = new();
    private readonly List<Nomad> nomads = new();
    private readonly List<Trail> trails = new();

    public MainPage()
    {
        InitializeComponent();
        InitializeGrid();
        StartGameLoop();
        StartTrailAnimationLoop();
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
                // Add trail layer
                var trailBoxView = new BoxView
                {
                    BackgroundColor = ColorConstants.BackgroundColor, // Use centralized constant
                    Margin = 0.5
                };
                GameGrid.Children.Add(trailBoxView);
                Grid.SetRow(trailBoxView, i);
                Grid.SetColumn(trailBoxView, j);
                trailCells[(i, j)] = trailBoxView;

                // Add nomad layer (on top of the trail layer)
                var nomadBoxView = new BoxView
                {
                    BackgroundColor = Colors.Transparent,
                    Margin = 0.5
                };
                GameGrid.Children.Add(nomadBoxView);
                Grid.SetRow(nomadBoxView, i);
                Grid.SetColumn(nomadBoxView, j);
                nomadCells[(i, j)] = nomadBoxView;
            }
        }

        int totalNomads = 20;
        for (int k = 0; k < totalNomads; k++)
        {
            var row = random.Next(Rows);
            var col = random.Next(Columns);

            // Use high-contrast colors
            var color = (k % 2 == 0) ? ColorConstants.RedNomadColor : ColorConstants.BlueNomadColor;
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
            UpdateProximityData();
            UpdateNomadViews();
            return true; // Repeat timer
        });
    }

    private void StartTrailAnimationLoop()
    {
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(TrailUpdateInterval), () =>
        {
            UpdateTrails();
            return true; // Repeat timer
        });
    }

    private void MoveNomads()
    {
        foreach (var nomad in nomads)
        {
            var previousPosition = (nomad.Row, nomad.Column);
            ClearNomadView(nomad);

            nomad.Act(nomads, Rows, Columns, random);

            // Add a trail where the nomad was
            var trail = new Trail(previousPosition.Item1, previousPosition.Item2, nomad.Color);
            trails.Add(trail);

            UpdateTrailView(trail);
            UpdateNomadView(nomad);
        }
    }

    private void UpdateProximityData()
    {
        foreach (var nomad in nomads)
        {
            // Placeholder for future proximity logic updates
        }
    }

    private void UpdateNomadViews()
    {
        foreach (var nomad in nomads)
        {
            UpdateNomadView(nomad);
        }
    }

    private void UpdateNomadView(Nomad nomad)
    {
        nomadCells[(nomad.Row, nomad.Column)].BackgroundColor = nomad.Color;
    }

    private void UpdateTrails()
    {
        var trailsToRemove = new List<Trail>();
        foreach (var trail in trails)
        {
            trail.Fade();
            UpdateTrailView(trail);
            if (trail.Opacity <= 0)
            {
                trailsToRemove.Add(trail);
                ClearTrailView(trail);
            }
        }
        trails.RemoveAll(trailsToRemove.Contains);
    }

    private void UpdateTrailView(Trail trail)
    {
        trailCells[(trail.Row, trail.Column)].BackgroundColor = trail.GetFadedColor();
    }

    private void ClearNomadView(Nomad nomad)
    {
        nomadCells[(nomad.Row, nomad.Column)].BackgroundColor = Colors.Transparent;
    }

    private void ClearTrailView(Trail trail)
    {
        trailCells[(trail.Row, trail.Column)].BackgroundColor = ColorConstants.TrailColor;
    }
}
