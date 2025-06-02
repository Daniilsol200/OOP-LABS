using EquipmentRegistryLib;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EquipmentRegistryApp
{
    public partial class MainWindow : Window
    {
        private readonly EquipmentDao equipmentDao;
        private readonly OrganizationDao organizationDao;
        private readonly EquipmentTypeDao typeDao;
        private readonly EquipmentService equipmentService;
        private string currentTable;
        private string currentTableMode;

        public MainWindow()
        {
            InitializeComponent();
            equipmentDao = new EquipmentDao();
            organizationDao = new OrganizationDao();
            typeDao = new EquipmentTypeDao();
            equipmentService = new EquipmentService(equipmentDao, organizationDao, typeDao);
            currentTable = "Оборудование";
            currentTableMode = "Equipment";
            UpdateDataGridVisibility();
            LoadData();
        }

        private void MenuEquipment_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Оборудование";
            currentTableMode = "Equipment";
            menuEquipment.IsChecked = true;
            menuOrganization.IsChecked = false;
            menuType.IsChecked = false;
            UpdateFormVisibility();
            UpdateDataGridVisibility();
            LoadData();
        }

        private void MenuOrganization_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Организации";
            currentTableMode = "Organizations";
            menuEquipment.IsChecked = false;
            menuOrganization.IsChecked = true;
            menuType.IsChecked = false;
            UpdateFormVisibility();
            UpdateDataGridVisibility();
            LoadData();
        }

        private void MenuType_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Типы оборудования";
            currentTableMode = "Types";
            menuEquipment.IsChecked = false;
            menuOrganization.IsChecked = false;
            menuType.IsChecked = true;
            UpdateFormVisibility();
            UpdateDataGridVisibility();
            LoadData();
        }

        private void UpdateFormVisibility()
        {
            equipmentForm.Visibility = currentTable == "Оборудование" ? Visibility.Visible : Visibility.Collapsed;
            organizationForm.Visibility = currentTable == "Организации" ? Visibility.Visible : Visibility.Collapsed;
            typeForm.Visibility = currentTable == "Типы оборудования" ? Visibility.Visible : Visibility.Collapsed;
            groupBoxForm.Header = currentTable == "Оборудование" ? "Данные оборудования" :
                                  currentTable == "Организации" ? "Данные организации" :
                                  "Данные типа оборудования";

            btnFilterByCost.Visibility = currentTable == "Оборудование" ? Visibility.Visible : Visibility.Collapsed;
            btnFilterByYear.Visibility = currentTable == "Оборудование" ? Visibility.Visible : Visibility.Collapsed;
            btnAverageCostByType.Visibility = currentTable == "Оборудование" ? Visibility.Visible : Visibility.Collapsed;
            btnCountByOrg.Visibility = currentTable == "Оборудование" ? Visibility.Visible : Visibility.Collapsed;

            ClearForm();
        }

        private void UpdateDataGridVisibility()
        {
            dgEquipment.Visibility = Visibility.Collapsed;
            dgOrganizations.Visibility = Visibility.Collapsed;
            dgTypes.Visibility = Visibility.Collapsed;
            dgOrgCount.Visibility = Visibility.Collapsed;
            dgTypeAverage.Visibility = Visibility.Collapsed;

            if (currentTable == "Оборудование")
            {
                if (currentTableMode == "Equipment")
                    dgEquipment.Visibility = Visibility.Visible;
                else if (currentTableMode == "OrgCount")
                    dgOrgCount.Visibility = Visibility.Visible;
                else if (currentTableMode == "TypeAverage")
                    dgTypeAverage.Visibility = Visibility.Visible;
            }
            else if (currentTable == "Организации")
            {
                dgOrganizations.Visibility = Visibility.Visible;
            }
            else if (currentTable == "Типы оборудования")
            {
                dgTypes.Visibility = Visibility.Visible;
            }
        }

        private void LoadData()
        {
            try
            {
                if (currentTable == "Оборудование")
                {
                    if (currentTableMode == "Equipment")
                    {
                        dgEquipment.ItemsSource = equipmentService.GetAll();
                    }
                    cmbOrganization.ItemsSource = equipmentService.GetAllOrganizations();
                    cmbType.ItemsSource = equipmentService.GetAllEquipmentTypes();
                }
                else if (currentTable == "Организации")
                {
                    dgOrganizations.ItemsSource = organizationDao.GetAll();
                }
                else if (currentTable == "Типы оборудования")
                {
                    dgTypes.ItemsSource = typeDao.GetAll();
                }

                ClearForm();
                txtStatus.Text = "Данные успешно загружены.";
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Ошибка: " + ex.Message;
            }
        }

        private void ClearForm()
        {
            if (currentTable == "Оборудование")
            {
                txtName.Text = "";
                txtYear.Text = "";
                txtCost.Text = "";
                cmbOrganization.SelectedIndex = -1;
                cmbType.SelectedIndex = -1;
            }
            else if (currentTable == "Организации")
            {
                txtOrgName.Text = "";
            }
            else if (currentTable == "Типы оборудования")
            {
                txtTypeName.Text = "";
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentTable == "Оборудование")
                {
                    if (string.IsNullOrWhiteSpace(txtName.Text))
                        throw new Exception("Название обязательно для заполнения.");
                    if (!int.TryParse(txtYear.Text, out int year) || year < 1900 || year > DateTime.Now.Year)
                        throw new Exception("Неверный год изготовления.");
                    if (!decimal.TryParse(txtCost.Text, out decimal cost) || cost < 0)
                        throw new Exception("Неверная стоимость.");
                    if (cmbOrganization.SelectedItem == null || cmbType.SelectedItem == null)
                        throw new Exception("Выберите организацию и тип.");

                    var existingEquipment = equipmentDao.GetAll().FirstOrDefault(eq => eq.EquipName.Equals(txtName.Text, StringComparison.OrdinalIgnoreCase));
                    if (existingEquipment != null)
                        throw new Exception($"Оборудование с названием '{txtName.Text}' уже существует.");

                    var equipment = new Equipment
                    {
                        EquipName = txtName.Text,
                        ManufactureYear = year,
                        Cost = cost,
                        Organization = (Organization)cmbOrganization.SelectedItem,
                        Type = (EquipmentType)cmbType.SelectedItem
                    };

                    equipmentService.Create(equipment);
                    txtStatus.Text = "Оборудование успешно добавлено.";
                }
                else if (currentTable == "Организации")
                {
                    if (string.IsNullOrWhiteSpace(txtOrgName.Text))
                        throw new Exception("Название организации обязательно для заполнения.");

                    var existingOrg = organizationDao.GetAll().FirstOrDefault(org => org.OrgName.Equals(txtOrgName.Text, StringComparison.OrdinalIgnoreCase));
                    if (existingOrg != null)
                        throw new Exception($"Организация с названием '{txtOrgName.Text}' уже существует.");

                    var organization = new Organization
                    {
                        OrgName = txtOrgName.Text
                    };

                    organizationDao.Create(organization);
                    txtStatus.Text = "Организация успешно добавлена.";
                }
                else if (currentTable == "Типы оборудования")
                {
                    if (string.IsNullOrWhiteSpace(txtTypeName.Text))
                        throw new Exception("Название типа обязательно для заполнения.");

                    var existingType = typeDao.GetAll().FirstOrDefault(t => t.TypeName.Equals(txtTypeName.Text, StringComparison.OrdinalIgnoreCase));
                    if (existingType != null)
                        throw new Exception($"Тип оборудования с названием '{txtTypeName.Text}' уже существует.");

                    var type = new EquipmentType
                    {
                        TypeName = txtTypeName.Text
                    };

                    typeDao.Create(type);
                    txtStatus.Text = "Тип оборудования успешно добавлен.";
                }

                LoadData();
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Ошибка: " + ex.Message;
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            currentTableMode = "Equipment";
            UpdateDataGridVisibility();
            LoadData();
        }

        private void DgEquipment_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                try
                {
                    var rowView = e.Row.Item as DataRowView;
                    if (rowView == null) return;

                    int equipId = rowView["equip_id"] != DBNull.Value ? Convert.ToInt32(rowView["equip_id"]) : 0;
                    string equipName = rowView["equip_name"]?.ToString();
                    if (!int.TryParse(rowView["manufacture_year"]?.ToString(), out int year) || year < 1900 || year > DateTime.Now.Year)
                        throw new Exception("Неверный год изготовления.");
                    if (!decimal.TryParse(rowView["cost"]?.ToString(), out decimal cost) || cost < 0)
                        throw new Exception("Неверная стоимость.");
                    string orgName = rowView["org_name"]?.ToString();
                    string typeName = rowView["type_name"]?.ToString();

                    var organization = organizationDao.GetAll().FirstOrDefault(o => o.OrgName == orgName);
                    var type = typeDao.GetAll().FirstOrDefault(t => t.TypeName == typeName);

                    if (string.IsNullOrWhiteSpace(equipName))
                        throw new Exception("Название обязательно для заполнения.");
                    if (organization == null)
                        throw new Exception("Организация не найдена.");
                    if (type == null)
                        throw new Exception("Тип оборудования не найден.");

                    var existingEquipment = equipmentDao.GetAll().FirstOrDefault(eq => eq.EquipName.Equals(equipName, StringComparison.OrdinalIgnoreCase) && eq.EquipId != equipId);
                    if (existingEquipment != null)
                        throw new Exception($"Оборудование с названием '{equipName}' уже существует.");

                    var equipment = new Equipment
                    {
                        EquipId = equipId,
                        EquipName = equipName,
                        ManufactureYear = year,
                        Cost = cost,
                        Organization = organization,
                        Type = type
                    };

                    if (equipId == 0) // New row
                    {
                        equipmentService.Create(equipment);
                        txtStatus.Text = "Оборудование успешно добавлено.";
                    }
                    else // Update existing row
                    {
                        equipmentService.Update(equipment);
                        txtStatus.Text = "Оборудование успешно обновлено.";
                    }

                    LoadData();
                }
                catch (Exception ex)
                {
                    txtStatus.Text = "Ошибка: " + ex.Message;
                    e.Cancel = true;
                }
            }
        }

        private void DgOrganizations_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                try
                {
                    var organization = e.Row.Item as Organization;
                    if (organization == null) return;

                    if (string.IsNullOrWhiteSpace(organization.OrgName))
                        throw new Exception("Название организации обязательно для заполнения.");

                    var existingOrg = organizationDao.GetAll().FirstOrDefault(o => o.OrgName.Equals(organization.OrgName, StringComparison.OrdinalIgnoreCase) && o.OrgId != organization.OrgId);
                    if (existingOrg != null)
                        throw new Exception($"Организация с названием '{organization.OrgName}' уже существует.");

                    if (organization.OrgId == 0) // New row
                    {
                        organizationDao.Create(organization);
                        txtStatus.Text = "Организация успешно добавлена.";
                    }
                    else // Update existing row
                    {
                        organizationDao.Update(organization);
                        txtStatus.Text = "Организация успешно обновлена.";
                    }

                    LoadData();
                }
                catch (Exception ex)
                {
                    txtStatus.Text = "Ошибка: " + ex.Message;
                    e.Cancel = true;
                }
            }
        }

        private void DgTypes_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                try
                {
                    var type = e.Row.Item as EquipmentType;
                    if (type == null) return;

                    if (string.IsNullOrWhiteSpace(type.TypeName))
                        throw new Exception("Название типа обязательно для заполнения.");

                    var existingType = typeDao.GetAll().FirstOrDefault(t => t.TypeName.Equals(type.TypeName, StringComparison.OrdinalIgnoreCase) && t.TypeId != type.TypeId);
                    if (existingType != null)
                        throw new Exception($"Тип оборудования с названием '{type.TypeName}' уже существует.");

                    if (type.TypeId == 0) // New row
                    {
                        typeDao.Create(type);
                        txtStatus.Text = "Тип оборудования успешно добавлен.";
                    }
                    else // Update existing row
                    {
                        typeDao.Update(type);
                        txtStatus.Text = "Тип оборудования успешно обновлен.";
                    }

                    LoadData();
                }
                catch (Exception ex)
                {
                    txtStatus.Text = "Ошибка: " + ex.Message;
                    e.Cancel = true;
                }
            }
        }

        private void BtnFilterByCost_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCost.Text))
                    throw new ArgumentException("Введите стоимость для фильтрации.");

                decimal maxCost = decimal.Parse(txtCost.Text);
                currentTableMode = "Equipment";
                UpdateDataGridVisibility();
                var filtered = equipmentService.FilterByCost(maxCost);
                dgEquipment.ItemsSource = filtered;
                txtStatus.Text = $"Фильтр по стоимости (меньше {maxCost}) применён. Найдено записей: {filtered.Count}.";
            }
            catch (FormatException)
            {
                txtStatus.Text = "Ошибка: Стоимость должна быть числом.";
            }
            catch (Exception ex)
            {
                txtStatus.Text = $"Ошибка фильтрации: {ex.Message}";
            }
        }

        private void BtnFilterByYear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtYear.Text))
                    throw new ArgumentException("Введите год для фильтрации.");

                int maxYear = int.Parse(txtYear.Text);
                currentTableMode = "Equipment";
                UpdateDataGridVisibility();
                var filtered = equipmentService.FilterByYear(maxYear);
                dgEquipment.ItemsSource = filtered;
                txtStatus.Text = $"Фильтр по году (меньше {maxYear}) применён. Найдено записей: {filtered.Count}.";
            }
            catch (FormatException)
            {
                txtStatus.Text = "Ошибка: Год должен быть числом.";
            }
            catch (Exception ex)
            {
                txtStatus.Text = $"Ошибка фильтрации: {ex.Message}";
            }
        }

        private void BtnAverageCostByType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentTableMode = "TypeAverage";
                UpdateDataGridVisibility();
                var result = equipmentService.AverageCostByType();
                dgTypeAverage.ItemsSource = result;
                txtStatus.Text = "Средняя стоимость по типам отображена.";
            }
            catch (Exception ex)
            {
                txtStatus.Text = $"Ошибка агрегации: {ex.Message}";
            }
        }

        private void BtnCountByOrg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentTableMode = "OrgCount";
                UpdateDataGridVisibility();
                var result = equipmentService.CountByOrganization();
                dgOrgCount.ItemsSource = result;
                txtStatus.Text = "Количество оборудования по организациям отображено.";
            }
            catch (Exception ex)
            {
                txtStatus.Text = $"Ошибка группировки: {ex.Message}";
            }
        }
    }
}