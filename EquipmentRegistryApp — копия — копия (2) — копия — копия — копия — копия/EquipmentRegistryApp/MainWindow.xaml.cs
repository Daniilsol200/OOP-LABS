using EquipmentRegistryLib;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            dgEquipment.PreviewKeyDown += DgEquipment_PreviewKeyDown;
            dgOrganizations.PreviewKeyDown += DgOrganizations_PreviewKeyDown;
            dgTypes.PreviewKeyDown += DgTypes_PreviewKeyDown;
            UpdateDataGridVisibility();
            LoadData();
        }

        private void DgEquipment_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && dgEquipment.SelectedItems.Count > 0)
            {
                try
                {
                    var result = MessageBox.Show("Вы уверены, что хотите удалить выбранные записи?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result != MessageBoxResult.Yes)
                    {
                        e.Handled = true;
                        return;
                    }

                    var selectedRows = dgEquipment.SelectedItems.Cast<DataRowView>().ToList();

                    foreach (var rowView in selectedRows)
                    {
                        int equipId = rowView["equip_id"] != DBNull.Value ? Convert.ToInt32(rowView["equip_id"]) : 0;
                        if (equipId > 0)
                        {
                            equipmentService.Delete(equipId);
                        }
                    }

                    txtStatus.Text = "Записи успешно удалены.";
                    LoadData();
                }
                catch (Exception ex)
                {
                    txtStatus.Text = $"Ошибка при удалении: {ex.Message}";
                    e.Handled = true;
                }
            }
        }

        private void DgOrganizations_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && dgOrganizations.SelectedItems.Count > 0)
            {
                try
                {
                    var result = MessageBox.Show("Вы уверены, что хотите удалить выбранные организации?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result != MessageBoxResult.Yes)
                    {
                        e.Handled = true;
                        return;
                    }

                    var selectedOrganizations = dgOrganizations.SelectedItems.Cast<Organization>().ToList();

                    foreach (var org in selectedOrganizations)
                    {
                        if (org.OrgId != 0)
                        {
                            var equipmentUsingOrg = equipmentDao.GetAll().Any(eq => eq.OrgId == org.OrgId);
                            if (equipmentUsingOrg)
                                throw new InvalidOperationException($"Нельзя удалить организацию '{org.OrgName}', так как она используется в оборудовании.");

                            organizationDao.Delete(org.OrgId);
                        }
                    }

                    txtStatus.Text = "Организации успешно удалены.";
                    LoadData();
                }
                catch (Exception ex)
                {
                    txtStatus.Text = $"Ошибка при удалении: {ex.Message}";
                    e.Handled = true;
                }
            }
        }

        private void DgTypes_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && dgTypes.SelectedItems.Count > 0)
            {
                try
                {
                    var result = MessageBox.Show("Вы уверены, что хотите удалить выбранные типы оборудования?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result != MessageBoxResult.Yes)
                    {
                        e.Handled = true;
                        return;
                    }

                    var selectedTypes = dgTypes.SelectedItems.Cast<EquipmentType>().ToList();

                    foreach (var type in selectedTypes)
                    {
                        if (type.TypeId != 0)
                        {
                            var equipmentUsingType = equipmentDao.GetAll().Any(eq => eq.TypeId == type.TypeId);
                            if (equipmentUsingType)
                                throw new InvalidOperationException($"Нельзя удалить тип '{type.TypeName}', так как он используется в оборудовании.");

                            typeDao.Delete(type.TypeId);
                        }
                    }

                    txtStatus.Text = "Типы успешно удалены.";
                    LoadData();
                }
                catch (Exception ex)
                {
                    txtStatus.Text = $"Ошибка при удалении: {ex.Message}";
                    e.Handled = true;
                }
            }
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
            groupBoxForm.Header = currentTable == "Оборудование" ? "Фильтры оборудования" : "";

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
                        var orgColumn = dgEquipment.Columns[4] as DataGridComboBoxColumn;
                        var typeColumn = dgEquipment.Columns[5] as DataGridComboBoxColumn;
                        if (orgColumn != null)
                            orgColumn.ItemsSource = organizationDao.GetAll();
                        if (typeColumn != null)
                            typeColumn.ItemsSource = typeDao.GetAll();
                    }
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
                txtYear.Text = "";
                txtCost.Text = "";
            }
        }

        private void DgEquipment_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                try
                {
                    var rowView = e.Row.Item as DataRowView;
                    if (rowView == null) return;
                    if (!e.Row.IsEditing) return; 

                    int equipId = rowView["equip_id"] != DBNull.Value ? Convert.ToInt32(rowView["equip_id"]) : 0;
                    string equipName = rowView["equip_name"]?.ToString();
                    if (!int.TryParse(rowView["manufacture_year"]?.ToString(), out int year) || year < 1900 || year > DateTime.Now.Year)
                        throw new Exception("Неверный год изготовления.");
                    if (!decimal.TryParse(rowView["cost"]?.ToString(), out decimal cost) || cost < 0)
                        throw new Exception("Неверная стоимость.");
                    int orgId = rowView["org_id"] != DBNull.Value ? Convert.ToInt32(rowView["org_id"]) : 0;
                    int typeId = rowView["type_id"] != DBNull.Value ? Convert.ToInt32(rowView["type_id"]) : 0;

                    if (string.IsNullOrWhiteSpace(equipName))
                        throw new Exception("Название обязательно для заполнения.");
                    if (orgId == 0)
                        throw new Exception("Необходимо выбрать организацию.");
                    if (typeId == 0)
                        throw new Exception("Необходимо выбрать тип оборудования.");

                    var organization = organizationDao.GetAll().FirstOrDefault(o => o.OrgId == orgId);
                    var type = typeDao.GetAll().FirstOrDefault(t => t.TypeId == typeId);

                    if (organization == null)
                        throw new Exception("Выбранная организация не существует.");
                    if (type == null)
                        throw new Exception("Выбранный тип оборудования не существует.");

                    var existingEquipment = equipmentDao.GetAll().FirstOrDefault(eq => eq.EquipName.Equals(equipName, StringComparison.OrdinalIgnoreCase) && eq.EquipId != equipId);
                    if (existingEquipment != null)
                        throw new Exception($"Оборудование с названием '{equipName}' уже существует.");

                    var equipment = new Equipment
                    {
                        EquipId = equipId,
                        EquipName = equipName,
                        ManufactureYear = year,
                        Cost = cost,
                        OrgId = orgId,
                        TypeId = typeId
                    };

                    if (equipId == 0) 
                    {
                        equipmentService.Create(equipment);
                        txtStatus.Text = "Оборудование успешно добавлено.";
                    }
                    else 
                    {
                        equipmentService.Update(equipment);
                        txtStatus.Text = "Оборудование успешно обновлено.";
                    }

                    LoadData();
                }
                catch (Exception ex)
                {
                    txtStatus.Text = $"Ошибка: {ex.Message}";
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

                    if (organization.OrgId == 0) 
                    {
                        organizationDao.Create(organization);
                        txtStatus.Text = "Организация успешно добавлена.";
                    }
                    else 
                    {
                        organizationDao.Update(organization);
                        txtStatus.Text = "Организация успешно обновлена.";
                    }

                    LoadData();
                }
                catch (Exception ex)
                {
                    txtStatus.Text = $"Ошибка: {ex.Message}";
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
                    txtStatus.Text = $"Ошибка: {ex.Message}";
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