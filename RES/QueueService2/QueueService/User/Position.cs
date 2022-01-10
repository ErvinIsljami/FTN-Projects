using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace QueueService.User
{
    public class Position
    {
        private double x;
        private double y;
        private double z;

        public Position()
        {
        }

        public Position(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        [Key, Column(Order = 0)]
        public double X { get => x; set => x = value; }
        [Key, Column(Order = 1)]
        public double Y { get => y; set => y = value; }
        [Key, Column(Order = 2)]
        public double Z { get => z; set => z = value; }
    }
}
