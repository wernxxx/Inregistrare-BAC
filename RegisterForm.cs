using System;
using System.Drawing;
using System.Windows.Forms;

namespace Inregistrare_BAC
{
    public class RegisterForm : Form
    {
        private TextBox idTextBox;
        private TextBox passwordTextBox;
        private TextBox cnpTextBox;
        private Button createBtn;
        private Button cancelBtn;

        public string UserId => idTextBox.Text.Trim();
        public string Password => passwordTextBox.Text;
        public string Cnp => cnpTextBox.Text;

        public RegisterForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Creare cont nou";
            this.Size = new Size(360, 260);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;

            Label idLabel = new Label() { Text = "ID utilizator:", Location = new Point(20, 20), Size = new Size(100, 20) };
            idTextBox = new TextBox() { Location = new Point(130, 20), Size = new Size(180, 25) };

            Label passwordLabel = new Label() { Text = "Parolă:", Location = new Point(20, 60), Size = new Size(100, 20) };
            passwordTextBox = new TextBox() { Location = new Point(130, 60), Size = new Size(180, 25), PasswordChar = '*' };

            Label cnpLabel = new Label() { Text = "CNP:", Location = new Point(20, 100), Size = new Size(100, 20) };
            cnpTextBox = new TextBox() { Location = new Point(130, 100), Size = new Size(180, 25) };
            cnpTextBox.KeyPress += CnpTextBox_KeyPress;

            createBtn = new Button() { Text = "Creează", Location = new Point(130, 140), Size = new Size(80, 30) };
            cancelBtn = new Button() { Text = "Anulează", Location = new Point(230, 140), Size = new Size(80, 30) };

            createBtn.Click += CreateBtn_Click;
            cancelBtn.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { idLabel, idTextBox, passwordLabel, passwordTextBox, cnpLabel, cnpTextBox, createBtn, cancelBtn });
        }

        public void SetId(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
                idTextBox.Text = id;
        }

        private void CnpTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserId) || string.IsNullOrWhiteSpace(Password) || Cnp.Length != 13)
            {
                MessageBox.Show("Completați toate câmpurile corect (CNP = 13 cifre).", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // success
            this.DialogResult = DialogResult.OK;
        }
    }
}
