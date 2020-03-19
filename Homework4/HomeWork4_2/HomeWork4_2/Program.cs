using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork4_2
{
    //声明委托类型
    public delegate void ClockHandler(object sender,RunEventArgs arg);

    public class RunEventArgs {
        public int Hour{ get; set; }
        public int Minute { get; set; }
    }

    public class Clock
    {
        public event ClockHandler Tick, Alarm;
        //设置滴答
        public void clockTick(int hour, int minute)
        {
            RunEventArgs args1 = new RunEventArgs()
            {
                Hour = hour,
                Minute = minute
            };
            Console.WriteLine($"{hour}:{minute}");
            Tick(this, args1);
        }
        //设置闹钟
        public void clockAlarm(int hour,int minute) {

            System.DateTime currentTime = new DateTime();
            currentTime = System.DateTime.Now;
            int currentHour = currentTime.Hour;
            int currentMinute = currentTime.Minute;

            RunEventArgs args2 = new RunEventArgs() {
                Hour=hour,
                Minute=minute
            };
            Console.WriteLine($"闹钟为{hour}:{minute}");

            if (currentHour == hour && currentMinute == minute)
            {
                Alarm(this, args2);
            }
            else {
                Console.WriteLine($"现在是{currentTime},未到设置闹钟时间");
            }

        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Clock clock = new Clock();

            clock.Tick += new ClockHandler(tick);
            clock.Alarm += alarm;

            void tick(object sender,RunEventArgs arg) {
                System.DateTime dateTime = new DateTime();
                dateTime = System.DateTime.Now;
                Console.WriteLine($"时间为{dateTime}");
            }

            void alarm(object sender,RunEventArgs arg) {

            }

            clock.clockTick(23, 13);
            clock.clockAlarm(23,01);
        }
    }
}
