﻿
namespace Tellus.Domain.Models
{
    public class DbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}
