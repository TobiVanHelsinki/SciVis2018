using Kitware.VTK;
using System;
using System.Collections;
using System.Collections.Generic;
using static SciVis.Model.ModelHelper;
using static SciVis.Helper;

namespace SciVis.Model
{
    public class MeteorData
    {
        /// <summary>
        /// no clue what this is
        /// </summary>
        public PointDataList<int> vtkGhostType;
        /// <summary>
        /// no clue what this is
        /// </summary>
        public PointDataList<int> vtkValidPointMask;

        /// <summary>
        /// rho density in grams per cubic centimeter. (g/cm3)
        /// </summary>
        public PointDataList<Single> rho;
        /// <summary>
        /// prs pressure in microbars (µbar)
        /// </summary>
        public PointDataList<Single> prs;
        /// <summary>
        /// tev temperature in electronvolt (eV)
        /// </summary>
        public PointDataList<Single> tev;
        /// <summary>
        /// xdt x component vectors in centimeters per second (cm/sec)
        /// </summary>
        public PointDataList<Single> xdt;
        /// <summary>
        /// ydt y component vectors in centimeters per second (cm/sec)
        /// </summary>
        public PointDataList<Single> ydt;
        /// <summary>
        /// zdt z component vectors in centimeters per second (cm/sec)
        /// </summary>
        public PointDataList<Single> zdt;
        /// <summary>
        /// snd sound speed in centimeters per second (cm/sec)
        /// </summary>
        public PointDataList<Single> snd;
        /// <summary>
        /// grd AMR grid reﬁnement level
        /// </summary>
        public PointDataList<Single> grd;
        /// <summary>
        /// mat material number id
        /// </summary>
        public PointDataList<Single> mat;
        /// <summary>
        /// v02 volume fraction water
        /// </summary>
        public PointDataList<Single> v02;
        /// <summary>
        /// v03 volume fraction of asteroid
        /// </summary>
        public PointDataList<Single> v03;

        /// <summary>
        /// the raw data, this class uses
        /// </summary>
        public vtkPointData PointData;

        /// <summary>
        /// create a new wrapper vor meteor point data
        /// </summary>
        /// <param name="iPointData"></param>
        public MeteorData(vtkPointData iPointData)
        {
            PointData = iPointData;
            vtkGhostType = new PointDataList<int>(PointData.GetAbstractArray(0));
            grd = new PointDataList<Single>(PointData.GetAbstractArray(1));
            mat = new PointDataList<Single>(PointData.GetAbstractArray(2));
            prs = new PointDataList<Single>(PointData.GetAbstractArray(3));
            rho = new PointDataList<Single>(PointData.GetAbstractArray(4));
            snd = new PointDataList<Single>(PointData.GetAbstractArray(5));
            tev = new PointDataList<Single>(PointData.GetAbstractArray(6));
            v02 = new PointDataList<Single>(PointData.GetAbstractArray(7));
            v03 = new PointDataList<Single>(PointData.GetAbstractArray(8));
            xdt = new PointDataList<Single>(PointData.GetAbstractArray(9));
            ydt = new PointDataList<Single>(PointData.GetAbstractArray(10));
            zdt = new PointDataList<Single>(PointData.GetAbstractArray(11));
            vtkValidPointMask = new PointDataList<int>(PointData.GetAbstractArray(12));
        }
    }

}
