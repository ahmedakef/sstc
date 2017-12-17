using System;
using System.IO;
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

        public static void add(string[] added_files){
            Stage.add_files(added_files);
        }
        public static void commit(string message){
            string[] traked_files = Stage.get_traked_files();
            int current_commit = Current.commit;
            Commit commit = new Commit(current_commit);
                
            foreach (string traked_file in traked_files)
            {
                commit.commit_file(traked_file);
            }
            commit.finish(message); 
            
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

                if(commit_depend_on == commit_num){ // the place for the origional file
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

        public static void status(){
            // ToDo 
            // 1_ iterate recursively over all files in the directory and put them in alist
            // 2_ compare them with Depend_on.prevFilesTracked() if one of them is missed so its new file
            // 3_ if you find it use helper.SHA_1() on the current and the output from checkout with  -
            //    the last commit to obtain if it is edited or no;


            /*string CurrentDirectoryPath = Directory.GetCurrentDirectory();
            DirectoryInfo CurrentDirectory = new DirectoryInfo(CurrentDirectoryPath);
            foreach (FileInfo fi in CurrentDirectory.GetFiles())
            {

            }*/
            
        }

    }

}
