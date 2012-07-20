using UIFramwork.UI.UIFrom;

namespace UIFramwork.UI.UIFrom
{
    public class FormLayout : IFormLayout
    {
        public string TableMarkupStart
        {
            get { return string.Format("<table>"); }
        }
        public string TableMarkupEnd
        {
            get { return "</table>"; }
        }
        public string TrMarkupStart(int height)
        {
            var mark = string.Format("<tr style='height:{0}px;'>", height);
            return mark;
        }
        public string TrMarkupEnd
        {
            get { return "</tr>"; }
        }
        public string TdMarkupStart(int width, string direction = "left")
        {
            var mark = string.Format("<td style='width:{0}px;' align='{1}'>", width, direction);
            return mark;
        }

        public string TdMarkupEnd
        {
            get { return "</td>"; }
        }

        public LayoutDirection Direction { get; set; }
        public int ColumnCount { get; set; }
        public int LabelColumnWidth { get; set; }
        public int GapColumnWidth { get; set; }
        public int ControlColumnWidth { get; set; }
        public int SpaceColumnWidth { get; set; }
        public int RowCount { get; set; }
        public int SpaceRow { get; set; }
        public int RowHeight { get; set; }
    }
}
