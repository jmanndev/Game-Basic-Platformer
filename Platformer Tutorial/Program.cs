using System;

namespace Platformer_Tutorial
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SimplePlatformer game = new SimplePlatformer())
            {
                game.Run();
            }
        }
    }
#endif
}

