/*
 * 2020/07/24
 * Paulo Meira
 * O programa aceita 2 CSV Inovar como input
 *      alunos do ano anterior
 *      alunos do ano corrente
 *          ano letivo
 *          processo
 *          nome
 *  OS CSV devem vir filtrados só com os alunos inscritos
 *  
 * Grava 
 *      CSV GMAIL com os alunos que entraram
 *      CSV GMAIL com os alunos que sairam
 */

using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static DateTime
            inicio,
            fim;

        static TimeSpan
            duracao;

        static bool
            teste_de_tempo_de_execucao = false;

        static string
             prim_input = String.Empty,
             prim_output = String.Empty;

        static Constants1
            consts = new Constants1();

        static Lst_Alunos
            lst_aaa = new Lst_Alunos(),
            lst_aac = new Lst_Alunos();

        static HashSet<int>
            alunos_que_ficaram = new System.Collections.Generic.HashSet<int>(),
            alunos_que_sairam = new System.Collections.Generic.HashSet<int>(),
            alunos_novos = new System.Collections.Generic.HashSet<int>();

        static Encoding encoding_dos_fich_lidos;

        static void Main(string[] args)
        {
            //Console.WriteLine("Teste de tempo de execução: (S/*) ");
            //teste_de_tempo_de_execucao = Console.ReadLine() == "S";
            Ler_alunos_do_ano_anterior();
            Ler_alunos_do_ano();
            inicio = DateTime.Now;
            Processar_alunos_novos();
            Processar_alunos_que_ficaram();
            Processar_alunos_que_sairam();
            fim = DateTime.Now;
            duracao = fim.Subtract(inicio);
            Console.WriteLine("Execução: " + duracao.ToString());
            Processar_CSV(alunos_novos, lst_aac, "Nome do CSV com os alunos novos: ", "active"); // Processar CSV dos que entraram
            Processar_CSV(alunos_que_sairam, lst_aaa, "Nome do CSV com os alunos que sairam: ", "suspended"); // Processar CSV dos que sairam
        }

        private static void Processar_CSV(HashSet<int> p_alunos, Lst_Alunos p_lista, string p_texto, string p_estado)
        {
            // declaração da lista de linhas para o CSV
            // declaração da linha para o CSV
            // para cada um no conjunto alunos_novos
            //  procurar na lista de alunos do ano
            //  se encontra  
            //      criar linha com os dados
            //          primeiro nome
            //          apelido
            //          construir endereço de email
            //          atribuir senha
            //          atribuir unidade
            //          estado da conta
            //      juntar a linha à lista
            // gravar o CSV

            List<List<string>> lst_csv = new List<List<string>>();
            List<string> lst_reg;

            foreach (int processo in p_alunos)
            {
                foreach (Aluno registo in p_lista.lista)
                {
                    if (processo == registo.processo)
                    {
                        lst_reg = new List<string>();
                        lst_reg.Add(registo.primeiro_nome);
                        lst_reg.Add(registo.apelido);
                        lst_reg.Add("a" + registo.processo.ToString() + "@alpoente.org");
                        lst_reg.Add(registo.senha);
                        lst_reg.Add(consts.UNIDADE_ORGANICA_ALUNOS);
                        lst_reg.Add(p_estado);
                        lst_reg.Add(registo.escola);
                        lst_reg.Add(registo.ano);
                        lst_reg.Add(registo.turma);
                        lst_csv.Add(lst_reg);
                    }
                }
            }
            Gravar_ficheiro(Aceitar_nome_do_ficheiro(p_texto), lst_csv, consts.PRIMEIRA_LINHA_CSV);
        }

        private static void Ler_alunos_do_ano_anterior()
        {
            //prim_input = String.Empty;
            //while(!File.Exists(prim_input))
            //    prim_input = Aceitar_nome_do_ficheiro("Primeiro input: ");
            prim_input = "2019_2020.csv";
            lst_aaa = Ler_ficheiro(prim_input);
            Console.WriteLine("Nº de alunos do ano anterior: " + lst_aaa.lista.Count.ToString());
        }

        private static void Ler_alunos_do_ano()
        {
            //prim_input = String.Empty;
            //while (!File.Exists(prim_input))
            //    prim_input = Aceitar_nome_do_ficheiro("Segundo input: ");
            prim_input = "2020_2021.csv";
            lst_aac = Ler_ficheiro(prim_input);
            Console.WriteLine("Nº de alunos do ano corrente: " + lst_aac.lista.Count.ToString());

        }

        private static void Processar_alunos_que_sairam()
        {
            Console.WriteLine("Processar alunos que sairam");
            alunos_que_sairam = new HashSet<int>(lst_aaa.conjunto);
            alunos_que_sairam.ExceptWith(lst_aac.conjunto);
        }

        private static void Processar_alunos_que_ficaram()
        {
            Console.WriteLine("Processar alunos que ficaram");
            alunos_que_ficaram = new HashSet<int>(lst_aaa.conjunto);
            alunos_que_ficaram.IntersectWith(lst_aac.conjunto);
        }

        private static void Processar_alunos_novos()
        {
            Console.WriteLine("Processar alunos que entraram");
            alunos_novos = new HashSet<int>(lst_aac.conjunto);
            alunos_novos.ExceptWith(lst_aaa.conjunto);
        }

        public static string Aceitar_nome_do_ficheiro(string p_legenda)
        {
            Console.WriteLine(p_legenda);
            return Class1.LerString(consts.MINIMO_DE_CARACTERES, consts.MAXIMO_DE_CARACTERES);
        }

        private static void Gravar_ficheiro(string p_nome_do_ficheiro, List<List<string>> p_lst_csv, string p_primeira_linha)
        {
            /*
             * se não tem nada para gravar
             *  avisa
             *  termina
             * senão
             *  abre ficheiro com o nome previamente aceite
             *  escreve o cabeçalho do ficheiro
             *  percorre a lista de linhas
             *  para cada linha
             *      constroi o registo
             *      grava o registo
             *  fecha o ficheiro
             */
            if (p_lst_csv.Count <= 0)
            {
                Console.WriteLine("Nada para ser gravado.");
                Console.ReadKey();
                return;
            }
            StreamWriter sw = new StreamWriter(p_nome_do_ficheiro, false, encoding_dos_fich_lidos);
            sw.WriteLine(p_primeira_linha);
            string str_aux_1 = String.Empty;
            foreach (List<string> aluno in p_lst_csv)
            {
                str_aux_1 = String.Empty;
                foreach (string str in aluno)
                    str_aux_1 += str + ";";
                str_aux_1 = str_aux_1.Substring(0, str_aux_1.Length - 1);
                sw.WriteLine(str_aux_1);
            }
            sw.Close();
        }

        private static Lst_Alunos Ler_ficheiro(string p_nome_do_csv)
        {
            Lst_Alunos lista = new Lst_Alunos();
            Int16 res = 0;

            // ler o CSV
            using (StreamReader parser = new StreamReader(p_nome_do_csv))
            {
                encoding_dos_fich_lidos = parser.CurrentEncoding;
                // para cada registo do CSV
                while (!parser.EndOfStream)
                {
                    string[] dados = parser.ReadLine().ToString().Split(';');
                    // se não é a linha de cabeçalhos
                    if (Int16.TryParse(dados[0], out res))
                    {
                        // processa novo aluno
                        Aluno aluno = new Aluno(
                            res, // ano letivo
                            Int16.Parse(dados[1]), // nº de processo
                            dados[2].ToString(), // nome completo
                            String.Empty, // email
                            String.Empty, // senha
                            consts.UNIDADE_ORGANICA_ALUNOS, // unidade organica
                            dados[3].ToString(), // escola
                            dados[4].ToString(), // ano escolar
                            dados[5].ToString() // turma                            
                            ) ;
                        // adicina à lista
                        lista.lista.Add(aluno);
                    }
                }
                // se a lista tem registos
                if (lista.lista != null)
                {
                    if (lista.lista.Count > 0)
                        // constroi conjunto
                        lista.Constroi_conjunto();
                }
                else // se falhou ...
                    Console.WriteLine("Não li nada");
            }
            return lista;
        }
    }
}
