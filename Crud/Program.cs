using System;
using System.ComponentModel;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace Crud
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Vamos criar uma lista de itens (string)");

                Console.WriteLine("\nQual será o título da sua lista?");
                string titulo = Console.ReadLine();

                List<string> lista = CriarLista();

                Console.Clear();
                Console.Write("\nDigite o nome do arquivo (a extensão .txt será adicionada automaticamente): ");
                string nomeArquivo = Console.ReadLine() + ".txt";

                string caminhoArquivo = $"C:\\{nomeArquivo}";
                GravarArquivo(caminhoArquivo, titulo, lista);

                AbrirArquivoNotepad(nomeArquivo, caminhoArquivo);

                Console.Clear();
                Console.WriteLine("\nDeseja editar o arquivo? s/n");
                string resposta = Convert.ToString(Console.ReadLine());

                if (resposta.ToLower() == "s")
                {
                    EditarArquivo(caminhoArquivo, titulo, lista);
                }

                Console.Clear();
                Console.WriteLine("\nArquivo gravado com sucesso. Deseja excluí-lo? s/n");
                resposta = Console.ReadLine();

                if (resposta == "s")
                {
                    DeletarArquivo(caminhoArquivo);
                }
                else if (resposta == "n")
                {
                    Console.Clear();
                    Console.WriteLine("\nPrograma finalizado!");
                }
                else
                {
                    throw new FormatException();
                }
            }
            catch (FormatException) { Console.WriteLine("Formato de caracter inválido."); }
            catch (Exception ex) { Console.WriteLine("Erro inesperado: " + ex.ToString()); }
        }

        private static void DeletarArquivo(string caminhoArquivo)
        {
            File.Delete(caminhoArquivo);

            Console.Clear();
            Console.WriteLine("Arquivo deletado com sucesso!");
        }

        private static void AbrirArquivoNotepad(string nomeArquivo, string caminhoArquivo)
        {
            Console.Clear();
            Console.WriteLine($"\nArquivo {nomeArquivo} gravado no caminho: {caminhoArquivo}\nDeseja abrir o arquivo? s/n");
            string resposta = Convert.ToString(Console.ReadLine());

            if (resposta.ToLower() == "s")
            {
                System.Diagnostics.Process.Start("notepad", caminhoArquivo);
            }
        }

        private static void EditarArquivo(string caminhoArquivo, string titulo, List<string> lista)
        {
            StreamReader arquivoReader = new StreamReader(caminhoArquivo);
            arquivoReader = File.OpenText(caminhoArquivo);
            string listaLinha = "\n";

            Console.Clear();
            Console.WriteLine("\nEscolha o número do item da lista que deseja editar: ");

            while (arquivoReader.EndOfStream != true)
            {
                listaLinha += arquivoReader.ReadLine() + "\n";
            }
            Console.WriteLine(listaLinha);

            arquivoReader.Close();

            int numeroItemEscolhido = Convert.ToInt32(Console.ReadLine());

            Console.Write("\nDigite o novo nome do item: ");
            lista[numeroItemEscolhido - 1] = $"{numeroItemEscolhido}. {Console.ReadLine()}";

            while (true)
            {
                Console.Clear();
                foreach (string item in lista)
                {                    
                    Console.WriteLine(item);
                }

                Console.WriteLine("\nDeseja editar outro item? Se sim, digite o número do item deseja editar. Se não, digite as tecla 'q'.");
                string resposta = Console.ReadLine();

                if (resposta.ToLower() == "q")
                {
                    break;
                }
                else
                {
                    Console.Write("\nDigite o novo nome do item: ");
                    lista[Convert.ToInt32(resposta) - 1] = resposta + ". " + Console.ReadLine();
                }
            }

            GravarArquivo(caminhoArquivo, titulo, lista);

            Console.WriteLine("\nLista editada e gravada com sucesso!");
        }

        static List<string> CriarLista()
        {
            Console.Clear();
            Console.WriteLine("\nDigite os itens da lista: (Digite a tecla 'q' para fechar a lista.)");
            List<string> lista = new List<string>();

            int contador = 0;

            while (true)
            {
                string item = Console.ReadLine();
                contador++;

                if (item == "q")
                {
                    break;
                }
                else
                {
                    lista.Add($"{contador}. {item}");
                }
            }
            return lista;
        }

        static void GravarArquivo(string caminhoArquivo, string titulo, List<string> lista)
        {
            StreamWriter arquivoWriter = new StreamWriter(caminhoArquivo);

            arquivoWriter.WriteLine($"Lista - {titulo}");
            foreach (string item in lista)
            {
                arquivoWriter.WriteLine(item);
                Console.WriteLine(item);
            }
            arquivoWriter.Close();
        }
    }
}
