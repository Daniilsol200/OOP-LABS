using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace EquipmentRegistryLib
{
    public abstract class DaoBase
    {
        protected readonly string connectionString;

        public DaoBase()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["EquipmentRegistryConnection"].ConnectionString;
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new Exception("Ошибка чтения строки подключения: " + ex.Message);
            }
        }
    }
}
