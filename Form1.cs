using System;
using System.Windows.Forms;

namespace Inregistrare_BAC
{
    public partial class Form1 : Form
    {
        private Form2 formInscriere;

        public Form1()
        {
            InitializeComponent();
            InitializeLoginForm();
        }

        private void InitializeLoginForm()
        {
            this.Text = "Autentificare BAC 2026";
            this.Size = new System.Drawing.Size(420, 320);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.LightSteelBlue;

            // TextBox ID
            TextBox idTextBox = new TextBox();
            idTextBox.Name = "idTextBox";
            idTextBox.Location = new System.Drawing.Point(140, 40);
            idTextBox.Size = new System.Drawing.Size(220, 28);
            idTextBox.TabIndex = 0;

            Label idLabel = new Label();
            idLabel.Text = "ID Utilizator:";
            idLabel.Location = new System.Drawing.Point(40, 44);
            idLabel.Size = new System.Drawing.Size(90, 20);

            // TextBox Parola
            TextBox parolaTextBox = new TextBox();
            parolaTextBox.Name = "parolaTextBox";
            parolaTextBox.Location = new System.Drawing.Point(140, 85);
            parolaTextBox.Size = new System.Drawing.Size(220, 28);
            parolaTextBox.PasswordChar = '*';
            parolaTextBox.Enabled = false;
            parolaTextBox.TabIndex = 1;

            Label parolaLabel = new Label();
            parolaLabel.Text = "Parola:";
            parolaLabel.Location = new System.Drawing.Point(40, 89);
            parolaLabel.Size = new System.Drawing.Size(90, 20);

            // TextBox CNP
            TextBox cnpTextBox = new TextBox();
            cnpTextBox.Name = "cnpTextBox";
            cnpTextBox.Location = new System.Drawing.Point(140, 130);
            cnpTextBox.Size = new System.Drawing.Size(220, 28);
            cnpTextBox.Visible = false;
            cnpTextBox.TabIndex = 2;

            Label cnpLabel = new Label();
            cnpLabel.Text = "CNP:";
            cnpLabel.Name = "cnpLabel";
            cnpLabel.Location = new System.Drawing.Point(40, 134);
            cnpLabel.Size = new System.Drawing.Size(90, 20);
            cnpLabel.Visible = false;

            // Buton Autentificare - always visible and larger for simplicity
            Button autentificareBtn = new Button();
            autentificareBtn.Name = "autentificareBtn";
            autentificareBtn.Text = "Autentifică-te";
            autentificareBtn.Location = new System.Drawing.Point(140, 175);
            autentificareBtn.Size = new System.Drawing.Size(220, 36);
            autentificareBtn.TabIndex = 3;
            autentificareBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);

            // Buton Creare cont - smaller, below authenticate
            Button creareContBtn = new Button();
            creareContBtn.Name = "creareContBtn";
            creareContBtn.Text = "Nu ai cont? Creează";
            creareContBtn.Location = new System.Drawing.Point(140, 220);
            creareContBtn.Size = new System.Drawing.Size(220, 30);
            creareContBtn.TabIndex = 4;
            creareContBtn.FlatStyle = FlatStyle.System;

            // help label
            Label helpLabel = new Label();
            helpLabel.Text = "Introduceți ID, apăsați 'Autentifică-te' și urmați pașii.";
            helpLabel.Location = new System.Drawing.Point(40, 260);
            helpLabel.Size = new System.Drawing.Size(320, 30);
            helpLabel.ForeColor = System.Drawing.Color.DarkBlue;

            this.Controls.AddRange(new Control[] { idTextBox, idLabel, parolaTextBox, parolaLabel, cnpTextBox, cnpLabel, autentificareBtn, creareContBtn, helpLabel });

            // Evenimente
            idTextBox.PreviewKeyDown += IdTextBox_PreviewKeyDown;
            parolaTextBox.PreviewKeyDown += ParolaTextBox_PreviewKeyDown;
            cnpTextBox.KeyPress += CnpTextBox_KeyPress;
            autentificareBtn.Click += AutentificareBtn_Click;
            creareContBtn.Click += CreareContBtn_Click;
        }

        private void IdTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
            {
                var idBox = (TextBox)sender;
                var idText = idBox.Text.Trim();
                if (UserStore.UserExists(idText))
                {
                    ((TextBox)this.Controls["parolaTextBox"]).Enabled = true;
                    ((TextBox)this.Controls["parolaTextBox"]).Focus();
                }
                else
                {
                    var result = MessageBox.Show("Utilizatorul nu există. Doriți să creați un cont nou?", "Cont inexistent", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        ShowRegisterDialogWithPrefilledId(idText);
                    }
                    else
                    {
                        idBox.Clear();
                        idBox.Focus();
                    }
                }
            }
        }

        private void ParolaTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox parolaBox = (TextBox)sender;
                var idBox = this.Controls["idTextBox"] as TextBox;
                var idText = idBox?.Text.Trim();

                if (idText == null)
                    return;

                if (UserStore.ValidateUser(idText, parolaBox.Text))
                {
                    Label label = ((Label)Controls["cnpLabel"]);
                    label.Visible = true;
                    ((TextBox)this.Controls["cnpTextBox"]).Visible = true;
                    ((TextBox)this.Controls["cnpTextBox"]).Focus();
                }
                else
                {
                    MessageBox.Show("Parola incorectă!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    parolaBox.Clear();
                }
            }
        }

        private void CnpTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void AutentificareBtn_Click(object sender, EventArgs e)
        {
            PerformFinalAuthentication();
        }

        private void PerformFinalAuthentication()
        {
            TextBox idBox = (TextBox)this.Controls["idTextBox"];
            TextBox pwBox = (TextBox)this.Controls["parolaTextBox"];
            TextBox cnpBox = (TextBox)this.Controls["cnpTextBox"];

            var id = idBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Introduceți ID-ul.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                idBox.Focus();
                return;
            }

            if (!UserStore.UserExists(id))
            {
                var res = MessageBox.Show("Utilizatorul nu există. Doriți să creați un cont nou?", "Cont inexistent", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    ShowRegisterDialogWithPrefilledId(id);
                }
                return;
            }

            if (!pwBox.Enabled || string.IsNullOrEmpty(pwBox.Text))
            {
                pwBox.Enabled = true;
                pwBox.Focus();
                MessageBox.Show("Introduceți parola și apăsați din nou butonul 'Autentifică-te' sau Enter.", "Parolă necesară", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!UserStore.ValidateUser(id, pwBox.Text))
            {
                MessageBox.Show("Parola incorectă!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pwBox.Clear();
                pwBox.Focus();
                return;
            }

            // show cnp controls
            ((Label)this.Controls["cnpLabel"]).Visible = true;
            cnpBox.Visible = true;

            if (string.IsNullOrEmpty(cnpBox.Text))
            {
                cnpBox.Focus();
                MessageBox.Show("Introduceți CNP și apăsați din nou butonul 'Autentifică-te'.", "CNP necesar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cnpBox.Text.Length != 13)
            {
                MessageBox.Show("CNP incorect! Trebuie să aibă exact 13 cifre.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cnpBox.Clear();
                cnpBox.Focus();
                return;
            }

            if (!UserStore.TryGetCnp(id, out var storedCnp))
            {
                MessageBox.Show("Utilizator inexistent.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (storedCnp != cnpBox.Text)
            {
                MessageBox.Show("CNP-ul nu corespunde utilizatorului.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cnpBox.Clear();
                cnpBox.Focus();
                return;
            }

            // success
            MessageBox.Show("Autentificare cu succes!\nRedirecționare către înscrierea BAC...", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            formInscriere = new Form2();
            formInscriere.Show();
            this.Hide();
        }

        private void CreareContBtn_Click(object sender, EventArgs e)
        {
            ShowRegisterDialogWithPrefilledId(null);
        }

        private void ShowRegisterDialogWithPrefilledId(string prefilledId)
        {
            using (var reg = new RegisterForm())
            {
                if (!string.IsNullOrWhiteSpace(prefilledId))
                    reg.SetId(prefilledId);

                if (reg.ShowDialog() == DialogResult.OK)
                {
                    var id = reg.UserId;
                    var pass = reg.Password;
                    var cnp = reg.Cnp;

                    if (!UserStore.AddUser(id, pass, cnp))
                    {
                        MessageBox.Show("Nu s-a putut crea contul. ID-ul este posibil deja folosit sau invalid.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MessageBox.Show("Cont creat cu succes! Acum vă puteți autentifica.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // fill id and enable password
                    var idBox = this.Controls["idTextBox"] as TextBox;
                    var pwBox = this.Controls["parolaTextBox"] as TextBox;
                    if (idBox != null)
                    {
                        idBox.Text = id;
                        pwBox.Enabled = true;
                        pwBox.Focus();
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set initial focus to ID textbox if present
            var idBox = this.Controls["idTextBox"] as TextBox;
            idBox?.Focus();
        }
    }
}
