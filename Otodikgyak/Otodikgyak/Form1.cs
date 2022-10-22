using Otodikgyak.Entities;
using Otodikgyak.MnbServiceReference1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

namespace Otodikgyak
{
    public partial class Form1 : Form
    {
       /*List<GetExchangeRatesRequestBody> ExchangeRateData = new List<GetExchangeRatesRequestBody>();
        */
        BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<string> Currencies = new BindingList<string>();



        public Form1()
        {
            InitializeComponent();
            dataGridView1.DataSource = Rates;
            comboBox1.DataSource = Currencies;

            GetCurrencies();
            RefreshData();
        }



       /* private void GetExchangeRates() {

            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = "EUR",
                startDate = "2020-01-01",
                endDate = "2020-06-30"
            };
            var response = mnbService.GetExchangeRates(request);
            var result = response.GetExchangeRatesResult;
            getXML(result);

        }*/

        

        private void RefreshData() {
            Rates.Clear();
            Web();
            getXML();
            Diagram();


        }
        private string Web()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = comboBox1.SelectedItem.ToString(),
                startDate = dateTimePicker1.Value.ToString(),
                endDate = dateTimePicker2.Value.ToString()
            };
            var response = mnbService.GetExchangeRates(request);
            var result = response.GetExchangeRatesResult;

            return result;
        }

        

        private void getXML() {
            var xml = new XmlDocument();
            xml.LoadXml(Web());

            foreach (XmlElement element in xml.DocumentElement)
            {
                // Létrehozzuk az adatsort és rögtön hozzáadjuk a listához
                // Mivel ez egy referencia típusú változó, megtehetjük, hogy előbb adjuk a listához és csak később töltjük fel a tulajdonságait
                var rate = new RateData();
                Rates.Add(rate);

                // Dátum
                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                // Valuta
                var childElement = (XmlElement)element.ChildNodes[0];
                if (childElement == null)
                    continue;
                rate.Currency = childElement.GetAttribute("curr");

                // Érték
                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                    rate.Value = value / unit;
            }


        }

        private void Diagram() {
            chartRateData.DataSource = Rates; //adatforráas a Rates lista

            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;

            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }

        private void GetCurrencies()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetCurrenciesRequestBody()
            {
            };

            var response = mnbService.GetCurrencies(request);
            var result = response.GetCurrenciesResult;
            var xml = new XmlDocument();
            xml.LoadXml(result);

            foreach (XmlElement element in xml.DocumentElement)
            {
                string currencies = element.InnerText;
                for (int i = 0; i < currencies.Length; i += 3)
                {
                    Currencies.Add(currencies.Substring(i, 3));
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
    
}
