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
        public static void status(){
            string CurrentDirectoryPath = Directory.GetCurrentDirectory();
            DirectoryInfo CurrentDirectory = new DirectoryInfo(CurrentDirectoryPath);
            foreach (FileInfo fi in CurrentDirectory.GetFiles())
            {

            }
        }
        public static void add(string[] added_files){
            string[] added = File.ReadAllLines(".sstc/branches/master/tracked.txt");
            using (StreamWriter sw = new StreamWriter(".sstc/branches/master/tracked.txt",true))
            {
                for (int i = 0; i < added_files.Length; i++)
                {
                    if(Array.IndexOf(added,added_files[i])==-1){
                        sw.WriteLine(added_files[i]);
                        Console.WriteLine("file added to the stage successfully");
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
                    
                    using (StreamWriter sw_do = new StreamWriter(".sstc/branches/master/depend_on.txt",true)){
                        foreach (string traked_file in traked_files){      
                            sw_do.WriteLine(traked_file+"->"+current_commit);
                        }
                    }
                }else{
                    List<string> prev_files_tracked = helper.prevFilesTracked();
                    foreach (string file in traked_files)
                    {
                        if(prev_files_tracked.IndexOf(file) == -1){ 
                            using (StreamWriter sw_do = new StreamWriter(".sstc/branches/master/depend_on.txt",true)){    
                                sw_do.WriteLine(file+"->"+current_commit);
                            }
                            helper.write_all_text_commit_node(current_commit,file);
                        }else{
                            string[] depend_on_content = File.ReadAllLines(".sstc/branches/master/depend_on.txt");
                            int commit_depend_on = Convert.ToInt32(depend_on_content[prev_files_tracked.IndexOf(file)].Split("->")[1]);
                            helper.write_changed_commit_node(current_commit,commit_depend_on,file);
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
