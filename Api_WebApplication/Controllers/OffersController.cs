using Api_WebApplication.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WeChipCredit.DAO;
using WeChipCredit.Models;

namespace Api_WebApplication.Controllers
{
    public class OffersController : ApiController
    {
        private List<Offer> Oferts = FakeOffers.GetFakeOffers();
        private OfferResult Results = new OfferResult();

        // GET: api/Offers
        public OfferResult Get()
        {
            try
            {
                if (Oferts.Count > 0)
                {
                    Results.Mensage = "Ok";
                    Results.Offers = Oferts;
                }
                else
                {
                    Results.Mensage = "Ofertas não localizadas";
                }
                return Results;
            }
            catch
            {
                Results.Mensage = "Ofertas não localizadas";
                return Results;
            }
        }

        //GET: api/Offers?name=da
        public OfferResult Get(string name)
        {
            try
            {
                var ofertsResult = Oferts.Where(w => w._Client.Name.Contains(name.ToUpper())).ToList();

                if (ofertsResult.Count > 0)
                {
                    Results.Mensage = "Ok";
                    Results.Offers = ofertsResult;
                }
                else
                {
                    Results.Mensage = "Ofertas não localizadas";
                }
                return Results;
            }
            catch
            {
                Results.Mensage = "Ofertas não localizadas";
                return Results;
            }

        }

        //GET: /api/Offers?cpf=402
        public OfferResult Get(long cpf)
        {
            try
            {

                var ofertsResult = Oferts.Where(w => w._Client.Cpf.Contains(cpf.ToString())).ToList();

                if (ofertsResult.Count > 0)
                {
                    Results.Mensage = "Ok";
                    Results.Offers = ofertsResult;
                }
                else
                {
                    Results.Mensage = "Ofertas não localizadas";
                }
                return Results;

            }
            catch
            {
                Results.Mensage = "Ofertas não localizadas";
                return Results;
            }

        }

    }
}
