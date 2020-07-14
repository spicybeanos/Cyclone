using System;
using System.Collections.Generic;
using System.IO;


namespace Cyclone.Lang
{
    public class CycloneCompiler
    {
        public VariableHandler Handler = new VariableHandler();
        public Dictionary<string, Function> FunctionMap = new Dictionary<string, Function>();
        public string version = "0.3 beta";
        public void Run(string code)
        {
            string[] codek = CompilerUtilities.split(code, ' ',true,true);

            if (codek[0].StartsWith("print"))
            {
                string _params = CompilerUtilities.GetEncapsulatedData(codek[0],'(',')');
                ConsoleFunctions.print(Handler.getAbsoluteValue(_params).ToString()+"");
            }
            else if (codek[0].StartsWith("println"))
            {
                string _params = CompilerUtilities.GetEncapsulatedData(codek[0], '(', ')');
                ConsoleFunctions.println(Handler.getAbsoluteValue(_params).ToString() + "");
            }
            else if (codek[0].StartsWith("type_of"))
            {
                string _params = CompilerUtilities.GetEncapsulatedData(codek[0],'(',')');
                ConsoleFunctions.print(Handler.getType(_params));
            }
            else if (codek[0].StartsWith("set_property"))
            {
                string _params = CompilerUtilities.GetEncapsulatedData(codek[0], '(', ')');
                string[] param_ = CompilerUtilities.split(_params,',',true,true);
                string _prop = Handler.getAbsoluteValue(param_[0]).ToString(),_val = Handler.getAbsoluteValue(param_[1]).ToString();

                if (!ConsoleFunctions.Properties.ContainsKey(_prop))
                {
                    ConsoleFunctions.Properties.Add(_prop,_val);
                    ConsoleFunctions.Debugln($"Added property {_prop} with value {ConsoleFunctions.Properties[_prop]}");
                }
                else
                {
                    ConsoleFunctions.Properties[_prop] = _val;
                    ConsoleFunctions.Debugln($"Set property {_prop} with value {ConsoleFunctions.Properties[_prop]}");
                }
            }
            else if(codek[0] == ">end" || codek[0] == ">exit" || codek[0] == ">stop")
            {
                ConsoleFunctions.print($"Done. \nPress any key to continue. \n");
                Console.ReadKey();
                System.Environment.Exit(0);
            }
            else if (codek[0].StartsWith("run_file"))
            {
                string _file = Handler.getAbsoluteValue(CompilerUtilities.GetEncapsulatedData(codek[0],'(',')')).ToString();
                RunFile(_file);
            }
            else if (codek[0].StartsWith(":"))
            {
                string _functionName = codek[0];
                _functionName = _functionName.Replace(":","");
                if (FunctionMap.ContainsKey(_functionName))
                {
                    RunFunction(_functionName);
                }
                else
                {
                    ConsoleFunctions.ThrowException($"the function {_functionName} does not exist.");
                }
            }
            else if (codek[0].StartsWith("bake_function_map"))
            {
                string _loc = Handler.getAbsoluteValue(CompilerUtilities.GetEncapsulatedData(codek[0],'(',')')).ToString();
                ConsoleFunctions.Debug($"Baking function map for {_loc}...\n");
                try
                {
                    BakeFunctionMaps(File.ReadAllText(_loc),'\r');
                    ConsoleFunctions.Debug($"Done baking function map for {_loc}!\n");
                }
                catch(Exception e)
                {
                    ConsoleFunctions.ThrowException($"An exception has occured while baking the function map for {_loc}:\n");
                    ConsoleFunctions.ThrowException(e.ToString());
                }

            }
            else if (codek[0].StartsWith("write_to_file"))
            {
                string encap = CompilerUtilities.GetEncapsulatedData(codek[0],'(',')');
                string[] _params = CompilerUtilities.split(encap,',');
                string _location = Handler.getAbsoluteValue(_params[0]).ToString() , _txt = Handler.getAbsoluteValue(_params[1]).ToString();
                try
                {
                    File.WriteAllText(_location,_txt);
                    ConsoleFunctions.Debug($"wrote to {_location} \n");
                }
                catch(Exception e)
                {
                    ConsoleFunctions.ThrowException("A file exception has occured: \n");
                    ConsoleFunctions.ThrowException(e.ToString() + "\n");
                }
            }
            else if(!CompilerUtilities.isKeyWord(codek[0]))
            {
                int n = codek.Length;
                if(n == 1)
                {
                    Handler.add(codek[0],null);
                }
                else if (n == 3)
                {
                    if (codek[1] == "=")
                    {
                        if (Handler.has(codek[0]))
                        {
                            Handler.setValueByString(codek[0], codek[2]);
                        }
                        else
                        {
                            Handler.addByString(codek[0], codek[2]);
                        }
                    }
                }
                else if(Handler.has(codek[0]))
                {
                    if (codek[1] == "=")
                    {
                        if (Handler.getType(codek[0]) == "int" || Handler.getType(codek[0]) == "double" || Handler.getType(codek[0]) == "float")
                        {
                            List<string> _tags = new List<string>();
                            List<string> _vars = new List<string>();
                            List<string> _toks = new List<string>();
                            for (int i = 2; i < codek.Length; i++)
                            {
                                _tags.Add(codek[i]);
                            }
                            for (int i = 0; i < _tags.Count; i++)
                            {
                                if (i % 2 == 0) { _vars.Add(_tags[i]); }
                                else { _toks.Add(_tags[i]); }
                            }
                            Handler.setByMultiMath(codek[0], _vars.ToArray(), _toks.ToArray());
                        }
                        else
                        {
                            List<string> str_ = new List<string>();
                            for(int i = 2; i < codek.Length; i++)
                            {
                                str_.Add(codek[i]);
                            }
                            Handler.setByMultiString(str_, codek[0]);
                        }
                    }
                }
                else if(n > 3 && !Handler.has(codek[0]))
                {
                    List<string> tags_ = new List<string>();
                    for(int i = 2; i < codek.Length; i++)
                    {
                        tags_.Add(codek[i]);
                    }
                    Handler.addByMultimath(codek[0],tags_.ToArray());
                }
            }
        }

        public void BakeFunctionMaps(string code, char endlineChar, bool removeEndl = true)
        {
            ConsoleFunctions.Debugln("Baking function map...");
            string[] _lines = CompilerUtilities.split(code, endlineChar, false, true);
            List<string> funcNames = new List<string>();
            int _nfunc = 0;
            for (int i = 0; i < _lines.Length; i++)
            {
                _lines[i] = _lines[i].Replace("\n", "");
                _lines[i] = _lines[i].Replace("\r", "");
            }
            foreach (string _l in _lines)
            {
                if (CompilerUtilities.split(_l, ' ')[0].StartsWith("function") || CompilerUtilities.split(_l, ' ')[0].StartsWith("func"))
                {
                    _nfunc++;
                    funcNames.Add(CompilerUtilities.split(_l,' ')[1]);
                }
            }
            List<string> _funcCode = new List<string>();
            string[] funcRawGroup = CompilerUtilities.GetGroupEncapsulatedData(code, '{', '}', false, true);
          
            for(int i=0;i< funcRawGroup.Length; i++)
            {
                string[] _temp = CompilerUtilities.split(funcRawGroup[i],'\n');
                Function temp = new Function();
                temp.Add(_temp);
                if (!FunctionMap.ContainsKey(funcNames[i]))
                { 
                    FunctionMap.Add(funcNames[i], temp);
                }
                else
                {
                    FunctionMap[funcNames[i]] = temp;
                }
            }
            ConsoleFunctions.Debugln("Done baking function map!");
        }

        public void RunFile(string filename)
        {
            try
            {
                BakeFunctionMaps(File.ReadAllText(filename), '\r');
                if (FunctionMap.ContainsKey("start"))
                {
                    RunFunction("start");
                }
                else
                {
                    ConsoleFunctions.Warn("a start function does not exists hence did not execute anything.");
                }
            }
            catch(Exception e)
            {
                ConsoleFunctions.ThrowException("A file exception has occured: \n");
                ConsoleFunctions.ThrowException(e.ToString() + "\n");
            }
            
        }

        public void RunFunction(string function)
        {
            foreach(string line in FunctionMap[function].Lines)
            {
                Run(line);
            }
        }

        public class Function
        {
            public List<string> Lines = new List<string>();
            public void Add(List<string> lines)
            {
                foreach(string _line in lines)
                {
                    string temp = _line;
                    temp = temp.Replace("\n","");
                    temp = temp.Replace("\r","");
                    Lines.Add(temp);
                }
            }
            public void Add(string[] lines)
            {
                foreach (string _line in lines)
                {
                    string temp = _line;
                    temp = temp.Replace("\n", "");
                    temp = temp.Replace("\r", "");
                    Lines.Add(temp);
                }
            }
        }

         public class CompilerUtilities{


            public static string[] split(string _string)
            {
                List<string> _r = new List<string>();
                string _e = "";
                bool _incSpaceChar = false;
                for (int i = 0; i < _string.Length; i++)
                {
                    if (_string[i] == '\"' || _string[i] == '\'')
                    {
                        _incSpaceChar = toggle(_incSpaceChar);
                    }
                    else if (_string[i] != ' ' || _incSpaceChar)
                    {
                        _e += _string[i];
                    }
                    else
                    {
                        _r.Add(_e);
                        _e = "";
                    }
                }
            
                _r.Add(_e);
                for (int i =0; i<_r.Count;i++)
                {
                    if(_r[i] == "" || _r[i] == " ")
                    {
                       _r.RemoveAt(i);
                    }
                }
                if(_r.Count == 0)
                {
                   _r.Add("");
                }

                return _r.ToArray();
            }

            public static string[] GetGroupEncapsulatedData(string _string,char a, char b, bool _removeNextLine, bool removeEmpty = true)
            {
                List<string> r = new List<string>();

                for(int i = 0; i < _string.Length; i++)
                {
                    string _conts = "";
                    if (_string[i] == a)
                    {
                        for (int j = i + 1; j < _string.Length && _string[j] != b; j++)
                        {
                            _conts += _string[j];
                        }
                    }                    
                    if(_removeNextLine)
                        _conts = _conts.Replace(Environment.NewLine,"");



                    if (removeEmpty && _conts != string.Empty)
                    {
                        r.Add(_conts);
                    }
                    else if (!removeEmpty)
                    {
                        r.Add(_conts);
                    }

                    _conts = "";
                }
                return r.ToArray();
            }

            public static string[] split(string _string, char _separater, bool _canHaveSeparator = true , bool _canHaveQuotes = true)
             {
                List<string> _r = new List<string>();
                string _e = "";
                bool _incSeparaterChar = false;
                for (int i = 0; i < _string.Length; i++)
                {
                    if ((_string[i] == '\"' || _string[i] == '\'') && _canHaveSeparator)
                    {
                        _incSeparaterChar = toggle(_incSeparaterChar);
                        if (_canHaveQuotes)
                        {
                           _e += $"{_string[i]}";
                        }
                    }
                    else if (_string[i] != _separater || _incSeparaterChar)
                    {
                        _e += _string[i];
                    }
                    else
                    {
                        _r.Add(_e);
                        _e = "";
                    }
                }
                _r.Add(_e);
                for (int i = 0; i < _r.Count; i++)
                {
                    if (_r[i] == "" || _r[i] == " ")
                    {
                        _r.RemoveAt(i);
                    }
                }
                if (_r.Count == 0)
                {
                    _r.Add("");
                }
                return _r.ToArray();
            }
            
            public static string[] split0(string _string, char _separater, bool _canHaveSeparator = true, bool _canHaveQuotes = false)
            {
                List<string> _r = new List<string>();
                string _e = "";
                bool _incSeparaterChar = false;
                for (int i = 0; i < _string.Length; i++)
                {
                    if ((_string[i] == '\"' || _string[i] == '\'') && _canHaveSeparator)
                    {
                        _incSeparaterChar = toggle(_incSeparaterChar);
                        if (_canHaveQuotes)
                        {
                            _e +=$"{_string[i]}";
                        }
                    }
                    else if (_string[i] != _separater || _incSeparaterChar)
                    {
                        _e += _string[i];
                    }
                    else
                    {
                        _r.Add(_e);
                        _e = "";
                    }
                }
                _r.Add(_e);
                return _r.ToArray();
            }
            
            public static bool toggle(bool b)
            {
            if (b)
                return false;
            else
                return true;
            }
         
            public static string GetEncapsulatedData(string s, char a, char b)
            {
                string c = "";
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == a)
                    {
                        for (int j = i + 1; j < s.Length && s[j] != b; j++)
                        {
                            c += s[j];
                        }
                    }
                }
               return c;
            } 

            public static bool isKeyWord(string s)
            {
                bool b = false;
                if (s == "(" || s == ")" || s == "func" || s == "function" || s == "{" || s == "}" || s == "" || s.Contains("//") || s.StartsWith(":"))
                {
                    b = true;
                }
                return b;
            }
         }

         public class ConsoleFunctions
         {
            public static ConsoleColor PrintColor = ConsoleColor.Green;
            public static ConsoleColor DefaultColor = ConsoleColor.White;
            public static ConsoleColor ErrorColor = ConsoleColor.Red;
            public static ConsoleColor DebugColor = ConsoleColor.Yellow;
            public static ConsoleColor WarningColor = ConsoleColor.Yellow;


            public static Dictionary<string,string> Properties = new Dictionary<string,string>();
            public static bool EnableDebug = true;


            #region Output


            /// <summary>
            /// Prints message 
            /// </summary>
            /// <param name="msg">message to be printed</param>
            public static void print(string msg)
            {
                if (Properties.ContainsKey("show_time"))
                {
                    msg = (Properties["show_time"] == "true") ?  $"[{DateTime.Now}]{msg}":msg;
                }
                Console.ForegroundColor = PrintColor;
                Console.Write(msg);
                Console.ForegroundColor = DefaultColor;
            }

            /// <summary>
            /// Prints message 
            /// </summary>
            /// <param name="msg">message to be printed</param>
            public static void print(object msg)
            {
                if (Properties.ContainsKey("show_time"))
                {
                    msg = (Properties["show_time"] == "true") ? $"[{DateTime.Now}]{msg.ToString()}" : msg.ToString();
                }
                Console.ForegroundColor = PrintColor;
                Console.Write(msg);
                Console.ForegroundColor = DefaultColor;
            }

            /// <summary>
            /// Prints message and goes to next line
            /// </summary>
            /// <param name="msg">message to be printed</param>
            public static void println(string msg)
            {
                if (Properties.ContainsKey("show_time"))
                {
                    msg = (Properties["show_time"] == "true") ?$"[{DateTime.Now}]{msg}" : msg;
                }

                Console.ForegroundColor = PrintColor;
                Console.Write(msg + "\n");
                Console.ForegroundColor = DefaultColor;
            }

            /// <summary>
            /// Prints message and goes to next line
            /// </summary>
            /// <param name="msg">message to be printed</param>
            public static void println(object msg)
            {
                if (Properties.ContainsKey("show_time"))
                {
                    msg = (Properties["show_time"] == "true") ? $"[{DateTime.Now}]{msg.ToString()}" : msg.ToString() ;
                }

                Console.ForegroundColor = PrintColor;
                Console.Write(msg+"\n");
                Console.ForegroundColor = DefaultColor;
            }

            /// <summary>
            /// Throws exceptions
            /// </summary>
            /// <param name="exc"> Exception to be thrown</param>
            public static void ThrowException(string exc)
            {
                if (Properties.ContainsKey("show_time"))
                {
                   exc = (Properties["show_time"] == "true") ? $"<X>[{DateTime.Now}]{exc}" : exc;
                }

                Console.ForegroundColor = ErrorColor;
                Console.Write(exc+"\n");
                Console.ForegroundColor = DefaultColor;
                if (Properties.ContainsKey("exit_on_exc"))
                {
                    if(Properties["exit_on_exc"] == "true")
                    {
                        System.Environment.Exit(1);
                    }
                }
            }

            /// <summary>
            /// Warns user
            /// </summary>
            /// <param name="warning">Warning to be warned</param>
            public static void Warn(string warning)
            {
                if (Properties.ContainsKey("show_time"))
                {
                    warning = (Properties["show_time"] == "true") ?$"<!>[{DateTime.Now}]{warning}" : warning;
                }

                Console.ForegroundColor = WarningColor;
                Console.Write(warning+"\n");
                Console.ForegroundColor = DefaultColor;
            }

            /// <summary>
            /// Prints debug messages when EnableDebug is enabled
            /// </summary>
            /// <param name="msg">Message to be debuged</param>
            public static void Debug(string msg)
            {
                if (Properties.ContainsKey("show_time"))
                {
                    msg = (Properties["show_time"] == "true") ? $"<Debug>[{DateTime.Now}]{msg}" : msg;
                }

                if (Properties.ContainsKey("enable_debug"))
                {
                    if(Properties["enable_debug"] == "true")
                    Console.ForegroundColor = DebugColor;
                    Console.Write(msg+"\n");
                    Console.ForegroundColor = DefaultColor;
                }                
            }

            /// <summary>
            /// Prints debug messages when EnableDebug is enabled
            /// </summary>
            /// <param name="msg">Message to be debuged</param>
            public static void Debug(object msg)
            {
                if (Properties.ContainsKey("show_time"))
                {
                    msg = (Properties["show_time"] == "true") ? $"<Debug>[{DateTime.Now}]{msg.ToString()}" : msg.ToString();
                }

                if (Properties.ContainsKey("enable_debug"))
                {
                    if(Properties["enable_debug"] == "true")
                    {
                        Console.ForegroundColor = DebugColor;
                        Console.Write(msg+"\n");
                        Console.ForegroundColor = DefaultColor;
                    }
                    
                }
            }

            /// <summary>
            /// Prints debug messages when EnableDebug is enabled
            /// </summary>
            /// <param name="msg">Message to be debuged</param>
            public static void Debugln(string msg)
            {
                if (Properties.ContainsKey("show_time"))
                {
                    msg = (Properties["show_time"] == "true") ? $"<Debug>[{DateTime.Now}]{msg}" : msg;
                }

                if (Properties.ContainsKey("enable_debug"))
                {
                    if (Properties["enable_debug"] == "true")
                    {
                    Console.ForegroundColor = DebugColor;
                    Console.WriteLine(msg+"\n");
                    Console.ForegroundColor = DefaultColor;
                    }
                    
                }
            }

            /// <summary>
            /// Prints debug messages when EnableDebug is enabled
            /// </summary>
            /// <param name="msg">Message to be debuged</param>
            public static void Debugln(object msg)
            {
                if (Properties.ContainsKey("show_time"))
                {
                    msg = (Properties["show_time"] == "true") ? $"<Debug>[{DateTime.Now}]{msg.ToString()}" : msg.ToString();
                }

                if (Properties.ContainsKey("enable_debug"))
                {
                    if(Properties["enable_debug"] == "true")
                    {
                    Console.ForegroundColor = DebugColor;
                    Console.WriteLine(msg+"\n");
                    Console.ForegroundColor = DefaultColor;
                    }
                }
            }
            #endregion

        }
    }
}
