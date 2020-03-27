using System;
using System.IO;
using System.Security.Cryptography;

namespace Lesson_2
{
    class LS2
    {
        string logs_file;
        string db_file;
        public static SHA256 hash(string password)   // яблоко
        {
            string salt = "uhuhaklcbsdkvblfvve";
            password = salt + password;
            return SHA256.Create(password);
        }
        public LS2(string filename)
        {
            logs_file = @"C:\Users\Геннадий\Desktop\Логи изменений.txt";
            db_file = filename;
        }
        public bool Authorization (string login,string password)
        {
            
            StreamReader reader = new StreamReader(db_file);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var login_and_password = line.Split(new char[] { ' ' });
                if (login_and_password[0] == login && SHA256.Create (login_and_password[1]) == hash( password))
                {
                    File.AppendAllText(logs_file, DateTime.Now + "\tВошёл пользователь >> " + login+"\n"); 
                    return true; 
                } 
            }
            return false;
        }
        public static int Incorrect_value_mini(string value)
        {
            while (true)
            {
                try
                {
                    Convert.ToInt32(value);
                    break;
                }
                catch
                {
                    Console.WriteLine("Данные введены некорректно, введите снова");
                    value = Console.ReadLine();
                }
            }
            int number = Convert.ToInt32(value);
            return (number);
        }
        public static string Get_Files(string file, string param)
        {
            string logs_file = @"C:\Users\Геннадий\Desktop\Логи изменений.txt";
            StreamReader reader = new StreamReader(file);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length > 1)
                {
                    if (!(line[0] == '#' | line[0] == ';'))
                    {
                        var str = line.Split(new char[] { '#', ';' }, 2, StringSplitOptions.RemoveEmptyEntries);
                        str[0] = str[0].Replace("\t", " ");
                        str = str[0].Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);

                        if (str[0] == param)
                        {
                            try
                            {
                                int param_size = param.Length + 1;
                                reader.Close();
                                string value_param= line.Substring(param_size, line.Length - param_size);
                                File.AppendAllText(logs_file, DateTime.Now + "\tПользователь просмотрел значение параметра >> " + param + " : " + value_param+"\n");
                                return param + " : " + value_param;
                            }
                            catch { return param + " : " + "нет значения"; }
                        }
                    }
                }
            }
            return ">>параметр " + param + " не найден.";
        }
        public static string Set_Files(string file, string new_param)
        {
            string logs_file = @"C:\Users\Геннадий\Desktop\Логи изменений.txt";
            bool Check = false;
            new_param = new_param.Replace("\t", " ");
            var new_param_and_new_comment = new_param.Split(new char[] { '#', ';' }, 2, StringSplitOptions.RemoveEmptyEntries);
            new_param = new_param_and_new_comment[0];
            string new_comment = "";
            if (new_param_and_new_comment.Length > 1)
            {
                new_comment = new_param_and_new_comment[1];
            }
            new_param = new_param.Replace("\t", " ");
            var new_param_key = new_param.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] LinesFile = File.ReadAllLines(file);
            using (StreamWriter writer = new StreamWriter(@"C:\Users\Геннадий\Desktop\conf_use.txt"))
            {
                foreach (var line in LinesFile)
                {
                    if (line.Length > 1)
                    {
                        if (!(line[0] == '#' | line[0] == ';'))
                        {
                            string old_comment = "";
                            var old_param_and_old_comment = line.Split(new char[] { '#', ';' }, 2, StringSplitOptions.RemoveEmptyEntries);
                            if (old_param_and_old_comment.Length > 1)
                            {
                                old_comment = old_param_and_old_comment[1];
                            }
                            string old_param = old_param_and_old_comment[0].Replace("\t", " ");
                            var old_param_key = old_param.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                if (new_param_key[0] == old_param_key[0])
                                {
                                    string result = "";
                                    if (old_param_key.Length == 1)
                                    {
                                        for (int i = 1; i < new_param_key.Length; i++)
                                        {
                                            result += new_param_key[i] + " ";
                                        }
                                        File.AppendAllText(logs_file, DateTime.Now + "\tПользователь изменил параметр " + old_param_key[0] + " на " + result+"\n");
                                    }
                                    if (old_param_key.Length > 1)
                                    {
                                        foreach (var value in new_param_key)
                                        {
                                            result += value + " ";
                                        }
                                    File.AppendAllText(logs_file, DateTime.Now + "\tПользователь изменил значение параметра " + old_param_key[0] + " на " + result+"\n");
                                    }
                                    if (old_comment.Length > 0)
                                    {
                                        result += " #" + old_comment;
                                    }
                                    if (new_comment.Length > 0)
                                    {
                                        result += " ;" + new_comment;
                                    }
                                    Check = true;
                                    writer.WriteLine(result);
                                }
                                else { writer.WriteLine(line); }
                            
                        }
                        else { writer.WriteLine(line); }
                    }
                    else { writer.WriteLine(line); }
                }
            }
            if (Check == true) { return "Параметр " + new_param_key[0] + " изменён"; }
            else { return "Параметр " + new_param_key[0] + " не найден"; }
        }
    }
}
