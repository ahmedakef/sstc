using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace sstc
{

 
    class Program
    {
        static void Main(string[] args)
        {
            int n = args.Length;
            foreach(string arg in args){
                if(arg == "init"){
                    sstc.init();
                }else if(arg == "add" && n >=2){
                    string[] added_files = new string[n-1];
                    Array.Copy(args,1,added_files,0,n-1);
                    sstc.add(added_files);
                    break;
                    
                }else if(arg == "commit" && n==2 ){
                    sstc.commit(args[1]);
                    break;
                }else if(arg ==  "log"){
                    sstc.log();
                }
            } 
            //string currentDirName = Directory.GetCurrentDirectory();
        }
    }
}
