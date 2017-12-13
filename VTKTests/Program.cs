using System;
using System.IO;
using System.Text;
using Kitware.VTK;

namespace VTKTests
{
    class Program
    {

        static void Main(string[] args)
        {
            Example_3DSphere();
            Example_ReadMeteorFile();

        }
        #region Example_3DSphere
        private static void Example_ReadMeteorFile()
        {
            var reader = Kitware.VTK.vtkXMLFileReadTester.New();
            //var reader = Kitware.VTK.vtkXMLParser.New();
            //reader.set
            reader.SetFileName("c:\\Users\\Tobiv\\Neu\\scivis\\oceans11.lanl.gov\\deepwaterimpact\\yA31\\300x300x300-AllScalars_resolution\\pv_insitu_300x300x300_00000.vtk");
            FileStream file = File.OpenRead("c:\\Users\\Tobiv\\Neu\\pv_insitu_300x300x300_00000.vtk");
            StreamReader filereader = new StreamReader(file);
            StringBuilder builder = new StringBuilder();
            while (true)
            {
                string line = filereader.ReadLine();
                if (line == null)
                {
                    break;
                }
                builder.Append(line);
            }
            string filecontent = builder.ToString();
            //reader.Update();
            var read = reader.Parse(filecontent);
            var z = reader.IsA("ImageData");
            Console.WriteLine("statistics!");
            string fileversion = reader.GetFileVersion();
            var name = reader.GetFileName();
            var parseres = reader.Parse();
        }
        #endregion
        #region Example_3DSphere
        static vtkSphereSource sphere;
        static vtkShrinkPolyData shrink;
        static vtkPolyDataMapper mapper;
        static vtkActor actor;
        static vtkRenderer ren1;
        static vtkRenderWindow renWin;
        static vtkRenderWindowInteractor iren;
        static vtkCamera camera;

        private static void Example_3DSphere()
        {
            // Create a simple sphere. A pipeline is created.         
            var sphere = new vtkSphereSource();
            sphere = vtkSphereSource.New();
            sphere.SetThetaResolution(8);
            sphere.SetPhiResolution(16);
            var shrink = vtkShrinkPolyData.New();
            shrink.SetInputConnection(sphere.GetOutputPort());
            shrink.SetShrinkFactor(0.9);

            var mapper = vtkPolyDataMapper.New();
            mapper.SetInputConnection(shrink.GetOutputPort());

            // The actor links the data pipeline to the rendering subsystem         
            var actor = vtkActor.New();
            actor.SetMapper(mapper);
            actor.GetProperty().SetColor(1, 0, 0);

            // Create components of the rendering subsystem         //         
            var ren1 = vtkRenderer.New();
            var renWin = vtkRenderWindow.New();
            renWin.AddRenderer(ren1);
            var iren = vtkRenderWindowInteractor.New();
            iren.SetRenderWindow(renWin);

            // Add the actors to the renderer, set the window size         //         
            ren1.AddViewProp(actor);
            renWin.SetSize(250, 250);
            renWin.Render();
            var camera = ren1.GetActiveCamera();
            camera.Zoom(1.5);

            // render the image and start the event loop         //         
            renWin.Render();
            iren.Initialize();
            iren.Start();

            deleteAllVTKObjects();
        }

        public static void deleteAllVTKObjects()
        {         //clean up vtk objects         
            if (sphere != null)
            {
                sphere.Dispose();
            }
            if (shrink != null)
            {
                shrink.Dispose();
            }
            if (mapper != null)
            {
                mapper.Dispose();
            }
            if (actor != null)
            {
                actor.Dispose();
            }
            if (ren1 != null)
            {
                ren1.Dispose();
            }
            if (renWin != null)
            {
                renWin.Dispose();
            }
            if (iren != null)
            {
                iren.Dispose();
            }
            if (camera != null)
            {
                camera.Dispose();
            }
        }
        #endregion
    }
}
