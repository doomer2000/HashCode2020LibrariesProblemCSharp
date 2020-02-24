using Hash.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Hash
{
    class Program
    {
        static void Main(string[] args)
        {
            string curFileName = "e_so_many_books";
            Data mainData = ReadData($"../../../Input/{curFileName}.txt");
            List<Day> days = new List<Day>();
            mainData.Books = mainData.Books.OrderByDescending(b => b.Score).ToList();
            Console.WriteLine("Main books list ordered by score");
            mainData.Libraries = mainData.Libraries.OrderBy(l => l.InitTime).ToList();
            Console.WriteLine("Libraries list ordered by init time");
            foreach (var lib in mainData.Libraries)
            {
                lib.Books.OrderByDescending(b => b.Score);
            }
            Console.WriteLine("Books list in libraries ordered by score");
            long resultScore = 0;
            int curLibId = 0;
            Console.WriteLine("Scaning started +");
            for (int i = curLibId; i <= mainData.Libraries.Count(); curLibId++)
            {
                Library lib = mainData.Libraries[curLibId];
                mainData.Days -= mainData.Libraries[curLibId].InitTime;
                Console.WriteLine($"{mainData.Days} days left ");
                int bookCanScan = mainData.Days * lib.BooksPerDay;
                days.Add(new Day()
                {
                    Library = lib
                });
                for (int j = 0; days.Last().Books.Count() < bookCanScan && j < lib.Books.Count(); j++)
                {
                    Book curBook = lib.Books[j];
                    if (mainData.Books.Contains(curBook))
                    {
                        days.Last().Books.Add(curBook);
                        resultScore += curBook.Score;
                        mainData.Books.Remove(mainData.Books.Where(x => x.Id == j).FirstOrDefault());
                    }
                }
                if(days.Count() >= mainData.Libraries.Count())
                {
                    Console.WriteLine("Scaning ended +_+");
                    break;
                }
                if(mainData.Days <= mainData.Libraries[curLibId+1].InitTime)
                {
                    break;
                }
            }
            using (FileStream f = File.Create($"../../../Input/{curFileName}_out.txt"))
            {
                f.Close();
            }

            using (StreamWriter writer = new StreamWriter($"../../../Input/{curFileName}_out.txt"))
            {
                writer.WriteLine(days.Count().ToString());
                foreach (Day d in days)
                {
                    writer.WriteLine($"{d.Library.Id} {d.Books.Count()}");
                    foreach (Book b in d.Books)
                    {
                        writer.Write($"{b.Id} ");
                    }
                    writer.WriteLine();
                }
                writer.Write($"//{resultScore}");
                writer.Close();
            }
        }

        static Data ReadData(string filePath)
        {
            string data = File.ReadAllText(filePath);
            var datas = data.Split('\n');
            Data dataFromFile = new Data();
            var arraysLength = datas[0].Split(' ');
            dataFromFile.Books = new List<Book>();
            dataFromFile.Libraries = new List<Library>();
            dataFromFile.Days = Int32.Parse(arraysLength[2]);
            var bookScores = datas[1].Split(' ');
            for(int i = 0;i< Int32.Parse(arraysLength[0]); i++)
            {
                dataFromFile.Books.Add(new Book() { Id = i, Score = Int32.Parse(bookScores[i]) });
            }
            Console.WriteLine($"Books readed from file {dataFromFile.Books.Count()}");
            int libCount = (datas.Length - 3);
            int curLib = 0;
            for (int i = 2;i <= libCount;i+=2)
            {
                var libInfo = datas[i].Split(' ');
                var libBooks = datas[i + 1].Split(' ');
                dataFromFile.Libraries.Add(new Library()
                {
                    Id = curLib,
                    Books = new List<Book>(),
                    InitTime = Int32.Parse(libInfo[1]),
                    BooksPerDay = Int32.Parse(libInfo[2])
                });
                curLib++;
                for(int k = 0; k<libBooks.Count() ;k++)
                {
                    dataFromFile.Libraries.Last().Books.Add(null);
                }
                for(int j = 0; j < libBooks.Length;j++)
                {
                    dataFromFile.Libraries.Last().Books[j] = dataFromFile.Books.Where(x => x.Id == Int32.Parse(libBooks[j])).First();
                }
            }
            Console.WriteLine($"Libs readed from file {dataFromFile.Libraries.Count()}");
            return dataFromFile;
        }
    }
}