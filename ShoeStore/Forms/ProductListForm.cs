using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using ShoeStore.Models;

namespace ShoeStore.Forms
{
    public partial class ProductListForm : Form
    {
        private List<Product> _allProducts = new List<Product>();
        private static ProductEditForm _editForm;

        public ProductListForm()
        {
            InitializeComponent();
            ApplyRoleVisibility();
            LoadProducts();
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Filter_Changed(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void ApplyRoleVisibility()
        {
            if (AppSession.IsGuest)
                _userLabel.Text = "Гость";
            else
                _userLabel.Text = AppSession.CurrentUser.FullName + " (" + AppSession.CurrentUser.RoleName + ")";

            bool canFilter = AppSession.IsManager || AppSession.IsAdmin;
            _searchBox.Enabled = canFilter;
            _supplierFilter.Enabled = canFilter;
            _sortBox.Enabled = canFilter;

            _addButton.Visible = AppSession.IsAdmin;
            _deleteButton.Visible = AppSession.IsAdmin;
            _ordersButton.Visible = AppSession.IsManager || AppSession.IsAdmin;
        }

        private void LoadProducts()
        {
            try
            {
                string sql = "SELECT p.ProductId, p.ProductName, p.Description, p.CategoryId, c.CategoryName, p.ManufacturerId, m.ManufacturerName, p.SupplierId, s.SupplierName, p.UnitId, u.UnitName, p.Price, p.Quantity, p.Discount, p.ImagePath FROM Products p INNER JOIN Categories c ON p.CategoryId = c.CategoryId INNER JOIN Manufacturers m ON p.ManufacturerId = m.ManufacturerId INNER JOIN Suppliers s ON p.SupplierId = s.SupplierId INNER JOIN Units u ON p.UnitId = u.UnitId ORDER BY p.ProductId;";

                DataTable table = DatabaseHelper.ExecuteQuery(sql);
                _allProducts.Clear();
                foreach (DataRow r in table.Rows)
                {
                    Product p = new Product();
                    p.ProductId = (int)r["ProductId"];
                    p.ProductName = r["ProductName"].ToString();
                    if (r["Description"] == DBNull.Value)
                        p.Description = "";
                    else
                        p.Description = r["Description"].ToString();
                    p.CategoryId = (int)r["CategoryId"];
                    p.CategoryName = r["CategoryName"].ToString();
                    p.ManufacturerId = (int)r["ManufacturerId"];
                    p.ManufacturerName = r["ManufacturerName"].ToString();
                    p.SupplierId = (int)r["SupplierId"];
                    p.SupplierName = r["SupplierName"].ToString();
                    p.UnitId = (int)r["UnitId"];
                    p.UnitName = r["UnitName"].ToString();
                    p.Price = (decimal)r["Price"];
                    p.Quantity = (int)r["Quantity"];
                    p.Discount = (int)r["Discount"];
                    if (r["ImagePath"] == DBNull.Value)
                        p.ImagePath = null;
                    else
                        p.ImagePath = r["ImagePath"].ToString();
                    _allProducts.Add(p);
                }

                ReloadSupplierFilter();
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки товаров: " + ex.Message);
            }
        }

        private void ReloadSupplierFilter()
        {
            _supplierFilter.Items.Clear();
            _supplierFilter.Items.Add("Все поставщики");
            List<string> added = new List<string>();
            foreach (Product p in _allProducts)
            {
                if (!added.Contains(p.SupplierName))
                {
                    added.Add(p.SupplierName);
                    _supplierFilter.Items.Add(p.SupplierName);
                }
            }
            _supplierFilter.SelectedIndex = 0;
        }

        private void RefreshGrid()
        {
            List<Product> data = new List<Product>();

            if (AppSession.IsManager || AppSession.IsAdmin)
            {
                string query = _searchBox.Text.Trim().ToLower();
                string supplier = "";
                if (_supplierFilter.SelectedItem != null)
                    supplier = _supplierFilter.SelectedItem.ToString();

                foreach (Product p in _allProducts)
                {
                    if (query != "")
                    {
                        string all = "";
                        all += p.ProductName + " ";
                        all += p.Description + " ";
                        all += p.CategoryName + " ";
                        all += p.ManufacturerName + " ";
                        all += p.SupplierName + " ";
                        all += p.UnitName;
                        if (!all.ToLower().Contains(query))
                            continue;
                    }

                    if (supplier != "" && supplier != "Все поставщики")
                    {
                        if (p.SupplierName != supplier)
                            continue;
                    }

                    data.Add(p);
                }

                if (_sortBox.SelectedIndex == 1)
                    data.Sort(delegate (Product a, Product b) { return a.Quantity - b.Quantity; });
                else if (_sortBox.SelectedIndex == 2)
                    data.Sort(delegate (Product a, Product b) { return b.Quantity - a.Quantity; });
            }
            else
            {
                foreach (Product p in _allProducts)
                    data.Add(p);
            }

            BindGrid(data);
        }

        private void BindGrid(List<Product> list)
        {
            _grid.Rows.Clear();
            _grid.Columns.Clear();

            _grid.Columns.Add("Id", "ID");
            _grid.Columns.Add("Name", "Наименование");
            _grid.Columns.Add("Category", "Категория");
            _grid.Columns.Add("Manuf", "Производитель");
            _grid.Columns.Add("Supplier", "Поставщик");
            _grid.Columns.Add("Unit", "Ед.");
            _grid.Columns.Add("Price", "Цена");
            _grid.Columns.Add("Qty", "Кол-во");
            _grid.Columns.Add("Discount", "Скидка, %");

            foreach (Product p in list)
            {
                string priceCell;
                if (p.Discount > 0)
                    priceCell = p.Price.ToString("N2") + " -> " + p.FinalPrice.ToString("N2");
                else
                    priceCell = p.Price.ToString("N2");

                int rowIndex = _grid.Rows.Add(p.ProductId, p.ProductName, p.CategoryName,
                    p.ManufacturerName, p.SupplierName, p.UnitName, priceCell, p.Quantity, p.Discount);

                _grid.Rows[rowIndex].Tag = p;
            }
        }

        private void Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (!AppSession.IsAdmin)
            {
                MessageBox.Show("Редактирование доступно только администратору.");
                return;
            }
            Product product = (Product)_grid.Rows[e.RowIndex].Tag;
            if (product != null)
                OpenEditForm(product);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            OpenEditForm(null);
        }

        private void OpenEditForm(Product product)
        {
            if (_editForm != null && !_editForm.IsDisposed)
            {
                MessageBox.Show("Окно редактирования уже открыто.");
                _editForm.Activate();
                return;
            }

            _editForm = new ProductEditForm(product);
            _editForm.FormClosed += EditForm_Closed;
            _editForm.ShowDialog(this);
        }

        private void EditForm_Closed(object sender, FormClosedEventArgs e)
        {
            _editForm = null;
            LoadProducts();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_grid.CurrentRow == null)
            {
                MessageBox.Show("Выберите товар для удаления.");
                return;
            }

            Product product = (Product)_grid.CurrentRow.Tag;
            if (product == null)
                return;

            if (MessageBox.Show("Удалить товар \"" + product.ProductName + "\"?", "Подтверждение",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                int count = (int)DatabaseHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM OrderItems WHERE ProductId = @id;",
                    new SqlParameter("@id", product.ProductId));

                if (count > 0)
                {
                    MessageBox.Show("Этот товар присутствует в заказах и не может быть удалён.");
                    return;
                }

                DatabaseHelper.ExecuteNonQuery(
                    "DELETE FROM Products WHERE ProductId = @id;",
                    new SqlParameter("@id", product.ProductId));

                if (product.ImagePath != null && product.ImagePath != "" && File.Exists(product.ImagePath))
                {
                    try { File.Delete(product.ImagePath); }
                    catch { }
                }

                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления: " + ex.Message);
            }
        }

        private void OrdersButton_Click(object sender, EventArgs e)
        {
            OrderListForm form = new OrderListForm();
            form.ShowDialog(this);
        }
    }
}
