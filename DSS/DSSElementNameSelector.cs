namespace DSS
{
    public class DSSElementNameSelector : IDSSSelector
    {
        public string ElementName { get; set; }

        public DSSElementNameSelector(string elementName)
        {
            ElementName = elementName;
        }
    }
}
