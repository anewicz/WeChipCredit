using System;
using System.Collections.Generic;

namespace WeChipCredit.Models
{
    public class Status
    {
        public int IdStatus { get; set; }
        public string NmStatus { get; set; }
        public bool IsFinalizingClient { get; set; }
        public bool IsSale { get; set; }
        public int CodStatus { get; set; }

        public Status(int id, string nmStatus, bool isFinalizingClient, bool isSale, int codStatus)
        {
            IdStatus = id;
            NmStatus = nmStatus;
            IsFinalizingClient = isFinalizingClient;
            IsSale = isSale;
            CodStatus = codStatus;
        }

        public Status()
        {
        }

        public static List<Status> GetStatus()
        {
            List<Status> StatusList = new List<Status> {
                new Status(1, "Nome Livre", false, false, 1),
                new Status(2, "Não deseja ser contatado", true, false, 7),
                new Status(3, "Cliente Aceitou Oferta", true, true, 9),
                new Status(4, "Caiu a ligação", false, false, 15),
                new Status(5, "Viajou", false, false, 19),
                new Status(6, "Falecido", true, false, 21)
            };
            return StatusList;
        }


    }

}
