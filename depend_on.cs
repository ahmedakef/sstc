using System;
using System.IO;


namespace sstc
{

    public static class Depend_on{
        
        private static string Path = ".sstc/branches/master/depend_on.txt" ;
        private static string[] depend_on_content = File.ReadAllLines(Path);

        public static int get_commit_num(string file_name){
            for (int i = 0; i < depend_on_content.Length; i++)
            {
                if(file_name == depend_on_content[i].Split("->")[0])
                    return Convert.ToInt32(depend_on_content[i].Split("->")[1]);
            }
            return -1;
        }
        public static void add_file(string file_name,int commit){
            using (StreamWriter sw_do = new StreamWriter(Path,true)){
                sw_do.WriteLine(file_name+"->"+commit);
            }
        }
        public static string[] prevFilesTracked(){
            string[] all_text =  File.ReadAllLines(Path);
            int n= depend_on_content.Length;
            string[] prev_files_tracked = new string[n];
            for (int i = 0; i < n; i++)
            {
                prev_files_tracked[i]=all_text[i].Split("->")[0];
            }
            return prev_files_tracked;
        }
        


    }
 
}

