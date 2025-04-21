using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentRegistryLib
{
    public class Equipment
    {
        public int EquipId { get; set; }         // Идентификатор экипировки
        public string EquipName { get; set; }    // Название экипировки
        public int ManufactureYear { get; set; } // Год изготовления
        public decimal Cost { get; set; }        // Стоимость
        public EquipmentType Type { get; set; }  // Ссылка на тип экипировки
        public Organization Organization { get; set; } // Ссылка на организацию
    }
}
