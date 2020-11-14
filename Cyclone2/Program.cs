using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Cyclone.Lang;

namespace Cyclone
{
    class Program
    {
        static CycloneCompiler compiler = new CycloneCompiler();
        static void Main(string[] args)
        {

            string nl = Environment.NewLine;
            
            string[] test_ = CycloneCompiler.CompilerUtilities.GetGroupEncapsulatedDataWithChildren($"(hello world! {nl}(hello world)){nl}(io netty{nl}(io netty)){nl}(oof {nl}(oof))",'(',')',false);

            foreach(string str_ in test_){
                Console.WriteLine(str_);
            }

            if(args.Length > 0){
                string path_ = args[0];
                //string[] conts_ ;
                if(File.Exists(path_) && (path_.EndsWith(".cyclone") || path_.EndsWith(".cy"))){
                    //conts_   = File.ReadAllLines(path_);
                    try{
                        compiler.RunFile(path_);
                    }
                    catch(Exception e){
                        CycloneCompiler.ConsoleFunctions.ThrowException($"Exception: {e}");
                    }
                    
                }
                else{
                    CycloneCompiler.ConsoleFunctions.ThrowException($"{path_} is not a .cy or .cyclone file hence did not run it. \r\n Press enter to continue.");
                    Console.ReadKey();
                }
            }
            else{
                CycloneCompiler.ConsoleFunctions.ThrowException("No file provided to run.Press any key to close.");
                Console.ReadKey();
            }                
        }
    }
}


/*

            Console.ForegroundColor = ConsoleColor.White;
            CycloneCompiler.ConsoleFunctions.print($"Running Cyclone version:{compiler.version}\n");
            CycloneCompiler.ConsoleFunctions.Properties.Add("enable_debug","false");
            string testString;
            do
            {
                testString = Console.ReadLine();
                compiler.Run(new string[]{testString});
            } 
            while (testString != ">end" && testString != ">exit" && testString != ">stop");
            CycloneCompiler.ConsoleFunctions.print("Done.");

*/