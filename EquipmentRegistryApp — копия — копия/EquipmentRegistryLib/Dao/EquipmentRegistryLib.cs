using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;

namespace EquipmentRegistryLib
{
    public class EquipmentTypeDao : IDao<EquipmentType>
    {
        private readonly EquipmentRegistryDataContext _context;

        public EquipmentTypeDao()
        {
            _context = new EquipmentRegistryDataContext(DaoBase.connectionString);
        }

        public List<EquipmentType> GetAll()
        {
            try
            {
                return _context.EquipmentTypes.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка типов оборудования: " + ex.Message);
            }
        }

        public EquipmentType GetById(int id)
        {
            try
            {
                return _context.EquipmentTypes.FirstOrDefault(t => t.TypeId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении типа оборудования с ID {id}: " + ex.Message);
            }
        }

        public int Create(EquipmentType entity)
        {
            try
            {
                _context.EquipmentTypes.InsertOnSubmit(entity);
                _context.SubmitChanges();
                return entity.TypeId;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при создании типа оборудования: " + ex.Message);
            }
        }

        public void Update(EquipmentType entity)
        {
            try
            {
                var existing = _context.EquipmentTypes.FirstOrDefault(t => t.TypeId == entity.TypeId);
                if (existing == null)
                    throw new Exception("Тип оборудования не найден.");

                existing.TypeName = entity.TypeName;
                _context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при обновлении типа оборудования: " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var entity = _context.EquipmentTypes.FirstOrDefault(t => t.TypeId == id);
                if (entity == null)
                    throw new Exception("Тип оборудования не найден.");

                _context.EquipmentTypes.DeleteOnSubmit(entity);
                _context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении типа оборудования с ID {id}: " + ex.Message);
            }
        }
    }
}