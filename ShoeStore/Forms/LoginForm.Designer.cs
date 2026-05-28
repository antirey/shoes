namespace ShoeStore.Forms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._titleLabel = new System.Windows.Forms.Label();
            this.loginLabel = new System.Windows.Forms.Label();
            this._loginBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this._passwordBox = new System.Windows.Forms.TextBox();
            this._loginButton = new System.Windows.Forms.Button();
            this._guestButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // _titleLabel
            //
            this._titleLabel.AutoSize = true;
            this._titleLabel.Location = new System.Drawing.Point(40, 20);
            this._titleLabel.Name = "_titleLabel";
            this._titleLabel.Size = new System.Drawing.Size(60, 13);
            this._titleLabel.TabIndex = 0;
            this._titleLabel.Text = "ShoeStore";
            //
            // loginLabel
            //
            this.loginLabel.AutoSize = true;
            this.loginLabel.Location = new System.Drawing.Point(40, 90);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(40, 13);
            this.loginLabel.TabIndex = 1;
            this.loginLabel.Text = "Логин:";
            //
            // _loginBox
            //
            this._loginBox.Location = new System.Drawing.Point(150, 87);
            this._loginBox.Name = "_loginBox";
            this._loginBox.Size = new System.Drawing.Size(220, 20);
            this._loginBox.TabIndex = 2;
            //
            // passwordLabel
            //
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(40, 130);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(45, 13);
            this.passwordLabel.TabIndex = 3;
            this.passwordLabel.Text = "Пароль:";
            //
            // _passwordBox
            //
            this._passwordBox.Location = new System.Drawing.Point(150, 127);
            this._passwordBox.Name = "_passwordBox";
            this._passwordBox.Size = new System.Drawing.Size(220, 20);
            this._passwordBox.TabIndex = 4;
            this._passwordBox.UseSystemPasswordChar = true;
            //
            // _loginButton
            //
            this._loginButton.Location = new System.Drawing.Point(40, 180);
            this._loginButton.Name = "_loginButton";
            this._loginButton.Size = new System.Drawing.Size(330, 35);
            this._loginButton.TabIndex = 5;
            this._loginButton.Text = "Войти";
            this._loginButton.UseVisualStyleBackColor = true;
            this._loginButton.Click += new System.EventHandler(this.LoginButton_Click);
            //
            // _guestButton
            //
            this._guestButton.Location = new System.Drawing.Point(40, 230);
            this._guestButton.Name = "_guestButton";
            this._guestButton.Size = new System.Drawing.Size(330, 35);
            this._guestButton.TabIndex = 6;
            this._guestButton.Text = "Войти как гость";
            this._guestButton.UseVisualStyleBackColor = true;
            this._guestButton.Click += new System.EventHandler(this.GuestButton_Click);
            //
            // LoginForm
            //
            this.AcceptButton = this._loginButton;
            this.ClientSize = new System.Drawing.Size(404, 321);
            this.Controls.Add(this._guestButton);
            this.Controls.Add(this._loginButton);
            this.Controls.Add(this._passwordBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this._loginBox);
            this.Controls.Add(this.loginLabel);
            this.Controls.Add(this._titleLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Авторизация";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label _titleLabel;
        private System.Windows.Forms.Label loginLabel;
        private System.Windows.Forms.TextBox _loginBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox _passwordBox;
        private System.Windows.Forms.Button _loginButton;
        private System.Windows.Forms.Button _guestButton;
    }
}
