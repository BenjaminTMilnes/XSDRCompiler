namespace DSS
{
    public class DSSClassSelector : IDSSSelector
    {
        public string Class { get; set; }

        public DSSClassSelector(string _class)
        {
            Class = _class;
        }
    }
}
