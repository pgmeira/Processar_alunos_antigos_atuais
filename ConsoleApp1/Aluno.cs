using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace ConsoleApp1
{
    class Aluno
    {
        private string _senha = String.Empty;

        public int ano_letivo { get; set; }
        public int processo { get; set; }
        public string nome_completo { get; set; }
        public string primeiro_nome { get; set; }
        public string apelido { get; set; }
        public string email { get; set; }
        public string senha { 
        get {
                if (_senha == String.Empty)
                    Gerar_senha();
                return _senha;
        }
            set { _senha = value; } 
            }
        public string unidade { get; set; }
        public string situacao { get; set; }
        public string escola{ get; set; }
        public string ano { get; set; }
        public string turma{ get; set; }

        private void Gerar_senha()
        {
            _senha = "a000" + processo.ToString() + "000";
        }
        /// <summary>
        /// Construtor de Aluno
        /// </summary>
        /// <param name="p_ano"> Ano Lectivo </param>
        /// <param name="p_processo"> Nº de processo </param>
        /// <param name="p_nome_completo"> Nome completo </param>
        /// <param name="p_email"> Email </param>
        /// <param name="p_senha"> Senha </param>
        /// <param name="p_unidade"> Unidade orgânica </param>
        public Aluno(int p_ano, int p_processo,string p_nome_completo,string p_email,string p_senha,string p_unidade, string p_escola, string p_ano_escolar, string p_turma)
        {
            ano_letivo = p_ano;
            processo = p_processo;
            nome_completo = p_nome_completo;
            email = p_email;
            senha = p_senha;
            unidade = p_unidade;
            escola = p_escola;
            ano = p_ano_escolar;
            turma = p_turma;
            primeiro_nome = p_nome_completo.Substring(0, p_nome_completo.IndexOf(' '));
            apelido= p_nome_completo.Substring(p_nome_completo.IndexOf(' '));
        }
    }
}
