using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EquipmentRegistryLib
{
    public class EquipmentService
    {
        private readonly EquipmentDao _equipmentDao;
        private readonly OrganizationDao _organizationDao;
        private readonly EquipmentTypeDao _typeDao;

        public EquipmentService(EquipmentDao equipmentDao, OrganizationDao organizationDao, EquipmentTypeDao typeDao)
        {
            _equipmentDao = equipmentDao ?? throw new ArgumentNullException(nameof(equipmentDao));
            _organizationDao = organizationDao ?? throw new ArgumentNullException(nameof(organizationDao));
            _typeDao = typeDao ?? throw new ArgumentNullException(nameof(typeDao));
        }

        public DataView GetAll()
        {
            var table = new DataTable("Equipment");
            table.Columns.Add("equip_id", typeof(int));
            table.Columns.Add("equip_name", typeof(string));
            table.Columns.Add("manufacture_year", typeof(int));
            table.Columns.Add("cost", typeof(decimal));
            table.Columns.Add("type_id", typeof(int));
            table.Columns.Add("type_name", typeof(string));
            table.Columns.Add("org_id", typeof(int));
            table.Columns.Add("org_name", typeof(string));

            var equipments = _equipmentDao.GetAll();
            var orgs = _organizationDao.GetAll();
            var types = _typeDao.GetAll();

            foreach (var equipment in equipments)
            {
                var org = orgs.FirstOrDefault(o => o.OrgId == equipment.OrgId);
                var type = types.FirstOrDefault(t => t.TypeId == equipment.TypeId);
                table.Rows.Add(
                    equipment.EquipId,
                    equipment.EquipName,
                    equipment.ManufactureYear,
                    equipment.Cost,
                    equipment.TypeId,
                    type?.TypeName ?? "Неизвестно",
                    equipment.OrgId,
                    org?.OrgName ?? "Неизвестно"
                );
            }

            return table.DefaultView;
        }

        public DataRow GetById(int id)
        {
            var equipment = _equipmentDao.GetById(id);
            if (equipment == null) return null;

            var table = GetAll().Table;
            return table.AsEnumerable().FirstOrDefault(r => r.Field<int>("equip_id") == id);
        }

        public int Create(Equipment equipment)
        {
            if (equipment == null) throw new ArgumentNullException(nameof(equipment));
            return _equipmentDao.Create(equipment);
        }

        public void Update(Equipment equipment)
        {
            if (equipment == null) throw new ArgumentNullException(nameof(equipment));
            _equipmentDao.Update(equipment);
        }

        public void Delete(int id)
        {
            if (id <= 0) throw new ArgumentException("ID должен быть больше 0.");
            _equipmentDao.Delete(id);
        }

        public List<Organization> GetAllOrganizations() => _organizationDao.GetAll();

        public List<EquipmentType> GetAllEquipmentTypes() => _typeDao.GetAll();

        public DataView FilterByCost(decimal maxCost)
        {
            var table = GetAll().Table;
            var filteredRows = table.AsEnumerable()
                .Where(row => row.Field<decimal>("cost") < maxCost)
                .CopyToDataTable();
            return filteredRows.DefaultView;
        }

        public DataView FilterByYear(int maxYear)
        {
            var table = GetAll().Table;
            var filteredRows = table.AsEnumerable()
                .Where(row => row.Field<int>("manufacture_year") < maxYear)
                .CopyToDataTable();
            return filteredRows.DefaultView;
        }

        public DataView AverageCostByType()
        {
            var table = GetAll().Table;
            var resultTable = new DataTable("AverageCostByType");
            resultTable.Columns.Add("type_name", typeof(string));
            resultTable.Columns.Add("average_cost", typeof(decimal));

            var averages = table.AsEnumerable()
                .GroupBy(row => row.Field<string>("type_name"))
                .Select(g => new
                {
                    TypeName = g.Key,
                    AverageCost = g.Average(row => row.Field<decimal>("cost"))
                });

            foreach (var item in averages)
            {
                resultTable.Rows.Add(item.TypeName, item.AverageCost);
            }

            return resultTable.DefaultView;
        }

        public DataView CountByOrganization()
        {
            var table = GetAll().Table;
            var resultTable = new DataTable("OrgCount");
            resultTable.Columns.Add("org_name", typeof(string));
            resultTable.Columns.Add("equipment_count", typeof(int));

            var counts = table.AsEnumerable()
                .GroupBy(row => row.Field<string>("org_name"))
                .Select(g => new
                {
                    OrgName = g.Key,
                    Count = g.Count()
                });

            foreach (var item in counts)
            {
                resultTable.Rows.Add(item.OrgName, item.Count);
            }

            return resultTable.DefaultView;
        }
    }
}