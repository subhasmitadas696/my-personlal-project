using System.Data;

namespace CTMS.Repository.Factory
{
    public interface ICTMSConnectionFactory
    {
        /// <summary>
        /// Gets the get connection.
        /// </summary>
        /// <value>The get connection.</value>
        IDbConnection GetConnection { get; }
    }
}
