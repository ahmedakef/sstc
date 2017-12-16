using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace sstc
{

    public class Commit{
        private int commit_num;
        private string commit_path;
        private string commit_content;
        private string[] all_text_splitted ;
        private int files_commited_num;
        private string[] files_commited;
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
        public void commit_file(string traked_file){
            if(Depend_on.get_commit_num(traked_file)==-1){
                string file_text = File.ReadAllText(traked_file);
                write_commit_node(traked_file,file_text);
                Depend_on.add_file(traked_file,commit_num);
            }else{
                int dependon_num = Depend_on.get_commit_num(traked_file);
                string current_content = File.ReadAllText(traked_file).Trim(' ','\n');
                Commit dependon_commit = new Commit(dependon_num);
                string source_content = dependon_commit.get_file_content(traked_file);
                string changes = helper.delta(source_content,traked_file);
                if(changes == "THEY ARE SAME"){
                    return;
                }
                write_commit_node(traked_file,changes);
            }
        }
        private void write_commit_node(string traked_file,string content){
            
            using (StreamWriter sw = new StreamWriter(commit_path,true))
            {
                sw.WriteLine(traked_file+"->"+helper.SHA_1(content));
                sw.WriteLine("----------------------");
                sw.WriteLine(content);
                sw.WriteLine("===***===sstc===***===");
            }
        }

        public void finish(string message){
            string commmit_hash;
            using (StreamWriter sw = new StreamWriter(commit_path,true))
            {
                string commit_content = File.ReadAllText(commit_path);
                commmit_hash = helper.SHA_1(commit_content);
                sw.Write(message+"->"+commmit_hash);
            }

            DateTime commit_date = DateTime.Now ;
            using (StreamWriter sw = new StreamWriter(".sstc/branches/master/commits_log.txt",true)){
                sw.WriteLine(commit_num+"->"+commmit_hash+"->"+commit_date+"->"+message);
            }

            Current.commit = commit_num;
            File.WriteAllText(".sstc/branches/master/tracked.txt",""); // remove from stageing area

            Console.WriteLine("files commited successfully");
        }


    }
 
}
