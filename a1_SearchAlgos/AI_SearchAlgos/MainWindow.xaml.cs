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

        private HexagonalTileSearchProblem _activeProblem;
        private Map _activeMap;

        private List<Polygon> OnScreenTiles;
        private List<Line> OnScreenPaths;

        public MainWindow()
        {
            Utils.Log.Start();
            InitializeComponent();

            OnScreenTiles = new List<Polygon>();
            OnScreenPaths = new List<Line>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_activeProblem == null)
            {
                _activeProblem = new HexagonalTileSearchProblem(10, 10, 0.5);
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
                this.OnScreenTiles.ElementAt(this._activeProblem.Start.ID).Fill = System.Windows.Media.Brushes.LightGreen;
                this.OnScreenTiles.ElementAt(this._activeProblem.Goal.ID).Fill = System.Windows.Media.Brushes.LightCoral;

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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ApplyBreadthFirstSearch();
        }

        private void ApplyBreadthFirstSearch()
        {
            if(this._activeProblem != null)
            {
                SearchResults sr = BreadthFirstSearch.Search(_activeProblem);
                if(sr.Solved)
                {
                    foreach (MapTile mt in sr.Path)
                    {
                        this.OnScreenTiles.ElementAt(mt.ID).Fill = System.Windows.Media.Brushes.CornflowerBlue;
                        this.OnScreenTiles.ElementAt(mt.ID).InvalidateVisual();
                    }
                }
            }
        }
    }
}
