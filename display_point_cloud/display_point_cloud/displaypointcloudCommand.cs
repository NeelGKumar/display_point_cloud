using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using System.IO;

namespace display_point_cloud
{
    public class displaypointcloudCommand : Command
    {
        public displaypointcloudCommand()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static displaypointcloudCommand Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "displaypointcloudCommand"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {           
            StreamReader reader = File.OpenText("point_cloud.txt");
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split(null);
                double x = double.Parse(items[0]);
                double y = double.Parse(items[1]);
                double z = double.Parse(items[2]);
                double w = double.Parse(items[3]);

                Point3d point = new Point3d(x, y, z);
                Sphere sp = new Sphere(point, Math.Abs(w));

                var attributes = new Rhino.DocObjects.ObjectAttributes();              
                
                if (w > 0)
                    attributes.ObjectColor = System.Drawing.Color.Blue;
                else
                    attributes.ObjectColor = System.Drawing.Color.Gray;

                attributes.ColorSource = Rhino.DocObjects.ObjectColorSource.ColorFromObject;                
                doc.Objects.AddSphere(sp, attributes);

            }
            
            doc.Views.Redraw();

            return Result.Success;
        }
    }
}
