using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueueService;
using System.IO;
using System.Reflection;
using System.Data.Entity;

namespace Tests
{
    [TestFixture]
    public class TestProgram
    {
        [Test]
        public void TestMainMethod()
        { 
            string str = "0" + Environment.NewLine;
            var input = new StringReader(str);
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);

            Program.Main(null);
        }

        [Test]
        public void TestMainMethod2()
        {
            string str = "1" + Environment.NewLine;
            str += "1" + Environment.NewLine;
            str += "0" + Environment.NewLine;
            str += "0" + Environment.NewLine;

            var input = new StringReader(str);
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);

            Program.Main(null);
        }

        [Test]
        public void TestMainMethod3()
        {
            
            string str = "2" + Environment.NewLine;
            Random r = new Random();
            str += Guid.NewGuid().ToString() + Environment.NewLine;
            str += $"{Guid.NewGuid().ToString()},31,true,53845" + Environment.NewLine;
            str += $"{r.Next(1000)},{r.Next(1000)}, {r.Next(1000)}" + Environment.NewLine;
            str += "0" + Environment.NewLine;
            str += "0" + Environment.NewLine;

            var input = new StringReader(str);
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);

            Program.Main(null);
        }

        [Test]
        public void TestClientMenu1()
        {

                string str = "1" + Environment.NewLine;
                str += "1" + Environment.NewLine;
                str += "1" + Environment.NewLine;
                str += "testQueues" + Environment.NewLine;

                str += "0" + Environment.NewLine;
                str += "0" + Environment.NewLine;

                var input = new StringReader(str);
                var output = new StringWriter();
                Console.SetIn(input);
                Console.SetOut(output);

                Program.Main(null);

        }
    }
}
