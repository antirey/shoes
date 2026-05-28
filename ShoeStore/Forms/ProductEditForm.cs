using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ShoeStore.Models;

namespace ShoeStore.Forms
{
    public partial class ProductEditForm : Form
    {
        private Product _product;
        private bool _isNew;
        private string _currentImagePath;

        public ProductEditForm(Product product)
        {
            if (product == null)
            {
                _isNew = true;
                _product = new Product();
            }
            else
            {
                _isNew = false;
                _product = product;
            }
            InitializeComponent();
            if (_isNew)
                this.Text = "Добавление товара";
            else
                this.Text = "Редактирование товара";
            FillReferences();
            FillFields();
        }

        private void FillReferences()
        {
            try
            {
                FillCombo(_categoryBox, "SELECT CategoryId AS Id, CategoryName AS Name FROM Categories ORDER BY CategoryName;");
                FillCombo(_manufacturerBox, "SELECT ManufacturerId AS Id, ManufacturerName AS Name FROM Manufacturers ORDER BY ManufacturerName;");
                FillCombo(_supplierBox, "SELECT SupplierId AS Id, SupplierName AS Name FROM Suppliers ORDER BY SupplierName;");
                FillCombo(_unitBox, "SELECT UnitId AS Id, UnitName AS Name FROM Units ORDER BY UnitName;");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки справочников: " + ex.Message);
            }
        }

        private void FillCombo(ComboBox box, string sql)
        {
            DataTable table = DatabaseHelper.ExecuteQuery(sql);
            box.DataSource = table;
            box.DisplayMember = "Name";
            box.ValueMember = "Id";
        }

        private void FillFields()
        {
            if (_isNew)
            {
                object maxId = DatabaseHelper.ExecuteScalar("SELECT ISNULL(MAX(ProductId), 0) + 1 FROM Products;");
                _idBox.Text = "(новый: " + maxId + ")";
                _priceBox.Value = 0;
                _quantityBox.Value = 0;
                _discountBox.Value = 0;
                return;
            }

            _idBox.Text = _product.ProductId.ToString();
            _nameBox.Text = _product.ProductName;
            _descBox.Text = _product.Description;
            _categoryBox.SelectedValue = _product.CategoryId;
            _manufacturerBox.SelectedValue = _product.ManufacturerId;
            _supplierBox.SelectedValue = _product.SupplierId;
            _unitBox.SelectedValue = _product.UnitId;
            _priceBox.Value = _product.Price;
            _quantityBox.Value = _product.Quantity;
            _discountBox.Value = _product.Discount;
            _currentImagePath = _product.ImagePath;
            LoadImage(_currentImagePath);
        }

        private void LoadImage(string path)
        {
            if (path != null && path != "" && File.Exists(path))
            {
                try
                {
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    _imageBox.Image = Image.FromStream(fs);
                    fs.Close();
                }
                catch
                {
                    _imageBox.Image = null;
                }
            }
            else
            {
                _imageBox.Image = null;
            }
        }

        private void SelectImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Изображения|*.png;*.jpg;*.jpeg;*.bmp";
            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                string imagesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                Directory.CreateDirectory(imagesDir);

                string ext = Path.GetExtension(dlg.FileName);
                string newName = "product_" + Guid.NewGuid().ToString("N") + ext;
                string newFullPath = Path.Combine(imagesDir, newName);

                File.Copy(dlg.FileName, newFullPath);

                if (_currentImagePath != null && _currentImagePath != "" && File.Exists(_currentImagePath))
                {
                    try { File.Delete(_currentImagePath); }
                    catch { }
                }

                _currentImagePath = newFullPath;
                LoadImage(_currentImagePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить изображение: " + ex.Message);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (_nameBox.Text.Trim() == "")
            {
                MessageBox.Show("Укажите наименование товара.");
                _nameBox.Focus();
                return;
            }
            if (_categoryBox.SelectedValue == null || _manufacturerBox.SelectedValue == null ||
                _supplierBox.SelectedValue == null || _unitBox.SelectedValue == null)
            {
                MessageBox.Show("Заполните все обязательные поля.");
                return;
            }

            object img;
            if (_currentImagePath != null)
                img = _currentImagePath;
            else
                img = DBNull.Value;

            object desc;
            if (_descBox.Text != null)
                desc = _descBox.Text;
            else
                desc = DBNull.Value;

            try
            {
                if (_isNew)
                {
                    string sql = "INSERT INTO Products (ProductName, Description, CategoryId, ManufacturerId, SupplierId, UnitId, Price, Quantity, Discount, ImagePath) VALUES (@name, @desc, @cat, @manuf, @sup, @unit, @price, @qty, @disc, @img);";
                    DatabaseHelper.ExecuteNonQuery(sql,
                        new SqlParameter("@name", _nameBox.Text.Trim()),
                        new SqlParameter("@desc", desc),
                        new SqlParameter("@cat", (int)_categoryBox.SelectedValue),
                        new SqlParameter("@manuf", (int)_manufacturerBox.SelectedValue),
                        new SqlParameter("@sup", (int)_supplierBox.SelectedValue),
                        new SqlParameter("@unit", (int)_unitBox.SelectedValue),
                        new SqlParameter("@price", _priceBox.Value),
                        new SqlParameter("@qty", (int)_quantityBox.Value),
                        new SqlParameter("@disc", (int)_discountBox.Value),
                        new SqlParameter("@img", img));
                }
                else
                {
                    string sql = "UPDATE Products SET ProductName = @name, Description = @desc, CategoryId = @cat, ManufacturerId = @manuf, SupplierId = @sup, UnitId = @unit, Price = @price, Quantity = @qty, Discount = @disc, ImagePath = @img WHERE ProductId = @id;";
                    DatabaseHelper.ExecuteNonQuery(sql,
                        new SqlParameter("@id", _product.ProductId),
                        new SqlParameter("@name", _nameBox.Text.Trim()),
                        new SqlParameter("@desc", desc),
                        new SqlParameter("@cat", (int)_categoryBox.SelectedValue),
                        new SqlParameter("@manuf", (int)_manufacturerBox.SelectedValue),
                        new SqlParameter("@sup", (int)_supplierBox.SelectedValue),
                        new SqlParameter("@unit", (int)_unitBox.SelectedValue),
                        new SqlParameter("@price", _priceBox.Value),
                        new SqlParameter("@qty", (int)_quantityBox.Value),
                        new SqlParameter("@disc", (int)_discountBox.Value),
                        new SqlParameter("@img", img));
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }
    }
}
