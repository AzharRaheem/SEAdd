using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test1
{
    public partial class Form1 : Form
    {
        MyDBContext db;
       
        public Form1()
        {
            InitializeComponent();
            db = new MyDBContext();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            int id = 1;
            Employee emp = new Employee();
            emp.name = nametxtBox.Text;
            SaveEmployee(emp);
            
        }

        private void DeleteEmployee(int id)
        {
            //db.Employees.Remove(db.Employees.Where(m => m.id == id).First());
            //db.SaveChanges();
        }

        private void SaveEmployee(Employee emp)
        {
            db.Employees.Add(emp);
            db.SaveChanges();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var employees = db.Employees.ToList();
            dataGridView1.DataSource = employees;
            int id = 1;
            //DeleteEmployee(id);
        }
    }
}
