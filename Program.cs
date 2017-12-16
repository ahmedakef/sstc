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
            string command = "";

            if (n >= 1) {
                command = args[0];
            }

            if (command != "") {
                if(command == "init"){
                    sstc.init();
                }else if(command == "add" && n >=2){
                    string[] added_files = new string[n-1];
                    Array.Copy(args,1,added_files,0,n-1);
                    sstc.add(added_files);                
                }else if(command == "commit" && n==2 ){
                    sstc.commit(args[1]);
                }else if(command ==  "log"){
                    sstc.log();
                }else if(command == "checkout" && n==2){
                    sstc.checkout(Convert.ToInt32(args[1]));
                }else if(command == "help"){
                    Console.WriteLine("init                 \t\t initialize a repo");
                    Console.WriteLine("add <file path> ...  \t\t to add files to staging area");
                    Console.WriteLine("commit               \t\t to commit changes");
                    Console.WriteLine("log                  \t\t to list commits history");
                    Console.WriteLine("checkout <commit_num>\t\t to change source code to this commit");
                    Console.WriteLine("help                 \t\t to list manual");
                }else{
                    Console.WriteLine("command {0} not found \n use help to know the commands",command);
                }
            } else {
                Console.WriteLine("use help to know the commands");
            }
        }
    }
}
