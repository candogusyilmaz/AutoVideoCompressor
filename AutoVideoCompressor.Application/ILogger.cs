using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoVideoCompressor.Application;
public interface ILogger
{
    void Info(string message);
    void Final(string message);
}
