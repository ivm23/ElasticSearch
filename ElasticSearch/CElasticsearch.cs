using System;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch
{
    public class CElasticsearch
    {
        private ElasticClient _client;
        private const String IndexName = "formid";
        private const String TypeName = "resp";
        private const String FieldName = "str";

        public void InitializeElasticsearch(String uri)
        {
            InitializeClient(uri);
            InitializeIndex();
        }

        private void InitializeIndex()
        {
            if (!_client.IndexExists(IndexName).Exists)
            {
                _client.CreateIndex(IndexName);
            }
        }

        private void InitializeClient(String uri)
        {
            if (String.IsNullOrEmpty(uri))
                throw new ArgumentNullException(nameof(uri));

            var node = new Uri(uri);
            var settings = new ConnectionSettings().DefaultIndex(IndexName);
            _client = new ElasticClient(settings);
        }

        public void AddToIndex(String productName)
        {
            if (String.IsNullOrEmpty(productName))
                throw new ArgumentNullException(nameof(productName));

            var jsonForAdd = new { str = productName };
            IIndexResponse indexResponse = _client.Index(jsonForAdd, i => i
                .Index(IndexName)
                .Type(TypeName)
                .Id(Guid.NewGuid())
                .Refresh(null)
            );

            if (!indexResponse.IsValid)
                throw indexResponse.OriginalException;
        }

        public void AddToIndex(IEnumerable<String> productsNamesList)
        {
            foreach (String productName in productsNamesList)
            {
                AddToIndex(productName);
            }
        }

        public long IndexLength() => _client.Count<Object>(p => p.Index(IndexName).Type(TypeName)).Count;

        private readonly char[] symbolsForDel = new char[] { '}', '{', ' ', '\n' , '\r', '\"'};
        private String GetWord(Object obj) => (((obj.ToString()).Split(':')[1]).Trim(symbolsForDel));     

        public IEnumerable<String> GetSimilarWords(String word, Int32 editDistance)
        {
            if (String.IsNullOrEmpty(word))
                throw new ArgumentNullException(nameof(word));

            ISearchResponse<Object> findDocuments = _client.Search<Object>(s => s.Index(IndexName).Type(TypeName).Query(q => q.Fuzzy(c => c
                                .Fuzziness(Fuzziness.EditDistance(editDistance)).Value(word).Field(FieldName))));

            IList<String> similarWords = new List<String>();

            foreach (Object document in findDocuments.Documents)
            {
                String similarWord = GetWord(document);

                if (word.CompareTo(similarWord) != 0 && !similarWords.Contains(similarWord))
                    similarWords.Add(GetWord(document));
            }
            return similarWords;
        }

    }
}
