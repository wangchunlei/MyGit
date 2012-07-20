using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domas.Web.Tools.UI.Form
{
    public class FormModel<T> : IFormModel<T> where T : class
    {
        public IFromRenderer<T> Renderer { get; set; }

        public IDictionary<string, object> Attributes
        { get; set; }

        public string SortPrefix
        { get; set; }

        public IFormLayout FormLayout { get;  set; }

        public FormModel()
        {
            Renderer=new FormRenderer<T>();
            FormLayout = new FormLayout
            {
                ColumnCount = 3,
                LabelColumnWidth = 80,
                GapColumnWidth = 10,
                ControlColumnWidth = 200,
                SpaceColumnWidth = 50,
                Direction = LayoutDirection.Horizontal,
                RowCount = 100,
                RowHeight = 20,
                SpaceRow = 5
            };
        }
    }
}
