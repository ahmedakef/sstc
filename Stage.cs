using System;
using System.IO;


namespace sstc
{

    public static class Stage{
        
        private static string Path = ".sstc/branches/master/tracked.txt" ;

        
        public static void add_files(string[] added_files){

            string[] tracked_files = get_traked_files();

            using (StreamWriter sw = new StreamWriter(Path,true))
            {
                foreach (string file in added_files)
                {
                    if(Array.IndexOf(tracked_files,file)==-1){
                        sw.WriteLine(file);
                        Console.WriteLine(file+" added to the stage successfully");
                    }
                }
            }
        }
        public static string[] get_traked_files(){
            return File.ReadAllLines(Path);;
        }
        public static void clear(){
            File.WriteAllText(Path,""); // remove from stageing area
        }
        

    }
 
}

