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
        List<PointLatLng> point = new List <PointLatLng>();

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

        private void addMarker(string ToolTip, string picName)
        {

            GMapMarker marker = new GMapMarker(point)
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
        }

        private void mrb_click(object sender, MouseButtonEventArgs e)
        {
            point.Add(Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y));

            switch (type.SelectedIndex){
                case 0:
                    addMarker("Person", "goblin_5.png");
                    break;
                case 1:
                    addMarker("Car", "car.png");
                    break;
                case 2:
                    addMarker("Place", "potion_2.png");
                    break;
                case 3:
                    addMarker("Route", "");
                    break;
                case 4:
                    addMarker("Area", "");
                    break;
                default: MessageBox.Show("Неопределен.");
                    break;
            }
        }
    }
}
