namespace DataAccess.DbAccess
{
    public interface ISqlDataAccess
    {
        Task ExecDataAsync<T>(string storeProcedure, T parameters, string connectionId = "Default");
        Task<IEnumerable<T>> LoadDataAsync<T, U>(string storeProcedure, U parameters, string connectionId = "Default");
        Task<int> SaveDataAsync<T>(string StoreProcedure, T Parameters, string connectionId = "Default");
    }
}