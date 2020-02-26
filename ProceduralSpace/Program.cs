using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralSpace
{
    enum State
    {
        Galaxy,
        System
    }

    class Program
    {
        private UPoint playerLocation;
        private State state;
        static void Main(string[] args)
        {
            var p = new Program();
            p.Run();
        }

        public Program()
        {
            // Initialise player's location
            var r = new Random();
            playerLocation = new UPoint((uint)r.Next(int.MaxValue), (uint)r.Next(int.MaxValue));
            state = State.Galaxy;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                if (state == State.Galaxy)
                {

                    RenderGalaxy();
                }
                else
                {
                    RenderSystem();
                }

                HandleUserInput();
            }
        }

        public void RenderGalaxy()
        {
            var windowSize = new UPoint(80, 30);
            
            var windowOrigin = new UPoint(
                playerLocation.X - (windowSize.X / 2),
                playerLocation.Y - (windowSize.Y / 2));

            Console.WriteLine("Navigate the galaxy using the cursor keys.");
            Console.WriteLine("Press Enter to view system details.");
            Console.WriteLine();
            Console.WriteLine($"Current Location: X:{playerLocation.X}, Y:{playerLocation.Y}");
            Console.WriteLine();

            for (uint y = 0; y < windowSize.Y; y++)
            {
                for (uint x = 0; x < windowSize.X; x++)
                {
                    var currentUPoint = new UPoint(
                        x + windowOrigin.X,
                        y + windowOrigin.Y);
                    var zone = new Zone(currentUPoint);

                    if(currentUPoint.X == playerLocation.X &&
                       currentUPoint.Y == playerLocation.Y)
                    {
                        Console.Write(zone.HasStar ? "@" : "@");
                    }
                    else
                    {
                        Console.Write(zone.HasStar  ? "*" : " ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            var current = new Zone(playerLocation);
            if (current.HasStar)
            {
                Console.WriteLine(current.Star!);
                if (current.Star!.HasPlanets)
                {
                    Console.WriteLine(" - Planetary bodies found.");
                }
            }
        }

        public void RenderSystem()
        {
            Console.WriteLine("Press Enter to view galaxy map.");
            Console.WriteLine();
            Console.WriteLine($"Current Location: X:{playerLocation.X}, Y:{playerLocation.Y}");
            Console.WriteLine();
            var zone = new Zone(playerLocation);
            if(!zone.HasStar)
            {
                Console.WriteLine("No star system at this location.");
                return;
            }
            Console.WriteLine(zone.Star!);
            if (!zone.Star!.HasPlanets)
            {
                Console.WriteLine(" - No planetary bodies.");
                return;
            }
            foreach (var planet in zone.Star!.Planets!)
            {
                Console.WriteLine($" - {planet.ToString()}");
                if (planet.HasMoons)
                {
                    foreach (var moon in planet.Moons!)
                    {
                        Console.WriteLine($"    - {moon.ToString()}");
                    }
                }
            }
        }

        private void HandleUserInput()
        {
            var key = Console.ReadKey();
            if (state == State.Galaxy)
            {
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        playerLocation = new UPoint(playerLocation.X - 1, playerLocation.Y);
                        break;
                    case ConsoleKey.RightArrow:
                        playerLocation = new UPoint(playerLocation.X + 1, playerLocation.Y);
                        break;
                    case ConsoleKey.UpArrow:
                        playerLocation = new UPoint(playerLocation.X, playerLocation.Y - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        playerLocation = new UPoint(playerLocation.X, playerLocation.Y + 1);
                        break;
                    case ConsoleKey.Enter:
                        state = State.System;
                        break;
                    default:
                        break;
                }
            }
            else if (state == State.System)
            {
                if (key.Key == ConsoleKey.Enter)
                {
                    state = State.Galaxy;
                }
            }
        }

    }
}
