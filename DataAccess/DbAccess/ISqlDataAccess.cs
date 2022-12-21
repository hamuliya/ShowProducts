namespace DataAccess.DbAccess
{
    public interface ISqlDataAccess
    {
        Task ExecData<T>(string storeProcedure, T parameters, string connectionId = "Default");
        Task<IEnumerable<T>> LoadData<T, U>(string storeProcedure, U parameters, string connectionId = "Default");
        int SaveData<T>(string storeProcedure, T parameters, string connectionId = "Default");
    }
}