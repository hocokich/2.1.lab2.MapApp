using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public abstract class CMapObject
    {
        private string title;
        private DateTime CreationDate;

        public CMapObject(string title)
        {
            this.title = title;
            CreationDate = DateTime.Now;
        } 

        public string getTitle()
        {
            return title;
        }
        public DateTime getCreationDate()
        {
            return CreationDate;
        }
        public abstract PointLatLng getFocus();
        public abstract GMapMarker getMarker();
        
        /*double getDistance(PointLatLng point)
        {
            return double;
        }*/
    }
}
