using System.Collections.Generic;
using System.Linq;

namespace WeChipCredit.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public sbyte Ddd { get; set; }
        public int Phone { get; set; }
        public float VlCredit { get; set; }
        //public int IdStatus { get; set; }
        //public Status _status
        //{
        //    get { return Status.GetStatus().Where(w => w.Id == IdStatus).FirstOrDefault(); }
        //    set { _status = Status.GetStatus().Where(w => w.Id == IdStatus).FirstOrDefault(); }
        //}

        public Status _Status { get; set; }

        public Client(int id, string name, string cpf, sbyte ddd, int phone, float vlCredit, Status _status)
        {
            Id = id;
            Name = name.ToUpper();
            Cpf = cpf;
            Ddd = ddd;
            Phone = phone;
            VlCredit = vlCredit;
            _Status = _status;
            //IdStatus = idStatus;
        }

        public Client()
        {
        }

        public static List<Client> GetFakeClients()
        {
            List<Status> _Status = Status.GetStatus();
            var status = _Status.Where(w => w.Id == 1).FirstOrDefault();
            
            List<Client> FakeClientList = new List<Client>
            {
                new Client(1, "Dayane Michalewicz", "40271589892", 48, 988421246, float.Parse("44,50"), status),
                new Client(2, "Fernanda Masp", "27004766039", 41, 978421246, float.Parse("66,50"), status),
                new Client(3, "Mauro Avelno", "00867531002", 47, 975821246, float.Parse("500,20"), status),
                new Client(4, "Andrade Almeida", "73138906052", 11, 999991246, float.Parse("320,50"), status),
                new Client(5, "Ben 10", "38078423063", 48, 988421246, float.Parse("120,50"), status),
                new Client(6, "Avantildo Lapolino", "83293461026", 48, 988421246, float.Parse("550,50"), status)
            };
            return FakeClientList;
        }
    }

}
