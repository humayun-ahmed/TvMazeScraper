using System;
using Rtl.TvMaze.Scraper.Repository.Entity.Base;

namespace Rtl.TvMaze.Scraper.Repository.Entity
{
    public class Cast : EntityBase
    {
        public Cast(string name, DateTime? birthday, int showId)
        {
            Name = name;
            Birthday = birthday;
            ShowId = showId;
        }

        public string Name { get; set; }
        public DateTime? Birthday { get; set; }

        public virtual int ShowId { get; set; }
        public virtual Show Show { get; set; }
    }
}
