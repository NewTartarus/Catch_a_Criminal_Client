namespace ScotlandYard.Interfaces
{
    using System.Collections.Generic;
    using System.Data;

    public interface IDbManager
    {
        List<object[]> Read(string sqlQuery, params IDbDataParameter[] parameters);
        int Execute(string sqlQuery, params IDbDataParameter[] parameters);
    }
}
