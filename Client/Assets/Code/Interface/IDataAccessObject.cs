namespace ScotlandYard.Interface
{
    using System.Collections.Generic;

    public interface IDataAccessObject<T>
    {
        List<T> ReadAll();
        List<T> Read(params object[] values);
        int Update(T value);
        int Delete(T value);
        int Insert(T value);
    }
}
