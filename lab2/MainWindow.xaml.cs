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
using System.Xml.Linq;


namespace lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<PointLatLng> points = new List<PointLatLng>();
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
        }*/

        private void addArea()
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

            if (lastArea != null)
                Map.Markers.Remove(lastArea);

            Map.Markers.Add(marker);
            //points.Clear();
        }
        void addPath()
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
            if (lastPath != null)
                Map.Markers.Remove(lastPath);

            lastPath = marker;
            Map.Markers.Add(marker);
        }
        private void mrb_click(object sender, MouseButtonEventArgs e)
        {
            //points.Add(Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y));

            var location = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);

            switch (type.SelectedIndex)
            {
                case 0:
                    objects.Add(new CPerson(mName.Text, "goblin.png", location));
                    Map.Markers.Add(objects[objects.Count - 1].getMarker());
                    break;
                case 1:
                    objects.Add(new CCar(mName.Text, "car.png", location));
                    Map.Markers.Add(objects[objects.Count - 1].getMarker());
                    break;
                case 2:
                    objects.Add(new CLocation(mName.Text, "location.png", location));
                    Map.Markers.Add(objects[objects.Count - 1].getMarker());
                    break;
                case 3:
                    points.Add(Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y));
                    addPath();
                    break;
                case 4:
                    points.Add(Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y));
                    addArea();
                    break;
                default:
                    MessageBox.Show("Неопределен.");
                    break;
            }
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
            searchedMarks.Clear();

            objects.Clear();

        }

        private void search(object sender, MouseButtonEventArgs e)
        {
            if (objects.Count < 1)
                return;

            var location = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);

            //Список расстояния всех точек
            List<double> allPoints = new List<double>();

            string allPointsMsg = "Все метки: " + "\n";
            for (var i = 0; i < objects.Count; i++)
            {
                //Находит расстояние не от ближайшей точки, а от перекрестия карты
                //Вроде починил, добавил гетфокус чтобы он оттуда брал координаты метки
                double complement = objects[i].getDistance(location, objects[i].getFocus());

                allPoints.Add(complement);

                allPointsMsg += objects[i].getTitle() + ": " + (int)complement + "м.\n";
            }
            allPoints.Sort();

            //CMapObject closeMark = map[allPoints[0]];

            MessageBox.Show("Ближайшая точка: " + (int)allPoints[0] + "м. \n" + "\n" + allPointsMsg);

            searchedMarks.Text = "Ближайшая точка: " + (int)allPoints[0] + "м. \n" + "\n" + allPointsMsg;

        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            if(objects.Count < 1) return;

            //var location = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);

            //Счетчик колличества одинаковых имен
            var j = 0;
            var firstMarkIndex = 0;

            //Список одинаковых элементов
            List<int> allPoints = new List<int>();

            for (var i = 0; i < objects.Count; i++)
            {
                if(objects[i].getTitle() == mNameSearch.Text)
                {
                    firstMarkIndex = i;
                    allPoints.Add(i);
                    j++;
                }
            }
            if (j > 1)
            {
                allPoints.Sort();
                Map.Position = objects[firstMarkIndex].getFocus();
                MessageBox.Show("Первая найденая метка.\n\nДругие найденые метки в списке справа.");

                searchedMarks.Text = "Похожих точек: " + j + ".\n\nРасстояние от найденой до других:\n";
                for(var i = 0; i < allPoints.Count-1; i++)
                {
                    searchedMarks.Text += i+1 + ". " + (int)objects[firstMarkIndex].getDistance(objects[firstMarkIndex].getFocus(), objects[allPoints[i]].getFocus()) + "м.\n";
                }
                return;
            }
            if (j == 1)//Если такая метка одна, то зумим её на карте
            {
                MessageBox.Show("Ваша метка.");
                Map.Position = objects[firstMarkIndex].getFocus();
                return;
            }
        }

        //Функция чисто для теста
        public int[] TwoSum(int[] nums, int target)
        {

            Dictionary<int, int> map = new Dictionary<int, int>(); // создание словаря

            for (int i = 0; i < nums.Length; i++)
            { // цикл по массиву

                int complement = target - nums[i]; // вычисление разности между целевым значением и текущим элементом массива

                if (map.ContainsKey(complement))
                { // проверка наличия элемента-дополнения в словаре
                    return new int[] { map[complement], i }; // возвращение пары индексов, если элемент-дополнение найден
                }
                map[nums[i]] = i; // добавление текущего элемента в словарь
            }
            throw new ArgumentException("No two sum solution"); // выброс исключения, если решение не найдено
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
