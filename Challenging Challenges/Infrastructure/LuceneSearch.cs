using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.WebPages;
using Challenging_Challenges.Models.Entities;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace Challenging_Challenges.Infrastructure
{
    public static class LuceneSearch
    {
        private static readonly string LuceneDir = HostingEnvironment.MapPath("~/LuceneIndex");
        private static FSDirectory _directory;
        private static FSDirectory Directory
        {
            get
            {
                if (_directory == null) _directory = FSDirectory.Open(new DirectoryInfo(LuceneDir));
                if (IndexWriter.IsLocked(_directory)) IndexWriter.Unlock(_directory);
                try
                {
                    _directory.ClearLock("write.lock");
                }
                catch
                {
                    // ignored
                }
                return _directory;
            }
        }

        public static void AddUpdateLuceneIndex(IEnumerable<SearchIndex> searchIndexes)
        {
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var searchIndex in searchIndexes) _addToLuceneIndex(searchIndex, writer);

                analyzer.Close();
                writer.Dispose();
            }
        }

        public static void AddUpdateLuceneIndex(SearchIndex searchIndex)
        {
            AddUpdateLuceneIndex(new List<SearchIndex> { searchIndex });
        }

        public static void ClearLuceneIndexRecord(string recordId)
        {
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var searchQuery = new TermQuery(new Term("Id", recordId.ToString()));
                writer.DeleteDocuments(searchQuery);

                analyzer.Close();
                writer.Dispose();
            }
        }

        public static bool ClearLuceneIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                using (var writer = new IndexWriter(Directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.DeleteAll();

                    analyzer.Close();
                    writer.Dispose();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static void Optimize()
        {
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }
        }

        public static IEnumerable<SearchIndex> Search(Sort sort, string input = "", string fieldName = "", int page = 0, int hitsLimit = 10)
        {
            var terms = input.Trim().Replace("-", " ").Replace(",", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim().TrimEnd('s','\'') + "*");
            input = string.Join(" ", terms);

            return _search(sort, input, fieldName, page, hitsLimit);
        }

        public static void UpdateIndex(SearchIndex searchIndex)
        {
            var writer = new IndexWriter(Directory,
                new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30),
                false,
                IndexWriter.MaxFieldLength.LIMITED);
            var document = _getDocument(searchIndex);
            writer.UpdateDocument(new Term("Id", document.Get("Id")), document);
            writer.Dispose();
        }

        public static IEnumerable<SearchIndex> GetIndexRecords(int page = 0, int hitsLimit = 1000)
        {
            if (!System.IO.Directory.EnumerateFiles(LuceneDir).Any()) return new List<SearchIndex>();

            var searcher = new IndexSearcher(Directory, true);
            var reader = IndexReader.Open(Directory, true);
            var docs = new List<Document>();
            var term = reader.TermDocs();
            if (page > 0) term.SkipTo((page * hitsLimit) - 1);
            for (int i = 0; i < hitsLimit; i++)
            {
                if (!term.Next()) break;
                docs.Add(searcher.Doc(term.Doc));
            }
            reader.Dispose();
            searcher.Dispose();
            return _mapLuceneToDataList(docs);
        }

        private static IEnumerable<SearchIndex> _search(Sort sort, string searchQuery, string searchField, int page, int hitsLimit)
        {
            if (IndexReader.IndexExists(Directory))
                using (var searcher = new IndexSearcher(Directory, true))
                {
                    var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                    IList<Document> luceneDocuments = new List<Document>();
                    int hitsUpperRange = (page + 1) * hitsLimit;
                    QueryParser parser;
                    if (!string.IsNullOrEmpty(searchField))
                    {
                        parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, searchField, analyzer);
                    }
                    else
                    {
                        parser = new MultiFieldQueryParser
                            (Lucene.Net.Util.Version.LUCENE_30, new[] { "Id", "AuthorId", "Title", "PreviewText", "Condition" ,
                                "Difficulty", "Section", "Language", "Rating", "Tags", "TimesSolved" }, analyzer);
                    }
                    Query query = searchQuery.IsEmpty() ? new MatchAllDocsQuery() : ParseQuery(searchQuery, parser);
                    TopDocs results = searcher.Search(query, null, hitsUpperRange, sort);
                    ScoreDoc[] scoreDocs = results.ScoreDocs;
                    for (int i = page * hitsLimit; i < results.TotalHits && i < hitsUpperRange; i++)
                    {
                        luceneDocuments.Add(searcher.Doc(scoreDocs[i].Doc));
                    }
                    analyzer.Close();
                    searcher.Dispose();
                    return _mapLuceneToDataList(luceneDocuments);
                }
            return null;
        }

        private static void _addToLuceneIndex(SearchIndex searchIndex, IndexWriter writer)
        {
            var document = _getDocument(searchIndex);
            writer.UpdateDocument(new Term("Id", document.Get("Id")), document);
        }

        private static Document _getDocument(SearchIndex searchIndex)
        {
            var doc = new Document();

            doc.Add(new Field("Id", searchIndex.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("AuthorId", searchIndex.AuthorId, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Title", searchIndex.Title, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("PreviewText", searchIndex.PreviewText, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Condition", searchIndex.Condition, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Difficulty", searchIndex.Difficulty.ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Section", searchIndex.Section, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Language", searchIndex.Language, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("TimesSolved", searchIndex.TimesSolved.ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Rating", searchIndex.Rating.ToString("#"), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Tags", searchIndex.Tags, Field.Store.YES, Field.Index.ANALYZED));

            return doc;
        }

        private static SearchIndex _mapLuceneDocumentToData(Document doc)
        {
            return new SearchIndex
            {
                Id = Guid.Parse(doc.Get("Id")),
                AuthorId = doc.Get("AuthorId"),
                Title = doc.Get("Title"),
                PreviewText = doc.Get("PreviewText"),
                Condition = doc.Get("Condition"),
                Difficulty = Convert.ToByte(doc.Get("Difficulty")),
                Section = doc.Get("Section"),
                Language = doc.Get("Language"),
                Rating = Convert.ToByte(doc.Get("Rating")),
                TimesSolved = Convert.ToInt32(doc.Get("TimesSolved")),
                Tags = doc.Get("Tags")
            };
        }

        private static IEnumerable<SearchIndex> _mapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(_mapLuceneDocumentToData).ToList();
        }

        private static Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }
    }
}
