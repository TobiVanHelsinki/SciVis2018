using Kitware.VTK;
using System;
using static SciVis.Helper;

namespace SciVis
{

    public class Program
    {
        public static string File = @"c:\Users\Tobiv\Neu\scivis\oceans11.lanl.gov\deepwaterimpact\yA31\300x300x300-AllScalars_resolution\pv_insitu_300x300x300_19021.vti";

        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    vtkImageData Data = null;
                    try
                    {
                        Data = ReadInData(File);
                    }
                    catch (Exception ex)
                    {
                        Display("Error Reading: ", ex);
                    }
                    try
                    {
                        Analyse(Data);
                    }
                    catch (Exception ex)
                    {
                        Display("Error Analysing: ", ex);
                    }
                    
                    //Test_vti_Data();
                    //ReadImageData(File);
                    //Display_Image_Data(File);
                }
                catch (Exception ex)
                {
                    Display("Programm Error: ", ex);
                    System.Diagnostics.Debugger.Break();
                }
            Console.ReadKey();
            }
            Console.ReadKey();
        }
        public static vtkImageData ReadInData(string FileName)
        {
            vtkXMLImageDataReader Reader = new vtkXMLImageDataReader();
            Reader.InitializeObjectBase();
            Reader.SetFileName(FileName);
            if (Reader.CanReadFile(FileName) == 0)
            {
                throw new Exception("Cannot Read File");
            }
            Reader.Update();
            return Reader.GetOutput();
        }
        public static void Analyse(vtkImageData FileContent)
        {
            MeteorData Data = new MeteorData(FileContent.GetPointData());
            float lastitem = 0;
            foreach (var item in Data.rho)
            {
                if (lastitem != item)
                {
                    lastitem = item;
                }
            }
        }
        #region Meteor File Test Stuff
        private static void Test_vti_Data()
        {
            vtkXMLFileReadTester Tester = new vtkXMLFileReadTester();
            Tester.InitializeObjectBase();
            Tester.SetFileName(File);
            Display("Normal can read: {0}, type: {1}, date: {2}", Tester.TestReadFile(), Tester.GetFileVersion(), Tester.GetFileDataType());
            //Display("Normal InitializeParser {0}", Tester.InitializeParser());
            //try
            //{
            //    //Display("Normal Parse {0}", Tester.Parse());
            //}
            //catch (Exception ex)
            //{
            //    Display("Normal Parsing Error: ", ex);
            //}

            var currentReader = vtkXMLImageDataReader.New();
            currentReader.InitializeObjectBase();
            //vtkCommand X = new vtkCommandTobi();
            //X.InitializeObjectBase();
            //currentReader.SetReaderErrorObserver(X);
            currentReader.SetFileName(File);
            Display("Reader HasExecutive: {0}, CanReadFile {1}", currentReader.HasExecutive(), currentReader.CanReadFile(File));
            currentReader.Update(); // here we read the file actually

            var exe = currentReader.GetExecutive();
            var xmlparser = currentReader.GetXMLParser();
            Display("xmlparser.InitializeParser {0}", xmlparser?.InitializeParser());
            Display("xmlparser.Parse {0}", xmlparser?.Parse());
            Display("xmlparser.Progress {0}", xmlparser?.GetProgress());

            Display("reader NumberOfPoints: {0}, release date: {1}, Step: {2}, StepRange: {3}", currentReader.GetNumberOfPoints(), currentReader.GetReleaseDataFlag(), currentReader.GetTimeStep(), currentReader.GetTimeStepRange());
            for (int i = 0; i < currentReader.GetTimeStepRange().Length; i++)
            {
                Display("TimeStepRange {0}: {1}", i, currentReader.GetTimeStepRange()[i]);
            }
            currentReader.UpdateDataObject();

            for (int i = 0; i < currentReader.GetNumberOfPointArrays(); i++)
            {
                Display("PointArray{0} Name:{1}, NoEl:{2}", i, currentReader.GetPointArrayName(i), currentReader.GetOutput(i)?.GetNumberOfCells());
            }
            //currentReader.num
            var outputdataset = currentReader.GetOutputAsDataSet(0);
            var pointx= outputdataset?.GetPoint(123459);
            var ncell = outputdataset?.GetNumberOfCells();
            var nel = outputdataset?.GetNumberOfElements(0);
            var npoints = outputdataset?.GetNumberOfPoints();
            Display("DataSet: Cells: {0}, Points: {2}, \"0\"?: {1}",ncell, nel, npoints);

            var outputdataobj = currentReader.GetOutputDataObject(0);
            Display("DataObj: 0: {0}, 1: {2}, 2: {1}", outputdataobj?.GetNumberOfElements(0), outputdataobj?.GetNumberOfElements(1), outputdataobj?.GetNumberOfElements(2));

            var output = currentReader.GetOutput(0);
            ncell = output.GetNumberOfCells();
            nel = output.GetNumberOfElements(0);
            var nel1 = output.GetNumberOfElements(1);
            npoints = output.GetNumberOfPoints();
            Display("output: Cells: {0}, Points: {2}, \"0\"?: {1}, \"1\"?: {3}", ncell, nel, npoints, nel1);

            var m = output.GetDataDimension();
            var n = output.GetDataObjectType();
            var o = output.GetCellData();
            var p = output.GetFieldData();
            var q = output.GetPointData();
            var aa = p.GetAbstractArray(0);
            var ab = p.GetNumberOfTuples();

            var PointData = output?.GetPointData();
            var arraynum = PointData?.GetNumberOfArrays();
            var sc = PointData?.GetScalars();
            var vec = PointData?.GetVectors();
            var ten = PointData?.GetTensors();
            var abstarray = PointData.GetAbstractArray(4);
            var valnum = abstarray.GetNumberOfValues();
            var tupnum = abstarray.GetNumberOfTuples();
            var value = abstarray.GetVariantValue(12345);
            var floatval = value.ToFloat();
            for (int i = 0; i < valnum; i+=100)
            {
                Display("{0}:{1}",i,abstarray.GetVariantValue(i).ToFloat());
            }
            currentReader.Dispose();
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
