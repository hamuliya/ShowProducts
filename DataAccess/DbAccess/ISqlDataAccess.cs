namespace DataAccess.DbAccess
{
    public interface ISqlDataAccess
    {
        Task execDataAsync<T>(string storeProcedure, T parameters, string connectionId = "Default");
        Task<IEnumerable<T>> loadDataAsync<T, U>(string storeProcedure, U parameters, string connectionId = "Default");
        Task<int> saveDataAsync<T>(string StoreProcedure, T Parameters, string connectionId = "Default");
    }
}