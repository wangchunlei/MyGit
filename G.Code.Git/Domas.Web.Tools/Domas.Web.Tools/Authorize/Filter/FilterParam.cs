namespace Domas.Web.Tools.Authorize
{
    public class FilterParam
    {
        public FilterParam(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
