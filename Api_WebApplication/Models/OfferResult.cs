using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChipCredit.Models;

namespace Api_WebApplication.Models
{
    public class OfferResult
    {
        public string Mensage; 
        public List<Offer> Offers { get; set; }
        

    }
}