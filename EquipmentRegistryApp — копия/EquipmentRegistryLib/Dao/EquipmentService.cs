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
        private DataTable _equipmentTable;

        public EquipmentService(EquipmentDao equipmentDao, OrganizationDao organizationDao, EquipmentTypeDao typeDao)
        {
            _equipmentDao = equipmentDao ?? throw new ArgumentNullException(nameof(equipmentDao));
            _organizationDao = organizationDao ?? throw new ArgumentNullException(nameof(organizationDao));
            _typeDao = typeDao ?? throw new ArgumentNullException(nameof(typeDao));
            InitializeDataTable();
        }

        private void InitializeDataTable()
        {
            _equipmentTable = new DataTable("Equipment");
            _equipmentTable.Columns.Add("equip_id", typeof(int));
            _equipmentTable.Columns.Add("equip_name", typeof(string));
            _equipmentTable.Columns.Add("manufacture_year", typeof(int));
            _equipmentTable.Columns.Add("cost", typeof(decimal));
            _equipmentTable.Columns.Add("type_id", typeof(int));
            _equipmentTable.Columns.Add("type_name", typeof(string));
            _equipmentTable.Columns.Add("org_id", typeof(int));
            _equipmentTable.Columns.Add("org_name", typeof(string));

            var equipments = _equipmentDao.GetAll();
            foreach (var equipment in equipments)
            {
                _equipmentTable.Rows.Add(
                    equipment.EquipId,
                    equipment.EquipName,
                    equipment.ManufactureYear,
                    equipment.Cost,
                    equipment.Type.TypeId,
                    equipment.Type.TypeName,
                    equipment.Organization.OrgId,
                    equipment.Organization.OrgName
                );
            }
        }

        public DataView GetAll()
        {
            return _equipmentTable.DefaultView;
        }

        public DataRow GetById(int id)
        {
            var equipment = _equipmentDao.GetById(id);
            if (equipment == null) return null;

            var row = _equipmentTable.AsEnumerable()
                .FirstOrDefault(r => r.Field<int>("equip_id") == id);
            return row;
        }

        public int Create(Equipment equipment)
        {
            if (equipment == null) throw new ArgumentNullException(nameof(equipment));
            int newId = _equipmentDao.Create(equipment);

            var row = _equipmentTable.NewRow();
            row["equip_id"] = newId;
            row["equip_name"] = equipment.EquipName ?? (object)DBNull.Value;
            row["manufacture_year"] = equipment.ManufactureYear;
            row["cost"] = equipment.Cost;
            row["type_id"] = equipment.Type.TypeId;
            row["type_name"] = equipment.Type.TypeName;
            row["org_id"] = equipment.Organization.OrgId;
            row["org_name"] = equipment.Organization.OrgName;
            _equipmentTable.Rows.Add(row);

            return newId;
        }

        public void Update(Equipment equipment)
        {
            if (equipment == null) throw new ArgumentNullException(nameof(equipment));
            _equipmentDao.Update(equipment);

            var row = _equipmentTable.AsEnumerable()
                .FirstOrDefault(r => r.Field<int>("equip_id") == equipment.EquipId);
            if (row != null)
            {
                row["equip_name"] = equipment.EquipName ?? (object)DBNull.Value;
                row["manufacture_year"] = equipment.ManufactureYear;
                row["cost"] = equipment.Cost;
                row["type_id"] = equipment.Type.TypeId;
                row["type_name"] = equipment.Type.TypeName;
                row["org_id"] = equipment.Organization.OrgId;
                row["org_name"] = equipment.Organization.OrgName;
            }
        }

        public void Delete(int id)
        {
            if (id <= 0) throw new ArgumentException("ID должен быть больше 0.");
            _equipmentDao.Delete(id);

            var row = _equipmentTable.AsEnumerable()
                .FirstOrDefault(r => r.Field<int>("equip_id") == id);
            if (row != null)
            {
                _equipmentTable.Rows.Remove(row);
            }
        }

        public List<Organization> GetAllOrganizations() => _organizationDao.GetAll();

        public List<EquipmentType> GetAllEquipmentTypes() => _typeDao.GetAll();

        public DataView FilterByCost(decimal maxCost)
        {
            var filteredRows = _equipmentTable.AsEnumerable()
                .Where(row => row.Field<decimal>("cost") < maxCost)
                .CopyToDataTable();
            return filteredRows.DefaultView;
        }

        public DataView FilterByYear(int maxYear)
        {
            var filteredRows = _equipmentTable.AsEnumerable()
                .Where(row => row.Field<int>("manufacture_year") < maxYear)
                .CopyToDataTable();
            return filteredRows.DefaultView;
        }

        public DataView AverageCostByType()
        {
            var resultTable = new DataTable("Equipment");
            resultTable.Columns.Add("equip_id", typeof(int));
            resultTable.Columns.Add("equip_name", typeof(string));
            resultTable.Columns.Add("manufacture_year", typeof(int));
            resultTable.Columns.Add("cost", typeof(decimal));
            resultTable.Columns.Add("type_id", typeof(int));
            resultTable.Columns.Add("type_name", typeof(string));
            resultTable.Columns.Add("org_id", typeof(int));
            resultTable.Columns.Add("org_name", typeof(string));

            var averages = _equipmentTable.AsEnumerable()
                .GroupBy(row => row.Field<string>("type_name"))
                .Select(g => new
                {
                    TypeName = g.Key,
                    AverageCost = g.Average(row => row.Field<decimal>("cost"))
                });

            foreach (var item in averages)
            {
                resultTable.Rows.Add(0, "", 0, item.AverageCost, 0, item.TypeName, 0, "");
            }

            return resultTable.DefaultView;
        }

        public DataView CountByOrganization()
        {
            var resultTable = new DataTable("OrgCount");
            resultTable.Columns.Add("org_name", typeof(string));
            resultTable.Columns.Add("equipment_count", typeof(int));

            var counts = _equipmentTable.AsEnumerable()
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