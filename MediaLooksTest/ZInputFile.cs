using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPLATFORMLib;
using System.IO;
namespace MediaLooksTest
{
    internal class ZInputFile
    {

        private MFileClass _source;
        public ZInputFile(String fileName)
        {
            String path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Videos\" + fileName);
            _source = new MFileClass();
            _source.FileNameSet(path, "");
            _source.PropsSet("loop", "true");
            _source.FilePlayStart();
        }

        public MFileClass GetSource()
        {
            return _source;
        }
    }
}
