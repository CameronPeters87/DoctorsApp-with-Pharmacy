using System.Collections.Generic;

namespace Sprint33.Models.CovidModels
{
    public class NewsViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
    }

    public class ArticleViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string UrlToImage { get; set; }
        public string PublishedAt { get; set; }
        public virtual Source Source { get; set; }
    }
}