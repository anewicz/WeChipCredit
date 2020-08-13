using System;
using System.Collections.Generic;

namespace WeChipCredit.Models
{
    public class DeliveryAddress 
    {
        public int IdAdress { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public DeliveryAddress()
        {
        }

        public DeliveryAddress(int id, string zipCode, string street, string number, string complement, string neighborhood, string city, string state)
        {
            IdAdress = id;
            ZipCode = zipCode;
            Street = street;
            Number = number;
            Complement = complement;
            Neighborhood = neighborhood;
            City = city;
            State = state;
        }

        public static List<DeliveryAddress> GetFakeAdress()
        {

            List<DeliveryAddress> FakeAdress = new List<DeliveryAddress>
            {
                new DeliveryAddress(1, "88070650", "Rua Infinita", "50A", "Casa Fundos, divisa da BR", "Estreito", "Florianopolis", "Santa Catarina"),
                new DeliveryAddress(2, "88095620", "Trav do Trabalhador", "44", "Ap 305", "Tude Bastos", "Praia Grande", "São Paulo"),
                new DeliveryAddress(3, "88080750", "Rua do Arco Iris", "S/N", "" , "Tude Bastos", "Praia Grande", "São Paulo")
            };
            return FakeAdress;
        }



    }

}
