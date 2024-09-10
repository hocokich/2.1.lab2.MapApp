using GMap.NET.WindowsPresentation;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace lab2
{
    internal class CPerson : CMapObject
    {
        PointLatLng location;
        GMapMarker marker;

        PointLatLng destination;

        public void CarArrived(object sender, EventArgs e)
        {
            //TODO: Сесть в машину
        }

        public CPerson(string title, string picName, PointLatLng location) : base(title)
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
    }
}
