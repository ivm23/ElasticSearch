using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElasticSearch;
using System.Collections.Generic;

namespace UnitTestElasticSearch
{
    [TestClass]
    public class ElasticsearchTests
    {
        [TestMethod]
        public void AddNewElementToIndex()
        {
            CElasticsearch elasticSearch = new CElasticsearch();

            elasticSearch.InitializeElasticsearch("http://localhost:9200");

            Int64 count = elasticSearch.IndexLength() + 1;
            elasticSearch.AddToIndex("8785");
            System.Threading.Thread.Sleep(1000);
            Int64 newCount = elasticSearch.IndexLength();


            Assert.AreEqual(count, newCount);
        }

        [TestMethod]
        public void AddNewElementsToIndex()
        {
            CElasticsearch elasticSearch = new CElasticsearch();

            elasticSearch.InitializeElasticsearch("http://localhost:9200");
            IList<String> newElements = new List<String>() { "123", "145", "12345" };

            Int64 count = elasticSearch.IndexLength() + newElements.Count;

            elasticSearch.AddToIndex(newElements);
            System.Threading.Thread.Sleep(1000);
            Int64 newCount = elasticSearch.IndexLength();

            Assert.AreEqual(count, newCount);
        }

        [TestMethod]
        public void FuzzySearch()
        {
            CElasticsearch elasticSearch = new CElasticsearch();

            elasticSearch.InitializeElasticsearch("http://localhost:9200");

            IList<String> similarWords = new List<String>()
            {
                "амба", "арба", "ара", "абаз", "ага", "раба", "абак"
            };

            IEnumerable<String> findWords = elasticSearch.GetSimilarWords("аба", 1);
            Boolean isWrong = false;
            foreach (String fWord in findWords)
            { 
                if (similarWords.Contains(fWord))
                    similarWords.Remove(fWord);
                else
                    isWrong = true;
            }
            Assert.IsFalse(isWrong);
        }
    }
}
