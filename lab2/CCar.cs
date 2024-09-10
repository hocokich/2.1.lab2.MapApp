using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace lab2
{
    public class CCar : CMapObject
    {
        PointLatLng location;
        GMapMarker marker;

        CRoute route;
        CPerson person;
        
        // событие прибытия
        public event EventHandler Arrived;

        public CCar(string title, string picName, PointLatLng location) : base(title)
        {
            this.location = location;

            marker = new GMapMarker(location)
            {
                Shape = new Image
                {
                    Width = 32, // ширина маркера
                    Height = 32, // высота маркера
                    ToolTip = title, // всплывающая подсказка
                    Source = new BitmapImage(new Uri("pack://application:,,,/pics/" + picName)) // картинка
                }
            };
        }

        public override PointLatLng getFocus()
        {
            return location;
        }

        public override GMapMarker getMarker()
        {
            return marker;
        }

        public void pessengerSeated(object sender, EventArgs e)
        {

        }

        public void moveByRoute()
        {
            foreach (var point  in route.getLocations())
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    marker.Position = point;
                });
                Thread.Sleep(500);
            }
            Arrived?.Invoke(this, null);
        }

        public void moveTo(PointLatLng endLocation )
        {
            //провайдер новигации
            RoutingProvider routingProvider = GMapProviders.GoogleMap;
            //определение маршрута
            MapRoute route = routingProvider.GetRoute(
                location,//начальная точка 
                endLocation,//конечная 
                false, //поиск по шоссе 
                false, //режим пещехода
                (int)15);
            //получение точек маршрута
            List<PointLatLng> routePoints = route.Points;
            this.route = new CRoute("r", routePoints);

            Thread newThread = new Thread(new ThreadStart(moveByRoute));
            newThread.Start();
        }
    }
}
