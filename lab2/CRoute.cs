using GMap.NET.WindowsPresentation;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Device.Location;
using System.Windows.Input;

namespace lab2
{
    internal class CRoute : CMapObject
    {
        List<PointLatLng> points;
        GMapMarker marker;

        public CRoute(string title, List<PointLatLng> points) : base(title)
        {
            this.points = points;

            if (points.Count < 2)
                return;
            marker = new GMapRoute(points)
            {
                Shape = new Path()
                {
                    Stroke = Brushes.DarkBlue, // цвет обводки
                    Fill = Brushes.DarkBlue, // цвет заливки
                    StrokeThickness = 4 // толщина обводки
                }
            };
        }

        public new double getDistance(PointLatLng point)
        {
            // точки в формате GMap.NET
            PointLatLng p1 = new PointLatLng(55.015104, 82.948034);
            PointLatLng p2 = new PointLatLng(55.018812, 82.940049);
            // точки в формате System.Device.Location
            GeoCoordinate c1 = new GeoCoordinate(p1.Lat, p2.Lng);
            GeoCoordinate c2 = new GeoCoordinate(p2.Lat, p2.Lng);
            // вычисление расстояния между точками в метрах
            double distance = c1.GetDistanceTo(c2);

            return distance;
        }

        public override PointLatLng getFocus()
        {
            return points[points.Count / 2];
        }

        public override GMapMarker getMarker()
        {
            return marker;
        }

        public void createRoute(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
