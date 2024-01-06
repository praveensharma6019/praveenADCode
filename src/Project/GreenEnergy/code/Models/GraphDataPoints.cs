using System;
using System.Runtime.Serialization;

namespace Sitecore.GreenEnergy.Website.Models
{
    [DataContract]
    public class GraphDataPoints
    {
        [DataMember(Name = "x")]
        public double? x = null;

        [DataMember(Name = "y")]
        public double? Y = null;

        public GraphDataPoints(double x, double y)
        {
            this.x = new double?(x);
            this.Y = new double?(y);
        }
    }
}