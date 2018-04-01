using System;
using System.Windows.Forms;

namespace GuiApp
{
    public partial class Form1 : Form
    {
        WcfVelib.VelibOperationsClient client;
        string selectedCity;
        string selectedStation;
        string[] stations;
        Label[] labels;


        public Form1()
        {
            InitializeComponent();
            client = new WcfVelib.VelibOperationsClient();

            selectedCity = null;
            selectedStation = null;

            Label[] labelsInit = { label1, label2, label3, label4, label5, label6};
            labels = labelsInit;

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }

            pictureBox1.ImageLocation = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/42/JCDecaux_logo.svg/1200px-JCDecaux_logo.svg.png";
            pictureBox2.ImageLocation = "http://unice.fr/formation/formation-initiale/epuing54/++resource++unice.gof.images/logos/epu.png";

            FillCitiesAsync();
        }


        private async void FillCitiesAsync()
        {
            string[] cities = await client.GetCitiesAsync();

            comboBox1.Items.Clear();

            if (cities != null)
            {
                foreach (string item in cities)
                {
                    comboBox1.Items.Add(item);
                }
            }
        }

        private async void FillStationsAsync(string city)
        {
            stations = await client.GetStationsAsync(city);

            comboBox2.Items.Clear();
            comboBox2.Text = "";

            if (stations != null)
            {
                foreach (string item in stations)
                {
                    comboBox2.Items.Add(item.Split('#')[0]);
                }
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCity = comboBox1.SelectedItem.ToString();
            FillStationsAsync(selectedCity);

            for (int i = 2; i < labels.Length-1; i++)
            {
                labels[i].Text = "";
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedStation = comboBox2.SelectedItem.ToString();

            foreach (string item in stations)
            {
                string[] splitted = item.Split('#');
                string name = splitted[0];
                string status = splitted[1];
                string available_bikes = splitted[2];
                string available_bike_stands = splitted[3];

                if (name.Equals(selectedStation))
                {
                    label3.Text = "Statut de la gare : " + status;
                    label4.Text = "Nombre de vélos restants : " + available_bikes;
                    label5.Text = "Nombre de stands disponibles : " + available_bike_stands;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
