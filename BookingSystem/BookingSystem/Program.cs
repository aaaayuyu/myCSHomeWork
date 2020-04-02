using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace BookingSystem
{

    //订单明细类,商品名称，单价，总额，数量
    public class OrderItem {

        public string ProduceName;
        public double Price;
        public double TotalPrice;
        public int Count;

        public OrderItem(string name,double price,int count) {

            this.ProduceName = name;
            this.Price = price;
            this.Count = count;
            this.TotalPrice =Price*Count;
        }
        //根据商品名称是否重复来判断订单明细List是否有重复的明细
        public Boolean equals(OrderItem ord1)
        {
            if (ProduceName == ord1.ProduceName&&Price==ord1.Price&&Count==ord1.Count) { return true; }
            else return false;
        }

        override public string ToString() {
            string str = "商品：" +ProduceName+"  单价："+Price+"  数量："+Count+"  总价："+TotalPrice;
            return str;
        }
    }

    /*订单类:订单号，时间，客户名，订单明细。
     * 一个订单可以对应多个订单明细。订单的订单明细不重复。
     */
    public class  Order{

        public string orderNumber;
        public DateTime time;
        public string CustomerName;
        public List<OrderItem> itemlist = new List<OrderItem>();//订单明细表  
              
        //添加订单明细,订单的订单明细不能重复
        public void  AddOrderItem() {

            Console.WriteLine("请输入商品名称");
            String proudceName = Convert.ToString(Console.ReadLine());
            Console.WriteLine("请输入该商品单价");
            Double price = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("请输入该数量");
            int count = Convert.ToInt32(Console.ReadLine());

            OrderItem item = new OrderItem(proudceName,price,count);
            int n ;
            for (n=0; n <= itemlist.Count - 1; n++)
            {
                if (itemlist[n].equals(item))
                {
                    Console.WriteLine("不能重复添加相同的商品明细");
                    goto there;
                }
            }
            itemlist.Add(item);
        there:;
        }

        public Order() {

            this.orderNumber = System.DateTime.Now.ToString("yyyyMMddHHmmss");//根据时间参数产生订单号
            this.time = System.DateTime.Now;
            Console.WriteLine("请输入客户名字");
            string customerN = Convert.ToString(Console.ReadLine());
            this.CustomerName = customerN;
            AddOrderItem();
            
        }

        //根据订单号判断订单是否重复
        public Boolean equals(Order order1) {
            if (this.orderNumber == order1.orderNumber) return true;
            else return false;
        }

        override public string ToString()
        {
            String str= "订单号：" + orderNumber + "  客户名：" + CustomerName + "  时间：" + time;
            foreach (OrderItem orderItem in itemlist)
            {
                str = "订单号：" + orderNumber + "  客户名：" + CustomerName + "  时间：" + time.ToString()+"  " + orderItem;
            }
            return str;
        }

        //订单、订单明细、客户、货物等类添加ToString方法，用来显示订单信息
        public void ShowOrder()
        {
            String str;
            str = "订单号：" + orderNumber + "  客户名：" + CustomerName + "  时间：" + time.ToString();
            Console.WriteLine(str);
            Console.WriteLine();
            Console.WriteLine("商品");
            int n = Console.CursorTop - 1;
            foreach (OrderItem ord in itemlist)
            {
                Console.WriteLine(ord.ProduceName);
            }
            Console.SetCursorPosition(10, n);
            Console.WriteLine("单价");
            Console.SetCursorPosition(10, n + 1);
            foreach (OrderItem ord in itemlist)
            {
                Console.WriteLine(ord.Price);
            }
            Console.SetCursorPosition(20, n);
            Console.WriteLine("数量");
            Console.SetCursorPosition(20, n + 1);
            foreach (OrderItem ord in itemlist)
            {
                Console.WriteLine(ord.Count);
            }
            Console.SetCursorPosition(30, n);
            Console.WriteLine("总价");
            Console.SetCursorPosition(30, n + 1);
            foreach (OrderItem ord in itemlist)
            {
                Console.WriteLine(ord.TotalPrice);
            }
        }
    }


    /*订单服务类:
     * 实现对订单的添加，删除，修改，查询（按照订单号、商品名称、客户等字段进行查询)。
     * 使用LINQ语言实现各种查询功能，查询结果按照订单总金额排序返回。
     * 在订单删除、修改失败时，能够产生异常并显示给客户错误信息。
     * 提供排序方法对保存的订单进行排序。默认按照订单号排序，也可以使用Lambda表达式进行自定义排序。
   */
    public class OrderService {

        public static  List<Order> ordersList = new List<Order>();

        //添加订单
        public OrderService(){

            Order order1 = new Order();
            for (int n = 0; n <= ordersList.Count - 1; n++) {
                if (ordersList[n].equals(order1)) {
                    Console.WriteLine("该订单号已存在");
                    goto there;
                }
            }
            ordersList.Add(order1);
            order1.ShowOrder();
        there:;
        }
        
        public void DeleteOrder() {

            Console.WriteLine("请输入要删除的订单的订单号");
            string number = Convert.ToString(Console.ReadLine());
            var info = (from ord in ordersList where ord.orderNumber == number select ord).FirstOrDefault();
            if (info != null)
            {
                Console.WriteLine($"请确认是否为此订单:{info}");
                Console.WriteLine("确认请按：1  返回请按：0");
                int check = Convert.ToInt32(Console.ReadLine());
                if (check == 1)
                {
                    foreach (Order n in ordersList)
                    {
                        if (n == info)
                        {
                            ordersList.Remove(n);
                        }
                    }
                    Console.WriteLine("删除成功");
                }
                else {
                    DeleteOrder();
                }
            }
            else
            {
                Console.WriteLine("请输入正确订单号");
            }

        }

        public void ModifyOrder() {

            Console.WriteLine("请输入需要修改的订单信息的订单号");
            string number = Convert.ToString(Console.ReadLine());

            try
            {
                var info =(from ord in ordersList where ord.orderNumber == number select ord).FirstOrDefault();
                Console.WriteLine($"该订单信息为：{info}");
                Console.WriteLine("请选择要修改的内容 1：顾客名称   2：增加订单明细");
                int option = Convert.ToInt32(Console.ReadLine());
                if (option == 1)
                {
                    Console.WriteLine("请输入修改后的顾客名字：");
                    string name = Convert.ToString(Console.ReadLine());
                    info.CustomerName = name;
                    Console.WriteLine(info);
                }
                else if (option == 2)
                {
                    info.AddOrderItem();
                }
                else {
                    Console.WriteLine("选择不合规范");
                    ModifyOrder();
                }
            }
            catch (Exception e) {
                Console.WriteLine("输入的订单号不正确，请再次确认");
                ModifyOrder();
            }
        }

         public void CheckOrder() {

            Console.WriteLine("请选择根据什么方式进行订单信息的查询");
            Console.WriteLine("1:按订单号  2：按客户姓名  3：按商品名称");
            int option = Convert.ToInt32(Console.ReadLine());

            if (option == 1)
            {
                Console.WriteLine("请输入要查询的订单的订单号");
                string number = Convert.ToString(Console.ReadLine());
                if (ordersList.Where(n => n.CustomerName == number) != null)
                {
                    var info = (from ord in ordersList where ord.orderNumber == number select ord).ToList();
                    Console.WriteLine("订单信息为：");
                    foreach (Order order in info) {
                        Console.WriteLine(order);
                    }
                    
                }
                else {
                    Console.WriteLine("此单号不存在");
                    CheckOrder();
               }
            }
            else if (option == 2)
            {
                Console.WriteLine("请输入需要查询的客户名字：");
                string name = Convert.ToString(Console.ReadLine());
                if (ordersList.Where(n => n.CustomerName == name) != null)
                {
                    var info = (ordersList.Where(n => n.CustomerName == name)).ToList();
                    Console.WriteLine("订单信息为：");
                    foreach (Order n in info)
                    {
                        Console.WriteLine(n);
                    }
                }
                else {
                    Console.WriteLine("客户名不存在");
                    CheckOrder();
                }

            }
            else if (option == 3)
            {
                Console.WriteLine("请输入需要查询的商品名字：");
                string name = Convert.ToString(Console.ReadLine());
                if (ordersList.Where(n => n.CustomerName == name) != null)
                {
                    var info = (ordersList.Where(n => n.CustomerName == name)).ToList();
                    Console.WriteLine("订单信息为：");
                    foreach (Order n in info)
                    {
                        Console.WriteLine(n);
                    }
                }
                else
                {
                    Console.WriteLine("找不到该商品相关信息");
                    CheckOrder();
                }
            }
            else
            {
                Console.WriteLine("请按规定选择");
                CheckOrder();
            }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            
            OrderService orderService1 = new OrderService();
            Console.WriteLine("请选择要进行的操作：1：查询订单  2：修改订单  3：删除订单  ");
            int n = Convert.ToInt32(Console.ReadLine());

            switch (n) {
                case 1:orderService1.CheckOrder(); break;
                case 2:orderService1.ModifyOrder();break;
                case 3:orderService1.DeleteOrder();break;
                default:break;
            }
        }
    }
}
