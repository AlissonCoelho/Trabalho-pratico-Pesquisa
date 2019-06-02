using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TP2
{

    class Pesquisas
    {
        class Celula
        {
            public roon dado { get; set; }
            public Celula prox { get; set; }
            public Celula ant { get; set; }
        }

        class Lista
        {
            private Celula Inicio;
            private Celula Fim;
            private int Tam;
            public Lista()
            {
                Inicio = Fim = new Celula();
                Inicio.prox = null;
                Inicio.ant = null;
                Tam = 0;
            }
            public int Tamanho()
            {
                return Tam;
            }
            public bool Vazia()
            {
                return Inicio == Fim;
            }
            public void Inserir(roon dado)
            {
                Celula temp = new Celula();
                temp.dado = dado;
                temp.prox = null;
                temp.ant = Fim;

                Fim.prox = temp;
                Fim = temp;
                Tam++;
            }
            public roon Remover(int pos)
            {
                if (pos < 0 || pos > (Tam - 1))
                {
                    return null;
                }

                Celula Ant = Inicio;
                for (int i = 0; i < pos; i++)
                    Ant = Ant.prox;

                Celula temp = Ant.prox;
                Ant.prox = temp.prox;

                if (temp.prox == null)
                    Fim = Ant;
                else
                {
                    temp.prox.ant = Ant;
                }

                Tam--;

                return temp.dado;
            }
            public roon pesquisar(int chave, ref int comparacoes)
            {
                comparacoes++;
                if (Tam == 0)
                    return null;

                Celula temp = Inicio.prox;
                for (int i = 0; i < Tam; i++)
                {
                    comparacoes++;
                    if (temp.dado.room_id == chave)
                        break;
                    temp = temp.prox;
                }

                comparacoes++;
                if (temp == null)
                    return null;

                return temp.dado;
            }

        }

        class Hash
        {
            Lista[] Tabela;
            int max;
            public Hash(int tam, bool teste = false)
            {
                if (teste)
                    max = tam;
                else
                    max = NumeroPrimo(tam - 1);

                Tabela = new Lista[max];

                for (int i = 0; i < max; i++)
                    Tabela[i] = new Lista();
            }

            //retorna o proximo numero primo
            private int NumeroPrimo(int numero)
            {
                for (int i = 2; i < numero - 1; i++)
                {
                    if (numero % i == 0)
                        return NumeroPrimo(++numero);
                }
                return numero;
            }
            private int HashCod(int chave)
            {
                return chave % max;
            }
            public void Inserir(roon dado, ref int custo)
            {
                if (Pesquisar(dado.room_id, ref custo) == null)
                {
                    int pos = HashCod(dado.room_id);
                    Tabela[pos].Inserir(dado);
                }
            }
            public roon Pesquisar(int chave, ref int comparacoes)
            {
                int pos = HashCod(chave);
                return Tabela[pos].pesquisar(chave, ref comparacoes);
            }
            public int Colisoes()
            {
                Lista teste;
                int colisoes = 0;
                for (int i = 0; i < max; i++)
                {
                    if (Tabela[i].Tamanho() > 1)
                    {
                        colisoes = Tabela[i].Tamanho() - 1;
                        teste = Tabela[i];
                    }
                }

                return colisoes;
            }
        }

        public class roon
        {
            public int room_id { get; set; }
            public string host_id { get; set; }
            public string room_type { get; set; }
            public string country { get; set; }
            public string city { get; set; }
            public string neighborhood { get; set; }
            public string reviews { get; set; }
            public string overall_satisfaction { get; set; }
            public string accommodates { get; set; }
            public string bedrooms { get; set; }
            public string price { get; set; }
            public string property_type { get; set; }
            public string indice { get; set; }

        }

        class No
        {
            public roon Dado;
            public No Esq;
            public No Dir;

            public No(roon _dado)
            {
                Dado = _dado;
                Esq = null;
                Dir = null;
            }
        }

        class ArvoreBinaria
        {

            private No Raiz;
            public No Maior;
            public int tam;

            public ArvoreBinaria()
            {
                Raiz = null;
                Maior = null;
                tam = 0;
            }

            //Verifica se Arvore Vazia
            public bool Vazia()
            {
                return Raiz == null;
            }

            //Adiciona NO na arvore
            public void Inserir(roon _dado, ref int comparacoes)
            {
                Raiz = Inserir(Raiz, _dado, ref comparacoes);

            }
            private No Inserir(No no, roon _dado, ref int comparacoes)
            {

                if (no == null)
                {
                    tam++;
                    comparacoes++;
                    no = new No(_dado);
                }
                else if (_dado.room_id < no.Dado.room_id)
                {
                    comparacoes += 2;
                    no.Esq = Inserir(no.Esq, _dado, ref comparacoes);
                }

                else if (_dado.room_id > no.Dado.room_id)
                {
                    comparacoes += 3;
                    no.Dir = Inserir(no.Dir, _dado, ref comparacoes);
                }


                return no;
            }

            //Remover NO da arvore
            private No Remover(No no, int _chave)
            {
                if (no == null)
                    Console.WriteLine("Erro: Registro nao encontrado");

                else if (_chave < no.Dado.room_id)
                    no.Esq = Remover(no.Esq, _chave);

                else if (_chave > no.Dado.room_id)
                    no.Dir = Remover(no.Dir, _chave);

                else
                {
                    if (no.Dir == null)
                        no = no.Esq;

                    else if (no.Esq == null)
                        no = no.Dir;

                    else
                        no.Esq = Antecessor(no, no.Esq);
                }
                return no;
            }
            private No Antecessor(No no, No ant)
            {
                if (ant.Dir != null) ant.Dir = Antecessor(no, ant.Dir);
                else
                {
                    no.Dado = ant.Dado;
                    ant = ant.Esq;
                }
                return ant;
            }

            //Pesquisar na arvore binaria
            public roon Pesquisar(int chave, ref int Comparacoes)
            {
                return Pesquisar(Raiz, chave, ref Comparacoes);
            }
            private roon Pesquisar(No no, int _chave, ref int Comparacoes)
            {
                if (no == null)
                {
                    Comparacoes++;
                    return null;
                }
                else if (_chave < no.Dado.room_id)
                {
                    Comparacoes += 2;
                    return Pesquisar(no.Esq, _chave, ref Comparacoes);
                }

                else if (_chave > no.Dado.room_id)
                {
                    Comparacoes += 3;
                    return Pesquisar(no.Dir, _chave, ref Comparacoes);
                }

                else if (_chave == no.Dado.room_id)
                {
                    Comparacoes += 4;
                    return no.Dado;
                }
                else
                    return null;

            }

            public int Altura(bool teste = false)
            {

                if (teste)
                    return Altura(Raiz);
                else
                {
                    int s = 0;
                    Altura(Raiz, 0, ref s, ref Maior);
                    return s;
                }
            }
            private int Altura(No no)
            {
                if (no == null)
                    return 0;

                return 1 + Math.Max(Altura(no.Esq),
                    Altura(no.Dir));

            }
            private void Altura(No no, int cont, ref int result, ref No MaisAlto)
            {
                if (no != null)
                {
                    Altura(no.Esq, ++cont, ref result, ref MaisAlto);
                    cont--;
                    Altura(no.Dir, ++cont, ref result, ref MaisAlto);

                    if (cont >= result)
                    {
                        result = cont;
                        MaisAlto = no;
                    }
                }
                else
                    cont = 0;


            }

            public int AlturaNo(roon Pes)
            {
                int resut = 0;
                AlturaNo(Raiz, 0, ref resut, Pes);
                return resut;
            }
            private void AlturaNo(No no, int cont, ref int result, roon Pes)
            {
                if (no != null)
                {
                    AlturaNo(no.Esq, ++cont, ref result, Pes);
                    cont--;
                    AlturaNo(no.Dir, ++cont, ref result, Pes);

                    if (no.Dado == Pes)
                    {
                        result = cont;
                    }
                }
                else
                    cont = 0;
            }

        }

        static void Main(string[] args)
        {
            //Define Horario Atual
            DateTime agora = DateTime.Now;
            string Horario = string.Format($"{agora.Day}/{agora.Month}/{agora.Year} {agora.Hour}:{agora.Minute}:{agora.Second}");
            int comp = 0;

            #region Cria Vetor Aleatorio
            Log("VetorOrdenado.txt", $"Criar vetor ordenado Analise {Horario}:\n");
            Stopwatch watch1 = Stopwatch.StartNew();
            //Criar vetor ordenado por chave - usado na pesquisa sequencial e pesquisa binaria - itens 1 e 2
            string[] lerArquivo = File.ReadAllLines("dados_aleatorios.txt");
            roon[] roons = new roon[lerArquivo.Length - 1];
            comp = 0;
            for (int i = 1; i <= roons.Length; i++)
            {
                string[] Roon = lerArquivo[i].Split('\t');

                roons[i - 1] = new roon();

                roons[i - 1].room_id = int.Parse(Roon[0]);
                roons[i - 1].host_id = Roon[1];
                roons[i - 1].room_type = Roon[2];
                roons[i - 1].country = Roon[3];
                roons[i - 1].city = Roon[4];
                roons[i - 1].neighborhood = Roon[5];
                roons[i - 1].reviews = Roon[6];
                roons[i - 1].overall_satisfaction = Roon[7];
                roons[i - 1].accommodates = Roon[8];
                roons[i - 1].bedrooms = Roon[9];
                roons[i - 1].price = Roon[10];
                roons[i - 1].property_type = Roon[11];
                roons[i - 1].indice = Roon[12];
            }
            watch1.Stop();
            long elapsedMs1 = watch1.ElapsedMilliseconds / 1000;
            Log("VetorOrdenado.txt", $"    Tempo Gasto: {elapsedMs1}segundos");
            Log("VetorOrdenado.txt", $"    Comparações: {comp}");
            Log("VetorOrdenado.txt", $"Fim");
            #endregion

            #region Arvore Binaria
            Log("ArvoreBinaria.txt", $"Criar Arvore Binaria Analise {Horario}:\n");
            string[] quartos = File.ReadAllLines("dados_aleatorios.txt");
            Stopwatch watch2 = Stopwatch.StartNew();
            //Criar Arvore Binaria com os dados aleatorios - usada no item 3 e item 6
            ArvoreBinaria ArvoreQuartos = new ArvoreBinaria();
            comp = 0;
            for (int i = 1; i < quartos.Length; i++)
            {
                roon Quarto = new roon();
                string[] Roon = quartos[i].Split('\t');

                Quarto.room_id = int.Parse(Roon[0]);
                Quarto.host_id = Roon[1];
                Quarto.room_type = Roon[2];
                Quarto.country = Roon[3];
                Quarto.city = Roon[4];
                Quarto.neighborhood = Roon[5];
                Quarto.reviews = Roon[6];
                Quarto.overall_satisfaction = Roon[7];
                Quarto.accommodates = Roon[8];
                Quarto.bedrooms = Roon[9];
                Quarto.price = Roon[10];
                Quarto.property_type = Roon[11];
                Quarto.indice = Roon[12];

                //Inserir Qurto na Arvore
                ArvoreQuartos.Inserir(Quarto, ref comp);
            }
            watch2.Stop();
            long elapsedMs2 = watch2.ElapsedMilliseconds / 1000;
            Log("ArvoreBinaria.txt", $"    Tempo Gasto: {elapsedMs2}segundos");
            Log("ArvoreBinaria.txt", $"    Comparações: {comp}");
            Log("ArvoreBinaria.txt", $"Fim");
            #endregion

            #region Ordem Alfabeticas Cidades
            Log("Alfabetico.txt", $"Criar vetor em ordem alfabetica Analise {Horario}:\n");
            Stopwatch watch3 = Stopwatch.StartNew();
            //Criar vetor ordenado por ordem alfabetica das cidades - usado no item 5
            string[] Ler = File.ReadAllLines("dados_aleatorios.txt");
            roon[] roons2 = new roon[Ler.Length - 1];
            comp = 0;
            for (int i = 1; i <= roons2.Length; i++)
            {
                string[] Roon = Ler[i].Split('\t');

                roons2[i - 1] = new roon();

                roons2[i - 1].room_id = int.Parse(Roon[0]);
                roons2[i - 1].host_id = Roon[1];
                roons2[i - 1].room_type = Roon[2];
                roons2[i - 1].country = Roon[3];
                roons2[i - 1].city = Roon[4];
                roons2[i - 1].neighborhood = Roon[5];
                roons2[i - 1].reviews = Roon[6];
                roons2[i - 1].overall_satisfaction = Roon[7];
                roons2[i - 1].accommodates = Roon[8];
                roons2[i - 1].bedrooms = Roon[9];
                roons2[i - 1].price = Roon[10];
                roons2[i - 1].property_type = Roon[11];
                roons2[i - 1].indice = Roon[12];
            }
            QuickSortAlfabetica(roons2, 0, roons2.Length - 1);
            watch3.Stop();
            long elapsedMs3 = watch3.ElapsedMilliseconds / 1000;
            Log("Alfabetico.txt", $"    Tempo Gasto: {elapsedMs3}segundos");
            Log("Alfabetico.txt", $"    Comparações: {comp}");
            Log("Alfabetico.txt", $"Fim");
            #endregion

            #region Tabel Hash
            Log("TabelaHas.txt", $"Criar Tabela Hash {Horario}:\n");
            string[] Ler4 = File.ReadAllLines("dados_aleatorios.txt");
            Stopwatch watch4 = Stopwatch.StartNew();
            comp = 0;
            Hash TabelaHas = new Hash(Ler4.Length - 1);
            //Criar vetor ordenado por ordem alfabetica das cidades - usado no item 5
            for (int i = 1; i < quartos.Length; i++)
            {
                roon Quarto = new roon();
                string[] Roon = Ler4[i].Split('\t');

                Quarto.room_id = int.Parse(Roon[0]);
                Quarto.host_id = Roon[1];
                Quarto.room_type = Roon[2];
                Quarto.country = Roon[3];
                Quarto.city = Roon[4];
                Quarto.neighborhood = Roon[5];
                Quarto.reviews = Roon[6];
                Quarto.overall_satisfaction = Roon[7];
                Quarto.accommodates = Roon[8];
                Quarto.bedrooms = Roon[9];
                Quarto.price = Roon[10];
                Quarto.property_type = Roon[11];
                Quarto.indice = Roon[12];

                //Inserir Qurto na Arvore
                TabelaHas.Inserir(Quarto, ref comp);
            }
            watch4.Stop();
            Hash TabelaHasTeste = new Hash(Ler4.Length - 1, true);
            //Criar vetor ordenado por ordem alfabetica das cidades - usado no item 5
            for (int i = 1; i < quartos.Length; i++)
            {
                roon Quarto = new roon();
                string[] Roon = Ler4[i].Split('\t');

                Quarto.room_id = int.Parse(Roon[0]);
                Quarto.host_id = Roon[1];
                Quarto.room_type = Roon[2];
                Quarto.country = Roon[3];
                Quarto.city = Roon[4];
                Quarto.neighborhood = Roon[5];
                Quarto.reviews = Roon[6];
                Quarto.overall_satisfaction = Roon[7];
                Quarto.accommodates = Roon[8];
                Quarto.bedrooms = Roon[9];
                Quarto.price = Roon[10];
                Quarto.property_type = Roon[11];
                Quarto.indice = Roon[12];

                //Inserir Qurto na Arvore
                TabelaHasTeste.Inserir(Quarto, ref comp);
            }
            long elapsedMs4 = watch4.ElapsedMilliseconds / 1000;
            Log("TabelaHas.txt", $"    Tempo Gasto: {elapsedMs4}segundos");
            Log("TabelaHas.txt", $"    Comparações: {comp}");
            Log("TabelaHas.txt", $"Fim");

            #endregion

            string option;

            do
            {
                #region Console
                Console.Clear();
                Console.WriteLine("1 - Pesquisar as informações de um quarto, usando o campo room_id como chave, usando uma pesquisa sequencial");
                Console.WriteLine("2 - Pesquisar as informações de um quarto, usando o campo room_id como chave, usando uma pesquisa binária");
                Console.WriteLine("3 - Pesquisar as informações de um quarto, usando o campo room_id como chave, usando uma árvore binária");
                Console.WriteLine("4 - Pesquisar as informações de um quarto, usando o campo room_id como chave, usando uma tabela Hash");
                Console.WriteLine("5 - Quartos disponíveis para cada uma das cidades");
                Console.WriteLine("6 - Pesquisar o quarto mais caro e mais barato de uma determinada cidade, informada pelo usuário");
                Console.WriteLine("7 - Sair");

                option = Console.ReadLine();
                DateTime now;
                string Hora;
                int x;
                Stopwatch watch;
                long elapsedMs;
                int QntComparacoes;
                string Encontrada;
                string Indice;
                roon pesq;
                #endregion

                switch (option)
                {
                    #region CASE 1 - Sequencial
                    case "1"://Pesquisar as informações de um quarto, usando o campo room_id como chave, usando uma pesquisa sequencial")
                        Console.Clear();
                        Console.WriteLine("Pesquisa Sequencial");
                        //Solicita chave para pesquisa
                        Console.WriteLine("Digite uma chave");
                        x = int.Parse(Console.ReadLine());

                        //Define Horario Atual
                        now = DateTime.Now;
                        Hora = string.Format($"{now.Day}/{now.Month}/{now.Year} {now.Hour}:{now.Minute}:{now.Second}");

                        Log("PesquisaSequencial.txt", "Dados Ordenados " + Hora);
                        PesquisaSequencial(roons, x);

                        Console.ReadKey();
                        break;
                    #endregion

                    #region CASE 2 - Pesquisa Binaria
                    case "2"://Pesquisar as informações de um quarto, usando o campo room_id como chave, usando uma pesquisa binária");
                        Console.Clear();
                        //Solicita chave para pesquisa
                        Console.WriteLine("Digite uma chave");
                        x = int.Parse(Console.ReadLine());

                        //Define Horario Atual
                        now = DateTime.Now;
                        Hora = string.Format($"{now.Day}/{now.Month}/{now.Year} {now.Hour}:{now.Minute}:{now.Second}");

                        Log("PesquisaBinaria.txt", "Dados Ordenados " + Hora);
                        PesquisaBinaria(roons, x);

                        Console.ReadKey();
                        break;
                    #endregion

                    #region CASE 3 - Arvore Binaria
                    case "3"://Pesquisar as informações de um quarto, usando o campo room_id como chave, usando uma árvore binária");
                        Console.Clear();
                        Console.WriteLine("Arvore Binaria");
                        Console.WriteLine();
                        Console.WriteLine("Digite uma chave");
                        x = int.Parse(Console.ReadLine());

                        //Define Horario Atual
                        now = DateTime.Now;
                        Hora = string.Format($"{now.Day}/{now.Month}/{now.Year} {now.Hour}:{now.Minute}:{now.Second}");

                        Log("PesquisaArvoreBinaria.txt", "Dados Aleatorios " + Hora);
                        Log("PesquisaArvoreBinaria.txt", "Pesquisa Arvore Binaria Analise:\n");
                        Log("PesquisaArvoreBinaria.txt", $"    ChavePesquisada {x}");
                        comp = 0;
                        watch = Stopwatch.StartNew();
                        pesq = ArvoreQuartos.Pesquisar(x, ref comp);
                        watch.Stop();
                        elapsedMs = watch.ElapsedMilliseconds / 1000;
                        Log("PesquisaArvoreBinaria.txt", $"    Tempo Gasto: {elapsedMs}segundos");
                        Encontrada = "Não";
                        Indice = "-";

                        if (pesq != null)
                        {
                            Encontrada = "Sim";
                            Indice = pesq.indice;
                            Imprimir(pesq);
                        }
                        Log("PesquisaArvoreBinaria.txt", $"    Chave Encontrada: {Encontrada}");
                        Log("PesquisaArvoreBinaria.txt", $"    Indice da Chave: {Indice}");
                        Log("PesquisaArvoreBinaria.txt", $"    Comparações: {comp}");
                        Log("PesquisaArvoreBinaria.txt", $"    Altura No: {ArvoreQuartos.AlturaNo(pesq)}");
                        Log("PesquisaArvoreBinaria.txt", $"Fim");

                        Console.WriteLine();
                        Console.WriteLine(ArvoreQuartos.Altura(true));
                        Console.WriteLine(ArvoreQuartos.Altura());
                        Memoria();

                        Console.ReadLine();

                        break;
                    #endregion

                    #region CASE 4 - Tabela Hash
                    case "4"://Pesquisar as informações de um quarto, usando o campo room_id como chave, usando uma tabela Hash");
                        Console.Clear();
                        Console.WriteLine("Arvore Binaria");
                        Console.WriteLine();
                        Console.WriteLine("Digite uma chave");
                        x = int.Parse(Console.ReadLine());

                        //Define Horario Atual
                        now = DateTime.Now;
                        Hora = string.Format($"{now.Day}/{now.Month}/{now.Year} {now.Hour}:{now.Minute}:{now.Second}");

                        Log("PesquisaTabelaHash.txt", "Pesquisa em tabela hash " + Hora);
                        Log("PesquisaTabelaHash.txt", $"    ChavePesquisada {x}");
                        comp = 0;
                        watch = Stopwatch.StartNew();
                        pesq = TabelaHas.Pesquisar(x, ref comp);
                        watch.Stop();
                        elapsedMs = watch.ElapsedMilliseconds / 1000;
                        Log("PesquisaTabelaHash.txt", $"    Tempo Gasto: {elapsedMs}segundos");
                        Encontrada = "Não";
                        Indice = "-";

                        if (pesq != null)
                        {
                            Encontrada = "Sim";
                            Indice = pesq.indice;
                            Imprimir(pesq);
                        }
                        Console.WriteLine(TabelaHas.Colisoes());
                        Log("PesquisaTabelaHash.txt", $"    Chave Encontrada: {Encontrada}");
                        Log("PesquisaTabelaHash.txt", $"    Indice da Chave: {Indice}");
                        Log("PesquisaTabelaHash.txt", $"    Comparações: {comp}");
                        Log("PesquisaTabelaHash.txt", $"    Numero maximo de Colisões: {TabelaHas.Colisoes()}");
                        Log("PesquisaTabelaHash.txt", $"Fim");
                        Console.WriteLine();
                        Console.WriteLine();
                        //////////////////////////////////////////////////////////////////////////
                        Log("PesquisaTabelaHashTeste.txt", "Pesquisa em tabela hash " + Hora);
                        Log("PesquisaTabelaHashTeste.txt", $"    ChavePesquisada {x}");
                        comp = 0;
                        watch = Stopwatch.StartNew();
                        pesq = TabelaHasTeste.Pesquisar(x, ref comp);
                        watch.Stop();
                        elapsedMs = watch.ElapsedMilliseconds / 1000;
                        Log("PesquisaTabelaHashTeste.txt", $"    Tempo Gasto: {elapsedMs}segundos");
                        Encontrada = "Não";
                        Indice = "-";

                        if (pesq != null)
                        {
                            Encontrada = "Sim";
                            Indice = pesq.indice;
                            Imprimir(pesq);
                        }
                        Console.WriteLine(TabelaHasTeste.Colisoes());
                        Log("PesquisaTabelaHashTeste.txt", $"    Chave Encontrada: {Encontrada}");
                        Log("PesquisaTabelaHashTeste.txt", $"    Indice da Chave: {Indice}");
                        Log("PesquisaTabelaHashTeste.txt", $"    Comparações: {comp}");
                        Log("PesquisaTabelaHashTeste.txt", $"    Numero maximo de Colisões:  {TabelaHasTeste.Colisoes()}");
                        Log("PesquisaTabelaHashTeste.txt", $"Fim");


                        Console.ReadLine();

                        break;
                    #endregion

                    #region CASE 5 - Quartos por cidade
                    case "5"://Contabilizar a quantidade de quartos disponíveis para cada uma das cidades. Deve ser exibido o nome de cada cidade e a quantidade de quartos disponíveis");
                        Console.Clear();
                        Console.WriteLine("Quartos por ciade");
                        Console.WriteLine();

                        //Define Horario Atual
                        now = DateTime.Now;
                        Hora = string.Format($"{now.Day}/{now.Month}/{now.Year} {now.Hour}:{now.Minute}:{now.Second}");

                        Log("Item_5.txt", "Dados_Aleatorios " + Hora);
                        QntComparacoes = 0;
                        watch = Stopwatch.StartNew();
                        int cont = 1;
                        for (int i = 0; i < roons2.Length - 1; i++)
                        {
                            //Compara se a cidade atual e a seguinte é a mesma e contabiliza
                            QntComparacoes++;
                            if (roons2[i].city == roons2[i + 1].city)
                            {
                                cont++;
                            }
                            //Verifica se a cidade atual e a seguinte são diferentes e imprime, ou se é a ultima cidade
                            QntComparacoes++;
                            if (roons2[i].city != roons2[i + 1].city || i + 1 >= roons2.Length - 1)
                            {
                                Console.WriteLine($"Cidade: {roons2[i].city} - Quantidade de Quartos: {cont}");
                                Console.WriteLine();
                                cont = 1;
                            }
                        }
                        watch.Stop();
                        elapsedMs = watch.ElapsedMilliseconds / 1000;
                        Log("Item_5.txt", $"    Tempo Gasto: {elapsedMs}segundos");
                        Log("Item_5.txt", $"    Comparações: {QntComparacoes}");
                        Log("Item_5.txt", $"Fim");
                        Console.ReadLine();
                        break;
                    #endregion

                    #region CASE 6 - Quartos mais caro e mais barato
                    case "6"://Pesquisar o quarto mais caro e mais barato de uma determinada cidade, informada pelo usuário");

                        //Define Horario Atual
                        now = DateTime.Now;
                        Hora = string.Format($"{now.Day}/{now.Month}/{now.Year} {now.Hour}:{now.Minute}:{now.Second}");

                        Log("Item_6.txt", "Dados_Aleatorios " + Hora);
                        List<roon> Quaros_Min = new List<roon>();
                        List<roon> Quaros_Max = new List<roon>();
                        roon roon_min = new roon();
                        roon roon_max = new roon();
                        float RoonMin_price = 1000000000;
                        float RoonMax_price = -1;

                        Console.Clear();
                        Console.WriteLine("6 - Preços dos quartos por cidade");
                        Console.WriteLine();
                        Console.WriteLine("Digite o nome da cidade a ser pesquisada");
                        string cidade = Console.ReadLine();
                        Log("Item_6.txt", $"    Cidade Pesquisada: {cidade}");
                        QntComparacoes = 0;
                        //Começa a contar tempo depois que digita o nome da cidade
                        watch = Stopwatch.StartNew();
                        //Verifica o menor e o maior valor 
                        float a;
                        for (int i = 0; i < roons2.Length; i++)
                        {
                            if (cidade == roons2[i].city)
                            {
                                QntComparacoes++;
                                a = float.Parse(roons2[i].price);
                                if (a < RoonMin_price)
                                {
                                    QntComparacoes++;
                                    RoonMin_price = float.Parse(roons2[i].price);
                                    roon_min = roons2[i];
                                }
                                if (a > RoonMax_price)
                                {
                                    QntComparacoes++;
                                    RoonMax_price = float.Parse(roons2[i].price);
                                    roon_max = roons2[i];
                                }
                            }
                        }

                        //Adiciona um dos quartos de menor valor na Lista
                        Quaros_Min.Add(roon_min);
                        //Adiciona um dos quartos de maior valor na Lista
                        Quaros_Max.Add(roon_max);

                        //Verificar quartos com os mesmos valores (minimo e maximo) e adiciona na Lista
                        for (int i = 0; i < roons2.Length; i++)
                        {
                            QntComparacoes++;
                            if (cidade == roons2[i].city)
                            {

                                a = float.Parse(roons2[i].price);
                                QntComparacoes++;
                                if (a == RoonMin_price && roons2[i] != roon_min)
                                {
                                    Quaros_Min.Add(roons2[i]);
                                }
                                QntComparacoes++;
                                if (a == RoonMax_price && roons2[i] != roon_max)
                                {
                                    Quaros_Max.Add(roons2[i]);
                                }
                            }
                        }

                        //Tempo para de contar depois que a lista é montada
                        watch.Stop();
                        elapsedMs = watch.ElapsedMilliseconds / 1000;
                        Log("Item_6.txt", $"    Tempo Gasto: {elapsedMs}segundos");
                        Log("Item_6.txt", $"    Comparações: {QntComparacoes}");
                        Log("Item_6.txt", $"Fim");
                        //imprime quartos
                        Console.Clear();
                        Console.WriteLine("******************************Qurato(s) mais caro(s):******************************");
                        foreach (roon qua in Quaros_Max)
                        {
                            Imprimir(qua);
                            Console.WriteLine();
                        }

                        Console.WriteLine("******************************Qurato(s) mais barato(s):******************************");
                        foreach (roon qua in Quaros_Min)
                        {
                            Imprimir(qua);
                            Console.WriteLine();
                        }

                        Console.ReadKey();
                        break;
                    #endregion

                    #region CASE 7 - sair
                    case "7"://Sair

                        return;
                    #endregion

                    #region default
                    default:
                        Console.WriteLine("Invalido");
                        break;
                        #endregion

                }
            } while (true);

        }

        //Ordenação Rapida
        static void QuickSortAlfabetica(roon[] A, int esquerda, int direita)
        {

            int pivo;
            roon temp;
            int i, j;
            i = esquerda;
            j = direita;

            pivo = ConverterInt(A[(esquerda + direita) / 2].city);

            while (i <= j)
            {

                while (ConverterInt(A[i].city) < pivo && i < direita)
                    i++;
                while (ConverterInt(A[j].city) > pivo && j > esquerda)
                    j--;

                if (i <= j)
                {
                    temp = A[i];
                    A[i] = A[j];
                    A[j] = temp;
                    i++;
                    j--;
                }
            }

            if (j > esquerda)
                QuickSortAlfabetica(A, esquerda, j);

            if (i < direita)
                QuickSortAlfabetica(A, i, direita);
        }

        //Ordenação Rapida
        static void QuickSort(roon[] A, int esquerda, int direita)
        {

            int pivo;
            roon temp;
            int i, j;
            i = esquerda;
            j = direita;
            pivo = A[(esquerda + direita) / 2].room_id;

            while (i <= j)
            {

                while (A[i].room_id < pivo && i < direita)
                    i++;
                while (A[j].room_id > pivo && j > esquerda)
                    j--;

                if (i <= j)
                {
                    temp = A[i];
                    A[i] = A[j];
                    A[j] = temp;
                    i++;
                    j--;
                }
            }

            if (j > esquerda)
                QuickSort(A, esquerda, j);

            if (i < direita)
                QuickSort(A, i, direita);
        }

        //Gerar arquivos de log
        public static void Log(string nomeArq, string texto)
        {
            // Cria um novo arquivo
            FileStream arq1 = new FileStream("Analise/" + nomeArq, FileMode.OpenOrCreate);

            //faz uma leitura do arquivo para ir ao fina do arquivo e assim nhão sobreescrever nenhum dado
            StreamReader ler = new StreamReader(arq1);
            ler.ReadToEnd();

            // declara instancia do StreamWriter 
            StreamWriter escreve = new StreamWriter(arq1);

            //obtem o texto no parametro da chamada da função
            escreve.WriteLine(texto);
            escreve.Close();
        }

        //pesquisa sequencial
        public static void PesquisaSequencial(roon[] roons, int chave)
        {
            roon quarto = null;
            Log("PesquisaSequencial.txt", "Pesquisa Sequencial Analise:\n");
            Log("PesquisaSequencial.txt", $"    ChavePesquisada {chave}");
            string Encontrada = "Não";
            int Indice = 0;
            int QntComparacoes = 0;
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < roons.Length; i++)
            {
                QntComparacoes++;
                if (roons[i].room_id == chave)
                {
                    Encontrada = "Sim";
                    Indice = i;
                    quarto = roons[i];
                    break;
                }
            }

            watch.Stop();
            long elapsedMs = watch.ElapsedMilliseconds / 1000;
            Log("PesquisaSequencial.txt", $"    Tempo Gasto: {elapsedMs}segundos");
            Log("PesquisaSequencial.txt", $"    Chave Encontrada: {Encontrada}");
            Log("PesquisaSequencial.txt", $"    Indice da Chave: {Indice}");
            Log("PesquisaSequencial.txt", $"    Comparações: {QntComparacoes}");
            Log("PesquisaSequencial.txt", $"Fim");

            Console.Clear();
            if (Encontrada == "Sim")
            {
                Console.WriteLine("Pesquisa sequencial - Informações do quarto:");
                Imprimir(quarto);
            }
            else
                Console.WriteLine("Pesquisa sequencial - Quarto não encontrado");

            Memoria();
        }

        //Pesquisa Binaria 
        public static void PesquisaBinaria(roon[] roons, int chave)
        {
            Log("PesquisaBinaria.txt", "Pesquisa Sequencial Analise:\n");
            Log("PesquisaBinaria.txt", $"    ChavePesquisada {chave}");
            string Encontrada = "Não";
            string Indice = "-";
            int QntComparacoes = 0;
            var watch = Stopwatch.StartNew();

            QuickSort(roons, 0, 127999);

            roon result = PesquisaBinaria(chave, roons, 0, 127999, ref QntComparacoes);

            watch.Stop();

            long elapsedMs = watch.ElapsedMilliseconds / 1000;
            Log("PesquisaBinaria.txt", $"   Tempo Gasto: {elapsedMs}segundos");

            //Imprimir resultado do qurto
            Console.Clear();
            if (result != null)
            {
                Encontrada = "Sim";
                Indice = result.indice;
                Console.WriteLine("Pesquisa Binaria - Informações do quarto:");
                Imprimir(result);
            }
            else
                Console.WriteLine("Pesquisa Binaria - quarto não encontrado");

            Log("PesquisaBinaria.txt", $"   Chave Encontrada: {Encontrada}");
            Log("PesquisaBinaria.txt", $"   Indice da Chave: {Indice}");
            Log("PesquisaBinaria.txt", $"   Comparações: {QntComparacoes}");
            Log("PesquisaBinaria.txt", $"Fim");

            Memoria();
        }

        //Metodo Privado de Pesquisa Binaria
        private static roon PesquisaBinaria(int chave, roon[] roons, int e, int d, ref int QntComparacoes)
        {

            int meio = (e + d) / 2;
            if (roons[meio].room_id == chave)
            {
                QntComparacoes++;
                return roons[meio];
            }

            else if (e >= d)
            {
                QntComparacoes += 2;
                return null; // nao encontrado
            }

            else if (roons[meio].room_id < chave)
            {
                QntComparacoes += 3;
                return PesquisaBinaria(chave, roons, meio + 1, d, ref QntComparacoes);
            }
            else
            {
                QntComparacoes += 3;
                return PesquisaBinaria(chave, roons, e, meio - 1, ref QntComparacoes);
            }
        }

        //Imprimir Infomarções do Quarto
        public static void Imprimir(roon quarto)
        {
            Console.WriteLine($"Codigo: {quarto.room_id}");
            Console.WriteLine($"host_id: {quarto.host_id}");
            Console.WriteLine($"Tipo do quarto: {quarto.room_type}");
            Console.WriteLine($"Pais: {quarto.country}");
            Console.WriteLine($"Cidade: {quarto.city}");
            Console.WriteLine($"Bairro: {quarto.neighborhood}");
            Console.WriteLine($"Visitas: {quarto.reviews}");
            Console.WriteLine($"Media Satisfação: {quarto.overall_satisfaction}");
            Console.WriteLine($"Acomodações: {quarto.accommodates}");
            Console.WriteLine($"Banheiros: {quarto.bedrooms}");
            Console.WriteLine($"Valor: {quarto.price}$");
            Console.WriteLine($"Propiedade: {quarto.property_type}");

        }

        //Memoria Utilizada
        public static void Memoria()
        {
            long ramUsage = Process.GetCurrentProcess().PeakWorkingSet64;
            long allocationInMB = ramUsage / (1024 * 1024);
            Console.WriteLine(" Memória utilizada: " + allocationInMB + "MB");
        }

        //Converte nome para interio - Soma dos valores char
        private static int ConverterInt(string nome)
        {
            string dado = nome.Trim().ToLower();
            int soma = 0;
            int x;

            for (int i = 0; i < 4; i++)
            {
                if (i > nome.Length - 1)
                    x = 0;
                else if (dado[i] == 'ç')
                    x = 'c' - 96;
                else if (dado[i] == 'ã')
                    x = 'a' - 96;
                else if (dado[i] == 'õ')
                    x = 'o' - 96;

                else
                    x = dado[i] - 96;

                soma += x * (int)Math.Pow(26, (4 - i - 1));
            }
            return soma;
        }
    }


}

