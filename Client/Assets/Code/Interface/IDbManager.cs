using System.Collections.Generic;
using System.Data;

namespace ScotlandYard.Interface
{
    public interface IDbManager
    {
        List<object[]> Read(string sqlQuery, params IDbDataParameter[] parameters);
        int Execute(string sqlQuery, params IDbDataParameter[] parameters);
    }
}
