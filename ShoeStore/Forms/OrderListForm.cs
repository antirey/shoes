using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ShoeStore.Forms
{
    public partial class OrderListForm : Form
    {
        public OrderListForm()
        {
            InitializeComponent();
            _addButton.Visible = AppSession.IsAdmin;
            _editButton.Visible = AppSession.IsAdmin;
            _deleteButton.Visible = AppSession.IsAdmin;
            LoadOrders();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            OpenEdit(0);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (_grid.CurrentRow != null)
                OpenEdit((int)_grid.CurrentRow.Cells["OrderId"].Value);
        }

        private void Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                OpenEdit((int)_grid.Rows[e.RowIndex].Cells["OrderId"].Value);
        }

        private void LoadOrders()
        {
            try
            {
                string sql = "SELECT o.OrderId, o.OrderCode, s.StatusName, p.Address AS PickupAddress, o.OrderDate, o.DeliveryDate FROM Orders o INNER JOIN OrderStatuses s ON o.StatusId = s.StatusId INNER JOIN PickupPoints p ON o.PickupPointId = p.PickupPointId ORDER BY o.OrderId DESC;";

                DataTable table = DatabaseHelper.ExecuteQuery(sql);
                _grid.DataSource = table;

                if (_grid.Columns.Contains("OrderId"))
                    _grid.Columns["OrderId"].HeaderText = "ID";
                if (_grid.Columns.Contains("OrderCode"))
                    _grid.Columns["OrderCode"].HeaderText = "Артикул";
                if (_grid.Columns.Contains("StatusName"))
                    _grid.Columns["StatusName"].HeaderText = "Статус";
                if (_grid.Columns.Contains("PickupAddress"))
                    _grid.Columns["PickupAddress"].HeaderText = "Пункт выдачи";
                if (_grid.Columns.Contains("OrderDate"))
                    _grid.Columns["OrderDate"].HeaderText = "Дата заказа";
                if (_grid.Columns.Contains("DeliveryDate"))
                    _grid.Columns["DeliveryDate"].HeaderText = "Дата выдачи";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки заказов: " + ex.Message);
            }
        }

        private void OpenEdit(int orderId)
        {
            if (!AppSession.IsAdmin)
            {
                MessageBox.Show("Редактирование заказов доступно только администратору.");
                return;
            }
            OrderEditForm form = new OrderEditForm(orderId);
            if (form.ShowDialog(this) == DialogResult.OK)
                LoadOrders();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_grid.CurrentRow == null)
            {
                MessageBox.Show("Выберите заказ.");
                return;
            }
            int id = (int)_grid.CurrentRow.Cells["OrderId"].Value;
            string code = _grid.CurrentRow.Cells["OrderCode"].Value.ToString();

            if (MessageBox.Show("Удалить заказ " + code + "?", "Подтверждение",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                DatabaseHelper.ExecuteNonQuery(
                    "DELETE FROM Orders WHERE OrderId = @id;",
                    new SqlParameter("@id", id));
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления: " + ex.Message);
            }
        }
    }
}
