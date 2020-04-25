using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OrderApp;

namespace HomeWork8
{
    public partial class FormOrderEidt : Form
    {
        public Order currentOrder;

        public FormOrderEidt()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            int id = int.Parse(this.textBox_Id.Text);
            this.currentOrder.OrderId = (uint)id;
            currentOrder.CreateTime = DateTime.Now;
            currentOrder.Customer = textBox_customer.Text;
            int index = int.Parse(textBox_Id.Text);
            double price = double.Parse(textBox_price.Text);
            int quantity = int.Parse(textBox_quantity.Text);
            OrderItem orderItem = new OrderItem((uint)index, textBox_goodsName.Text, price, (uint)quantity);
            if (!currentOrder.Equals(orderItem))
            {
                currentOrder.AddItem(orderItem);
            }
        }
    }
}
