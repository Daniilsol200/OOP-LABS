using System;
using System.Collections.Generic;
using System.Linq;

namespace EquipmentRegistryLib
{
    public class OrganizationDao : IDao<Organization>
    {
        private readonly EquipmentRegistryDataContext _context;

        public OrganizationDao()
        {
            _context = new EquipmentRegistryDataContext(DaoBase.connectionString);
        }

        public List<Organization> GetAll()
        {
            try
            {
                return _context.Organizations.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка организаций: " + ex.Message);
            }
        }

        public Organization GetById(int id)
        {
            try
            {
                return _context.Organizations.FirstOrDefault(o => o.OrgId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении организации с ID {id}: " + ex.Message);
            }
        }

        public int Create(Organization entity)
        {
            try
            {
                _context.Organizations.InsertOnSubmit(entity);
                _context.SubmitChanges();
                return entity.OrgId;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при создании организации: " + ex.Message);
            }
        }

        public void Update(Organization entity)
        {
            try
            {
                var existing = _context.Organizations.FirstOrDefault(o => o.OrgId == entity.OrgId);
                if (existing == null)
                    throw new Exception("Организация не найдена.");

                existing.OrgName = entity.OrgName;
                _context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при обновлении организации: " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var entity = _context.Organizations.FirstOrDefault(o => o.OrgId == id);
                if (entity == null)
                    throw new Exception("Организация не найдена.");

                _context.Organizations.DeleteOnSubmit(entity);
                _context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении организации с ID {id}: " + ex.Message);
            }
        }
    }
}