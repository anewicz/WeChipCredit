using System.Collections.Generic;

namespace WeChipCredit.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string NmProduct { get; set; }
        public float VlPrice { get; set; }
        public string TpProduct { get; set; }
        public int CodProduct { get; set; }


        public Product(int id, string nmProduct, float vlPrice, string tpProduct, int codProduct)
        {
            Id = id;
            NmProduct = nmProduct;
            VlPrice = vlPrice;
            TpProduct = tpProduct;
            CodProduct = codProduct;
        }

        public Product()
        {
        }

        public static List<Product> GetProducts()
        {
            List<Product> ProductsList = new List<Product>
            {
                new Product(1,"Mouse", float.Parse("20,00"), "HARDWARE", 15),
                new Product(2,"Teclado", float.Parse("30,00"), "HARDWARE", 106),
                new Product(3,"Monitor 17’", float.Parse("350,00"), "HARDWARE", 200),
                new Product(4,"Pen Drive 8 GB", float.Parse("30,00"), "HARDWARE", 211),
                new Product(5,"Pen Drive 16 GB", float.Parse("50,00"), "HARDWARE", 314),
                new Product(6,"AVAST", float.Parse("199,99"), "SOFTWARE", 459),
                new Product(7,"Pacote Office", float.Parse("499,00"), "SOFTWARE", 1104),
                new Product(8,"Spotify (3 meses)", float.Parse("45,50"), "SOFTWARE", 1108),
                new Product(9,"Netflix (1 mês)", float.Parse("19,90"), "SOFTWARE", 1107)
            };
            return ProductsList;
        }
    } 

}