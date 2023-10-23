﻿using dal.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dal.Entities;
using bus;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace gui.PatientForm.PrescriptionForm.MedInvoiceForm
{
    public partial class frmMedInvoice : Form
    {
        public int PatientID;
        private readonly PrescriptionService prescription = new PrescriptionService();
        private readonly MedicineInvoiceService  medInvoice = new MedicineInvoiceService();
        private readonly MedicineInvoiceDetailService medInvoiceDetails = new MedicineInvoiceDetailService();

        public frmMedInvoice()
        {
            InitializeComponent();
        }
        private ListViewItem unique = new ListViewItem();
        private void frmMedInvoice_Load(object sender, EventArgs e)
        {
            txtTotal.Text = Total().ToString();
            string s = string.Format("Mã DT:   {0}", GeneratePrescriptionID());
            lblIDMed.Text = s;

        }
        public void AddItemsToList(ListViewItem lvi)
        {
            lvMedInvoice.Items.Add(lvi);
        }
        private int GeneratePrescriptionID()
        {
            Random rnd = new Random();
            return rnd.Next(1, 10000000);
        }
        private int GenerateInvoiceID()
        {
            Random rnd = new Random();
            return rnd.Next(1, 10000000);
        }
        private int GetID(string name)
        {
            using (var model = new DentalModel())
            {
                var a = (from b in model.Medicines
                         where b.MedicineName == name
                         select b.MedicineID).FirstOrDefault();
                return a;
            }
        }
        private decimal Total()
        {
            decimal total = 0;
            foreach (ListViewItem lv in lvMedInvoice.Items)
            {
                total += Convert.ToDecimal(lv.SubItems[6].Text);
            }
            return total;
        }
        private void lvMedInvoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMedInvoice.SelectedItems.Count > 0)
            {
                ListViewItem lv = lvMedInvoice.SelectedItems[0];
                unique = lv;
            }
        }
        private DateTime GetDate()
        {
            return DateTime.Now;
        }
        private void btnSaveDetails_Click(object sender, EventArgs e)
        {
            //neu hoa don thuoc ton tai thi ko cho sua, else sua don thuoc
            try
            {
                using (var model = new DentalModel())
                {
                    foreach (ListViewItem lv in lvMedInvoice.Items)
                    {
                        var item1 = new Prescription()
                        {
                            Patient_ID = int.Parse(PatientID.ToString()),
                            MedicineID = GetID(lv.SubItems[1].Text),
                            Quantity = int.Parse(lv.SubItems[4].Text),
                            TotalAmount = decimal.Parse(Total().ToString())
                        };
                        model.Prescriptions.Add(item1);
                    }
                    model.SaveChanges();
                    /*var item2 = new MedicineInvoice()
                    {
                        Date = DateTime.Now,
                        TotalAmount = decimal.Parse(Total().ToString())
                    };
                    medInvoice.InsertNew(item2);

                    var item3 = new MedicineInvoiceDetail()
                    {

                    };
                    medInvoiceDetails.InsertNew(item3);*/
                    MessageBox.Show("Done!","Annoucement!",MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
