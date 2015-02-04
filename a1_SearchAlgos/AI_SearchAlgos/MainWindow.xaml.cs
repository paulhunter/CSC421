using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AI_SearchAlgos
{
    using Model;
    using Utils;
    using Search;
    using Search.Heuristics;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int POLYGON_SIZE = 45;
        private const int POLYGON_SPACE = 10;
        private const double POLYGON_WIDTH = 1.732051 * (POLYGON_SIZE + POLYGON_SPACE); //SQRT(3) * (PolygonSize + 5);
        private const double POLYGON_HEIGHT = (POLYGON_SIZE + POLYGON_SPACE) * 2;
        private const double POLYGON_V_DISTANCE = 0.75 * POLYGON_HEIGHT;

        private MainWindow _thisInstance;

        private uint map_Height = 5;
        private uint map_Width = 5;
        private uint map_PercentFree = 50;

        private HexagonalTileSearchProblem _activeProblem;
        private SearchResults _activeResults;
        private String _activeResultsMethod;
        private Map _activeMap;

        private List<Polygon> OnScreenTiles;
        private List<Line> OnScreenPaths;

        public MainWindow()
        {
            Utils.Log.Start();
            InitializeComponent();

            this._thisInstance = this;
            Init();
        }

        private void Init()
        {
            OnScreenTiles = new List<Polygon>();
            OnScreenPaths = new List<Line>();

            Algorithms.Items.Add(new BreadthFirstSearch());
            Algorithms.Items.Add(new DepthFirstSearch());
            Algorithms.Items.Add(new IterativeDeepeningSearch());
            Algorithms.Items.Add(new BestFirstSearch(new EuclidianDistance()));
            Algorithms.Items.Add(new BestFirstSearch(new TileDistanceHeuristic()));
            Algorithms.Items.Add(new AStarSearch(new EuclidianDistance()));
            Algorithms.Items.Add(new AStarSearch(new TileDistanceHeuristic()));
            Algorithms.Items.Add(new HillClimbSearch(new EuclidianDistance()));
            Algorithms.Items.Add(new HillClimbSearch(new TileDistanceHeuristic()));
            
            Algorithms.SelectedIndex = 0;


            RefeshUIComponents();

        }


        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            if (_activeProblem == null)
            {
                _activeProblem = new HexagonalTileSearchProblem(map_Width, map_Height, map_PercentFree/(100.0));
                Map m = _activeProblem.SearchSpace;
                Log.Info(string.Format("App: Map created has {0:0.00} free paths of target {1:0.00}", m.FreePathPercentage * 100, 0.2 * 100));
            }
            else
            {
                _activeProblem.Reset();
            }
            ClearMap();
            this._activeMap = _activeProblem.SearchSpace;
            UpdateMap();
            this._activeResults = null;
            RefeshUIComponents();
        }

        private void ClearMap()
        {
            if(OnScreenTiles.Count > 0)
            {
                this.Landscape.Children.Clear();
                this.Paths.Children.Clear();
                this.Landscape.InvalidateVisual();
                this.Paths.InvalidateVisual();
                OnScreenTiles.Clear();
                OnScreenPaths.Clear();
            }
            
        }

        private void RefeshUIComponents()
        {
            this._thisInstance.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.MapWidth_tb.Text = string.Format("{0:0}", this.map_Width);
                    this.MapHeight_tb.Text = string.Format("{0:0}", this.map_Height);
                    this.MapPercentFree_tb.Text = string.Format("{0:0}", this.map_PercentFree);
                    if(this._activeResults != null)
                    {
                        this.SR_gp.Header = "Search Results - " + _activeResultsMethod;
                        this.TimeComplexity_lbl.Content = string.Format("{0:0}", this._activeResults.TimeComplexity);
                        this.SpaceComplexity_lbl.Content = string.Format("{0:0}", this._activeResults.SpaceComplexity);
                        this.RunTime_lbl.Content = string.Format("{0:0.000}", this._activeResults.TimeInMilliseconds / 1000.0);
                        this.PathLength_lbl.Content = string.Format("{0:0}", this._activeResults.Path == null ? 0 : this._activeResults.Path.Count - 1);

                        if (this._activeResults.Solved)
                        {
                            foreach (MapTile mt in _activeResults.Path)
                            {
                                this.OnScreenTiles.ElementAt(mt.ID).Fill = System.Windows.Media.Brushes.CornflowerBlue;
                                this.OnScreenTiles.ElementAt(mt.ID).InvalidateVisual();
                            }
                        }
                    }
                     

                    if(this._activeProblem != null)
                    {
                        this.OnScreenTiles.ElementAt(this._activeProblem.Start.ID).Fill = System.Windows.Media.Brushes.LightGreen;
                        this.OnScreenTiles.ElementAt(this._activeProblem.Goal.ID).Fill = System.Windows.Media.Brushes.LightCoral;
                        this.TileCount_lbl.Content = string.Format("{0:0}", this._activeProblem.SearchSpace.Size);
                        this.Obstactle_lbl.Content = string.Format("~{0:0}% / {0:0.00}%",
                            this._activeProblem.IntendedFreeObstaclePercentage,
                            this._activeProblem.ActualFreeObstaclePercentage);
                    }
                    
                }));
        }

        private void UpdateMap()
        {
            if(this._activeMap != null)
            {
                int x, y;
                Point pc; //Center point of Tile

                for(y = 0; y < this._activeMap.Height; y++)
                {
                    for (x = 0; x < this._activeMap.Width; x++)
                    {
                        // Calculate the center of the polygon with coordinates x,y
                        pc = CenterOfTile(x, y);
                        GeneratePolygon(pc);
                    }
                }
                foreach(Tuple<MapTile, MapTile> mtt in _activeMap._edges)
                {
                    GeneratePath(mtt.Item1, mtt.Item2);
                }
                foreach(Polygon p in OnScreenTiles)
                {
                    this.Landscape.Children.Add(p);
                }
                foreach(Line l in OnScreenPaths)
                {
                    this.Paths.Children.Add(l);
                }

                //Highlight Start and End as Light Green and Red

                this.Landscape.InvalidateVisual();
                this.Paths.InvalidateVisual();
            }
        }

        /// <summary>
        /// Generate a polygon object using POLYGON_SIZE that is centered
        /// on the X, Y coordinates provided. 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        private Polygon GeneratePolygon(Point Center)
        {
            PointCollection pc = new PointCollection();
            double a, xo, yo;
            int i;
            for(i = 0; i < 6; i++)
            {
                a = (Math.PI / 3) * (i + 0.5);
                xo = Center.X + (POLYGON_SIZE * Math.Cos(a));
                yo = Center.Y + (POLYGON_SIZE * Math.Sin(a));
                pc.Add(new System.Windows.Point(xo, yo));
            }

            Polygon r = new Polygon()
            {
                Points = pc,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                Fill = System.Windows.Media.Brushes.ForestGreen,
                Stroke = System.Windows.Media.Brushes.Black,
                StrokeThickness = 2
            };
            this.OnScreenTiles.Add(r);
            return r;
        }

        /// <summary>
        /// Generate a line to be used on screen to represent a path. The 
        /// order of the parmaters does not matter.
        /// </summary>
        /// <param name="A">Origin</param>
        /// <param name="B">Destination</param>
        /// <returns>A line between the centers of A and B</returns>
        private Line GeneratePath(MapTile A, MapTile B)
        {
            Point ap = CenterOfTile(A.X, A.Y);
            Point bp = CenterOfTile(B.X, B.Y);
            Line l =  new Line()
            {
                X1 = ap.X,
                Y1 = ap.Y,
                X2 = bp.X,
                Y2 = bp.Y,
                Stroke = System.Windows.Media.Brushes.Black,
                StrokeThickness = 5,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                VerticalAlignment = System.Windows.VerticalAlignment.Top
            };
            OnScreenPaths.Add(l);
            return l;
        }

        /// <summary>
        /// Calculate the center position of a tile using its x,y coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Point CenterOfTile(int x, int y)
        {
            Point p;
            double xc, yc;
            if (y % 2 == 0)
            {
                xc = (POLYGON_WIDTH * (x + 0.5));
            }
            else
            {
                xc = (POLYGON_WIDTH * (x + 1));
            }
            yc = (0.5 * POLYGON_HEIGHT) + (y * POLYGON_V_DISTANCE);
            p = new Point(xc, yc);
            return p;
        }

        private void StartSearch_Click(object sender, RoutedEventArgs e)
        {
            ISearchAlgorithm sa = (ISearchAlgorithm)this.Algorithms.SelectedItem;
            _activeResults = sa.Search(_activeProblem);
            _activeResultsMethod = sa.ToString();
            RefeshUIComponents();
        }

        
        private void ValidateMapWidth()
        {
            uint new_w;
            if(uint.TryParse(this.MapWidth_tb.Text, out new_w))
            {
                if(new_w > 20)
                    new_w = 20;
                map_Width = new_w;
            }
            this.MapWidth_tb.Text = string.Format("{0:0}", map_Width);
        }

        private void ValidateMapHeight()
        {
            uint new_h;
            if(uint.TryParse(this.MapHeight_tb.Text, out new_h))
            {
                if(new_h > 20)
                    new_h = 20;
                map_Height = new_h;
            }
            this.MapHeight_tb.Text = string.Format("{0:0}", map_Height);
        }

        private void ValidateMapPathPerc()
        {
            uint new_p;
            if(uint.TryParse(this.MapPercentFree_tb.Text, out new_p))
            {
                if(new_p > 100)
                    new_p = 100;
                map_PercentFree = new_p;
            }
            this.MapPercentFree_tb.Text = string.Format("{0:0}", map_PercentFree);
        }

        private void ValidateTextbox(TextBox element)
        {
            if (element == this.MapWidth_tb)
            {
                ValidateMapWidth();
            }
            else if (element == this.MapHeight_tb)
            {
                ValidateMapHeight();
            }
            else if (element == this.MapPercentFree_tb)
            {
                ValidateMapPathPerc();
            }
            
        }


        private void textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidateTextbox(sender as TextBox);
        }

        private void Map_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ValidateTextbox(sender as TextBox);
                (sender as TextBox).SelectAll();
            }
        }

        private void Map_tb_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        } 

    }
}
