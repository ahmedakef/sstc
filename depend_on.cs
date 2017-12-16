using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace sstc
{

    public static class Depend_on{
        
        public static string[] depend_on_content = File.ReadAllLines(".sstc/branches/master/depend_on.txt");

        public static int get_commit_num(string file_name){
            for (int i = 0; i < depend_on_content.Length; i++)
            {
                if(file_name == depend_on_content[i].Split("->")[0])
                    return Convert.ToInt32(depend_on_content[i].Split("->")[1]);
            }
            return -1;
        }
        public static void add_file(string file_name,int commit){
            using (StreamWriter sw_do = new StreamWriter(".sstc/branches/master/depend_on.txt",true)){
                sw_do.WriteLine(file_name+"->"+commit);
            }
        }
        


    }
 
}

