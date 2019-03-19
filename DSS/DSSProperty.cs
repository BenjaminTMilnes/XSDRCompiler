namespace DSS
{
    public class DSSProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public DSSProperty() { }

        public DSSProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
