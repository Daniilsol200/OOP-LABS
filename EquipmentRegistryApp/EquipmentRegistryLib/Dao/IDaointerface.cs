using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentRegistryLib
{
    public interface IDao<T>
    {
        List<T> GetAll();      // Получить все записи
        T GetById(int id);     // Получить запись по ID
        void Create(T entity); // Создать новую запись
        void Update(T entity); // Обновить существующую запись
        void Delete(int id);   // Удалить запись по ID
    }
}
