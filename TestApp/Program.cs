using System;
using System.Text.Json;
using libPantryDotNet;

namespace TestApp
{
    class Program
    {
        public static string yourPantryIdHere = "";
        public static string basket_to_mess_with = "test_basket";
        public static Tuple<string, string> exampleObject;
        static void Main(string[] args)
        {
            exampleObject = new Tuple<string, string>("item1", "1");
            //It must be serialized before it can be sent. 
            //string EO = JsonSerializer.Serialize(exampleObject);    
            string EO = JsonSerializer.Serialize(exampleObject);
            Console.WriteLine(EO);
            //It's valid Json!
            
            var pantry = new Pantry();
            var pInfo = pantry.GetPantry(yourPantryIdHere);
            Console.WriteLine("Your pantry is called " + pInfo.name);
            Console.WriteLine("Your pantry is " + pInfo.percentFull + "% full.");
            Console.WriteLine("You have " + pInfo.baskets.Count + " baskets.");
            Console.WriteLine("Your Baskets:");
            foreach (var basket in pInfo.baskets)
            {
                Console.WriteLine(basket);
            }

            if (pInfo.baskets.Contains(basket_to_mess_with))
            {
                Console.WriteLine("Deleting " + basket_to_mess_with);
                Console.WriteLine(pantry.DeleteBasket(basket_to_mess_with, yourPantryIdHere, true));
            }
            
            Console.WriteLine("Creating a basket called " + basket_to_mess_with);
            Console.WriteLine(pantry.CreateBasket(basket_to_mess_with, EO, yourPantryIdHere));
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Updating a value in that basket");
            exampleObject = new Tuple<string, string>("Item1", "Updated!");
            
            EO = JsonSerializer.Serialize(exampleObject);
            
            Console.WriteLine(pantry.UpdateBasket(basket_to_mess_with, EO, yourPantryIdHere));
            
            Console.WriteLine("Reading back the Basket");
            Console.WriteLine(pantry.GetBasket(basket_to_mess_with, yourPantryIdHere));
            
            Console.WriteLine("Press Any Key to close");
            Console.ReadKey();
        }
    }
}