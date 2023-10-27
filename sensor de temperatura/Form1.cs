using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;  

namespace sensor_de_temperatura
{
    public partial class Form1 : Form
    {
        delegate void setTextDelegate(string value);
        public SerialPort ArduinoPort
        {
            get;
        }
    public Form1()
        {
            InitializeComponent();
            ArduinoPort = new System.IO.Ports.SerialPort();
            ArduinoPort.PortName = "COM4";
            ArduinoPort.BaudRate = 9600;
            ArduinoPort.DataBits = 8;
            ArduinoPort.ReadTimeout = 500;
            ArduinoPort.WriteTimeout = 500;
            ArduinoPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            this.button1.Click += button1_Click;
        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string dato=ArduinoPort.ReadLine();
            EscribirTxt(dato);

        }
        private void EscribirTxt(string dato)
        {
            if (InvokeRequired)
                try
                {
                    Invoke(new setTextDelegate(EscribirTxt), dato);
                }
                catch { }
            else 
                label1.Text=dato;
            



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            button2.Enabled = true;
            try
            {
                if (!ArduinoPort.IsOpen)
                    ArduinoPort.Open();
                if (int.TryParse(label2.Text, out int temperatureLimit))
                {
                    string limitString = textBox1.ToString();
                    ArduinoPort.Write(limitString);

                }
                else
                {
                    MessageBox.Show("Ingresa un valor numérico válido en el TextBox del limite de la temperatura.");

                }
                label1.Text = "Conexión OK";
                label1.ForeColor = System.Drawing.Color.Lime;


            }
            catch
            {
                MessageBox.Show("Configure el puerto de comunicación correcto o Desconecte");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            button1.Enabled = true;
            button2.Enabled = false;
            if (ArduinoPort.IsOpen)
                ArduinoPort.Close();
            label2.Text = "Desconectado";
            label2.ForeColor = System.Drawing.Color.Red;
            label1.Text = "00.0";
        }
    }
}
