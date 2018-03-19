using System;
using System.IO;
using System.Linq;
using System.Text;
using Kitware.VTK;

namespace VTKTests
{
    class Program
    {

        static void Main(string[] args)
        {
            switch (args.Count() > 0 ? args[0] : "")
            {
                case "sphere":
                    Example_3DSphere();
                    break;
                case "readtest":
                    try
                    {
                        Example_ReadMeteorFile();
                    }
                    catch (Exception ex)
                    {
                    }
                    break;
                default:
                    break;
            }
            //Example_3DSphere();
            Console.ReadKey();
        }
        #region Example_ReadMeteorFile
        private static void Example_ReadMeteorFile()
        {
            //string fileName = "c:\\Users\\Tobiv\\Neu\\pv_insitu_300x300x300_00000.vtk";
            string fileName = @"c:\Users\Tobiv\Neu\scivis\oceans11.lanl.gov\deepwaterimpact\yA31\300x300x300-AllScalars_resolution\pv_insitu_300x300x300_19021.vti";
            //string fileName = "c:/Users/Tobiv/Neu/pv_insitu_300x300x300_00000.vti";
            //ReadImageData(fileName);
            Display_Image_Data(fileName);
            string fileContent = IO.standardIO.ReadFile_TextContent(fileName);
            //Set the reader
            var currentReader = Kitware.VTK.vtkXMLImageDataReader.New();
            //var currentReader = Kitware.VTK.vtkXMLFileReadTester.New();
            //var currentReader = Kitware.VTK.vtkXMLParser.New();

            //currentReader.SetFileName(fileName);

            //reader.Update();

            //Do some Test Stuff with this reader
            Console.WriteLine("Statistics!");
            Console.WriteLine("IsA(\"ImageData\"): " + currentReader.IsA("ImageData"));
            //Console.WriteLine("FileVersion: " + currentReader.GetFileVersion());
            Console.WriteLine("GetName: " + currentReader.GetFileName());
            //Console.WriteLine("Parse(): " + currentReader.Parse());
            //Console.WriteLine("Parse(filecontent): " + currentReader.Parse(fileContent));
        }

        private static void ReadImageData(string filePath)
        {
            // Path to vtk data must be set as an environment variable
            // VTK_DATA_ROOT = "C:\VTK\vtkdata-5.8.0"
            //vtkTesting test = vtkTesting.New();
            //string root = test.GetDataRoot();
            //filePath = System.IO.Path.Combine(root, @"Data\vase_1comp.vti");



            // reader
            // Read all the data from the file
            vtkXMLImageDataReader reader = vtkXMLImageDataReader.New();
            if (reader.CanReadFile(filePath) == 0)
            {
                //MessageBox.Show("Cannot read file \"" + filePath + "\"", "Error", MessageBoxButtons.OK);
                //return;
            }
            reader.SetFileName(filePath);
            reader.Update(); // here we read the file actually

            // mapper
            vtkDataSetMapper mapper = vtkDataSetMapper.New();
            mapper.SetInputConnection(reader.GetOutputPort());

            // actor
            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);
            actor.GetProperty().SetRepresentationToWireframe();

            // get a reference to the renderwindow of our renderWindowControl1
            vtkRenderWindow renderWindow = vtkRenderWindow.New();
            // renderer
            var renderers = renderWindow.GetRenderers();
            vtkRenderer renderer = renderers.GetFirstRenderer();
            if (renderer == null)
            {
                vtkRenderer n = new vtkOpenGLRenderer();
                renderWindow.AddRenderer(n);
                renderer = renderers.GetFirstRenderer();
            }
            // set background color
            renderer.SetBackground(0.2, 0.3, 0.4);
            // add our actor to the renderer
            renderer.AddActor(actor);

        }

        private static void Display_Image_Data(string filePath)
        {
            vtkXMLImageDataReader reader = vtkXMLImageDataReader.New();
            if (reader.CanReadFile(filePath) == 0)
            {
                //MessageBox.Show("Cannot read file \"" + filePath + "\"", "Error", MessageBoxButtons.OK);
                //return;
            }
            reader.SetFileName(filePath);
            reader.Update(); // here we read the file actually

            // mapper

            var shrink = vtkShrinkPolyData.New();
            vtkDataSetMapper mapper = vtkDataSetMapper.New();

            shrink.SetInputConnection(reader.GetOutputPort());
            mapper.SetInputConnection(reader.GetOutputPort());
            shrink.SetShrinkFactor(0.9);

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
