using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using ShoeStore.Models;

namespace ShoeStore.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string login = _loginBox.Text.Trim();
            string password = _passwordBox.Text;

            if (login == "" || password == "")
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            try
            {
                string sql = "SELECT u.UserId, u.Login, u.FullName, u.RoleId, r.RoleName FROM Users u INNER JOIN Roles r ON u.RoleId = r.RoleId WHERE u.Login = @login AND u.Password = @password;";

                DataTable table = DatabaseHelper.ExecuteQuery(sql,
                    new SqlParameter("@login", login),
                    new SqlParameter("@password", password));

                if (table.Rows.Count == 0)
                {
                    MessageBox.Show("Неверный логин или пароль.");
                    return;
                }

                DataRow row = table.Rows[0];
                UserInfo user = new UserInfo();
                user.UserId = (int)row["UserId"];
                user.Login = row["Login"].ToString();
                user.FullName = row["FullName"].ToString();
                user.RoleId = (int)row["RoleId"];
                user.RoleName = row["RoleName"].ToString();
                AppSession.CurrentUser = user;

                OpenProductList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка базы данных: " + ex.Message);
            }
        }

        private void GuestButton_Click(object sender, EventArgs e)
        {
            AppSession.Clear();
            OpenProductList();
        }

        private void OpenProductList()
        {
            Hide();
            ProductListForm form = new ProductListForm();
            form.ShowDialog();
            _loginBox.Text = "";
            _passwordBox.Text = "";
            Show();
        }
    }
}
