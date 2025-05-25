using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EquipmentRegistryLib
{
    public class EquipmentDao : DaoBase, IDao<Equipment>
    {

        public List<Equipment> GetAll()
        {
            var equipments = new List<Equipment>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = @"
                        SELECT e.equip_id, e.equip_name, e.manufacture_year, e.cost, 
                               t.type_id, t.type_name, o.org_id, o.org_name
                        FROM Equipment e
                        JOIN EquipmentType t ON e.type_id = t.type_id
                        JOIN Organization o ON e.org_id = o.org_id";
                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            equipments.Add(new Equipment
                            {
                                EquipId = reader.GetInt32(0),
                                EquipName = reader.GetString(1),
                                ManufactureYear = reader.GetInt32(2),
                                Cost = reader.GetDecimal(3),
                                Type = new EquipmentType { TypeId = reader.GetInt32(4), TypeName = reader.GetString(5) },
                                Organization = new Organization { OrgId = reader.GetInt32(6), OrgName = reader.GetString(7) }
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Ошибка при получении списка оборудования: " + ex.Message);
            }
            return equipments;
        }

        public Equipment GetById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = @"
                        SELECT e.equip_id, e.equip_name, e.manufacture_year, e.cost, 
                               t.type_id, t.type_name, o.org_id, o.org_name
                        FROM Equipment e
                        JOIN EquipmentType t ON e.type_id = t.type_id
                        JOIN Organization o ON e.org_id = o.org_id
                        WHERE e.equip_id = @id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Equipment
                                {
                                    EquipId = reader.GetInt32(0),
                                    EquipName = reader.GetString(1),
                                    ManufactureYear = reader.GetInt32(2),
                                    Cost = reader.GetDecimal(3),
                                    Type = new EquipmentType { TypeId = reader.GetInt32(4), TypeName = reader.GetString(5) },
                                    Organization = new Organization { OrgId = reader.GetInt32(6), OrgName = reader.GetString(7) }
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Ошибка при получении оборудования с ID {id}: " + ex.Message);
            }
            return null;
        }

        public void Create(Equipment entity)
        {
            try
            {
                if (entity.Type == null || entity.Organization == null)
                    throw new ArgumentException("Тип экипировки и организация должны быть указаны.");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = @"
                        INSERT INTO Equipment (equip_name, manufacture_year, cost, type_id, org_id)
                        VALUES (@name, @year, @cost, @typeId, @orgId)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", entity.EquipName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@year", entity.ManufactureYear);
                        command.Parameters.AddWithValue("@cost", entity.Cost);
                        command.Parameters.AddWithValue("@typeId", entity.Type.TypeId);
                        command.Parameters.AddWithValue("@orgId", entity.Organization.OrgId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Ошибка при создании оборудования: " + ex.Message);
            }
        }

        public void Update(Equipment entity)
        {
            try
            {
                if (entity.Type == null || entity.Organization == null)
                    throw new ArgumentException("Тип экипировки и организация должны быть указаны.");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = @"
                        UPDATE Equipment
                        SET equip_name = @name, manufacture_year = @year, cost = @cost, 
                            type_id = @typeId, org_id = @orgId
                        WHERE equip_id = @id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", entity.EquipId);
                        command.Parameters.AddWithValue("@name", entity.EquipName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@year", entity.ManufactureYear);
                        command.Parameters.AddWithValue("@cost", entity.Cost);
                        command.Parameters.AddWithValue("@typeId", entity.Type.TypeId);
                        command.Parameters.AddWithValue("@orgId", entity.Organization.OrgId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Ошибка при обновлении оборудования: " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "DELETE FROM Equipment WHERE equip_id = @id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Ошибка при удалении оборудования с ID {id}: " + ex.Message);
            }
        }
    }
}