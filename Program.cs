using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using System.Threading.Tasks;

namespace CalcPrimosParalelismoCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var tamanho = 0;
            bool mostrar = false;
            Console.WriteLine($"tamanho limite: {int.MaxValue}\nlembre que seu poder de processamento tem limites!\nQual tamanho quer testar");
            var primos = new List<int>();
            try
            {
                tamanho = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("err:FormatException");
                return;
            }catch(OverflowException)
            {
                Console.WriteLine("err:OverflowException");
                return;
            }
            Console.WriteLine("deseja ver a lista de numeros?\n1. Sim\n2.Nao");
            var resposta = 0;
            try{
                resposta = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("err:FormatException");
                return;
            }
            catch (OverflowException)
            {
                Console.WriteLine("err:OverflowException");
                return;
            }
            if (resposta == 1)
            {
                mostrar = true;
            }
            else
            {
                if(resposta!= 2)
                {
                    Console.WriteLine("não entendi... não vou fazer nada para continuarmos sendo amigos!");
                    return;
                }
                mostrar = false;
            }
            var numeros = CriaListaInt(tamanho);
            var inico = DateTime.Now;
            foreach (var num in numeros)
            {
                if(eprimo(num))
                {
                    primos.Add(num.Num);
                }
            }
            var fim = DateTime.Now;
            Mostra(primos,mostrar);
            primos.Clear();
            Console.WriteLine();
            string Linear = $"\ntempos de execucao linear: {fim - inico}";
            inico = DateTime.Now;
            var listaPrimos = numeros.Select(Numero =>
            {
                return Task.Factory.StartNew(() =>
                {
                    if (eprimo(Numero))
                    {
                        primos.Add(Numero.Num);
                    }
                });
            }).ToArray();
            Task.WaitAll(listaPrimos);
            fim = DateTime.Now;
            Mostra(primos, mostrar);
            Console.WriteLine(Linear +"\n" +$"tempos de execucao paralelizado: {fim - inico}");
        }
        static List<Numero> CriaListaInt(int tamanho)
        {
            var A = new List<Numero>();
            Console.WriteLine("criando lista");
            for (int i = 0; i < tamanho; i++)
            {
                A.Add(new Numero(i));
            }
            return A;
        }
        static void Mostra(List<int> primos,bool mostrar)
        {
            if (mostrar)
            {
                Console.WriteLine();
                foreach (var p in primos)
                {
                    Console.Write($"{p}, ");
                }
            }
        }
        static bool eprimo(Numero nm)
        {
            int num = nm.Num;
            if ((num % 2 == 0 && num != 2)|| num < 2) return false;
            else
            {
                for(int i =2; i*i <= num; i++)
                {
                    if (num % i == 0) return false;
                }
            }
            return true;
        }
    }
}
