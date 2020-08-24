using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Lst_Alunos
    {
        public List<Aluno> lista { get; set; }

        public HashSet<int> conjunto;

        public void Constroi_conjunto()
        {
            conjunto = new HashSet<int>();
            foreach(Aluno aluno in lista)
            {
                conjunto.Add(aluno.processo);
            }
        }

        public Lst_Alunos()
        {
            lista = new List<Aluno>();
        }
    }
}
