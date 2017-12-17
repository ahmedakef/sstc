using System;
using System.IO;


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
                        changes += "-/>"+i+"/>"+source_lines[i]+"\n";
                    }
                    if(i<dest_lines_count - still_same_from_last){
                        changes += "+/>"+i+"/>"+dest_lines[i]+"\n";
                    }
                }
            


            return changes;
        }

    }

 
}
