using fortest.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fortest.Models
{
    public class AllViewModel
    {
        public List<Explore> ExplorModels { get; set; }
        public List<Article> ArticleModels { get; set; }
        public List<ClientsReview> ReviewModels { get; set; }
        public List<HowWork> HowWorkModels { get; set; }
    }
}
