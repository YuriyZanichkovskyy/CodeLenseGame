using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.CodeSense;

namespace CodeLenseGame
{
    public class LetsPlayDataPoint : DataPoint<string>
    {
        public async override Task<string> GetDataAsync()
        {
            return await Task.FromResult(string.Empty);
        }
    }
}
