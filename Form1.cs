using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Net.Configuration;
using System.Text.RegularExpressions;



namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        public static bool ValidaEnderecoEmail(string enderecoEmail)
        {
            try
            {
                //define a expressão regulara para validar o email
                string texto_Validar = enderecoEmail;
                Regex expressaoRegex = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");

                // testa o email com a expressão
                if (expressaoRegex.IsMatch(texto_Validar))
                {
                    // o email é valido
                    return true;
                }
                else
                {
                    // o email é inválido
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SendMail()
        {
            
            String Destinatario = "luciano.s@samsung.com";
            String Remetente = "luciano.s@samsung.com";
            String Assunto = "Teste de Envio de e-mail";
            String enviaMensagem = "Você está com seu dispositivo desligado";
            try
            {
                // valida o email
                bool bValidaEmail = ValidaEnderecoEmail(Destinatario);

                // Se o email não é validao retorna uma mensagem
                //if (bValidaEmail == false)
                    //return ("Email do destinatário inválido: ");

                // cria uma mensagem
                MailMessage mensagemEmail = new MailMessage(Remetente, Destinatario, Assunto, enviaMensagem);

                SmtpClient client = new SmtpClient("smtp.samsung.net", 25);
                client.EnableSsl = true;
                NetworkCredential cred = new NetworkCredential("luciano.s@samsung.com", "Klylink1186#");
                client.Credentials = cred;

                // inclui as credenciais
                client.UseDefaultCredentials = true;

                // envia a mensagem
                client.Send(mensagemEmail);

                //return "Mensagem enviada para  " + Destinatario + " às " + DateTime.Now.ToString() + ".";
            }
            catch (Exception ex)
            {
                string erro = ex.InnerException.ToString();
                //return ex.Message.ToString() + erro;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string MyConnection2 = "datasource=105.103.29.24;username=Luciano;password=Luciano405060#";
            string Query = "select ip, datalteracao from mobile_control.mobile where status = 'Em Uso' OR status = 'Em Atraso' OR status = 'NG';";
            var MyConn2 = new MySqlConnection(MyConnection2);
            MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
            MyConn2.Open();
            using (MySqlDataAdapter MyAdapter = new MySqlDataAdapter())
            {
                MyAdapter.SelectCommand = MyCommand2;
            }
            // here i have assign dTable object to the dataGridView1 object to display data.               
            //MyConn2.Close();  

            var dr = MyCommand2.ExecuteReader();

            DateTime datatual = DateTime.Today;
            int i = 0;
            string[] ipmobile = new string[100];
            DateTime[] datalteracao = new DateTime[100];
            while (dr.Read())
            {
                if(dr.GetString(0) != ""){
                    ipmobile[i] = dr.GetString(0);
                    datalteracao[i] = Convert.ToDateTime(dr.GetString(1));
                    System.Console.WriteLine("O IP = " + ipmobile[i]);

                }else{
                    ipmobile[i] = "NG";
                }
                
                i++;
            }
            i = 0;
            
            foreach (string ping in ipmobile)
            {
                
                if (ipmobile[i] != "NG")
                {
                    String resposta = Convert.ToString(Pinger.TestaPing(ipmobile[i]));
                    textBox2.Text = ipmobile[i];
                    
                    TimeSpan intervalor = datatual - datalteracao[i];
                    System.Console.WriteLine("Dias" + intervalor.Days);
                    if (intervalor.Days > 10)
                    {
                        SendMail();
                    }
                    i++;
                }
                else
                {
                    System.Console.WriteLine("Não tem IP");
                }
                
            }
            
            
            
        }

        
    }
}
