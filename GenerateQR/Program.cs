using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenerateQR.Processor;
using Newtonsoft.Json;

namespace GenerateQR
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static async Task Main(string[] args)
        {
            if (args.Length > 0)
            {
                AllocConsole();
                Console.WriteLine($"Read file:{args[0]}");
                try
                {
                    var outputs = await Task.WhenAll(JsonConvert.DeserializeObject<InputData[]>(File.ReadAllText(args[0]))
                        .Select(async x =>
                        {
                            var pr = new PrintData();
                            await pr.Load(x);
                            var printer = new SinglePdfProcessor(pr);
                            printer.Print();
                            Console.WriteLine(pr.OutputPath);
                            return pr.OutputPath;
                        }));
                    var mp = new MergePdfProcessor(outputs);
                    mp.Merge();
                    Console.WriteLine(mp.OutputPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    Console.ReadKey();
                }
                finally
                {
                    FreeConsole();
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }

        // http://msdn.microsoft.com/en-us/library/ms681944(VS.85).aspx
        /// <summary>
        /// Allocates a new console for the calling process.
        /// </summary>
        /// <returns>nonzero if the function succeeds; otherwise, zero.</returns>
        /// <remarks>
        /// A process can be associated with only one console,
        /// so the function fails if the calling process already has a console.
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int AllocConsole();

        // http://msdn.microsoft.com/en-us/library/ms683150(VS.85).aspx
        /// <summary>
        /// Detaches the calling process from its console.
        /// </summary>
        /// <returns>nonzero if the function succeeds; otherwise, zero.</returns>
        /// <remarks>
        /// If the calling process is not already attached to a console,
        /// the error code returned is ERROR_INVALID_PARAMETER (87).
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int FreeConsole();
    }
}
