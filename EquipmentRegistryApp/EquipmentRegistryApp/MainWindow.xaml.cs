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

        public MainWindow()
        {
            InitializeComponent();
            equipmentDao = new EquipmentDao();
            organizationDao = new OrganizationDao();
            typeDao = new EquipmentTypeDao();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Загрузка данных в таблицу
                dgEquipment.ItemsSource = equipmentDao.GetAll();

                // Загрузка справочных данных в выпадающие списки
                cmbOrganization.ItemsSource = organizationDao.GetAll();
                cmbType.ItemsSource = typeDao.GetAll();

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
            txtName.Text = "";
            txtYear.Text = "";
            txtCost.Text = "";
            cmbOrganization.SelectedIndex = -1;
            cmbType.SelectedIndex = -1;
            selectedEquipment = null;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
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

                var equipment = new Equipment
                {
                    EquipName = txtName.Text,
                    ManufactureYear = year,
                    Cost = cost,
                    Organization = (Organization)cmbOrganization.SelectedItem,
                    Type = (EquipmentType)cmbType.SelectedItem
                };

                equipmentDao.Create(equipment);
                LoadData();
                txtStatus.Text = "Оборудование успешно добавлено.";
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

                selectedEquipment.EquipName = txtName.Text;
                selectedEquipment.ManufactureYear = year;
                selectedEquipment.Cost = cost;
                selectedEquipment.Organization = (Organization)cmbOrganization.SelectedItem;
                selectedEquipment.Type = (EquipmentType)cmbType.SelectedItem;

                equipmentDao.Update(selectedEquipment);
                LoadData();
                txtStatus.Text = "Оборудование успешно обновлено.";
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
                if (selectedEquipment == null)
                    throw new Exception("Оборудование не выбрано.");

                equipmentDao.Delete(selectedEquipment.EquipId);
                LoadData();
                txtStatus.Text = "Оборудование успешно удалено.";
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
            if (dgEquipment.SelectedItem is Equipment equipment)
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
        }
    }
}