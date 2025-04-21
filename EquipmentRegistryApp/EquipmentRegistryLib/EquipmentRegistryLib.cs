using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EquipmentRegistryLib
{
    public class EquipmentTypeDao : DaoBase, IDao<EquipmentType>
    {
        public List<EquipmentType> GetAll()
        {
            var types = new List<EquipmentType>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT type_id, type_name FROM EquipmentType";
                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            types.Add(new EquipmentType
                            {
                                TypeId = reader.GetInt32(0),
                                TypeName = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Ошибка при получении списка типов оборудования: " + ex.Message);
            }
            return types;
        }

        public EquipmentType GetById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT type_id, type_name FROM EquipmentType WHERE type_id = @id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new EquipmentType
                                {
                                    TypeId = reader.GetInt32(0),
                                    TypeName = reader.GetString(1)
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Ошибка при получении типа оборудования с ID {id}: " + ex.Message);
            }
            return null;
        }

        public void Create(EquipmentType entity)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "INSERT INTO EquipmentType (type_name) VALUES (@name)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", entity.TypeName ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Ошибка при создании типа оборудования: " + ex.Message);
            }
        }

        public void Update(EquipmentType entity)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "UPDATE EquipmentType SET type_name = @name WHERE type_id = @id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", entity.TypeId);
                        command.Parameters.AddWithValue("@name", entity.TypeName ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Ошибка при обновлении типа оборудования: " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "DELETE FROM EquipmentType WHERE type_id = @id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Ошибка при удалении типа оборудования с ID {id}: " + ex.Message);
            }
        }
    }
}
