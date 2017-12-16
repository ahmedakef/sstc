using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace sstc
{
    public static class helper{
        public static string SHA_1(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
        public static List<string> prevFilesTracked(){
            string[] all_text =  File.ReadAllLines(".sstc/branches/master/depend_on.txt");

            List<string> prev_files_tracked = new List<string>();
            foreach (string line in all_text)
            {
                prev_files_tracked.Add(line.Split("->")[0]);
            }
            return prev_files_tracked;
        }
        public static void write_all_text_commit_node(int commit_num,string traked_file){
            Commit commit = new Commit(commit_num);
            string file_text = File.ReadAllText(traked_file);
            commit.write_changed_commit_node(traked_file,file_text);
            Depend_on.add_file(traked_file,commit_num);
        }

        public static void write_changed_commit_node(int commit_num,int dependon_num,string traked_file){
            
            string dependon_path = ".sstc/branches/master/"+dependon_num+".txt";
            string changes = delta_from_commited(traked_file,dependon_num);
            if(changes == "THEY ARE SAME"){
                return;
            }
            Commit commit = new Commit(commit_num);
            commit.write_changed_commit_node(traked_file,changes);
        }
        public static string delta_from_commited(string file_name,int commit_num){
            Commit commit = new Commit(commit_num);
            string current_content = File.ReadAllText(file_name).Trim(' ','\n');
            string source_content = commit.get_file_content(file_name);
            string changes= delta(source_content,current_content);
            return changes;
        }

        public static string delta(string source,string dest){
            if(SHA_1(source)==SHA_1(dest)){
                return "THEY ARE SAME";
            }

            string[] source_lines = source.Split("\n");
            string[] dest_lines = dest.Split("\n");
            int source_lines_count = source_lines.Length;
            int dest_lines_count = dest_lines.Length;
            int still_same_from_start = 0;
            int still_same_from_last = 0;
            int min_lines = Math.Min(source_lines_count,dest_lines_count);
            int max_lines = Math.Max(source_lines_count,dest_lines_count);
            string changes = "";
            for (int i = 0; i < min_lines; i++)
            {
                if(source_lines[i]==dest_lines[i]){
                    still_same_from_start++;
                }else{
                    break;
                }
            }
            for (int i = source_lines.Length-1, j = dest_lines.Length-1 ; i >=0 && j>=0 ; i--,j--){
                if(source_lines[i]==dest_lines[j]){
                    still_same_from_last++;
                }else{
                    break;
                }
            }

                for (int i = still_same_from_start; i < max_lines -still_same_from_last ; i++)
                {
                    if(i<source_lines_count - still_same_from_last){
                        changes += "-/>"+i+"/>"+source_lines[i];
                    }
                    if(i<dest_lines_count - still_same_from_last){
                        changes += "+/>"+i+"/>"+dest_lines[i];
                    }
                    changes+="\n";
                }
            


            return changes;
        }

    }

 
}
