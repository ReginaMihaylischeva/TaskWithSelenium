using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using TestTask.Properties;

namespace TestTask
{
    class Program
    {
        private static readonly Report reportData = new Report();
        private static readonly string dateFormat = "MM/dd/yyyy";
        private static readonly DateTime date = DateTime.UtcNow;
        private static int countFiles = 0;
        static void Main()
        {
            var customersQueueF31 = new Queue<string>();
            var customersQueueFBL5N = new Queue<string>();
            var customers = new Queue<string>();
           

            using (StreamReader fstream = new StreamReader("F31.txt"))
            {
                fstream.ReadLine();
                fstream.ReadLine();
                fstream.ReadLine();
                fstream.ReadLine();
                fstream.ReadLine();
                while (!fstream.EndOfStream)
                {
                    string textFromFile = fstream.ReadLine();
                    if (!textFromFile.Contains('-'))
                    {
                        customersQueueF31.Enqueue(textFromFile.Split(' ')[0].Substring(1));
                    }
                }
            }

            using (StreamReader fstream = new StreamReader("FBL5N.txt"))
            {
                while (!fstream.EndOfStream)
                {
                    string textFromFile = fstream.ReadLine();
                    string textSub = textFromFile.Substring(11);
                    customersQueueFBL5N.Enqueue(textSub);
                }
            }

            foreach (string customer in customersQueueF31)
            {
                foreach (string customerFBL5N in customersQueueFBL5N)
                {
                    if (customerFBL5N.Equals(customer))
                    {
                        customers.Enqueue(customer);
                    }
                }
            }

            using (StreamReader fstream = new StreamReader("F31.txt"))
            {
                fstream.ReadLine();
                fstream.ReadLine();
                fstream.ReadLine();
                fstream.ReadLine();
                fstream.ReadLine();

                using (StreamReader fstream1 = new StreamReader("FBL5N.txt"))
                {
                    foreach (var customer in customers)
                    {
                        while (!fstream.EndOfStream)
                        {
                            string textFromFile = fstream.ReadLine();
                            if (textFromFile.Contains(customer))
                            {
                                string[] textFromFileSplit = textFromFile.Split('|');
                                AddCreditLimit(new string[]{
                                  textFromFileSplit[1],
                                  textFromFileSplit[3],
                                  textFromFileSplit[4],
                                  FormatSum(textFromFileSplit[6]),
                                  FormatSum(textFromFileSplit[8])
                                });
                            }
                        }

                        fstream.BaseStream.Seek(0, SeekOrigin.Begin);
                        fstream.DiscardBufferedData();

                        while (!fstream1.EndOfStream)
                        {

                            string textFromFile1 = fstream1.ReadLine();
                            if (textFromFile1.Contains(customer))
                            {
                                fstream1.ReadLine();
                                fstream1.ReadLine();
                                fstream1.ReadLine();
                                fstream1.ReadLine();
                                fstream1.ReadLine();
                                fstream1.ReadLine();
                                textFromFile1 = fstream1.ReadLine();
                                while (!textFromFile1.Contains("**") && !textFromFile1.Contains("DocumentNo"))
                                {
                                    if (textFromFile1.Contains("*"))
                                    {
                                        AddReceivables(new string[]{
                                           $"Итого сумма дебиторской задолжности покупателя {customer}",
                                           FormatSum( textFromFile1.Split('|')[9]),
                                           textFromFile1.Split('|')[10],
                                           string.Empty,
                                        });
                                    }

                                    else if (!textFromFile1.Contains("------------------"))
                                    {
                                        string[] textFromFileSplit = textFromFile1.Split('|');
                                        AddReceivables(new string[]{
                                        textFromFileSplit[2],
                                        textFromFileSplit[3],
                                        textFromFileSplit[4],
                                        FormatTyp(textFromFileSplit[5]),
                                        textFromFileSplit[6],
                                        textFromFileSplit[7],
                                        FormatArrear(textFromFileSplit[8]),
                                        FormatSum(textFromFileSplit[9]),
                                        textFromFileSplit[10],
                                        textFromFileSplit[11],
                                        });
                                    }
                                    textFromFile1 = fstream1.ReadLine();
                                }                                                              
                            }
                        }

                        string html = Resources.template;
                        html = reportData.GetHtmlText("data") + html;
                        reportData.Date = date.ToString(dateFormat);
                        html = html.Replace("@date", date.ToString(dateFormat));
                        byte[] buffer = Encoding.Default.GetBytes(html);
                        File.WriteAllBytesAsync($"Customer{customer}.html", buffer);
                        countFiles++;

                        reportData.CreditLimit.Clear();
                        reportData.Receivables.Clear();

                        fstream1.BaseStream.Seek(0, SeekOrigin.Begin);
                        fstream1.DiscardBufferedData();
                    }
                }
            }

            Console.WriteLine($"Количество созданных файлов {countFiles}.");
            Thread.Sleep(100);
        }

        private static void AddCreditLimit(string[] mass)
        {
            reportData.CreditLimit.Add(mass);
        }

        private static void AddReceivables(string[] mass)
        {
            reportData.Receivables.Add(mass);
        }

        private static string FormatArrear(string arrear)
        {
            if (arrear.Contains('-'))
            {
                arrear = arrear.Remove(arrear.Length - 1);
                arrear = arrear.TrimStart();
                arrear = string.Format("-{0}", arrear);
            }

            return arrear;
        }

        private static string FormatTyp(string arrear)
        {
            if (arrear.Contains("D1"))
            {
                arrear = "реализация товара";
            }

            return arrear;
        }

        private static string FormatSum(string arrear)
        {
            return arrear.Replace('.', ' ');
        }
    }



    internal class Report
    {
        public Report()
        {
            CreditLimit = new List<string[]>();
            Receivables = new List<string[]>();
        }

        public string Date { get; set; }

        public List<string[]> CreditLimit { get; set; }

        public List<string[]> Receivables { get; set; }

        public string GetHtmlText(string varName)
        {
            return "<script>" + varName + "=" + JsonSerializer.Serialize(this) + "</script>";
        }
    }
}
