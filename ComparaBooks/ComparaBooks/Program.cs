using System;

namespace ComparaBooks
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Gestor gestor = new Gestor();
                gestor.CreaArchivoFinal(@"C:\Users\srojasc\Downloads\_eBooks");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("Continuar...");
            Console.ReadLine();
        }
    }
}
