using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ReadAndWriteHTML
{
    class Program
    {
        static void Main(string[] args)
        {
            string line = string.Empty;
            string replaceText = string.Empty;
            var stringComplete = new List<string>();


            string[] allfiles = System.IO.Directory.GetFiles(@"D:\Colombia", "*.asp", SearchOption.AllDirectories);

            foreach (var file in allfiles)
            {
                FileInfo info = new FileInfo(file);
                Console.WriteLine("Procesando: " + info.FullName);

                ////Head------------------
                var fileName = info.FullName;

                replaceText = "<!-- #include virtual='/sitios-empresariales/colombia/pauta/gtm_head.asp' -->";
                replaceText += System.Environment.NewLine;
                //replaceText += "</head>";

                stringComplete = StringReplaceCompleteHead(fileName, "</head>", replaceText);

                if (stringComplete.Count > 0)
                {
                    ReplaceInFile(fileName, stringComplete[0].ToString(), stringComplete[1].ToString());
                }

                //Body------------------
                replaceText = "<!-- #include virtual='/sitios-empresariales/colombia/pauta/gtm_body.asp' -->";
                stringComplete = StringReplaceComplete(fileName, "<body", replaceText);

                if (stringComplete.Count > 0)
                {
                    ReplaceInFile(fileName, stringComplete[0].ToString(), stringComplete[1].ToString());
                }
            }
            Console.ReadKey();
        }

        static public void ReplaceInFile(string filePath, string searchText, string replaceText)
        {
            StreamReader reader = new StreamReader(filePath);
            string content = reader.ReadToEnd();
            reader.Close();

            content = Regex.Replace(content, searchText, replaceText);

            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(content);
            writer.Close();
        }


        static public List<string> StringReplaceComplete(string filePath, string searchText, string replaceText)
        {
            string replaceInLineBody = "";
            var IndexesList = new List<string>();

            // Create a StreamReader  
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                // Read line by line  
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.ToLower().Contains(searchText.ToLower()))
                    {
                        replaceInLineBody = line + System.Environment.NewLine + replaceText;
                        IndexesList.Add(line);
                        IndexesList.Add(replaceInLineBody);
                    }
                }
            }

            return IndexesList;
        }


        static public List<string> StringReplaceCompleteHead(string filePath, string searchText, string replaceText)
        {
            string replaceInLineBody = "";
            var IndexesList = new List<string>();

            // Create a StreamReader  
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                // Read line by line  
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.ToLower().Contains(searchText.ToLower()))
                    {
                        string NewValue = Regex.Replace(line.ToLower(), searchText.ToLower(), (replaceText.ToLower() + searchText));

                        replaceInLineBody = System.Environment.NewLine + NewValue + System.Environment.NewLine;
                        IndexesList.Add(line);
                        IndexesList.Add(replaceInLineBody);
                    }
                }
            }

            return IndexesList;
        }
        
    }
}
