using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EquipmentRegistryLib
{
    public class OrganizationDao : DaoBase, IDao<Organization>
    {
        public List<Organization> GetAll()
        {
            var organizations = new List<Organization>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT org_id, org_name FROM Organization";
                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            organizations.Add(new Organization
                            {
                                OrgId = reader.GetInt32(0),
                                OrgName = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Ошибка при получении списка организаций: " + ex.Message);
            }
            return organizations;
        }

        public Organization GetById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT org_id, org_name FROM Organization WHERE org_id = @id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Organization
                                {
                                    OrgId = reader.GetInt32(0),
                                    OrgName = reader.GetString(1)
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Ошибка при получении организации с ID {id}: " + ex.Message);
            }
            return null;
        }

        public void Create(Organization entity)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "INSERT INTO Organization (org_name) VALUES (@name)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", entity.OrgName ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Ошибка при создании организации: " + ex.Message);
            }
        }

        public void Update(Organization entity)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "UPDATE Organization SET org_name = @name WHERE org_id = @id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", entity.OrgId);
                        command.Parameters.AddWithValue("@name", entity.OrgName ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Ошибка при обновлении организации: " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "DELETE FROM Organization WHERE org_id = @id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Ошибка при удалении организации с ID {id}: " + ex.Message);
            }
        }
    }
}