using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace FMS.PieChart
{
    public interface IBrushSelector
    {
        Brush SelectBrush(object item, int index);
    }
}
