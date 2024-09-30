using CTMS.Web.Areas.AdminConsole;

namespace CTMS.Web.Areas.AdminConsole
{
    public class BaseProvider
    {
        public BaseProvider(IDataBaseHelper dataBaseHelper)
        {
            DataBaseHelper = dataBaseHelper;
        }

        public IDataBaseHelper DataBaseHelper { get; }
    }
}
