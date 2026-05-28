using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ShoeStore.Forms
{
    public partial class OrderEditForm : Form
    {
        private int _orderId;
        private bool _isNew;

        public OrderEditForm(int orderId)
        {
            _orderId = orderId;
            if (orderId == 0)
                _isNew = true;
            else
                _isNew = false;
            InitializeComponent();
            if (_isNew)
                this.Text = "Добавление заказа";
            else
                this.Text = "Редактирование заказа";
            FillReferences();
            FillFields();
        }

        private void HasDelivery_Changed(object sender, EventArgs e)
        {
            _deliveryDatePicker.Enabled = _hasDeliveryBox.Checked;
        }

        private void FillReferences()
        {
            try
            {
                DataTable statuses = DatabaseHelper.ExecuteQuery("SELECT StatusId AS Id, StatusName AS Name FROM OrderStatuses ORDER BY StatusId;");
                _statusBox.DataSource = statuses;
                _statusBox.DisplayMember = "Name";
                _statusBox.ValueMember = "Id";

                DataTable points = DatabaseHelper.ExecuteQuery("SELECT PickupPointId AS Id, Address AS Name FROM PickupPoints ORDER BY Address;");
                _pickupBox.DataSource = points;
                _pickupBox.DisplayMember = "Name";
                _pickupBox.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки справочников: " + ex.Message);
            }
        }

        private void FillFields()
        {
            _orderDatePicker.Value = DateTime.Today;
            _deliveryDatePicker.Value = DateTime.Today.AddDays(5);

            if (_isNew)
            {
                object next = DatabaseHelper.ExecuteScalar("SELECT ISNULL(MAX(OrderId), 0) + 1 FROM Orders;");
                _codeBox.Text = "ORD-" + Convert.ToInt32(next).ToString("D5");
                return;
            }

            try
            {
                DataTable t = DatabaseHelper.ExecuteQuery(
                    "SELECT OrderCode, StatusId, PickupPointId, OrderDate, DeliveryDate FROM Orders WHERE OrderId = @id;",
                    new SqlParameter("@id", _orderId));
                if (t.Rows.Count == 0)
                    return;
                DataRow r = t.Rows[0];

                _codeBox.Text = r["OrderCode"].ToString();
                _statusBox.SelectedValue = (int)r["StatusId"];
                _pickupBox.SelectedValue = (int)r["PickupPointId"];
                _orderDatePicker.Value = (DateTime)r["OrderDate"];
                if (r["DeliveryDate"] != DBNull.Value)
                {
                    _hasDeliveryBox.Checked = true;
                    _deliveryDatePicker.Value = (DateTime)r["DeliveryDate"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки заказа: " + ex.Message);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (_codeBox.Text.Trim() == "")
            {
                MessageBox.Show("Укажите артикул заказа.");
                return;
            }
            if (_statusBox.SelectedValue == null || _pickupBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите статус и пункт выдачи.");
                return;
            }

            object deliveryDate;
            if (_hasDeliveryBox.Checked)
                deliveryDate = _deliveryDatePicker.Value.Date;
            else
                deliveryDate = DBNull.Value;

            try
            {
                if (_isNew)
                {
                    object uid;
                    if (AppSession.CurrentUser != null)
                        uid = AppSession.CurrentUser.UserId;
                    else
                        uid = DBNull.Value;

                    DatabaseHelper.ExecuteNonQuery(
                        "INSERT INTO Orders (OrderCode, StatusId, PickupPointId, OrderDate, DeliveryDate, UserId) VALUES (@code, @status, @pickup, @odate, @ddate, @uid);",
                        new SqlParameter("@code", _codeBox.Text.Trim()),
                        new SqlParameter("@status", (int)_statusBox.SelectedValue),
                        new SqlParameter("@pickup", (int)_pickupBox.SelectedValue),
                        new SqlParameter("@odate", _orderDatePicker.Value.Date),
                        new SqlParameter("@ddate", deliveryDate),
                        new SqlParameter("@uid", uid));
                }
                else
                {
                    DatabaseHelper.ExecuteNonQuery(
                        "UPDATE Orders SET OrderCode = @code, StatusId = @status, PickupPointId = @pickup, OrderDate = @odate, DeliveryDate = @ddate WHERE OrderId = @id;",
                        new SqlParameter("@id", _orderId),
                        new SqlParameter("@code", _codeBox.Text.Trim()),
                        new SqlParameter("@status", (int)_statusBox.SelectedValue),
                        new SqlParameter("@pickup", (int)_pickupBox.SelectedValue),
                        new SqlParameter("@odate", _orderDatePicker.Value.Date),
                        new SqlParameter("@ddate", deliveryDate));
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
