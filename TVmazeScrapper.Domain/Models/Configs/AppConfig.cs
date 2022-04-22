using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVmazeScrapper.Domain.Models.Configs
{
    public record AppConfig
    {
        public string DbConnStr { get; init; }

        public bool UseMigration { get; init; }

    }
}
