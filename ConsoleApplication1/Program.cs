using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElasticSearch;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            CElasticsearch el = new CElasticsearch();
            el.InitializeElasticsearch("http://localhost:9200");

            var a = el.GetSimilarWords("абдомен", 1);
            foreach(var i in a)
            {
                Console.WriteLine(i);
            }
        }
    }
}

