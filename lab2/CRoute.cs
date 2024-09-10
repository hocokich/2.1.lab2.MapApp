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

namespace lab2
{
    internal class CRoute : CMapObject
    {
        List<PointLatLng> locations;

        GMapMarker marker;

        public CRoute(string title, List<PointLatLng> locations) : base(title)
        {
            this.locations = locations;

            if (locations.Count < 2)
                return;
            marker = new GMapRoute(locations)
            {
                Shape = new Path()
                {
                    Stroke = Brushes.DarkBlue, // цвет обводки
                    Fill = Brushes.DarkBlue, // цвет заливки
                    StrokeThickness = 4 // толщина обводки
                }
            };
        }

        public List<PointLatLng> getLocations()
        {
            return locations;
        }

        public override PointLatLng getFocus()
        {
            return locations[locations.Count/2];
        }

        public override GMapMarker getMarker()
        {
            return marker;
        }
    }
}
