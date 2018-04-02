using Kitware.VTK;
using SciVis.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static SciVis.Helper;

namespace SciVis
{

    class Program
    {
        static string PathTobi = @"c:\Users\Tobiv\Neu\scivis\oceans11.lanl.gov\deepwaterimpact\yA31\300x300x300-AllScalars_resolution\";
        static string PathGregor = @"C:\Store\WasserVTK\";
        static string Path = PathTobi;
        static string FileName = @"pv_insitu_300x300x300_03429.vti"; 
        static string File { get => Path + FileName; }

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                FileName = args[0];
            }

            vtkOutputWindow.SetGlobalWarningDisplay(0);
            //if (Debugger.IsAttached)
            //{
            //    Console.SetWindowSize(Console.WindowWidth - 20, Console.WindowHeight);
            //    Console.SetBufferSize(Console.WindowWidth, Console.BufferHeight);
            //}

            vtkImageData Data = null;
            try
            {
                Data = IO.VTK_IO.ReadInData(File);
            }
            catch (Exception ex)
            {
                Display("Error Reading: ", ex);
                Console.Beep(500, 1500);
                if (Debugger.IsAttached) Debugger.Break();
                return;
            }
            try
            {
                #region Test Stuff
                //Test_vti_Data(File);
                //ReadImageData(File);
                //Display_Image_Data(File);
                #endregion
                #region Tobi Stuff
                //AnalyseMaterials(Data);
                //Analyse(Data);
                //Bericht_Map(Data);
                //AnalyseRho(Data);
                //Analyse_mat(Data);
                #endregion
                #region Gregor Stuff
                BorderScan(Data);
                #endregion

            }
            catch (Exception ex)
            {
                Display("Error Analysing: ", ex);
                Console.Beep(500, 1500);
                if (Debugger.IsAttached) Debugger.Break();
                return;
            }
            Console.Beep(3000, 500);
            if (Debugger.IsAttached) Debugger.Break();
            //Console.ReadKey();
        }

        // Erfasst alle Grenzpunkte eines bestimmten Materials und stellt sie in einer Bordermap fest
        private static void BorderScan(vtkImageData FileContent)
        {
            MeteorData Data = new MeteorData(FileContent.GetPointData());
            List<long> BorderMap = new List<long>();

            //legt Material, nachdem gesucht wird, fest
            float Material = 3;
            bool BodyFound = false;

            for(int x = 0; x <= 300; x++)
            {
                for (int y = 0; y <= 300; y++)
                {
                    for (int z = 0; z <= 300; z++)
                    {
                        var (Index, Value) = Data.mat.GetPoint(x, y, z);
                        if (Material == Value && BodyFound == false)
                        {
                            BorderMap.Add(Index);
                            BodyFound = true;
                        }
                        else if(Material != Value && BodyFound == true)
                        {
                            BorderMap.Add(Index);
                            BodyFound = false;
                        }
                    }
                }
            }
        }

        #region Tobias' Stuff

        static void Analyse_VarStat(vtkImageData FileContent)
        {
            (long Index, Single Value) BiggestValue = (0, 0);
            MeteorData Data = new MeteorData(FileContent.GetPointData());
            BiggestValue = (0, 0);
            foreach (var item in Data.rho)
            {
                if (item.Value > BiggestValue.Value)
                {
                    BiggestValue = item;
                }
            }
            Display("Densest Point at {0} is {1}", BiggestValue.Index, BiggestValue.Value);
            BiggestValue = (0, 0);
            foreach (var item in Data.prs)
            {
                if (item.Value > BiggestValue.Value)
                {
                    BiggestValue = item;
                }
            }
            Display("Preasurest Point at {0} is {1}", BiggestValue.Index, BiggestValue.Value);
        }
        /// <summary>
        /// stellt eine Dichte Analyse nach intressanten Bereichen zusammen
        /// </summary>
        /// <remarks>
        /// Autor: Tobias Pabst
        /// </remarks>
        /// <param name="FileContent"></param>
        static void Analyse_rho(vtkImageData FileContent)
        {
            DisplayRemoveLines();
            MeteorData Data = new MeteorData(FileContent.GetPointData());
            var CurrentList = Data.rho;
            List<(int, int, int, float)> Counter_ValOne = new List<(int, int, int, float)>();
            List<(int, int, int, float)> Counter_ValNearlyOne = new List<(int, int, int, float)>();
            List<(int, int, int, float)> Counter_ValMiddle = new List<(int, int, int, float)>();
            List<(int, int, int, float)> Counter_ValBottom = new List<(int, int, int, float)>();
            List<(int, int, int, float)> Counter_ValHell = new List<(int, int, int, float)>();
            int maxZ = 300;
            for (int z = 0; z < maxZ; z++)
            {
                DisplayProgress("", z);
                //Parallel.For(0, 300,(y) =>
                for (int y = 0; y < 300; y++)
                {
                    for (int x = 0; x < 300; x++)
                    {
                        float val = CurrentList.GetPoint(x, y, z).Value;
                        if (val >= 1)
                        {
                            Counter_ValOne.Add((x, y, z, val));
                        }
                        else if (val >= 0.9 && val < 1)
                        {
                            Counter_ValNearlyOne.Add((x, y, z, val));
                        }
                        else if (val >= 0.1 && val < 0.9)
                        {
                            Counter_ValMiddle.Add((x, y, z, val));
                        }
                        else if (val >= 0.001 && val < 0.1)
                        {
                            Counter_ValBottom.Add((x, y, z, val));
                        }
                        else if (val < 0.001)
                        {
                            Counter_ValHell.Add((x, y, z, val));
                        }
                    }
                }/*);*/
            }
            DisplayRemoveLines();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
            int ProcessedData = Counter_ValOne.Count + Counter_ValNearlyOne.Count + Counter_ValMiddle.Count + Counter_ValBottom.Count + Counter_ValHell.Count;
            Display(CurrentList.Name + "-Werte " + FileName);

            void DisplayAndDrawBounds(string title, List<(int, int, int, float)> Counter_List)
            {
                StringBuilder nv = new StringBuilder();
                foreach (var item in title.Take(10))
                {
                    nv.Append(item);
                }
                nv.Append(' ', 10 - nv.Length);
                string Bounds = String.Format(nv + " Items:{0,8} Upper Bound {1,3} Lower Bound {2,3}", Counter_List.Count(), Counter_List.MinOrDefault(x => x.Item3), Counter_List.MaxOrDefault(x => x.Item3));


                double rel = maxZ > 30 ? 10 : 1;
                double min = Counter_List.MinOrDefault(x => x.Item3);
                double max = Counter_List.MaxOrDefault(x => x.Item3);
                int FirstLength = (int)Math.Round(min / rel, 0);
                int SecondLength = (int)Math.Ceiling((max - min) / rel);
                int ThirdLength = (int)Math.Floor((maxZ - max) / rel);
                if (SecondLength == 0 && min != 0 && max != 0)
                {
                    SecondLength = 1;
                    if (ThirdLength > 0)
                    {
                        ThirdLength--;
                    }
                    else
                    {
                        FirstLength--;
                    }
                }
                if (FirstLength + SecondLength + ThirdLength < 30)
                {
                    ThirdLength++;
                }
                if (FirstLength + SecondLength + ThirdLength > 30)
                {
                    if (ThirdLength != 0)
                    {
                        ThirdLength--;
                    }
                    else
                    {
                        FirstLength--;
                    }
                }
                var b = new StringBuilder();
                b.Append(' ');
                b.Append('|');
                b.Append(' ', FirstLength);
                b.Append('#', SecondLength);
                b.Append(' ', ThirdLength);
                b.Append('|');
                string Border = b.ToString();

                Console.SetBufferSize(Bounds.Length < Console.WindowWidth ? Console.WindowWidth : (Bounds.Length + 10), Console.WindowHeight);
                Display(Bounds + Border);
            }


            DisplayAndDrawBounds(">1", Counter_ValOne);
            DisplayAndDrawBounds("0.9-1", Counter_ValNearlyOne);
            DisplayAndDrawBounds("0.1-0.9", Counter_ValMiddle);
            DisplayAndDrawBounds("0.001-0.1", Counter_ValBottom);
            DisplayAndDrawBounds("<0.001", Counter_ValHell);
            Console.WriteLine();
        }
        /// <summary>
        /// Erstellt eine material verteilungs analyse
        /// </summary>
        /// <remarks>
        /// Autor: Tobias Pabst
        /// </remarks>
        /// <param name="FileContent"></param>
        static void Analyse_mat(vtkImageData FileContent)
        {
            const int LowerZBorder = 0;
            const int HigherZBorder = 300;
            MeteorData Data = new MeteorData(FileContent.GetPointData());
            FileContent.Dispose();
            var materialOccurrences = new long[4];
            DisplayRemoveLines();

            for (int z = LowerZBorder; z < HigherZBorder; z++)
            {
                DisplayProgress("newZ ", z - LowerZBorder);
                for (int y = 0; y < 300; y++)
                {
                    for (int x = 0; x < 300; x++)
                    {
                        float val = Data.mat.GetPoint(x, y, z).Value;
                        materialOccurrences[(int)val]++;
                    }
                }
            }
            DisplayRemoveLines();
            Display("Material {0,1} {1,-1} at {2}", "Total", "Relative", FileName);
            double s = materialOccurrences.Sum(); //(300d*300d*(HigherZBorder- LowerZBorder));
            for (int i = 0; i < materialOccurrences.Length; i++)
            {
                long val = materialOccurrences[i];
                double rel = val / s;
                Display("{0, -5} {1,8} {2,-1:0.###########}", i, val, rel);
            }

        }
        /// <summary>
        /// Erstellt eine dichte analyse von pro z-ebene
        /// </summary>
        /// <remarks>
        /// Autor: Tobias Pabst
        /// </remarks>
        /// <param name="FileContent"></param>
        static void Bericht_Map(vtkImageData FileContent)
        {
            MeteorData Data = new MeteorData(FileContent.GetPointData());

            (List<(int, int, int, Single)>, float) AnalyseLayer(int z)
            {
                var Resuls = new List<(int, int, int, Single)>();
                for (int x = 0; x < 300; x++)
                {
                    for (int y = 0; y < 300; y++)
                    {
                        var (Index, Value) = Data.rho.GetPoint(x, y, z);
                        Resuls.Add((x, y, z, Value));
                    }
                }
                var mx = Resuls.Max(x => x.Item4);
                var mn = Resuls.Min(x => x.Item4);
                Display("Layer {0}, Min: {1} Max:{2}, Diff: {3}", z, mn, mx, mx - mn);
                return (Resuls, mx - mn);
            };
            List<(int, int, int, Single)> BestResult;
            float BestResultValue = 0;
            for (int i = 0; i < 60; i++)
            {
                var x = AnalyseLayer(i);
                if (x.Item2 > BestResultValue)
                {
                    BestResult = x.Item1;
                    BestResultValue = x.Item2;
                }
            }
            Console.ReadKey();
        }
        #endregion

        #region Meteor File Test Stuff
        static void Test_vti_Data(string FileName)
        {
            vtkXMLFileReadTester Tester = new vtkXMLFileReadTester();
            Tester.InitializeObjectBase();
            Tester.SetFileName(FileName);
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
            currentReader.SetFileName(FileName);
            Display("Reader HasExecutive: {0}, CanReadFile {1}", currentReader.HasExecutive(), currentReader.CanReadFile(FileName));
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
            Console.ReadKey();
            for (int i = 0; i < valnum; i+=100)
            {
                Display("{0}:{1}",i,abstarray.GetVariantValue(i).ToFloat());
            }
            currentReader.Dispose();
        }

        static void ReadImageData(string FileName)
        {
            // reader
            // Read all the data from the file
            vtkXMLImageDataReader reader = vtkXMLImageDataReader.New();
            if (reader.CanReadFile(FileName) == 0)
            {
                Display("Cannot read file \"" + FileName + "\"", "Error");
                //return;
            }
            reader.SetFileName(FileName);
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

        static void Display_Image_Data(string FileName)
        {
            vtkXMLImageDataReader reader = vtkXMLImageDataReader.New();
            if (reader.CanReadFile(FileName) == 0)
            {
                //MessageBox.Show("Cannot read file \"" + filePath + "\"", "Error", MessageBoxButtons.OK);
                //return;
            }
            reader.SetFileName(FileName);
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

        static void Example_3DSphere()
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

        static void deleteAllVTKObjects()
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
