using System;
using System.Security.Cryptography;
using System.Text;
using Cyclone.Lang;

namespace Cyclone
{
    class Program
    {
        static CycloneCompiler compiler = new CycloneCompiler();
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            CycloneCompiler.ConsoleFunctions.print($"Running Cyclone version:{compiler.version}");
            CycloneCompiler.ConsoleFunctions.Properties.Add("enable_debug","false");
            string testString;
            do
            {
                testString = Console.ReadLine();
                compiler.Run(testString);
            } 
            while (testString != ">end" && testString != ">exit" && testString != ">stop");
            CycloneCompiler.ConsoleFunctions.print("Done.");
        }
    }
}
