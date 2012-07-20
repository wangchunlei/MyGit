using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domas.Web.Tools.UI.Form
{
    public interface IFormLayout
    {
        LayoutDirection Direction { get; set; }
        int ColumnCount { get; set; }
        int LabelColumnWidth { get; set; }
        int GapColumnWidth { get; set; }
        int ControlColumnWidth { get; set; }
        int SpaceColumnWidth { get; set; }
        int RowCount { get; set; }
        int SpaceRow { get; set; }
        int RowHeight { get; set; }

        string TableMarkupStart { get; }
        string TableMarkupEnd { get; }
        string TrMarkupStart(int height);
        string TrMarkupEnd { get; }
        string TdMarkupStart(int width);
        string TdMarkupEnd { get; }
    }

    public enum LayoutDirection
    {
        Horizontal,
        Verticality
    }
}
