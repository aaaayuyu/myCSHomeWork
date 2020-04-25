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
    public partial class Form1 : Form
    {
        OrderService OrderService;
        BindingSource fieldsBS = new BindingSource();
        public String Keyword { get; set; }

        public Form1()
        {
            InitializeComponent();
            OrderService = new OrderService();

            OrderItem orderItem = new OrderItem(1, "苹果", 5, 2);           
            Order order1 = new Order(1,"老虎",new List<OrderItem>());
            order1.AddItem(orderItem);
            OrderService.AddOrder(order1);

            OrderItem orderItem2 = new OrderItem(1, "肉", 25, 2);
            Order order2 = new Order(2, "狮子",new List<OrderItem>() );
            order2.AddItem(orderItem2);
            OrderService.AddOrder(order2);

            orderbindingSource.DataSource = OrderService.Orders;
            itembindingSource.DataSource = OrderService.Orders;
            comboBox1.SelectedIndex = 0;
            
        }

        private void QueryAll()
        {
            orderbindingSource.DataSource = OrderService.Orders;
            itembindingSource.DataSource = OrderService.Orders;
            orderbindingSource.ResetBindings(false);
            itembindingSource.ResetBindings(false);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                      
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "订单号"://根据订单号查询
                    int.TryParse(textBox1.Text, out int id);
                    orderbindingSource.DataSource = OrderService.GetOrder((uint)id);
                    itembindingSource.DataSource = OrderService.GetOrder((uint)id);
                    break;
                case "客户名字"://根据客户名                  
                    orderbindingSource.DataSource = OrderService.QueryOrdersByCustomerName(textBox1.Text);
                    itembindingSource.DataSource= OrderService.QueryOrdersByCustomerName(textBox1.Text); 
                    break;
                case "商品名称"://根据商品名
                    orderbindingSource.DataSource = OrderService.QueryOrdersByGoodsName(textBox1.Text);
                    itembindingSource.DataSource = OrderService.QueryOrdersByCustomerName(textBox1.Text);
                    break;
                case "全部订单":
                    orderbindingSource.DataSource = OrderService.Orders;
                    itembindingSource.DataSource = OrderService.Orders;
                    break;
            }
            orderbindingSource.ResetBindings(true);
            itembindingSource.ResetBindings(true);
        }

        private void button2_Click(object sender, EventArgs e)//添加订单
        {
            FormOrderEidt formOrderEidt = new FormOrderEidt();
            if (formOrderEidt.ShowDialog() == DialogResult.OK)
            {
                OrderService.AddOrder(formOrderEidt.currentOrder);
                QueryAll();
            }
        }

        private void button_alter_Click(object sender, EventArgs e)//修改订单
        {
            FormOrderEidt formOrderEidt = new FormOrderEidt();
            if (formOrderEidt.ShowDialog() == DialogResult.OK)
            {
                OrderService.UpdateOrder(formOrderEidt.currentOrder);
                QueryAll();
            }
        }

        private void button_delete_Click(object sender, EventArgs e)//删除订单
        {
            Order order = orderbindingSource.Current as Order;
            
            if (order == null)
            {
                MessageBox.Show("请选择一个订单进行删除");
                return;
            }
            OrderService.RemoveOrder(order.OrderId);
            QueryAll();
        }

        private void button_import_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result.Equals(DialogResult.OK))
            {
                String fileName = openFileDialog1.FileName;
                OrderService.Import(fileName);
                QueryAll();
            }
        }

        private void button_out_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result.Equals(DialogResult.OK))
            {
                String fileName = saveFileDialog1.FileName;
                OrderService.Export(fileName);
            }
        }
    }
}
