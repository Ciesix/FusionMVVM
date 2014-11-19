using System;
using System.Diagnostics;

namespace Application
{
    public partial class App
    {
        /// <summary>
        /// Initializes a new instance of the App class.
        /// </summary>
        public App()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (Debugger.IsAttached) Debugger.Break();
            }
        }
    }
}
