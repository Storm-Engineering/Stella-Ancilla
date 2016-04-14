using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
/*Project of Storm Engineering for creating forerunner like Ancillas (Artificial intelligences 
By Peter Hakwe*/
namespace Ancilla___Stella
{
    class Program
    {
        static String book = "";
        static JObject dictionary = null;
        static Dictionary<string, string[]> thesaurus = null;
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            randomWrite("Hi, my name is Stella.|Hello, I am Stella|Greetings...|I am Stella");
            write("I am a forerunner ancilla or is what you humans\ncall an AI (artificial intelligence).");
            write("I am currently classified as a level 1 ancilla.");
            write("Please wait a moment while I startup.");
            System.Threading.Thread.Sleep(2000);
            load();
            while (true)
            {
                String text = Console.ReadLine();
                parseText(text);
                Console.Write(">");
            }
        }
        private static void load()
        {
            progressBar(0);
            book = File.ReadAllText("Resources/book.txt");//.Replace('\n',' ');
            progressBar(30);
            string json = File.ReadAllText(@"Resources\dictionary.json");
            dictionary = (JObject)JsonConvert.DeserializeObject(json);
            progressBar(40);
            String t = File.ReadAllText(@"Resources\mthesaur.txt");
            thesaurus = new Dictionary<string, string[]>();
            int i = 0;
            int count = t.Split('\n').Length;
            foreach (string root in t.Split('\n'))
            {
                if(i%500==0)
                progressBar((int)((double)i / count * 100));
                try
                {
                    thesaurus.Add(root.Split(',')[0], root.Replace(root.Split(',')[0], "").Split(','));
                }
                catch (Exception)
                {
                    string[] temp = new string[1000];
                    string[] something = root.Replace(root.Split(',')[0], "").Split(',');
                    something.CopyTo(temp, 0);
                    thesaurus[root.Split(',')[0]].CopyTo(temp, something.Length);
                    thesaurus[root.Split(',')[0]] = temp;
                }
                i += 1;

            }
            Console.Clear();
            progressBar(100);
            Console.WriteLine("Complete!");
            Console.Write("\nI am now ready for service...\n\n>");
        }
        private static void progressBar(int percentage)
        {
            Console.Clear();
            Console.WriteLine("Reading information databases...");
            string final = "[";
            int number = percentage / 5;
            for (int i = 0; i < number; i++)
            {
                final += "-";
            }
            for (int i = number; i < 20; i++)
            {
                final += " ";
            }
            final += "]\n";
            Console.Write(final);
        }
        private static void parseText(String text)
        {
            string t = grab(@"(w\w+)\s.+", text);
            switch (t)
            {
                case "what":
                    what(text);
                    break;
                case "where":
                    where(text);
                    break;
                case "when":
                    when(text);
                    break;
                case "why":
                    why(text);
                    break;
                case "who":
                    who(text);
                    break;
                default:
                    randomWrite("I am not able to respond to that question.|I cannot answer that question.|It seems that is out of my ability to answer that.");
                    break;
            }
        }

        private static void why(string text)
        {
            throw new NotImplementedException();
        }

        private static void when(string text)
        {
            throw new NotImplementedException();
        }
        private static void where(string s)
        {
            final = s.Replace("where ", "").Replace("is that ", "").Replace("is ", "").Replace("is the ", "").Replace("was the ","");
            string answer = grab("(" + final + @"(.{5,50})\sis\sin(\.|\n))", book);
            if (answer != "")
            {
                write("What I found:");
                write(answer);
            }
            else if (!getDefinition(final).Contains("Could not"))
            {
                write("This is what is the dictionary says:");
                write(getDefinition(final));
            }
            else
                checkDatabases();
        }
        private static void who(string s)
        {
            final = s.Replace("who ", "").Replace("is that ", "").Replace("is ", "").Replace("was the ","");
            string answer  = grab("("+final+@"(.{5,50})(\.|\n))",book);
            if (answer != "")
            {
                write("What I found:");
                write(answer);
            }
            else
                checkDatabases();

        }
        static string final = "";
        private static void what(string s)
        {
            string word = grab(@"what is another word for (\w+)",s);
            if (word!="")
            {                
                if (thesaurus.ContainsKey(word))
                {
                    write("Words similar to " + word + ":");
                    foreach (var item in thesaurus[word])
                    {
                        Console.Write(item + ",");
                    }
                    write("");
                }
                else
                {
                    randomWrite("Sorry I was unable to find anything.|Unable to find anything.|Couldn't find anything.|Sorry I couldn't find anything.");
                }
                            
                return;
            }
            else
            {
                word = grab(@"are synonyms for (\w+)",s);
                if (word != "")
                {
                    write("Words similar to " + word + ":" + thesaurus[word][1]);
                    return;
                }
            }
            final = s.Replace("what ", "").Replace("is a ", "").Replace("is ", "").Replace("was the ","").Replace("was ","");
            string answer = grab(@"(" + final + @" is a .+)", book);
            if (answer != "")
            {
                write("What I found:");
                write(answer);
            }
            else
            {
                if (checkDefinition() == "")
                {
                    checkDatabases();
                }
            }
        }
        private static string getDefinition(string word)
        {
            try
            {
                return "Definition:  "+dictionary[word.ToUpper()].ToString(); 
            }
            catch (Exception)
            {

                return "Could not find in dictionary";
            }
            
        }
        private static string checkDefinition()
        {
            write("Checking dictionary....\n");
            string definition = getDefinition(final);
            if (definition.Contains(":"))
            {
                write(definition);
                return definition;
            }
            else
                return "";
        }
        /// <summary>
        /// Grabs a regex match from a string
        /// </summary>
        /// <param name="r">Regex as a string</param>
        /// <param name="text">Text to look through</param>
        /// <returns></returns>
        public static string grab(string r, string text)
        {
            Regex re = new Regex(r, RegexOptions.IgnoreCase);
            Match m = re.Match(text);
            if (m.Success)
            {
                return m.Groups[1].ToString();
            }
            else
            {
                return "";

            }
        }
        private static void checkDatabases()
        {
            randomWrite("I could not find the answer.|It seems that is not in my informational databases.|I am sorry I seemed to have failed to find " +
                    "the information");
            write("\nI am now searching all availible databases and giving you all responses...\n\n");
            int i = 1;
            write("Here is what I found:");
            foreach (string s in grabMulti(@"(.{0,200}" + final + @".*?(\.|\n))", book))
            {

                if (s != null)
                {
                    Console.Write(i.ToString() + ":");
                    write(s);
                    i += 1;
                }


            }
        }
        public static string[] grabMulti(string r, string text)
        {
            Regex re = new Regex(r, RegexOptions.IgnoreCase);
            MatchCollection m = re.Matches(text);
            if (m.Count > 0 && m[0].Success)
            {
                string[] groups = new string[50];
                for (var i = 0; i < m.Count; i++)
                {
                    if (i > 49)
                    {
                        write("Matches exceeded limit.");
                        break;
                    }
                    groups[i] = m[i].Groups[1].ToString();

                }
                return groups;
            }
            else
            {
                return new string[] { "Nothing." };
            }
        }
        static void write(string s)
        {
            Console.WriteLine(s);
        }
        static void randomWrite(string s)
        {
            string[] st = s.Split('|');
            Random r = new Random();
            int ran = r.Next(0, st.Length);
            write(st[ran]);
        }
        static void gatherData()
        {

        }
    }
}

