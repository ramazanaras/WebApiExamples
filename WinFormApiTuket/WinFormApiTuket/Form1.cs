using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



//referencelara ekledik
using System.Net.Http;
namespace WinFormApiTuket
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //http://localhost:8091/api/sehir   //local IIS'deki urli verdik.
        private async void button1_Click(object sender, EventArgs e)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8091/");

            //asenkron olduğunu belirtmek için await anahtar kelimesini kullanıyoruz
            HttpResponseMessage response = await client.GetAsync("api/sehir");
            //HttpResponseMessage response = await client.GetAsync("api/sehir/2");
            string result = await response.Content.ReadAsStringAsync();//json datayı asenkron olarak çekiyoruz

            label1.Text = result;
        }
    }
}
