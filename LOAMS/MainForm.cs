using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Disruptor;
using MktSrvcAPI;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars;

namespace LOAMS
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        

        public MainForm()
        {
            InitializeComponent();


        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void MainForm_Load(object sender, EventArgs e)
        {
        }
        
        private void Barmanager_ItemClick(object sender, ItemClickEventArgs e)
        {
            if(e.Item.Caption == "Level 2")
            {
                Console.WriteLine("Click");
            }
        }
    }
}
