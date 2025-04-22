using EquipmentRegistryLib;
using System;
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
        private Equipment selectedEquipment;
        private Organization selectedOrganization;
        private EquipmentType selectedType;
        private string currentTable;

        public MainWindow()
        {
            InitializeComponent();
            equipmentDao = new EquipmentDao();
            organizationDao = new OrganizationDao();
            typeDao = new EquipmentTypeDao();
            currentTable = "Оборудование"; // По умолчанию
            SetupDataGridColumns();
            LoadData();
        }

        private void MenuEquipment_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Оборудование";
            menuEquipment.IsChecked = true;
            menuOrganization.IsChecked = false;
            menuType.IsChecked = false;
            UpdateFormVisibility();
            SetupDataGridColumns();
            LoadData();
        }

        private void MenuOrganization_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Организации";
            menuEquipment.IsChecked = false;
            menuOrganization.IsChecked = true;
            menuType.IsChecked = false;
            UpdateFormVisibility();
            SetupDataGridColumns();
            LoadData();
        }

        private void MenuType_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Типы оборудования";
            menuEquipment.IsChecked = false;
            menuOrganization.IsChecked = false;
            menuType.IsChecked = true;
            UpdateFormVisibility();
            SetupDataGridColumns();
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
            ClearForm();
        }

        private void SetupDataGridColumns()
        {
            dgEquipment.Columns.Clear();

            if (currentTable == "Оборудование")
            {
                dgEquipment.Columns.Add(new DataGridTextColumn
                {
                    Header = "ID",
                    Binding = new System.Windows.Data.Binding("EquipId"),
                    Width = 50,
                    ElementStyle = (Style)FindResource("CenteredCellStyle"),
                    HeaderStyle = (Style)FindResource("CenteredHeaderStyle")
                });
                dgEquipment.Columns.Add(new DataGridTextColumn
                {
                    Header = "Название",
                    Binding = new System.Windows.Data.Binding("EquipName"),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                    ElementStyle = (Style)FindResource("LeftAlignedCellStyle"),
                    HeaderStyle = (Style)FindResource("CenteredHeaderStyle")
                });
                dgEquipment.Columns.Add(new DataGridTextColumn
                {
                    Header = "Год",
                    Binding = new System.Windows.Data.Binding("ManufactureYear"),
                    Width = 80,
                    ElementStyle = (Style)FindResource("CenteredCellStyle"),
                    HeaderStyle = (Style)FindResource("CenteredHeaderStyle")
                });
                dgEquipment.Columns.Add(new DataGridTextColumn
                {
                    Header = "Стоимость",
                    Binding = new System.Windows.Data.Binding("Cost"),
                    Width = 120,
                    ElementStyle = (Style)FindResource("RightAlignedCellStyle"),
                    HeaderStyle = (Style)FindResource("CenteredHeaderStyle")
                });
                dgEquipment.Columns.Add(new DataGridTextColumn
                {
                    Header = "Организация",
                    Binding = new System.Windows.Data.Binding("Organization.OrgName"),
                    Width = 190,
                    ElementStyle = (Style)FindResource("LeftAlignedCellStyle"),
                    HeaderStyle = (Style)FindResource("CenteredHeaderStyle")
                });
                dgEquipment.Columns.Add(new DataGridTextColumn
                {
                    Header = "Тип",
                    Binding = new System.Windows.Data.Binding("Type.TypeName"),
                    Width = 190,
                    ElementStyle = (Style)FindResource("LeftAlignedCellStyle"),
                    HeaderStyle = (Style)FindResource("CenteredHeaderStyle")
                });
            }
            else if (currentTable == "Организации")
            {
                dgEquipment.Columns.Add(new DataGridTextColumn
                {
                    Header = "ID",
                    Binding = new System.Windows.Data.Binding("OrgId"),
                    Width = 50,
                    ElementStyle = (Style)FindResource("CenteredCellStyle"),
                    HeaderStyle = (Style)FindResource("CenteredHeaderStyle")
                });
                dgEquipment.Columns.Add(new DataGridTextColumn
                {
                    Header = "Название организации",
                    Binding = new System.Windows.Data.Binding("OrgName"),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                    ElementStyle = (Style)FindResource("LeftAlignedCellStyle"),
                    HeaderStyle = (Style)FindResource("CenteredHeaderStyle")
                });
            }
            else if (currentTable == "Типы оборудования")
            {
                dgEquipment.Columns.Add(new DataGridTextColumn
                {
                    Header = "ID",
                    Binding = new System.Windows.Data.Binding("TypeId"),
                    Width = 50,
                    ElementStyle = (Style)FindResource("CenteredCellStyle"),
                    HeaderStyle = (Style)FindResource("CenteredHeaderStyle")
                });
                dgEquipment.Columns.Add(new DataGridTextColumn
                {
                    Header = "Название типа",
                    Binding = new System.Windows.Data.Binding("TypeName"),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                    ElementStyle = (Style)FindResource("LeftAlignedCellStyle"),
                    HeaderStyle = (Style)FindResource("CenteredHeaderStyle")
                });
            }
        }

        private void LoadData()
        {
            try
            {
                if (currentTable == "Оборудование")
                {
                    dgEquipment.ItemsSource = equipmentDao.GetAll();
                    cmbOrganization.ItemsSource = organizationDao.GetAll();
                    cmbType.ItemsSource = typeDao.GetAll();
                }
                else if (currentTable == "Организации")
                {
                    dgEquipment.ItemsSource = organizationDao.GetAll();
                }
                else if (currentTable == "Типы оборудования")
                {
                    dgEquipment.ItemsSource = typeDao.GetAll();
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
                selectedEquipment = null;
            }
            else if (currentTable == "Организации")
            {
                txtOrgName.Text = "";
                selectedOrganization = null;
            }
            else if (currentTable == "Типы оборудования")
            {
                txtTypeName.Text = "";
                selectedType = null;
            }

            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentTable == "Оборудование")
                {
                    // Валидация ввода
                    if (string.IsNullOrWhiteSpace(txtName.Text))
                        throw new Exception("Название обязательно для заполнения.");
                    if (!int.TryParse(txtYear.Text, out int year) || year < 1900 || year > DateTime.Now.Year)
                        throw new Exception("Неверный год изготовления.");
                    if (!decimal.TryParse(txtCost.Text, out decimal cost) || cost < 0)
                        throw new Exception("Неверная стоимость.");
                    if (cmbOrganization.SelectedItem == null || cmbType.SelectedItem == null)
                        throw new Exception("Выберите организацию и тип.");

                    // Проверка уникальности имени
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

                    equipmentDao.Create(equipment);
                    txtStatus.Text = "Оборудование успешно добавлено.";
                }
                else if (currentTable == "Организации")
                {
                    if (string.IsNullOrWhiteSpace(txtOrgName.Text))
                        throw new Exception("Название организации обязательно для заполнения.");

                    // Проверка уникальности имени организации
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

                    // Проверка уникальности названия типа
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

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentTable == "Оборудование")
                {
                    if (selectedEquipment == null)
                        throw new Exception("Оборудование не выбрано.");

                    // Валидация ввода
                    if (string.IsNullOrWhiteSpace(txtName.Text))
                        throw new Exception("Название обязательно для заполнения.");
                    if (!int.TryParse(txtYear.Text, out int year) || year < 1900 || year > DateTime.Now.Year)
                        throw new Exception("Неверный год изготовления.");
                    if (!decimal.TryParse(txtCost.Text, out decimal cost) || cost < 0)
                        throw new Exception("Неверная стоимость.");
                    if (cmbOrganization.SelectedItem == null || cmbType.SelectedItem == null)
                        throw new Exception("Выберите организацию и тип.");

                    // Проверка уникальности имени при обновлении
                    var existingEquipment = equipmentDao.GetAll()
                        .FirstOrDefault(eq => eq.EquipName.Equals(txtName.Text, StringComparison.OrdinalIgnoreCase) && eq.EquipId != selectedEquipment.EquipId);
                    if (existingEquipment != null)
                        throw new Exception($"Оборудование с названием '{txtName.Text}' уже существует.");

                    selectedEquipment.EquipName = txtName.Text;
                    selectedEquipment.ManufactureYear = year;
                    selectedEquipment.Cost = cost;
                    selectedEquipment.Organization = (Organization)cmbOrganization.SelectedItem;
                    selectedEquipment.Type = (EquipmentType)cmbType.SelectedItem;

                    equipmentDao.Update(selectedEquipment);
                    txtStatus.Text = "Оборудование успешно обновлено.";
                }
                else if (currentTable == "Организации")
                {
                    if (selectedOrganization == null)
                        throw new Exception("Организация не выбрана.");

                    if (string.IsNullOrWhiteSpace(txtOrgName.Text))
                        throw new Exception("Название организации обязательно для заполнения.");

                    // Проверка уникальности имени при обновлении
                    var existingOrg = organizationDao.GetAll()
                        .FirstOrDefault(org => org.OrgName.Equals(txtOrgName.Text, StringComparison.OrdinalIgnoreCase) && org.OrgId != selectedOrganization.OrgId);
                    if (existingOrg != null)
                        throw new Exception($"Организация с названием '{txtOrgName.Text}' уже существует.");

                    selectedOrganization.OrgName = txtOrgName.Text;
                    organizationDao.Update(selectedOrganization);
                    txtStatus.Text = "Организация успешно обновлена.";
                }
                else if (currentTable == "Типы оборудования")
                {
                    if (selectedType == null)
                        throw new Exception("Тип оборудования не выбран.");

                    if (string.IsNullOrWhiteSpace(txtTypeName.Text))
                        throw new Exception("Название типа обязательно для заполнения.");

                    // Проверка уникальности названия типа при обновлении
                    var existingType = typeDao.GetAll()
                        .FirstOrDefault(t => t.TypeName.Equals(txtTypeName.Text, StringComparison.OrdinalIgnoreCase) && t.TypeId != selectedType.TypeId);
                    if (existingType != null)
                        throw new Exception($"Тип оборудования с названием '{txtTypeName.Text}' уже существует.");

                    selectedType.TypeName = txtTypeName.Text;
                    typeDao.Update(selectedType);
                    txtStatus.Text = "Тип оборудования успешно обновлён.";
                }

                LoadData();
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Ошибка: " + ex.Message;
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentTable == "Оборудование")
                {
                    if (selectedEquipment == null)
                        throw new Exception("Оборудование не выбрано.");

                    equipmentDao.Delete(selectedEquipment.EquipId);
                    txtStatus.Text = "Оборудование успешно удалено.";
                }
                else if (currentTable == "Организации")
                {
                    if (selectedOrganization == null)
                        throw new Exception("Организация не выбрана.");

                    // Проверка, используется ли организация в оборудовании
                    var equipmentUsingOrg = equipmentDao.GetAll().Any(eq => eq.Organization.OrgId == selectedOrganization.OrgId);
                    if (equipmentUsingOrg)
                        throw new Exception("Нельзя удалить организацию, которая используется в оборудовании.");

                    organizationDao.Delete(selectedOrganization.OrgId);
                    txtStatus.Text = "Организация успешно удалена.";
                }
                else if (currentTable == "Типы оборудования")
                {
                    if (selectedType == null)
                        throw new Exception("Тип оборудования не выбран.");

                    // Проверка, используется ли тип в оборудовании
                    var equipmentUsingType = equipmentDao.GetAll().Any(eq => eq.Type.TypeId == selectedType.TypeId);
                    if (equipmentUsingType)
                        throw new Exception("Нельзя удалить тип оборудования, который используется в оборудовании.");

                    typeDao.Delete(selectedType.TypeId);
                    txtStatus.Text = "Тип оборудования успешно удалён.";
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
            LoadData();
        }

        private void DgEquipment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentTable == "Оборудование" && dgEquipment.SelectedItem is Equipment equipment)
            {
                selectedEquipment = equipment;
                txtName.Text = equipment.EquipName;
                txtYear.Text = equipment.ManufactureYear.ToString();
                txtCost.Text = equipment.Cost.ToString();
                cmbOrganization.SelectedItem = cmbOrganization.Items.Cast<Organization>()
                    .FirstOrDefault(o => o.OrgId == equipment.Organization.OrgId);
                cmbType.SelectedItem = cmbType.Items.Cast<EquipmentType>()
                    .FirstOrDefault(t => t.TypeId == equipment.Type.TypeId);
                btnUpdate.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
            else if (currentTable == "Организации" && dgEquipment.SelectedItem is Organization organization)
            {
                selectedOrganization = organization;
                txtOrgName.Text = organization.OrgName;
                btnUpdate.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
            else if (currentTable == "Типы оборудования" && dgEquipment.SelectedItem is EquipmentType type)
            {
                selectedType = type;
                txtTypeName.Text = type.TypeName;
                btnUpdate.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
        }
    }
}