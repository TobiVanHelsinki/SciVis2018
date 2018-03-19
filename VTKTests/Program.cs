using System;
using System.IO;
using System.Linq;
using System.Text;
using Kitware.VTK;
using static SciVis.Helper;
namespace SciVis
{
    public static class Helper
    {
        public static void Display(string format, params object[] args)
        {
            Console.WriteLine(String.Format(format, args));
        }
    }
    public class Program
    {
        public static string File = @"c:\Users\Tobiv\Neu\scivis\oceans11.lanl.gov\deepwaterimpact\yA31\300x300x300-AllScalars_resolution\pv_insitu_300x300x300_19021.vti";

        static void Main(string[] args)
        {
            Test_vti_Data();
            //ReadImageData(fileName);
            //Display_Image_Data(fileName);
            Console.ReadKey();
        }
        #region Meteor File Stuff
        private static void Test_vti_Data()
        {
            vtkXMLFileReadTester Tester = new vtkXMLFileReadTester();
            Tester.SetFileName(File);
            Display("Normal can read: {0}, type: {1}, date: {2}", Tester.TestReadFile(), Tester.GetFileVersion(), Tester.GetFileDataType());

            vtkXMLFileReadTester Tester2 = vtkXMLFileReadTester.New();
            Tester2.SetFileName(File);
            Display("staticnew can read: {0}, type: {1}, date: {2}", Tester2.TestReadFile(), Tester2.GetFileVersion(), Tester2.GetFileDataType());
            
            var currentReader = vtkXMLImageDataReader.New();
            currentReader.SetFileName(File);
            var a = currentReader.CanReadFile(File);
            Display("reader NumberOfPoints: {0}, release date: {1}, Step: {2}, StepRange: {3}", currentReader.GetNumberOfPoints(), currentReader.GetReleaseDataFlag(), currentReader.GetTimeStep(), currentReader.GetTimeStepRange());
            for (int i = 0; i < currentReader.GetNumberOfPointArrays(); i++)
            {
                Display("PointArray{0} Name:{1}",i, currentReader.GetPointArrayName(i));
            }
            var selec = currentReader.GetPointDataArraySelection();
            //Display("PointArray{0} Name:{1}",selec.);
        }

        public static void ReadImageData()
        {
            // reader
            // Read all the data from the file
            vtkXMLImageDataReader reader = vtkXMLImageDataReader.New();
            if (reader.CanReadFile(File) == 0)
            {
                Display("Cannot read file \"" + File + "\"", "Error");
                //return;
            }
            reader.SetFileName(File);
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

        public static void Display_Image_Data()
        {
            vtkXMLImageDataReader reader = vtkXMLImageDataReader.New();
            if (reader.CanReadFile(File) == 0)
            {
                //MessageBox.Show("Cannot read file \"" + filePath + "\"", "Error", MessageBoxButtons.OK);
                //return;
            }
            reader.SetFileName(File);
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
