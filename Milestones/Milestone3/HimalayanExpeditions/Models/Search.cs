using cloudscribe.Pagination.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HimalayanExpeditions.Models
{
    public class Search
    {
        public Search()
        {
            ExpeditionList = new PagedResult<Expedition>();
        }
        public int? Year { get; set; }
        public int Count { get; set; }
        public string Peak { get; set; }
        public string Season { get; set; }
        public string TerminationReason { get; set; }
        public PagedResult<Expedition> ExpeditionList { get; set; }
        public bool Climbed { get; internal set; } //For if we decide to combine these searches
        public int? PageIndex { get; set; }
    }
}