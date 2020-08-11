using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChipCredit.Models
{
    static class CPFValidate
    {
        public static bool ValidationCPF(string pCPF)
        {
            if (pCPF == "00000000000" || pCPF == "11111111111" || pCPF == "22222222222" || pCPF == "33333333333")
                return false;

            if (pCPF == "44444444444" || pCPF == "55555555555" || pCPF == "66666666666" || pCPF == "77777777777")
                return false;

            if (pCPF == "88888888888" || pCPF == "99999999999" || pCPF == "01234567890")
                return false;


            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;

            int soma;
            int resto;

            pCPF = pCPF.Trim();
            pCPF = pCPF.Replace(".", "").Replace("-", "");

            if (pCPF.Length != 11)
            {
                return false;
            }
            tempCpf = pCPF.Substring(0, 9);

            soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * (multiplicador1[i]);
            }
            resto = soma % 11;

            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            int soma2 = 0;

            for (int i = 0; i < 10; i++)
            {
                soma2 += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            }

            resto = soma2 % 11;

            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            digito = digito + resto.ToString();
            return pCPF.EndsWith(digito);
        }

    }
}
