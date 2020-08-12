using System.Collections.Generic;
using System.Linq;
using WeChipCredit.Models;

namespace WeChipCredit.DAO
{
    public class FakeOffers
    {
        public static List<Offer> GetFakeOffers2()
        {
            List<Offer> _FakeBaseOffers = new List<Offer>();
            return _FakeBaseOffers;
        }
        public static List<Offer> GetFakeOffers()
        {
            List<Offer> _FakeBaseOffers = new List<Offer>();
            List<Product> _Products = Product.GetProducts();
            List<Client> _Clients = Client.GetFakeClients();
            List<Status> _Status = Status.GetStatus();
            List<DeliveryAddress> _FakeAddress = DeliveryAddress.GetFakeAdress();
            

            foreach (var c in _Clients)
            {
                var client = _Clients.Where(w => w.IdClient == c.IdClient).FirstOrDefault();

                Offer offer = new Offer();
                {
                    offer.IdOffer = _FakeBaseOffers.Count() + 1;
                    offer._Client = client;
                    offer.TotalOffer = 0;
                    offer._Products = new List<Product>();
                }
                _FakeBaseOffers.Add(offer);
            }
            /*FAKE 1 VENDA*/
            var offert = _FakeBaseOffers.Where(w => w._Client.IdClient == 1).FirstOrDefault();
            var status = _Status.Where(w => w.IdStatus == 3).FirstOrDefault();
            var adress = _FakeAddress.Where(w => w.IdAdress == 3).FirstOrDefault();
            offert._Client._Status = status;
            offert._Client._Address = adress;
            var product = _Products.Where(w => w.IdProduct == 2).FirstOrDefault();
            offert._Products.Add(product);

            /*FAKE 2 CAIU LIGAÇÃO*/
            offert = _FakeBaseOffers.Where(w => w._Client.IdClient == 2).FirstOrDefault();
            status = _Status.Where(w => w.IdStatus == 4).FirstOrDefault();
            offert._Client._Status = status;

            /*FAKE 3 VIAJOU*/
            offert = _FakeBaseOffers.Where(w => w._Client.IdClient == 3).FirstOrDefault();
            status = _Status.Where(w => w.IdStatus == 5).FirstOrDefault();
            offert._Client._Status = status;


            /*FAKE 4 FALECIDO*/
            offert = _FakeBaseOffers.Where(w => w._Client.IdClient == 5).FirstOrDefault();
            status = _Status.Where(w => w.IdStatus == 6).FirstOrDefault();
            offert._Client._Status = status;

            /*FAKE 5 NAO QUER CONTATO*/
            offert = _FakeBaseOffers.Where(w => w._Client.IdClient == 4).FirstOrDefault();
            status = _Status.Where(w => w.IdStatus == 2).FirstOrDefault();
            offert._Client._Status = status;

            /*FAKE 6 VENDA*/
            offert = _FakeBaseOffers.Where(w => w._Client.IdClient == 6).FirstOrDefault();
            status = _Status.Where(w => w.IdStatus == 3).FirstOrDefault();
            adress = _FakeAddress.Where(w => w.IdAdress == 1).FirstOrDefault();
            offert._Client._Status = status;
            offert._Client._Address = adress;
            product = _Products.Where(w => w.IdProduct == 9).FirstOrDefault();
            offert._Products.Add(product);
            product = _Products.Where(w => w.IdProduct == 8).FirstOrDefault();
            offert._Products.Add(product);
            product = _Products.Where(w => w.IdProduct == 6).FirstOrDefault();
            offert._Products.Add(product);
            product = _Products.Where(w => w.IdProduct == 2).FirstOrDefault();
            offert._Products.Add(product);
            product = _Products.Where(w => w.IdProduct == 5).FirstOrDefault();
            offert._Products.Add(product);

            return _FakeBaseOffers;

        }


    }
}