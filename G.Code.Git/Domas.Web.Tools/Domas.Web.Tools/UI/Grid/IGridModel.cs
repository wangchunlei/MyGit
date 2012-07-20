using System.Collections.Generic;
using Domas.Web.Tools.Sorting;
using Domas.Web.Tools.UI.Grid.Syntax;

namespace Domas.Web.Tools.UI.Grid
{
	/// <summary>
	/// Defines a grid model
	/// </summary>
	public interface IGridModel<T> where T: class 
	{
		IGridRenderer<T> Renderer { get; set; }
		IList<GridColumn<T>> Columns { get; }
		IGridSections<T> Sections { get; }
		string EmptyText { get; set; }
		IDictionary<string, object> Attributes { get; set; }
		GridSortOptions SortOptions { get; set; }
		string SortPrefix { get; set; }
	}
}