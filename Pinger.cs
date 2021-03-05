using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Windows.Forms;


namespace WindowsFormsApplication3
{
    class Pinger
    {
        public String ip;
        public int id;
        public string imei;
        public string status;
        public string responsavel;
        public DateTime datalteracao;
        public static string resp;


        public String Ping()
        {
            return resp;
            
        }


         public static async Task TestaPing(string ip)
        {

            try
            {
                Ping pinger = new Ping();
                PingReply resposta = await pinger.SendPingAsync(ip);
                ExibeInfoRespostaPing(resposta);
                pinger.PingCompleted += pinger_PingCompleted;
                resp = Convert.ToString(resposta.RoundtripTime);
                
            }
            catch
            {
                throw;
            }
            
        }

        private static void pinger_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            try
            {
                PingReply resposta = e.Reply;
                if (e.Cancelled)
                {
                    System.Console.WriteLine("Ping para foi cancelado");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Execption ocoreu: "+ex);
            }
        }

        private static void ExibeInfoRespostaPing(PingReply resposta)
        {
            System.Console.WriteLine("Resultado do Ping " + resposta.Address);
            System.Console.WriteLine("RoundTrip took:" + resposta.RoundtripTime);
            String resp = Convert.ToString(resposta.RoundtripTime);

        }


    }

   
}
