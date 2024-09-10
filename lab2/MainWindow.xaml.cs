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
//Работа с картой
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;


namespace lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<PointLatLng> points = new List <PointLatLng>();
        GMapMarker lastPath = null;
        GMapMarker lastArea = null;

        List<CMapObject> objects = new List<CMapObject>();


        public MainWindow()
        {
            InitializeComponent();
        }

        private void MapLoaded(object sender, RoutedEventArgs e)
        {
            // настройка доступа к данным
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            // установка провайдера карт
            //Map.MapProvider = BingMapProvider.Instance;
            Map.MapProvider = GMapProviders.GoogleMap;
            //Map.MapProvider = YandexMapProvider.Instance;

            // установка зума карты
            Map.MinZoom = 2;
            Map.MaxZoom = 17;
            Map.Zoom = 15;
            // установка фокуса карты

            Map.Position = new PointLatLng(55.013185, 82.950809);

            // настройка взаимодействия с картой
            Map.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            Map.CanDragMap = true;
            Map.DragButton = MouseButton.Left;

            
        }

        /*private void addMarker(string ToolTip, string picName)
        {
            GMapMarker marker = new GMapMarker(points[0])
            {
                Shape = new Image
                {
                    Width = 32, // ширина маркера
                    Height = 32, // высота маркера
                    ToolTip = ToolTip, // всплывающая подсказка
                    Source = new BitmapImage(new Uri("pack://application:,,,/pics/" + picName)) // картинка
                }
            };

            Map.Markers.Add(marker);
            points.Clear();
        }
        private void addArea()
        {

        }*/
        private void addPath()
        {
            if (points.Count < 2)
                return;
            GMapMarker marker = new GMapRoute(points)
            {
                Shape = new Path()
                {
                    Stroke = Brushes.DarkBlue, // цвет обводки
                    Fill = Brushes.DarkBlue, // цвет заливки
                    StrokeThickness = 4 // толщина обводки
                }
            };
            if(lastPath != null)
                Map.Markers.Remove(lastPath);

            lastPath = marker;
            Map.Markers.Add(marker);
        }

        private void mrb_click(object sender, MouseButtonEventArgs e)
        {
            //points.Add(Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y));

            var locations = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);

            switch (type.SelectedIndex){
                case 0:
                    objects.Add(new CPerson(mName.Text, "goblin.png", locations));
                    Map.Markers.Add(objects[objects.Count - 1].getMarker());
                    break;
                case 1:
                    objects.Add(new CCar(mName.Text, "car.png", locations));
                    Map.Markers.Add(objects[objects.Count - 1].getMarker());
                    break;
                case 2:
                    objects.Add(new CLocation(mName.Text, "location.png", locations));
                    Map.Markers.Add(objects[objects.Count - 1].getMarker());
                    break;
                case 3:
                    points.Add(Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y));
                    addPath();
                    break;
                case 4:
                    //addMarker("Area", "");
                    break;
                default: MessageBox.Show("Неопределен.");
                    break;
            }
        }

        private void area_Click(object sender, RoutedEventArgs e)
        {
            if (points.Count < 3)
                return;
            GMapMarker marker = new GMapPolygon(points)
            {
                Shape = new Path
                {
                    Stroke = Brushes.Black, // стиль обводки
                    Fill = Brushes.Violet, // стиль заливки
                    Opacity = 0.2 // прозрачность
                }
            };
            Map.Markers.Add(marker);
            points.Clear();
        }

        private void type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lastPath = null;
            lastArea = null;
            points.Clear();
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            lastPath = null;
            lastArea = null;
            points.Clear();
            Map.Markers.Clear();
        }

        private void addRoute_Click(object sender, RoutedEventArgs e)
        {
            objects.Add(new CRoute(mName.Text, points));
            if (lastPath != null)
                Map.Markers.Remove(lastPath);

            Map.Markers.Add(objects[objects.Count - 1].getMarker());
        }
    }
}
