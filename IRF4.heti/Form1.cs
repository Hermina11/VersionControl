using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace IRF4.heti
{
    public partial class Form1 : Form
    {
        RealEstateEntities context = new RealEstateEntities(); //ORM peldanyositasa
        List<Flat> Flats;

        
        
        public Form1()
        {
            InitializeComponent();
            LoadData();
            Excel.Application xlApp; // A Microsoft Excel alkalmazás
            Excel.Workbook xlWB; // A létrehozott munkafüzet
            Excel.Worksheet xlSheet; // Munkalap a munkafüzeten belül
        }


        void LoadData()
        {
            Flats = context.Flat.ToList();
        }

    }
}
