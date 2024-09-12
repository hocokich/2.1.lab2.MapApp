using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab2
{
    public abstract class CMapObject
    {
        private string title;
        private DateTime CreationDate;

        PointLatLng point;

        public CMapObject(string title, PointLatLng location)
        {
            this.title = title;

            CreationDate = DateTime.Now;

            point = location;

        }

        protected CMapObject(string title)
        {
            this.title = title;
        }

        public string getTitle()
        {
            return title;
        }

        public DateTime getCreationDate()
        {
            return CreationDate;
        }

        public double getDistance(PointLatLng startPoint, PointLatLng endPoint)
        {
            // точки в формате GMap.NET
            PointLatLng p1 = startPoint;
            PointLatLng p2 = endPoint;
            // точки в формате System.Device.Location
            GeoCoordinate c1 = new GeoCoordinate(p1.Lat, p2.Lng);
            GeoCoordinate c2 = new GeoCoordinate(p2.Lat, p2.Lng);
            // вычисление расстояния между точками в метрах
            double distance = c1.GetDistanceTo(c2);

            return distance;
        }

        public abstract PointLatLng getFocus();

        public abstract GMapMarker getMarker();
    }
}
