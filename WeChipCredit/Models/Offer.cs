using System;
using System.Collections.Generic;

namespace WeChipCredit.Models
{
    public class Offer : ICloneable
    {
        public int IdOffer { get; set; }
        public Client _Client { get; set; }
        public List<Product> _Products { get; set; }



        public float TotalOffer
        {
            get { return CalcTotalOffer(); }
            set { _ = CalcTotalOffer(); }

        }

        public float NewVlCredit
        {
            get { return CalcNewVlCredit(); }
            set { _ = CalcNewVlCredit(); }
        }



        public float CalcTotalOffer()
        {
            float result = 0;
            if (_Products != null)
            {
                foreach (var s in _Products)
                {
                    result += s.VlPrice;
                }
            }
            return result;
        }

        public float CalcNewVlCredit()
        {
            float result = _Client.VlCredit - TotalOffer;
            return result;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}