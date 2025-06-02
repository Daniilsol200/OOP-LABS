using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace EquipmentRegistryLib
{
    [Table(Name = "Equipment")]
    public class Equipment
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, Name = "equip_id")]
        public int EquipId { get; set; }

        [Column(Name = "equip_name")]
        public string EquipName { get; set; }

        [Column(Name = "manufacture_year")]
        public int ManufactureYear { get; set; }

        [Column(Name = "cost")]
        public decimal Cost { get; set; }

        [Column(Name = "type_id")]
        public int TypeId { get; set; }

        [Column(Name = "org_id")]
        public int OrgId { get; set; }

        private EntityRef<EquipmentType> _Type;
        [Association(Storage = "_Type", ThisKey = "TypeId", OtherKey = "TypeId")]
        public EquipmentType Type
        {
            get { return _Type.Entity; }
            set { _Type.Entity = value; if (value != null) TypeId = value.TypeId; }
        }

        private EntityRef<Organization> _Organization;
        [Association(Storage = "_Organization", ThisKey = "OrgId", OtherKey = "OrgId")]
        public Organization Organization
        {
            get { return _Organization.Entity; }
            set { _Organization.Entity = value; if (value != null) OrgId = value.OrgId; }
        }
    }

    [Table(Name = "EquipmentType")]
    public class EquipmentType
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, Name = "type_id")]
        public int TypeId { get; set; }

        [Column(Name = "type_name")]
        public string TypeName { get; set; }
    }

    [Table(Name = "Organization")]
    public class Organization
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, Name = "org_id")]
        public int OrgId { get; set; }

        [Column(Name = "org_name")]
        public string OrgName { get; set; }
    }
}