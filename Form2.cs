using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Inregistrare_BAC
{
    public partial class Form2 : Form
    {
        private CheckedListBox checkedListBox;
        private MonthCalendar monthCalendar;
        private ListBox inscrieriListBox;
        private Label zileLabel;
        private DateTime dataExamenBAC = new DateTime(2026, 6, 8); // Data primul examen BAC 2026

        public class Materie    
        {
            public string Nume { get; set; }
            public override string ToString() => Nume;
        }

        public Form2()
        {
            InitializeComponent();
            InitializeInscriereForm();
            SeteazaLista();
        }

        private void InitializeInscriereForm()
        {
            this.Name = "mainForm";
            this.Text = "Înscriere Examene BAC";
            this.Size = new System.Drawing.Size(640, 480);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.Gainsboro;
            this.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.ForeColor = Color.Navy;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Titlu
            Label titluLabel = new Label();
            titluLabel.Text = "Înscriere examene BAC";
            titluLabel.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold);
            titluLabel.ForeColor = Color.DarkBlue;
            titluLabel.Location = new Point(10, 10);
            titluLabel.Size = new Size(600, 30);
            titluLabel.TextAlign = ContentAlignment.MiddleCenter;

            // CheckedListBox Materii
            checkedListBox = new CheckedListBox();
            checkedListBox.Name = "inputCheckedListBox";
            checkedListBox.CheckOnClick = true;
            checkedListBox.ForeColor = Color.Indigo;
            checkedListBox.Location = new Point(20, 60);
            checkedListBox.Size = new Size(240, 180);
            checkedListBox.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular);

            Label materieLabel = new Label();
            materieLabel.Text = "Selectați materiile:";
            materieLabel.Location = new Point(20, 40);
            materieLabel.Size = new Size(200, 20);
            materieLabel.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            // MonthCalendar
            monthCalendar = new MonthCalendar();
            monthCalendar.Location = new Point(280, 60);
            monthCalendar.MaxSelectionCount = 1;
            monthCalendar.MaxDate = dataExamenBAC;
            monthCalendar.MinDate = DateTime.Today;
            monthCalendar.FirstDayOfWeek = Day.Monday;
            monthCalendar.TitleBackColor = Color.LightBlue;
            monthCalendar.TrailingForeColor = Color.Gray;

            Label dataLabel = new Label();
            dataLabel.Text = "Alegeţi data examenului:";
            dataLabel.Location = new Point(280, 40);
            dataLabel.Size = new Size(200, 20);
            dataLabel.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            // Buton Inscriere
            Button inscriereBtn = new Button();
            inscriereBtn.Text = "Înscrie-mă";
            inscriereBtn.Location = new Point(20, 250);
            inscriereBtn.Size = new Size(540, 40);
            inscriereBtn.BackColor = Color.LightSkyBlue;
            inscriereBtn.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);

            // ListBox Inscrieri
            inscrieriListBox = new ListBox();
            inscrieriListBox.Location = new Point(20, 305);
            inscrieriListBox.Size = new Size(540, 110);
            inscrieriListBox.BackColor = Color.LightSkyBlue;
            inscrieriListBox.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular);

            Label inscrieriLabel = new Label();
            inscrieriLabel.Text = "Materii înscrise:";
            inscrieriLabel.Location = new Point(20, 285);
            inscrieriLabel.Size = new Size(200, 20);
            inscrieriLabel.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            // Label Zile Ramase
            zileLabel = new Label();
            zileLabel.Location = new Point(20, 425);
            zileLabel.Size = new Size(540, 25);
            zileLabel.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            zileLabel.ForeColor = Color.DarkRed;

            this.Controls.AddRange(new Control[] {
                titluLabel, materieLabel, dataLabel, inscrieriLabel, checkedListBox,
                monthCalendar, inscriereBtn, inscrieriListBox, zileLabel
            });

            // Evenimente
            inscriereBtn.Click += InscriereBtn_Click;
            monthCalendar.DateSelected += MonthCalendar_DateSelected;
        }

        private void SeteazaLista()
        {
            checkedListBox.Items.Clear();
            checkedListBox.Items.Add("Limba și Literatura Română");
            checkedListBox.Items.Add("Istorie");
            checkedListBox.Items.Add("Matematică");
            checkedListBox.Items.Add("Biologie");
            checkedListBox.Items.Add("Chimie");
            checkedListBox.Items.Add("Fizică");
            checkedListBox.Items.Add("Informatică");
        }

        private void InscriereBtn_Click(object sender, EventArgs e)
        {
            List<string> materiiSelectate = new List<string>();

            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                if (checkedListBox.GetItemChecked(i))
                {
                    materiiSelectate.Add(checkedListBox.Items[i].ToString());
                }
            }

            if (materiiSelectate.Count == 0)
            {
                MessageBox.Show("Selectați cel puțin o materie.", "Atenție",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            inscrieriListBox.Items.Clear();
            foreach (string materie in materiiSelectate)
            {
                inscrieriListBox.Items.Add($"- {materie}");
            }

            CalculeazaZileRamase();
            MessageBox.Show($"Înscriere confirmată pentru {materiiSelectate.Count} materii.",
                "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MonthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            CalculeazaZileRamase();
        }

        private void CalculeazaZileRamase()
        {
            

            int zileRamase = (dataExamenBAC.Date - DateTime.Today).Days;
            if (zileRamase < 0)
            {
                zileLabel.Text = $"Data examenului ({dataExamenBAC:dd.MM.yyyy}) a trecut.";
            }
            else if (zileRamase == 0)
            {
                zileLabel.Text = $"Astăzi e primul examen ({dataExamenBAC:dd.MM.yyyy}). Mult succes!";
            }
            else
            {
                zileLabel.Text = $"Zile până la primul examen ({dataExamenBAC:dd.MM.yyyy}): {zileRamase}";
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            CalculeazaZileRamase();
            monthCalendar.SetSelectionRange(DateTime.Today, DateTime.Today);
        }
    }
}
