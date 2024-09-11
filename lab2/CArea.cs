using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace lab2
{
    internal class CArea : CMapObject
    {
        List<PointLatLng> points;

        GMapMarker marker;


        public CArea(string title, List<PointLatLng> points) : base(title)
        {
            /*this.points = points;

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
            };*/
        }

        public override PointLatLng getFocus()
        {
            throw new NotImplementedException();
        }

        public override GMapMarker getMarker()
        {
            throw new NotImplementedException();
        }
    }
}
