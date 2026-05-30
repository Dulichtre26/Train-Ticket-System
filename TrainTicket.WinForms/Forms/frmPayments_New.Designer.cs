namespace TrainTicket.WinForms.Forms
{
    partial class frmPayments_New
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            _card = new Guna2Panel();
            lblQrCode = new Label();
            _picQrCode = new PictureBox();
            _btnConfirm = new Guna2Button();
            _txtTransactionId = new Guna2TextBox();
            lblTransactionId = new Label();
            _cboPaymentMethod = new Guna2ComboBox();
            lblPaymentMethod = new Label();
            _lblInfo = new Label();
            btnClose = new Guna2Button();
            _card.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_picQrCode).BeginInit();
            SuspendLayout();
            // 
            // _card
            // 
            _card.BorderRadius = 12;
            _card.Controls.Add(lblQrCode);
            _card.Controls.Add(_picQrCode);
            _card.Controls.Add(_btnConfirm);
            _card.Controls.Add(_txtTransactionId);
            _card.Controls.Add(lblTransactionId);
            _card.Controls.Add(_cboPaymentMethod);
            _card.Controls.Add(lblPaymentMethod);
            _card.Controls.Add(_lblInfo);
            _card.CustomizableEdges = customizableEdges7;
            _card.Location = new Point(15, 15);
            _card.Name = "_card";
            _card.ShadowDecoration.CustomizableEdges = customizableEdges8;
            _card.Size = new Size(570, 320);
            _card.TabIndex = 0;
            // 
            // lblQrCode
            // 
            lblQrCode.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblQrCode.Location = new Point(360, 65);
            lblQrCode.Name = "lblQrCode";
            lblQrCode.Size = new Size(180, 20);
            lblQrCode.TabIndex = 6;
            lblQrCode.Text = "Quét mã để thanh toán";
            // 
            // _picQrCode
            // 
            _picQrCode.BackColor = Color.White;
            _picQrCode.Location = new Point(368, 90);
            _picQrCode.Name = "_picQrCode";
            _picQrCode.Size = new Size(160, 160);
            _picQrCode.SizeMode = PictureBoxSizeMode.Zoom;
            _picQrCode.TabIndex = 7;
            _picQrCode.TabStop = false;
            // 
            // _btnConfirm
            // 
            _btnConfirm.BorderRadius = 10;
            _btnConfirm.CustomizableEdges = customizableEdges1;
            _btnConfirm.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnConfirm.ForeColor = Color.White;
            _btnConfirm.Location = new Point(66, 231);
            _btnConfirm.Name = "_btnConfirm";
            _btnConfirm.ShadowDecoration.CustomizableEdges = customizableEdges2;
            _btnConfirm.Size = new Size(217, 39);
            _btnConfirm.TabIndex = 5;
            _btnConfirm.Text = "✅ Xác nhận thanh toán";
            _btnConfirm.Click += _btnConfirm_Click;
            // 
            // _txtTransactionId
            // 
            _txtTransactionId.BorderRadius = 8;
            _txtTransactionId.CustomizableEdges = customizableEdges3;
            _txtTransactionId.DefaultText = "";
            _txtTransactionId.Font = new Font("Segoe UI", 9F);
            _txtTransactionId.Location = new Point(20, 155);
            _txtTransactionId.Margin = new Padding(3, 4, 3, 4);
            _txtTransactionId.Name = "_txtTransactionId";
            _txtTransactionId.PlaceholderText = "Nhập mã giao dịch từ cổng TT";
            _txtTransactionId.SelectedText = "";
            _txtTransactionId.ShadowDecoration.CustomizableEdges = customizableEdges4;
            _txtTransactionId.Size = new Size(300, 36);
            _txtTransactionId.TabIndex = 4;
            // 
            // lblTransactionId
            // 
            lblTransactionId.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTransactionId.Location = new Point(20, 135);
            lblTransactionId.Name = "lblTransactionId";
            lblTransactionId.Size = new Size(200, 20);
            lblTransactionId.TabIndex = 3;
            lblTransactionId.Text = "Mã giao dịch (nếu có)";
            // 
            // _cboPaymentMethod
            // 
            _cboPaymentMethod.BackColor = Color.Transparent;
            _cboPaymentMethod.BorderRadius = 8;
            _cboPaymentMethod.CustomizableEdges = customizableEdges5;
            _cboPaymentMethod.DrawMode = DrawMode.OwnerDrawFixed;
            _cboPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboPaymentMethod.FocusedColor = Color.Empty;
            _cboPaymentMethod.Font = new Font("Segoe UI", 10F);
            _cboPaymentMethod.ForeColor = Color.FromArgb(68, 88, 112);
            _cboPaymentMethod.ItemHeight = 30;
            _cboPaymentMethod.Items.AddRange(new object[] { "Cash", "BankTransfer", "MoMo", "VNPay" });
            _cboPaymentMethod.Location = new Point(20, 85);
            _cboPaymentMethod.Name = "_cboPaymentMethod";
            _cboPaymentMethod.ShadowDecoration.CustomizableEdges = customizableEdges6;
            _cboPaymentMethod.Size = new Size(300, 36);
            _cboPaymentMethod.TabIndex = 2;
            _cboPaymentMethod.SelectedIndexChanged += _cboPaymentMethod_SelectedIndexChanged;
            // 
            // lblPaymentMethod
            // 
            lblPaymentMethod.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPaymentMethod.Location = new Point(20, 65);
            lblPaymentMethod.Name = "lblPaymentMethod";
            lblPaymentMethod.Size = new Size(200, 20);
            lblPaymentMethod.TabIndex = 1;
            lblPaymentMethod.Text = "Phương thức thanh toán";
            // 
            // _lblInfo
            // 
            _lblInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblInfo.Location = new Point(18, 18);
            _lblInfo.Name = "_lblInfo";
            _lblInfo.Size = new Size(384, 30);
            _lblInfo.TabIndex = 0;
            _lblInfo.Text = "Xác nhận thanh toán cho vé số ?";
            // 
            // btnClose
            // 
            btnClose.BorderRadius = 16;
            btnClose.CustomizableEdges = customizableEdges9;
            btnClose.FillColor = Color.FromArgb(80, 255, 255, 255);
            btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            btnClose.HoverState.ForeColor = Color.White;
            btnClose.Location = new Point(560, 10);
            btnClose.Name = "btnClose";
            btnClose.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnClose.Size = new Size(32, 32);
            btnClose.TabIndex = 1;
            btnClose.Text = "✕";
            btnClose.Click += btnClose_Click;
            // 
            // frmPayments_New
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 350);
            Controls.Add(btnClose);
            Controls.Add(_card);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            Name = "frmPayments_New";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Xác nhận thanh toán";
            Load += frmPayments_New_Load;
            _card.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_picQrCode).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel _card;
        private System.Windows.Forms.Label _lblInfo;
        private System.Windows.Forms.Label lblPaymentMethod;
        private Guna.UI2.WinForms.Guna2ComboBox _cboPaymentMethod;
        private System.Windows.Forms.Label lblTransactionId;
        private Guna.UI2.WinForms.Guna2TextBox _txtTransactionId;
        private Guna.UI2.WinForms.Guna2Button _btnConfirm;
        private System.Windows.Forms.Label lblQrCode;
        private System.Windows.Forms.PictureBox _picQrCode;
        private Guna.UI2.WinForms.Guna2Button btnClose;
    }
}