using System;
using System.Collections.Generic;
using System.Text;
using static Cyclone.Lang.CycloneCompiler;

namespace Cyclone.Lang
{
    public class VariableHandler
    {
        public Dictionary<string, object> GlobalVariables = new Dictionary<string, object>();

        /*
         * 
         */
        
       
        public void handleNullVar(string var)
        {
            addOrSetByString(var,"");
        }

        public void remove(string var)
        {
            try
            {
                GlobalVariables.Remove(var);
            }
            catch(Exception e)
            {
                ConsoleFunctions.ThrowException(e.ToString());
            }
        }

        public void addOrSetByString(string var_,string value_){
            if (has(var_))
            {
                setValueByString(var_, value_);
                //Console.WriteLine($"set {Handler.getAbsoluteValue(codek[0])} to {codek[2]}");
            }
            else
            {
                addByString(var_, value_);
                //Console.WriteLine($"add {Handler.getAbsoluteValue(codek[0])} to {codek[2]}");
            }
        }

        public VariableHandler()
        {
            GlobalVariables.Add("$registry",new object());
        }

        #region Math

        public int DoMath(int obj1 , string token , int obj2)
        {
            int ret = 0 ;

            switch (token)
            {
                case "+":
                    ret = obj1 + obj2;
                    break;
                case "-":
                    ret = obj1 - obj2;
                    break;
                case "*":
                    ret = obj1 * obj2;
                    break;
                case "/":
                    if(obj2!=0)
                        ret = obj1 / obj2;
                    break;
                case "^":
                    ret = (int)Math.Pow(obj1,obj2);
                    break;
                case "%":
                    if(obj2 != 0)
                        ret = obj1 % obj2;
                    break;
            }

            return ret;
        }
        public float DoMath(float obj1, string token, float obj2)
        {
            float ret = 0;

            switch (token)
            {
                case "+":
                    ret = obj1 + obj2;
                    break;
                case "-":
                    ret = obj1 - obj2;
                    break;
                case "*":
                    ret = obj1 * obj2;
                    break;
                case "/":
                    if(obj2!=0)
                        ret = obj1 / obj2;
                    break;
                case "^":
                    ret = (float)Math.Pow(obj1, obj2);
                    break;
                case "%":
                    if(obj2!=0)
                        ret = obj1 % obj2;
                    break;
            }

            return ret;
        }
        public double DoMath(double obj1, string token, double obj2)
        {
            double ret = 0;

            switch (token)
            {
                case "+":
                    ret = obj1 + obj2;
                    break;
                case "-":
                    ret = obj1 - obj2;
                    break;
                case "*":
                    ret = obj1 * obj2;
                    break;
                case "/":
                    if(obj2!=0)
                        ret = obj1 / obj2;
                    break;
                case "^":
                    ret = (float)Math.Pow(obj1, obj2);
                    break;
                case "%":
                    if(obj2!=0)
                        ret = obj1 % obj2;;
                    break;
            }

            return ret;
        }


        /// <summary>
        /// Does math from lhs to rhs regardless of dmas.
        /// brackets not supported.
        /// </summary>
        /// <param name="objs">Numbers to be opperated on</param>
        /// <param name="tokens">oparands ei +-*/%^(length should be 1 less than objs)</param>
        /// <returns>int</returns>
        public int MultiMath(int[] objs, string[] tokens)
        {

            // Math will be done from left to right

                     

            List<int> _objs = new List<int>();
            List<string> _tokens = new List<string>();

            foreach(int i_ in objs){
                _objs.Add(i_);
            }
            foreach(string s_ in tokens){
                _tokens.Add(s_);
            }
            int _res = _objs[0];
            _objs.RemoveAt(0);
            int _ctr = 0;
            foreach(int num in _objs)
            {
                _res = DoMath(_res, _tokens[_ctr], num);
                _ctr++;
            }
            return _res;
        }

        /// <summary>
        /// Does math from lhs to rhs regardless of dmas.
        /// brackets not supported.
        /// </summary>
        /// <param name="objs">Numbers to be opperated on</param>
        /// <param name="tokens">oparands ei +-*/%^(length should be 1 less than objs)</param>
        /// <returns>float</returns>
        public float MultiMath(float[] objs, string[] tokens)
        {

            // Math will be done from left to right



            List<float> _objs = new List<float>();
            List<string> _tokens = new List<string>();

            foreach (float i_ in objs)
            {
                _objs.Add(i_);
            }
            foreach (string s_ in tokens)
            {
                _tokens.Add(s_);
            }
            float _res = _objs[0];
            _objs.RemoveAt(0);
            int _ctr = 0;
            foreach (float num in _objs)
            {
                _res = DoMath(_res, _tokens[_ctr], num);
                _ctr++;
            }
            return _res;
        }

        /// <summary>
        /// Does math from lhs to rhs regardless of dmas.
        /// brackets not supported.
        /// </summary>
        /// <param name="objs">Numbers to be opperated on</param>
        /// <param name="tokens">oparands ei +-*/%^ (length should be 1 less than objs)</param>
        /// <returns>double</returns>
        public double MultiMath(double[] objs, string[] tokens)
        {

            // Math will be done from left to right



            List<double> _objs = new List<double>();
            List<string> _tokens = new List<string>();

            foreach (double i_ in objs)
            {
                _objs.Add(i_);
            }
            foreach (string s_ in tokens)
            {
                _tokens.Add(s_);
            }
            double _res = _objs[0];
            _objs.RemoveAt(0);
            int _ctr = 0;
            foreach (double num in _objs)
            {
                _res = DoMath(_res, _tokens[_ctr], num);
                _ctr++;
            }
            return _res;
        }

        #endregion

        #region get

        public bool isNumber(string var)
        {
            if (getType(var) == "int"|| getType(var) == "float"|| getType(var) == "double")
            {
                return true;
            }
            return false;
        }

        public string getType(string var)
        {
            if (GlobalVariables.ContainsKey(var))
            {
                string type_ = Var.GetTypeOfRaw(GlobalVariables[var].ToString());
                return getAbsoluteValue(var).GetType().ToString();
            }
            else
            {
                string type_ = Var.GetTypeOfRaw(var);
                return getAbsoluteValue(var).GetType().ToString();
            }
        }
        public object getValFromVar(string var)
        {
            return GlobalVariables[var];
        }


        public object getAbsoluteValue(string value)
        {
            if (GlobalVariables.ContainsKey(value))
            {
                return GlobalVariables[value];
            }
            else if (value == "$null")
            {
                return new object();
            }
            else
            {
                return Var.GetValueRaw(value);
            }
        }


        public object getUnchangedValue(string value)
        {
            if (GlobalVariables.ContainsKey(value))
            {
                return GlobalVariables[value];
            }
            else if (value == "$null")
            {
                return new object();
            }
            else
            {
                return Var.GetUnchangedvalueRaw(value);
            }
        }
        public string getNumericGroupType(string[] vars)
        {
            foreach(string str_ in vars)
            {
                if (getType(str_) != "int")
                    return getType(str_);
            }
            return "int";
        }

        public bool isTrue(string condition)
        {
            bool b = false;

            string[] _str = CycloneCompiler.CompilerUtilities.split(condition,' ');
            switch (getType(_str[0]))
            {
                case "bool":
                    b = BooleanOpperations.ComparitiveOpperation((bool)getAbsoluteValue(_str[0]), (bool)getAbsoluteValue(_str[2]), getAbsoluteValue(_str[1]).ToString());
                    break;
                case "int":
                    b = BooleanOpperations.ComparitiveOpperation((int)getAbsoluteValue(_str[0]), (int)getAbsoluteValue(_str[2]), getAbsoluteValue(_str[1]).ToString());
                    break;
                case "float":
                    b = BooleanOpperations.ComparitiveOpperation((float)getAbsoluteValue(_str[0]), (float)getAbsoluteValue(_str[2]), getAbsoluteValue(_str[1]).ToString());
                    break;
                case "double":
                    b = BooleanOpperations.ComparitiveOpperation((double)getAbsoluteValue(_str[0]), (double)getAbsoluteValue(_str[2]), getAbsoluteValue(_str[1]).ToString());
                    break;
                case "string":
                    b = BooleanOpperations.ComparitiveOpperation(getAbsoluteValue(_str[0]).ToString(), getAbsoluteValue(_str[2]).ToString(), getAbsoluteValue(_str[1]).ToString());
                    break;

            }

            return b;
        }

        #endregion

        #region add
        
        public void addByString(string var,string value){
            if (var.Contains("<") && var.Contains(">"))
            {
                string _type = CycloneCompiler.CompilerUtilities.GetEncapsulatedData(var, '<', '>');
                var = var.Replace($"<{_type}>","");

                try
                {
                    switch (_type)
                    {
                        case "int":
                            if(value != "$null")
                                GlobalVariables.Add(var,(int)getAbsoluteValue(value));
                            else
                                GlobalVariables.Add(var,null);
                            break;
                        case "float":
                            float f__ = (float)getAbsoluteValue(value);
                            if(value != "$null")
                                GlobalVariables.Add(var,f__);
                            else
                                GlobalVariables.Add(var,null);
                            break;
                        case "double":
                            if(value != "$null")
                                GlobalVariables.Add(var,(double)getAbsoluteValue(value));
                            else
                                GlobalVariables.Add(var,null);
                            break;
                        case "bool":
                            if(value != "$null")
                                GlobalVariables.Add(var,(bool)getAbsoluteValue(value));
                            else
                                GlobalVariables.Add(var,null);
                            break;
                        case "string":
                            if(value != "$null")
                                GlobalVariables.Add(var,(int)getAbsoluteValue(value) + "");
                            else
                                GlobalVariables.Add(var,null);
                            break;
                        default:
                            GlobalVariables.Add(var,new object());
                        break;
                    }
                    
                }
                catch(Exception e)
                {
                    ConsoleFunctions.ThrowException($"A type error has occured with cast \"{_type}\" and value \"{value}\" \n");
                    ConsoleFunctions.ThrowException(e.ToString());
                }
                
            }
            else
            {
                GlobalVariables.Add(var,getAbsoluteValue(value));
            }

        }
        public void add(string var,object value){
            if (!GlobalVariables.ContainsKey(var))
                GlobalVariables.Add(var, value);
            else
                CycloneCompiler.ConsoleFunctions.ThrowException($"{var} already exists! \n");
        }
        public void addByMultimath(string var , string[] tags)
        {
            List<string> _tokens = new List<string>();
            List<string> _vals = new List<string>();
            for(int i= 0; i < tags.Length; i++)
            {
                if (i % 2 == 0) { _vals.Add(tags[i]); } else { _tokens.Add(tags[i]); }
            }
            if (getNumericGroupType(_vals.ToArray()) == "int")
            {
                List<int> _temp = new List<int>();
                foreach(string str_ in _vals)
                {
                    _temp.Add((int)getAbsoluteValue(str_));
                }
                int tempVal_ = MultiMath(_temp.ToArray(),_tokens.ToArray());
                addByString(var,tempVal_+"");
            }
            else if (getNumericGroupType(_vals.ToArray()) == "float")
            {
                List<float> _temp = new List<float>();
                foreach (string str_ in _vals)
                {
                    _temp.Add((float)getAbsoluteValue(str_));
                }
                float tempVal_ = MultiMath(_temp.ToArray(), _tokens.ToArray());
                addByString(var, tempVal_ + "");
            }
            else if (getNumericGroupType(_vals.ToArray()) == "double")
            {
                List<double> _temp = new List<double>();
                foreach (string str_ in _vals)
                {
                    _temp.Add((double)getAbsoluteValue(str_));
                }
                double tempVal_ = MultiMath(_temp.ToArray(), _tokens.ToArray());
                addByString(var, tempVal_ + "");
            }
        }
        
        #endregion

        #region set
        public void setValueByString(string var, string value)
        {
            if (GlobalVariables.ContainsKey(var))
            {
                GlobalVariables[var] = getAbsoluteValue(value);
            }
            else
            {
                CycloneCompiler.ConsoleFunctions.ThrowException($"{var} does not exist.");
            }
        }

        public void setValue(string var , object value)
        {
            if (GlobalVariables.ContainsKey(var))
            {
                GlobalVariables[var] = value;
            }
            else
            {
                CycloneCompiler.ConsoleFunctions.ThrowException($"{var} does not exist.");
            }
        }

        public void setByMultiMath(string var,string[] vals,string[] tokens)
        {
            string _varType = getType(var);
            if(_varType == "int")
            {
                int[] _nums = new int[vals.Length];
                for(int i = 0; i < _nums.Length; i++)
                {
                    try
                    {
                        _nums[i] = int.Parse(getAbsoluteValue(vals[i]).ToString());
                    }
                    catch { }
                }
                setValueByString(var,MultiMath(_nums, tokens)+"");
            }
            else if(_varType == "float")
            {
                float[] _nums = new float[vals.Length];
                for (int i = 0; i < _nums.Length; i++)
                {
                    try
                    {
                        _nums[i] = float.Parse(getAbsoluteValue(vals[i]).ToString());
                    }
                    catch { }
                }
                setValueByString(var, MultiMath(_nums, tokens) + "");
            }
            else if(_varType == "double")
            {
                double[] _nums = new double[vals.Length];
                for (int i = 0; i < _nums.Length; i++)
                {
                    try
                    {
                        _nums[i] = double.Parse(getAbsoluteValue(vals[i]).ToString());
                    }
                    catch(Exception e)
                    {
                        CycloneCompiler.ConsoleFunctions.Debug($"An unexpected exception occured : \n {e.ToString()} \n Please report this error to github.");
                    }
                }
                setValueByString(var, MultiMath(_nums, tokens) + "");
            }
        }

        public void setByMultiString(List<string> strings_, string var)
        {
            if (GlobalVariables.ContainsKey(var))
            {
                GlobalVariables[var] = StringConcat(strings_);
            }
            else
            {
                CycloneCompiler.ConsoleFunctions.ThrowException($"{var} does not exist");
            }
        }

        #endregion

        #region string functions
        public string StringConcat(List<string> _strings)
        {
            // add string like this : "your total is " total "."
            //currently no way to remove strings
            string res_ = "";
            List<string> strs_ = new List<string>();
            List<string> oops_ = new List<string>();
            for(int i = 0; i < _strings.Count; i++)
            {
                if (i % 2 == 0) { strs_.Add(_strings[i]); } else { oops_.Add(_strings[i]); }
            }
            res_ = strs_[0];
            strs_.RemoveAt(0);
            int ctr = 0;
            foreach(string _temp_ in strs_)
            {
                if (oops_[ctr] == "+")
                {
                    res_ += getAbsoluteValue(_temp_);
                }else if (oops_[ctr] == "-")
                {
                    res_.Replace(getAbsoluteValue(_temp_).ToString(), "");
                }
                ctr++;
            }
            return res_;
        }
        #endregion

        public bool has(string var)
        {
            return GlobalVariables.ContainsKey(var);
        }  

        public class BooleanOpperations
        {
            public static bool boolOpperation(bool var , string token)
            {
                bool b;

                switch (token)
                {
                    case "!":
                        b = !var;
                        break;
                    default:
                        b = var;
                        break;
                }

                return b;
            }
            public static bool ComparitiveOpperation(string var1,string var2,string token)
            {
                bool res = false;
                switch (token)
                {
                    case "==":
                        res = var1 == var2;
                        break;
                    case "!=":
                        res = var1 != var2;
                        break;
                    case "contains":
                        res = var1.Contains(var2);
                        break;
                    case "starts_with":
                        res = var1.StartsWith(var2);
                        break;
                    case "ends_with":
                        res = var1.EndsWith(var2);                        
                        break;
                    default:
                        CycloneCompiler.ConsoleFunctions.ThrowException($"{token} is an invalid comparision");
                        break;
                }
                return res;
            }
            public static bool ComparitiveOpperation(int var1 , int var2 , string token)
            {
                
                bool res = false;

                switch (token)
                {
                    case ">":
                        res = var1 > var2;
                        break;
                    case "<":
                        res = var1 < var2;
                        break;
                    case ">=":
                        res = var1 >= var2;
                        break;
                    case "<=":
                        res = var1 <= var2;
                        break;
                    case "==":
                        res = var1 == var2;
                        break;
                    case "!=":
                        res = var1 != var2;
                        break;
                    default:
                        CycloneCompiler.ConsoleFunctions.ThrowException($"{token} is an invalid comparision");
                        break;
                }

                return res;
            }
            public static bool ComparitiveOpperation(long var1 , long var2 , string token)
            {
                
                bool res = false;

                switch (token)
                {
                    case ">":
                        res = var1 > var2;
                        break;
                    case "<":
                        res = var1 < var2;
                        break;
                    case ">=":
                        res = var1 >= var2;
                        break;
                    case "<=":
                        res = var1 <= var2;
                        break;
                    case "==":
                        res = var1 == var2;
                        break;
                    case "!=":
                        res = var1 != var2;
                        break;
                    default:
                        CycloneCompiler.ConsoleFunctions.ThrowException($"{token} is an invalid comparision");
                        break;
                }

                return res;
            }
            public static bool ComparitiveOpperation(float var1, float var2, string token)
            {

                bool res = false;

                switch (token)
                {
                    case ">":
                        res = var1 > var2;
                        break;
                    case "<":
                        res = var1 < var2;
                        break;
                    case ">=":
                        res = var1 >= var2;
                        break;
                    case "<=":
                        res = var1 <= var2;
                        break;
                    case "==":
                        res = var1 == var2;
                        break;
                    case "!=":
                        res = var1 != var2;
                        break;
                    default:
                        CycloneCompiler.ConsoleFunctions.ThrowException($"{token} is an invalid comparision");
                        break;
                }

                return res;
            }
            public static bool ComparitiveOpperation(double var1, double var2, string token)
            {

                bool res = false;

                switch (token)
                {
                    case ">":
                        res = var1 > var2;
                        break;
                    case "<":
                        res = var1 < var2;
                        break;
                    case ">=":
                        res = var1 >= var2;
                        break;
                    case "<=":
                        res = var1 <= var2;
                        break;
                    case "==":
                        res = var1 == var2;
                        break;
                    case "!=":
                        res = var1 != var2;
                        break;
                    default:
                        CycloneCompiler.ConsoleFunctions.ThrowException($"{token} is an invalid comparision");
                        break;
                }

                return res;
            }            
            public static bool ComparitiveOpperation(bool var1,bool var2,string token) 
            {
                bool res = false;
                switch (token)
                {
                    case "&&":
                        res = var1 && var2;
                        break;
                    case "||":
                        res = var1 || var2;
                        break;
                    case "==":
                        res = var1 == var2;
                        break;
                    case "!=":
                        res = var1 != var2;
                        break;
                }
                return res;
            }
        }
    }

    public class Var
    {
        /*
            \\n :\n
            \\r :\r
            %/  :\\
            ./  :Environment.CurrentDirectory
            %n  :Environment.NewLine
         */
        
        public static object GetValueRaw(string value)
        {
            if (value != "")
            {
                if (IsNumeric(value) && !value.Contains(".") && !value.Contains("f"))
                {
                    try{
                        int i = int.Parse(value);
                        return i;
                    }
                    catch{
                        long i = long.Parse(value);
                        return i;
                    }
                    
                }
                else if (value == "true")
                {
                    return true;
                }
                else if (value == "false")
                {
                    return false;
                }
                else if (IsNumeric(value) && value.Contains(".") && value.EndsWith("f"))
                {
                    value = value.Replace("f", "");
                    float f = float.Parse(value);
                    return f;
                }
                else if(IsNumeric(value) && value.Contains(".") && value.EndsWith("f"))
                {
                    string s__ = value.Replace("f","");
                    float d = float.Parse(s__);
                    return d;
                }
                else if (IsNumeric(value) && value.Contains("."))
                {
                    double d = double.Parse(value);
                    return d;
                }
                else if (value.StartsWith('\"') && value.EndsWith('\"'))
                {
                    string _temp = "" + value.ToString() + "";
                    _temp = _temp.Replace("\\n", "\n");
                    _temp = _temp.Replace("\\r", "\r");
                    _temp = _temp.Replace("%n", Environment.NewLine);
                    _temp = _temp.Replace("\\v", "\v");
                    _temp = _temp.Replace("%/", "\\");
                    _temp = _temp.Replace("%47", "\\");
                    _temp = _temp.Replace("%37", "%");
                    _temp = _temp.Replace("%t", "\t");
                    _temp = _temp.Replace("%40", "(");
                    _temp = _temp.Replace("%41", ")");
                    _temp = _temp.Replace("%123", "{");
                    _temp = _temp.Replace("%125", "}");
                    _temp = _temp.Replace("./", Environment.CurrentDirectory);
                    _temp = _temp.Replace("\"", "");
                    _temp = _temp.Replace("\'", "");
                    _temp = _temp.Replace("%34", "\"");
                    _temp = _temp.Replace("%39", "\'");
                    return _temp;
                }
            }
            else
            {
                CycloneCompiler.ConsoleFunctions.ThrowException($"{value} is null");
                throw new Exception($"{value} is null");
                return "@@NULL_STR_EXC@@";
            }
            CycloneCompiler.ConsoleFunctions.ThrowException($"{value} is not valid or does not exist");
            throw new Exception($"{value} is not valid or does not exist");
            return "@@TRASH_VALUE@@";
        }
        public static object GetCastedObject(string value, string type)
        {
            if (type == "int")
            {
                return int.Parse(value);
            }
            else if (type == "double")
            {
                return double.Parse(value);
            }
            else if (type == "long")
            {
                value = value.Replace("l", "");
                return long.Parse(value);
            }
            else if(type == "float")
            {
                value = value.Replace("f", "");
                return float.Parse(value);
            }
            else if(type == "bool")
            {
                if(value == "true"){
                    return true;
                }
                else{
                    return false;
                }
            }
            else
            {
                return value;
            }
        }
        public static string GetTypeOfRaw(string value)
        {
            if (IsNumeric(value) && !value.Contains(".")&& !value.EndsWith("l"))
            {                
                try{
                    int i = int.Parse(value);
                    return "int";
                }catch{
                    return "long";
                }
            }
            if (IsNumeric(value) && !value.Contains(".") && value.EndsWith("l"))
            {
                return "long";    
            }
            else if (IsNumeric(value) && value.Contains(".") && value.EndsWith("f"))
            {
                return "float";
            }
            else if(value == "true" || value == "false")
            {
                return "bool";
            }
            else if (IsNumeric(value) && value.Contains("."))
            {
                return "double";
            }
            return "string";
        }
        public static object GetUnchangedvalueRaw(string value){
            if (value != "")
            {
                if (IsNumeric(value) && !value.Contains("."))
                {
                    int i = int.Parse(value);
                    return i;
                }
                else if (value == "true")
                {
                    return true;
                }
                else if (value == "false")
                {
                    return false;
                }
                else if (IsNumeric(value) && value.Contains(".") && value.EndsWith("f"))
                {
                    value = value.Replace("f", "");
                    float f = float.Parse(value);
                    return f;
                }
                else if (IsNumeric(value) && value.Contains("."))
                {
                    double d = double.Parse(value);
                    return d;
                }
                else if (value.StartsWith('\"') && value.EndsWith('\"'))
                {
                    //value = value.Replace("\\n", "\n");
                    //value = value.Replace("\\r", "\r");
                    //value = value.Replace("\\v", "\v");
                    //value = value.Replace("%/", "\\");
                    //value = value.Replace("%47", "\\");
                    //value = value.Replace("%37", "%");
                    //value = value.Replace("%t", "\t");
                    //value = value.Replace("%40", "(");
                    //value = value.Replace("%41", ")");
                    //value = value.Replace("%123", "{");
                    //value = value.Replace("%125", "}");
                    //value = value.Replace("./", Environment.CurrentDirectory);
                    //value = value.Replace("\"", "");
                    //value = value.Replace("\'", "");
                    //value = value.Replace("%34", "\"");
                    //value = value.Replace("%39", "\'");
                    return value;
                }
            }
            else
            {
                CycloneCompiler.ConsoleFunctions.Warn($"{value} is null");
                return "$null";
            }
            CycloneCompiler.ConsoleFunctions.ThrowException($"{value} is not valid or does not exist");
            return "@@TRASH_VALUE@@";
        }
        public static bool IsNumeric(string value)
        {
            string numVals = "-+1234567890.f";
            foreach(char c in value)
            {
                if (!numVals.Contains(c))
                {
                    return false;
                }
            }
            return true;
        }
    }

}
