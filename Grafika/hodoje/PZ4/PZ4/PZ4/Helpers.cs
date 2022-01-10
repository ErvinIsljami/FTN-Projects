using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace PZ4
{
    public class Helpers
    {
        public static GeometryModel3D GenerateCubeGeometryModel(EntityType entityType, Colors color)
        {
            // Will create a cube in center of the map
            var geoMod3d = new GeometryModel3D();
            var meshGeo3d = new MeshGeometry3D();

            meshGeo3d.Positions.Add(new Point3D(0, 0, 0.0005));
            meshGeo3d.Positions.Add(new Point3D(0.015, 0, 0.0005));
            meshGeo3d.Positions.Add(new Point3D(0, 0.015, 0.0005));
            meshGeo3d.Positions.Add(new Point3D(0.015, 0.015, 0.0005));
            meshGeo3d.Positions.Add(new Point3D(0, 0, 0.015));
            meshGeo3d.Positions.Add(new Point3D(0.015, 0, 0.015));
            meshGeo3d.Positions.Add(new Point3D(0, 0.015, 0.015));
            meshGeo3d.Positions.Add(new Point3D(0.015, 0.015, 0.015));

            //switch (entityType)
            //{
            //    case EntityType.Substation:
            //        meshGeo3d.Positions.Add(new Point3D(0, 0, 0.0005));
            //        meshGeo3d.Positions.Add(new Point3D(0.015, 0, 0.0005));
            //        meshGeo3d.Positions.Add(new Point3D(0, 0.015, 0.0005));
            //        meshGeo3d.Positions.Add(new Point3D(0.015, 0.015, 0.0005));
            //        meshGeo3d.Positions.Add(new Point3D(0, 0, 0.015));
            //        meshGeo3d.Positions.Add(new Point3D(0.015, 0, 0.015));
            //        meshGeo3d.Positions.Add(new Point3D(0, 0.015, 0.015));
            //        meshGeo3d.Positions.Add(new Point3D(0.015, 0.015, 0.015));
            //        break;

            //    case EntityType.Node:
            //        meshGeo3d.Positions.Add(new Point3D(0, 0, 0.0005));
            //        meshGeo3d.Positions.Add(new Point3D(0.01, 0, 0.0005));
            //        meshGeo3d.Positions.Add(new Point3D(0, 0.01, 0.0005));
            //        meshGeo3d.Positions.Add(new Point3D(0.01, 0.01, 0.0005));
            //        meshGeo3d.Positions.Add(new Point3D(0, 0, 0.015));
            //        meshGeo3d.Positions.Add(new Point3D(0.01, 0, 0.015));
            //        meshGeo3d.Positions.Add(new Point3D(0, 0.01, 0.015));
            //        meshGeo3d.Positions.Add(new Point3D(0.01, 0.01, 0.015));
            //        break;

            //    case EntityType.Switch:
            //        meshGeo3d.Positions.Add(new Point3D(0, 0, 0.0005));
            //        meshGeo3d.Positions.Add(new Point3D(0.005, 0, 0.0005));
            //        meshGeo3d.Positions.Add(new Point3D(0, 0.005, 0.0005));
            //        meshGeo3d.Positions.Add(new Point3D(0.005, 0.005, 0.0005));
            //        meshGeo3d.Positions.Add(new Point3D(0, 0, 0.015));
            //        meshGeo3d.Positions.Add(new Point3D(0.005, 0, 0.015));
            //        meshGeo3d.Positions.Add(new Point3D(0, 0.005, 0.015));
            //        meshGeo3d.Positions.Add(new Point3D(0.005, 0.005, 0.015));
            //        break;
            //}
            meshGeo3d.TriangleIndices.Add(2);
            meshGeo3d.TriangleIndices.Add(3);
            meshGeo3d.TriangleIndices.Add(1);

            meshGeo3d.TriangleIndices.Add(2);
            meshGeo3d.TriangleIndices.Add(1);
            meshGeo3d.TriangleIndices.Add(0);

            meshGeo3d.TriangleIndices.Add(7);
            meshGeo3d.TriangleIndices.Add(1);
            meshGeo3d.TriangleIndices.Add(3);

            meshGeo3d.TriangleIndices.Add(7);
            meshGeo3d.TriangleIndices.Add(5);
            meshGeo3d.TriangleIndices.Add(1);

            meshGeo3d.TriangleIndices.Add(6);
            meshGeo3d.TriangleIndices.Add(5);
            meshGeo3d.TriangleIndices.Add(7);

            meshGeo3d.TriangleIndices.Add(6);
            meshGeo3d.TriangleIndices.Add(4);
            meshGeo3d.TriangleIndices.Add(5);

            meshGeo3d.TriangleIndices.Add(6);
            meshGeo3d.TriangleIndices.Add(2);
            meshGeo3d.TriangleIndices.Add(4);

            meshGeo3d.TriangleIndices.Add(2);
            meshGeo3d.TriangleIndices.Add(0);
            meshGeo3d.TriangleIndices.Add(4);

            meshGeo3d.TriangleIndices.Add(2);
            meshGeo3d.TriangleIndices.Add(7);
            meshGeo3d.TriangleIndices.Add(3);

            meshGeo3d.TriangleIndices.Add(2);
            meshGeo3d.TriangleIndices.Add(6);
            meshGeo3d.TriangleIndices.Add(7);

            meshGeo3d.TriangleIndices.Add(0);
            meshGeo3d.TriangleIndices.Add(1);
            meshGeo3d.TriangleIndices.Add(5);

            meshGeo3d.TriangleIndices.Add(0);
            meshGeo3d.TriangleIndices.Add(5);
            meshGeo3d.TriangleIndices.Add(4);
            geoMod3d.Geometry = meshGeo3d;

            var diffMat = new DiffuseMaterial();
            switch (color)
            {
                case Colors.Green:
                    diffMat.Brush = Brushes.Green;
                    break;

                case Colors.Red:
                    diffMat.Brush = Brushes.Red;
                    break;

                case Colors.Black:
                    diffMat.Brush = Brushes.Black;
                    break;
            }
            geoMod3d.Material = diffMat;
            return geoMod3d;
        }
    }
}
