namespace ShoeStore.Forms
{
    partial class OrderEditForm
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
            this.codeLabel = new System.Windows.Forms.Label();
            this._codeBox = new System.Windows.Forms.TextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this._statusBox = new System.Windows.Forms.ComboBox();
            this.pickupLabel = new System.Windows.Forms.Label();
            this._pickupBox = new System.Windows.Forms.ComboBox();
            this.orderDateLabel = new System.Windows.Forms.Label();
            this._orderDatePicker = new System.Windows.Forms.DateTimePicker();
            this.deliveryDateLabel = new System.Windows.Forms.Label();
            this._hasDeliveryBox = new System.Windows.Forms.CheckBox();
            this._deliveryDatePicker = new System.Windows.Forms.DateTimePicker();
            this._saveButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // codeLabel
            //
            this.codeLabel.AutoSize = true;
            this.codeLabel.Location = new System.Drawing.Point(30, 30);
            this.codeLabel.Name = "codeLabel";
            this.codeLabel.Size = new System.Drawing.Size(50, 13);
            this.codeLabel.TabIndex = 0;
            this.codeLabel.Text = "Артикул:";
            //
            // _codeBox
            //
            this._codeBox.Location = new System.Drawing.Point(200, 27);
            this._codeBox.Name = "_codeBox";
            this._codeBox.Size = new System.Drawing.Size(250, 20);
            this._codeBox.TabIndex = 1;
            //
            // statusLabel
            //
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(30, 72);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(44, 13);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "Статус:";
            //
            // _statusBox
            //
            this._statusBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._statusBox.Location = new System.Drawing.Point(200, 69);
            this._statusBox.Name = "_statusBox";
            this._statusBox.Size = new System.Drawing.Size(250, 21);
            this._statusBox.TabIndex = 3;
            //
            // pickupLabel
            //
            this.pickupLabel.AutoSize = true;
            this.pickupLabel.Location = new System.Drawing.Point(30, 114);
            this.pickupLabel.Name = "pickupLabel";
            this.pickupLabel.Size = new System.Drawing.Size(78, 13);
            this.pickupLabel.TabIndex = 4;
            this.pickupLabel.Text = "Пункт выдачи:";
            //
            // _pickupBox
            //
            this._pickupBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._pickupBox.Location = new System.Drawing.Point(200, 111);
            this._pickupBox.Name = "_pickupBox";
            this._pickupBox.Size = new System.Drawing.Size(250, 21);
            this._pickupBox.TabIndex = 5;
            //
            // orderDateLabel
            //
            this.orderDateLabel.AutoSize = true;
            this.orderDateLabel.Location = new System.Drawing.Point(30, 156);
            this.orderDateLabel.Name = "orderDateLabel";
            this.orderDateLabel.Size = new System.Drawing.Size(72, 13);
            this.orderDateLabel.TabIndex = 6;
            this.orderDateLabel.Text = "Дата заказа:";
            //
            // _orderDatePicker
            //
            this._orderDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._orderDatePicker.Location = new System.Drawing.Point(200, 153);
            this._orderDatePicker.Name = "_orderDatePicker";
            this._orderDatePicker.Size = new System.Drawing.Size(250, 20);
            this._orderDatePicker.TabIndex = 7;
            //
            // deliveryDateLabel
            //
            this.deliveryDateLabel.AutoSize = true;
            this.deliveryDateLabel.Location = new System.Drawing.Point(30, 198);
            this.deliveryDateLabel.Name = "deliveryDateLabel";
            this.deliveryDateLabel.Size = new System.Drawing.Size(72, 13);
            this.deliveryDateLabel.TabIndex = 8;
            this.deliveryDateLabel.Text = "Дата выдачи:";
            //
            // _hasDeliveryBox
            //
            this._hasDeliveryBox.AutoSize = true;
            this._hasDeliveryBox.Location = new System.Drawing.Point(200, 197);
            this._hasDeliveryBox.Name = "_hasDeliveryBox";
            this._hasDeliveryBox.Size = new System.Drawing.Size(65, 17);
            this._hasDeliveryBox.TabIndex = 9;
            this._hasDeliveryBox.Text = "Указать";
            this._hasDeliveryBox.UseVisualStyleBackColor = true;
            this._hasDeliveryBox.CheckedChanged += new System.EventHandler(this.HasDelivery_Changed);
            //
            // _deliveryDatePicker
            //
            this._deliveryDatePicker.Enabled = false;
            this._deliveryDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._deliveryDatePicker.Location = new System.Drawing.Point(295, 195);
            this._deliveryDatePicker.Name = "_deliveryDatePicker";
            this._deliveryDatePicker.Size = new System.Drawing.Size(155, 20);
            this._deliveryDatePicker.TabIndex = 10;
            //
            // _saveButton
            //
            this._saveButton.Location = new System.Drawing.Point(280, 320);
            this._saveButton.Name = "_saveButton";
            this._saveButton.Size = new System.Drawing.Size(180, 35);
            this._saveButton.TabIndex = 11;
            this._saveButton.Text = "Сохранить";
            this._saveButton.UseVisualStyleBackColor = true;
            this._saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            //
            // _cancelButton
            //
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(90, 320);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(180, 35);
            this._cancelButton.TabIndex = 12;
            this._cancelButton.Text = "Отмена";
            this._cancelButton.UseVisualStyleBackColor = true;
            //
            // OrderEditForm
            //
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(504, 381);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._saveButton);
            this.Controls.Add(this._deliveryDatePicker);
            this.Controls.Add(this._hasDeliveryBox);
            this.Controls.Add(this.deliveryDateLabel);
            this.Controls.Add(this._orderDatePicker);
            this.Controls.Add(this.orderDateLabel);
            this.Controls.Add(this._pickupBox);
            this.Controls.Add(this.pickupLabel);
            this.Controls.Add(this._statusBox);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this._codeBox);
            this.Controls.Add(this.codeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Заказ";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label codeLabel;
        private System.Windows.Forms.TextBox _codeBox;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ComboBox _statusBox;
        private System.Windows.Forms.Label pickupLabel;
        private System.Windows.Forms.ComboBox _pickupBox;
        private System.Windows.Forms.Label orderDateLabel;
        private System.Windows.Forms.DateTimePicker _orderDatePicker;
        private System.Windows.Forms.Label deliveryDateLabel;
        private System.Windows.Forms.CheckBox _hasDeliveryBox;
        private System.Windows.Forms.DateTimePicker _deliveryDatePicker;
        private System.Windows.Forms.Button _saveButton;
        private System.Windows.Forms.Button _cancelButton;
    }
}
