namespace Framework.Core.Attributes
{
    /**
     * DatabaseAttribute holds two arguments which are : 
     *          Schema, 
     *          Table...
     */
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DatabaseAttribute : Attribute
    {
        private readonly string _schema;
        private readonly string _table;

        public DatabaseAttribute(string schema, string table)
        {
            _schema = schema;
            _table = table;
        }

        public string Schema => _schema!;
        public string Table => _table!;
    }
}