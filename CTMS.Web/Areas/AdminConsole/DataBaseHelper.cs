namespace CTMS.Web.Areas.AdminConsole
{
    public interface IDataBaseHelper
    {
        string ConnectionString { get; }
    }
    /// <summary>
    /// Get the Connectionstring name
    /// </summary>
    /// <auther>Manas Bej</auther>
    public class DataBaseHelper : IDataBaseHelper
    {
        public DataBaseHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}
