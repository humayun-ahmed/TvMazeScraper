using System.Collections.Generic;
using Rtl.TvMaze.Scraper.Repository.Entity.Base;

namespace Rtl.TvMaze.Scraper.Repository.Entity
{
    public class Show : EntityBase
    {
        public Show(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public virtual List<Cast> Casts { get; set; }
    }
}
