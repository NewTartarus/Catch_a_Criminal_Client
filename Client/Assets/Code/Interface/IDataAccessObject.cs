using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScotlandYard.Interface
{
    public interface IDataAccessObject<T>
    {
        List<T> ReadAll();
        List<T> Read(params object[] values);
        int Update(T value);
        int Delete(T value);
        int Insert(T value);
    }
}
