using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.ViewModels
{
    public class ChartViewModel
    {
    }

    #region  stackLine data model
    
    public class LineChartVMObj
    {
        public string title { get; set; }
        public LineChartVM data { get; set; }
    }
    public class LineChartVM
    {
        public List<LineChartData> chartDatas { get; set; }
        public List<string> chartLabels { get; set; }

    }
    public class LineChartData
    {
      public List<double> data { get; set; }
      public string name { get; set; }
    }

    public class LineDataPoint
    {
        public List<LineData> datas { get; set; }
        public string label { get; set; } 
    }
    public class LineData
    {
        public double data { get; set; }
        public string name { get; set; }
    }
    #endregion


    #region

    public class BasicLinePoint
    {
        public double data { get; set; }
        public string label { get; set; }
    }

    #endregion
    public class PieData {
        public double value { get; set; }
        public string name { get; set; }
    }

}