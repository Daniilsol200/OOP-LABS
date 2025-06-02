using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace EquipmentRegistryLib
{
    public class EquipmentDao : DaoBase, IDao<Equipment>
    {
        private readonly EquipmentRegistryDataContext _context;

        public EquipmentDao()
        {
            _context = new EquipmentRegistryDataContext(connectionString);
            DataLoadOptions options = new DataLoadOptions();
            options.LoadWith<Equipment>(e => e.Type);
            options.LoadWith<Equipment>(e => e.Organization);
            _context.LoadOptions = options;
        }

        public List<Equipment> GetAll()
        {
            try
            {
                return _context.Equipment.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка оборудования: " + ex.Message);
            }
        }

        public Equipment GetById(int id)
        {
            try
            {
                return _context.Equipment.FirstOrDefault(e => e.EquipId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении оборудования с ID {id}: " + ex.Message);
            }
        }

        public int Create(Equipment entity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entity.EquipName))
                    throw new ArgumentException("Название оборудования обязательно для заполнения.");
                if (entity.TypeId <= 0)
                    throw new ArgumentException("Тип оборудования должен быть указан.");
                if (entity.OrgId <= 0)
                    throw new ArgumentException("Организация должна быть указана.");

                var type = _context.EquipmentTypes.FirstOrDefault(t => t.TypeId == entity.TypeId);
                var org = _context.Organizations.FirstOrDefault(o => o.OrgId == entity.OrgId);
                if (type == null)
                    throw new ArgumentException("Указанный тип оборудования не существует.");
                if (org == null)
                    throw new ArgumentException("Указанная организация не существует.");

                entity.Type = type;
                entity.Organization = org;
                _context.Equipment.InsertOnSubmit(entity);
                _context.SubmitChanges();
                return entity.EquipId;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при создании оборудования: " + ex.Message);
            }
        }

        public void Update(Equipment entity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entity.EquipName))
                    throw new ArgumentException("Название оборудования обязательно для заполнения.");
                if (entity.TypeId <= 0)
                    throw new ArgumentException("Тип оборудования должен быть указан.");
                if (entity.OrgId <= 0)
                    throw new ArgumentException("Организация должна быть указана.");

                var existing = _context.Equipment.FirstOrDefault(e => e.EquipId == entity.EquipId);
                if (existing == null)
                    throw new Exception("Оборудование не найдено.");

                // Проверяем существование типа и организации
                var type = _context.EquipmentTypes.FirstOrDefault(t => t.TypeId == entity.TypeId);
                var org = _context.Organizations.FirstOrDefault(o => o.OrgId == entity.OrgId);
                if (type == null)
                    throw new ArgumentException("Указанный тип оборудования не существует.");
                if (org == null)
                    throw new ArgumentException("Указанная организация не существует.");

                existing.EquipName = entity.EquipName;
                existing.ManufactureYear = entity.ManufactureYear;
                existing.Cost = entity.Cost;
                existing.TypeId = entity.TypeId;
                existing.OrgId = entity.OrgId;
                existing.Type = type;
                existing.Organization = org;
                _context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при обновлении оборудования: " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var entity = _context.Equipment.FirstOrDefault(e => e.EquipId == id);
                if (entity == null)
                    throw new Exception("Оборудование не найдено.");

                _context.Equipment.DeleteOnSubmit(entity);
                _context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении оборудования с ID {id}: " + ex.Message);
            }
        }
    }
}