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

namespace Monitor_Serial
{
    public partial class Form1 : Form
    {

        string RxString;

        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void atualizaCom()
        {
            //Limpa todos os items em cbPortas caso existam
            cbPortas.Items.Clear();
            /* Para cada nome de porta de comunicação serial indentificado
             * será atribuido á variável 's' */
            foreach (string s in SerialPort.GetPortNames())
            {
                //adiciona a variável  's' a cada item de cbPortas
                cbPortas.Items.Add(s);
            }
            //Seleciona o item de índice 0 em cbPortas
            cbPortas.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            atualizaCom();

            cbBaud.SelectedText = "9600";

            paridade();

            cdBitsDados.SelectedText = "8";

            bitsParadas();

            btnFechar.Enabled = false;
            btnAbrir.Enabled = true;
            btnSair.Enabled = true;
        }

        private void cbPortas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void paridade()
        {
            int i = 0;

            //Limpa todos os items em cbParidade caso exixtam
            cbParidade.Items.Clear();

            //Para cada nome de paridade identificado será atribuido á variavel 's' 
            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                cbParidade.Items.Add(s);

                /*verifica se o nome recebido é 'None',
                 * caso seja, seleciona o seu índice para exibição
                 */
                if (s == "None")
                    cbParidade.SelectedIndex = i;

                i++;  //incrementa a variável i
            }
        }

        private void bitsParadas()
        {
            int i = 0;

            cdBitsParadas.Items.Clear();

            foreach(string str in Enum.GetNames(typeof(StopBits)))
            {
                cdBitsParadas.Items.Add(str);

                if (str == "One")
                    cdBitsParadas.SelectedIndex = i;

                i++;
            }
        }

        private void cdBitsDados_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbBaud_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cdBitsParadas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            if (Serial.IsOpen == true) 
                Serial.Close();

            Serial.PortName = cbPortas.Text;
            Serial.BaudRate = Int32.Parse(cbBaud.Text);
            Serial.Parity = (Parity)cbParidade.SelectedIndex;
            Serial.DataBits = Int32.Parse(cdBitsDados.Text);
            Serial.StopBits = (StopBits)cdBitsParadas.SelectedIndex;

            try
            {
                Serial.Open();
                btnAbrir.Enabled = false;
                btnFechar.Enabled = true;
                btnFechar.Enabled = false;
            }
            catch
            {
                MessageBox.Show("Não foi possivel abrir a porta selecionada",
                    "ATENÇÂO",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                btnFechar.Enabled = false;
                btnAbrir.Enabled = true;
                btnSair.Enabled = false;
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Serial.Close();
            btnFechar.Enabled = false;
            btnSair.Enabled = true;
            btnSair.Enabled = true;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Serial.Close();
            Close();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if(Serial.IsOpen)
            {
                Serial.Write(txtTransmite.Text + "\r\n");
                txtTransmite.Clear();
            }
        }

        private void txtTransmite_TextChanged(object sender, EventArgs e)
        {

        }

        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            RxString = Serial.ReadExisting();

            //Chama outra thread para escrever o dado em alguma posição do formulário
            this.Invoke(new EventHandler(trataDadoRecebido));
        }

        private void trataDadoRecebido(object sender, EventArgs e)
        {
            txtRecebe.Text += RxString;
        }
    }
}
