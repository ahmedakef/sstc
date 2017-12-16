using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace sstc
{

    public static class Current{
        

        public static int commit{
            get{
                string prev_commit = File.ReadAllLines(".sstc/current.txt")[1].Split("->")[1];
                int current_commit = Convert.ToInt32(prev_commit)+1;
                return current_commit;
            }
            set{
                string current_content = "branch->master\n";
                current_content += "commit->"+value;
                File.WriteAllText(".sstc/current.txt",current_content);
            }
        }
        


    }
 
}

