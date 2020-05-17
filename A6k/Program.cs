using System;

using OpenToolkit.Core;
using OpenToolkit.Windowing.Desktop;

namespace A6k
{
    public class Program

    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (Game game = new Game())
            {
                //Run takes a double, which is how many frames per second it should strive to reach.
                //You can leave that out and it'll just update as fast as the hardware will allow it.
                game.Run();
            }
        }

    }
}
