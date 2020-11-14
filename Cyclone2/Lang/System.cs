using System;
using Cyclone.Lang;
using System.Collections.Generic;

namespace Cyclone.Lang.CycloneSystem
{
    public class String
    {
        public string StringValue{
            get{
                return StringValue;
            }
            set{
                StringValue = value.ToString();
            }
        }
        public char[] ToCharArray(){
            return StringValue.ToCharArray();
        }
        public static string doMultiString(string str, Cyclone.Lang.VariableHandler handler)
        {
            List<string> str_ = new List<string>();
            string[] codek = CycloneCompiler.CompilerUtilities.split(str,' ');
            for (int i = 2; i < codek.Length; i++)
            {
                str_.Add(codek[i]);
            }
            string res_ = "";
            
            List<string> _opper = new List<string>();
            List<string> _vals = new List<string>();

            for(int i=0;i<str_.Count;i++)
            {
                if(i%2==0 || i==0){
                    _vals.Add(str_[i]);
                }else{
                    _opper.Add(str_[i]);
                }
                
            }
            Console.WriteLine();
            
            for(int i=0;i<_opper.Count;i++)
            {
                if(_opper[i].Contains("+")){
                    res_ += handler.getAbsoluteValue(_vals[i]).ToString() + handler.getAbsoluteValue(_vals[i+1]).ToString();
                    Console.WriteLine(handler.getAbsoluteValue(_vals[i]).ToString() + handler.getAbsoluteValue(_vals[i+1]).ToString());
                }
                else if(_opper[i].Contains("-")){
                    res_ += handler.getAbsoluteValue(_vals[i]).ToString().Replace(handler.getAbsoluteValue(_vals[i+1]).ToString(),"");
                    Console.WriteLine(handler.getAbsoluteValue(_vals[i]).ToString().Replace(handler.getAbsoluteValue(_vals[i+1]).ToString(),""));
                }
                else if(_opper[i].Contains("~")){
                    res_ = res_.Replace(handler.getAbsoluteValue(_vals[i]).ToString(),"");
                    Console.WriteLine(res_.Replace(handler.getAbsoluteValue(_vals[i]).ToString(),""));
                }
            }

            return res_;
        }
        public string[] Split(char spliter_){
            return CycloneCompiler.CompilerUtilities.split(StringValue,spliter_);
        }
        public static string[] Split(string value_,char spliter_){
            return CycloneCompiler.CompilerUtilities.split(value_,spliter_);
        }
        public static string GetEncapsulatedData(string str_,char a, char b)
        {
            return CycloneCompiler.CompilerUtilities.GetEncapsulatedData(str_, a, b);
        }
        public string GetEncapsulatedData( char a, char b)
        {
            return CycloneCompiler.CompilerUtilities.GetEncapsulatedData(StringValue, a, b);
        }
    }
    public class Bool
    {
        public static bool CheckCondition(string str_, VariableHandler handler)
        {
            string enc_ = CycloneCompiler.CompilerUtilities.GetEncapsulatedData(str_,'(',')');
            string[]  data_ =  CycloneCompiler.CompilerUtilities.split(enc_,' ');
        
            bool res = false;
            if(data_.Length == 1)
            {
                string var_ = data_[0].Replace("!","");
                string opp_ = data_[0].Replace(var_,"");

                bool b = (bool)handler.getAbsoluteValue(var_);
                res = VariableHandler.BooleanOpperations.boolOpperation(b,opp_);
            }
            else if(data_.Length == 3)
            {
                string v1 = data_[0], op = data_[1] , v2 = data_[2];
                res = CheckBool($"{handler.getAbsoluteValue(v1)}",$"{handler.getAbsoluteValue(v2)}", op);
            }

            return res;
        }
        public static bool CheckBool(string var1 , string var2 , string opp_)
        {
            string type = Var.GetTypeOfRaw(var1);
            bool res = false;

            if(type == "int")
            {              
                int i = int.Parse(var1),j = int.Parse(var2);
                res = VariableHandler.BooleanOpperations.ComparitiveOpperation(i,j,opp_);
            }
            else if(type == "float")
            {              
                float i = float.Parse(var1),j = float.Parse(var2);
                res = VariableHandler.BooleanOpperations.ComparitiveOpperation(i,j,opp_);
            }
            else if(type == "double")
            {              
                double i = double.Parse(var1),j = double.Parse(var2);
                res = VariableHandler.BooleanOpperations.ComparitiveOpperation(i,j,opp_);
            }
            else if(type == "long")
            {              
                long i = long.Parse(var1),j = long.Parse(var2);
                res = VariableHandler.BooleanOpperations.ComparitiveOpperation(i,j,opp_);
            }
            else if(type == "bool")
            {              
                bool i = bool.Parse(var1),j = bool.Parse(var2);
                res = VariableHandler.BooleanOpperations.ComparitiveOpperation(i,j,opp_);
            }
            else if(type == "string")
            {              
                string i = var1, j = var2;
                res = VariableHandler.BooleanOpperations.ComparitiveOpperation(i,j,opp_);
            }

            return res;
        }
    }
    public class Array
    {
        public object[] Data;
        public string Type;
        public Array() { }
        public Array(string type , object[] data)
        {
            Type = type;
            System.Array.Copy(data, Data, data.Length);
        }
        public Array(string type, string[] data)
        {
            Type = type;
            Data = new object[data.Length];
            for(int i = 0; i < data.Length; i++)
            {
                Data[i] = Var.GetValueRaw(data[i]);
            }
        }
        public Array(string type,string data)
        {
            string enc_ = String.GetEncapsulatedData(data,'[',']');
            string[] arr_ = String.Split(enc_,',');
            Data = new object[arr_.Length];
            Type = type;
            
            for(int i = 0; i < arr_.Length; i++)
            {
                Data[i] = Var.GetCastedObject(type, arr_[i]);
            }
        }
        public Array(string data)
        {
            string enc_ = String.GetEncapsulatedData(data, '[', ']');
            string[] arr_ = String.Split(enc_, ',');
            Data = new object[arr_.Length];

            for (int i = 0; i < arr_.Length; i++)
            {
                Data[i] = Var.GetValueRaw(arr_[i]);
            }
            Type = Var.GetTypeOfRaw(Data[0] + "");            
        }
        
    }
}