using Kitware.VTK;
using System;
using static SciVis.Helper;

namespace SciVis.IO
{
    public static class VTK_IO
    {
        /// <summary>
        /// Read a vti File and Returns the ImageData. Can throw.
        /// </summary>
        /// <param name="FileName"></param>
        /// <exception cref="Exception"/>
        /// <returns></returns>
        public static vtkImageData ReadInData(string FileName)
        {
            vtkXMLImageDataReader Reader = new vtkXMLImageDataReader();
            Reader.InitializeObjectBase();
            Reader.SetFileName(FileName);
            if (Reader.CanReadFile(FileName) == 0)
            {
                throw new Exception("Cannot Read File");
            }
            Display("Start Reading File");
            Reader.Update();
            var ret = Reader.GetOutput();
            Reader?.Dispose();
            return ret;
        }

    }
}
