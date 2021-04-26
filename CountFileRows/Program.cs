using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace CountFileRows
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Task<string> task = GetFileRowCountAllAsync(args);
            Clipboard.SetText(task.Result);
        }


        static async Task<string> GetFileRowCountAllAsync(string[] args)
        {
            string[] fileRowCounts = await Task.WhenAll(args.OrderBy(x => x).Select(GetFileRowCountAsync));
            return string.Join("\r\n", fileRowCounts);
        }

        static async Task<string> GetFileRowCountAsync(string path)
        {
            if (!File.Exists(path))
            {
                return path;
            }

            using (var stream = File.Open(path, FileMode.Open))
            {
                var tr = new StreamReader(stream);
                var count = 0;
                while (true)
                {
                    var line = await tr.ReadLineAsync();
                    if (line == null)
                    {
                        break;
                    }
                    count++;
                }
                return $"{path}\t{Path.GetFileName(path)}\t{count}";
            }
        }
    }
}
