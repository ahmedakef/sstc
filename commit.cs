using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace sstc
{

    public class Commit{
        int commit_num;
        string commit_path;
        string commit_content;
        string[] all_text_splitted ;
        int files_commited_num;
        string[] files_commited;
        public Commit(int commit_num){
            this.commit_num=commit_num;
            this.commit_path = ".sstc/branches/master/"+commit_num+".txt";
            if(File.Exists(commit_path)){
                this.commit_content = File.ReadAllText(commit_path);
                this.all_text_splitted = this.commit_content.Split("===***===sstc===***===");
                this.files_commited_num = all_text_splitted.Length-1;
                this.files_commited = get_files_commited();
                
            }
        }
        public string[] get_files_commited(){
            string[] files_commited = new string[files_commited_num];
            for (int i = 0; i < files_commited_num; i++)
            {
                string file_info = all_text_splitted[i];
                files_commited[i] = file_info.Split("----------------------")[0].Split("->")[0].Trim('\n');
            }

            return files_commited;
        }
        public string get_file_content(string file_name){
            if(Array.IndexOf(files_commited,file_name)==-1)
                return "";

            int file_index = Array.IndexOf(files_commited,file_name);
            string file_info =all_text_splitted [ file_index ];
            string source_content = file_info.Split("----------------------")[1].Trim(' ','\n');
            return source_content;
        }
        public void write_changed_commit_node(string traked_file,string changes){
            using (StreamWriter sw = new StreamWriter(commit_path,true))
            {
                sw.WriteLine(traked_file+"->"+helper.SHA_1(changes));
                sw.WriteLine("----------------------");
                sw.WriteLine(changes);
                sw.WriteLine("===***===sstc===***===");
            }
        }


    }
 
}
