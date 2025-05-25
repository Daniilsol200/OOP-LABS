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
}