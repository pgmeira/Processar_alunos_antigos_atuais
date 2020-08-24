using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Class1
    {
        /// <summary>
        /// 2020/07/14
        /// Ler uma string com um tamanho minimo
        /// </summary>
        /// <param name="p_tamanho"></param>
        /// <returns></returns>
        public static string LerString(int p_minimo, int p_maximo)
        {
            string aux_001 = String.Empty;
            while (aux_001.Length < p_minimo||aux_001.Length>p_maximo)
                aux_001=Console.ReadLine();
            return aux_001;
        }

    }
}
