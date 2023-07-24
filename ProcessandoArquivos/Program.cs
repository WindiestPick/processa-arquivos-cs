using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace ProcessandoArquivos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<List<string>> impResult = new List<List<string>>();
            List<List<string>> expResult = new List<List<string>>();

            expResult = filtraDadosExportacao("SP", "..\\..\\..\\arquivos\\EXP_2022.csv");
            Console.WriteLine("terminou exp");
            impResult = filtraDadosImportacao("SP", "..\\..\\..\\arquivos\\IMP_2022.csv", expResult);
            Console.WriteLine("terminou imp");
            exportaArq(impResult, "SP");


            Console.WriteLine("terminou");
            Console.ReadLine();
        }

        static List<List<string>> filtraDadosExportacao(string estado, string localArq)
        {
            var reader = new StreamReader(File.OpenRead(localArq));
            List<List<string>> arqFormat = new List<List<string>>();
            arqFormat.Add(new List<string>() { "NCM", "MES", "VALOR_EXP", "VALOR_IMP" });     

            int contador = 0;


            while (!reader.EndOfStream)
            {

                string line = reader.ReadLine();
                line = line.Replace("\"", "");
                string[] values = line.Split(';');
                
                if (estado == values[5])
                {
                    contador = 0;
                    for (int i = 0; i < arqFormat.Count(); ++i)
                    {
                        if (arqFormat[i][0] == values[2] && arqFormat[i][1] == values[1])
                        {
                            int res = int.Parse(arqFormat[i][2]) + int.Parse(values[10]);
                            arqFormat[i][2] = res.ToString();
                            contador++;
                            break;
                        }   
                    }
                    if (contador < 1)
                    {
                        arqFormat.Add(new List<string>() { values[2], values[1], values[10], "0" });
                    }
                }
              
            }
            
            return arqFormat;
        }

        static List<List<string>> filtraDadosImportacao(string estado, string localArq, List<List<string>> lista)
        {
            List<List<string>> arqFormat = new List<List<string>>();
            var reader = new StreamReader(File.OpenRead(localArq));
            int contador = 0;

            arqFormat = lista;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                line = line.Replace("\"", "");
                string[] values = line.Split(';');

                if (estado == values[5])
                {
                    for(int i = 0; i < arqFormat.Count(); i++)
                    {
                        contador = 0;

                        if (arqFormat[i][0] == values[2] && arqFormat[i][1] == values[1])
                        {
                            int res = int.Parse(arqFormat[i][3]) + int.Parse(values[10]);
                            arqFormat[i][3] = res.ToString();
                            contador++;
                            break;
                        }

                    }
                    if (contador < 1)
                    {
                        arqFormat.Add(new List<string>() { values[2], values[1], "0", values[10] });
                    }
                }


            }

            return arqFormat;
        }


        static void exportaArq(List<List<string>> lista, string estado)
        {
            string[] meses = { "jan", "fev", "mar", "abr", "jun", "jul", "ago", "set", "out", "nov", "dez", "ano" };
            int contador = 0;

            System.IO.File.Create("..\\..\\..\\arquivos\\resultado\\" + estado + ".csv").Close();
            System.IO.TextWriter arquivo = System.IO.File.AppendText("..\\..\\..\\arquivos\\resultado\\" + estado +".csv");


            foreach (List<string> ls in lista)
            {
                if (contador < 1)
                {
                    List<string> cabecalho = new List<string>();
                    cabecalho.Add("NCM");
                    foreach(string mes in meses)
                    {
                        cabecalho.Add(mes + "_exp");
                        cabecalho.Add(mes + "_imp");
                        cabecalho.Add(mes + "_net");
                    }
                    arquivo.WriteLine(cabecalho);
                    contador++;
                    lista.RemoveAt(0);
                }
                else
                {
                    List<string> corpo = new List<string>();
                    corpo.Add(lista[contador][0]);

                    for (int i = 0; i < 13; i++)
                    {
                        int excl = -1;
                        for (int j = 0; i < lista.Count(); i++)
                        {
                            if (corpo[0] == lista[j][0] && int.Parse(lista[j][1]) == i + 1)
                            {
                                int res = int.Parse(lista[j][2]) - int.Parse(lista[j][3]);
                                corpo.Add(lista[j][2]);
                                corpo.Add(lista[j][3]);
                                corpo.Add(res.ToString());
                                excl = j;
                                break;
                            }
                        }
                        if (excl == -1 && i + 1 > 13)
                        {
                            corpo.Add("0");
                            corpo.Add("0");
                            corpo.Add("0");
                        }
                        else
                        {
                            lista.RemoveAt(excl);
                        }
                        if (i+1 == 12)
                        {
                            int soma1 = 0;
                            int soma2 = 0;
                            int soma3 = 0;
                            for (int b = 0; b < corpo.Count(); b =+ 3)
                            {
                                soma1 += int.Parse(corpo[b]);
                                soma2 += int.Parse(corpo[b+1]);
                                soma3 += int.Parse(corpo[b+2]);
                            }
                            corpo.Add(soma1.ToString());
                            corpo.Add(soma2.ToString());
                            corpo.Add(soma3.ToString());
                        }


                    }
                    arquivo.WriteLine(corpo);
                }
            }


        }

    }
}


/*
            var reader = new StreamReader(File.OpenRead(@"C:\Users\gusta\Documents\TesteEmprego\EXP_2022.csv"));
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                
                Console.WriteLine(line);
                
                
            }
 
*/
