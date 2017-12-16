using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace sstc
{

    public static class sstc {
        public static void init(){
            try{
                if(Directory.Exists(".sstc")){
                    Console.WriteLine("a sstc repository is already initialized");
                }else{
                    Directory.CreateDirectory(".sstc");
                    using (StreamWriter sw = File.CreateText(".sstc/current.txt"))
                    {
                        sw.WriteLine("branch->master");
                        sw.WriteLine("commit->0");
                    }
                    Directory.CreateDirectory(".sstc/branches");
                    Directory.CreateDirectory(".sstc/branches/master");
                    File.Create(".sstc/branches/master/0.txt");
                    File.Create(".sstc/branches/master/tracked.txt");
                    File.Create(".sstc/branches/master/depend_on.txt");
                    Console.WriteLine("Repository created successfully");
                }
            }catch(Exception e){
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
        /*public static void status(){
            string CurrentDirectoryPath = Directory.GetCurrentDirectory();
            DirectoryInfo CurrentDirectory = new DirectoryInfo(CurrentDirectoryPath);
            foreach (FileInfo fi in CurrentDirectory.GetFiles())
            {

            }
        }*/
        public static void add(string[] added_files){
            string[] added = File.ReadAllLines(".sstc/branches/master/tracked.txt");
            using (StreamWriter sw = new StreamWriter(".sstc/branches/master/tracked.txt",true))
            {
                for (int i = 0; i < added_files.Length; i++)
                {
                    if(Array.IndexOf(added,added_files[i])==-1){
                        sw.WriteLine(added_files[i]);
                        Console.WriteLine(added_files[i]+" added to the stage successfully");
                    }
                }
            }
        }
        public static void commit(string message){
            string[] traked_files = File.ReadAllLines(".sstc/branches/master/tracked.txt");
            string prev_commit = File.ReadAllLines(".sstc/current.txt")[1].Split("->")[1];
            int current_commit = Convert.ToInt32(prev_commit)+1;
            string current_commit_path = ".sstc/branches/master/"+current_commit+".txt";

                if(prev_commit == "0"){
                    foreach (string traked_file in traked_files){      
                        helper.write_all_text_commit_node(current_commit,traked_file);
                    }
                    
                }else{
                    List<string> prev_files_tracked = helper.prevFilesTracked();
                    foreach (string traked_file in traked_files)
                    {
                        if(prev_files_tracked.IndexOf(traked_file) == -1){ 
                            helper.write_all_text_commit_node(current_commit,traked_file);
                        }else{
                            int commit_depend_on = Depend_on.get_commit_num(traked_file);
                            helper.write_changed_commit_node(current_commit,commit_depend_on,traked_file);
                        }
                    }
                }
            
            string commmit_hash;
            using (StreamWriter sw = new StreamWriter(current_commit_path,true))
            {
                string commit_content = File.ReadAllText(current_commit_path);
                commmit_hash = helper.SHA_1(commit_content);
                sw.Write(message+"->"+commmit_hash);
            }

            DateTime commit_date = DateTime.Now ;
            using (StreamWriter sw = new StreamWriter(".sstc/branches/master/commits_log.txt",true)){
                sw.WriteLine(current_commit+"->"+commmit_hash+"->"+commit_date+"->"+message);
            }

            string current_content = "branch->master\n";
            current_content += "commit->"+current_commit;
            File.WriteAllText(".sstc/current.txt",current_content);
            File.WriteAllText(".sstc/branches/master/tracked.txt",""); // remove from stageing area

            Console.WriteLine("files commited successfully");
        }
        public static void checkout(int commit_num){
            Commit commit = new Commit(commit_num);
            string[] files_commited = commit.get_files_commited();
            for (int i = 0; i < files_commited.Length ; i++)
            {
                string[] file_changes = commit.get_file_content(files_commited[i]).Split("\n");
                int commit_depend_on = Depend_on.get_commit_num(files_commited[i]);
                Commit dependon_commit = new Commit(commit_depend_on);
                List<string> dependon_file = new List<string>(dependon_commit.get_file_content(files_commited[i]).Split("\n"));
                if(commit_depend_on == commit_num){
                    File.WriteAllLines(files_commited[i],dependon_file);

                    continue;
                }
                
                foreach (string line in file_changes){
                    bool add = line[0]=='+';
                    int line_num = Convert.ToInt32(line.Split("/>")[1]);
                    string line_content = line.Split("/>")[2];
                    if(add){
                        dependon_file.Insert(line_num,line_content);
                    }else{
                        dependon_file.Remove(line_content);
                        
                    }
                }
                File.WriteAllLines(files_commited[i],dependon_file);

            }
        }
        public static void log(){
            string[] commits = File.ReadAllLines(".sstc/branches/master/commits_log.txt");
            foreach (string line in commits)
            {
                string[] sections = line.Split("->");
                Console.WriteLine(sections[0]+ " "+sections[1]+" "+sections[2]+ "  "+sections[3]);
            }
        }
    }

}
