using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace EquipmentRegistryLib
{
    public class EquipmentRegistryDataContext : DataContext
    {
        public Table<Equipment> Equipment;
        public Table<EquipmentType> EquipmentTypes;
        public Table<Organization> Organizations;

        public EquipmentRegistryDataContext(string connectionString) : base(connectionString) { }
    }

    [Table(Name = "Equipment")]
    public class Equipment
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int EquipId { get; set; }

        [Column]
        public string EquipName { get; set; }

        [Column]
        public int ManufactureYear { get; set; }

        [Column]
        public decimal Cost { get; set; }

        [Column]
        public int TypeId { get; set; }

        [Column]
        public int OrgId { get; set; }

        [Association(Storage = "_Type", ThisKey = "TypeId", OtherKey = "TypeId")]
        public EquipmentType Type { get; set; }

        [Association(Storage = "_Organization", ThisKey = "OrgId", OtherKey = "OrgId")]
        public Organization Organization { get; set; }
    }

    [Table(Name = "EquipmentType")]
    public class EquipmentType
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int TypeId { get; set; }

        [Column]
        public string TypeName { get; set; }
    }

    [Table(Name = "Organization")]
    public class Organization
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int OrgId { get; set; }

        [Column]
        public string OrgName { get; set; }
    }
}