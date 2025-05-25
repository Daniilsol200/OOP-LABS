using System;
using System.Collections.Generic;

namespace EquipmentRegistryLib
{
    public interface IDao<T>
    {
        List<T> GetAll();
        T GetById(int id);
        int Create(T entity); 
        void Update(T entity);
        void Delete(int id);
    }
}