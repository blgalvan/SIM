using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace SIM
{
    class ChartControlGraph
    {
        //Codigo que visualiza el valor del ejex y eje y de un punto de la recta al situar encima de ella el ratón
        static public void Chart1_GetToolTipText(object sender, System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs e)
        {

            // Check selevted chart element and set tooltip text
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                DataPoint dp = e.HitTestResult.Series.Points[i];
                e.Text = string.Format("{0:F2}; {1:F2}", dp.XValue, dp.YValues[0]);
            }
        }
    }
}
