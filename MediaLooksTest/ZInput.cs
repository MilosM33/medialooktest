﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPLATFORMLib;
namespace MediaLooksTest
{
    internal class ZInput
    {

        private MFileClass _source;
        public ZInput(String path)
        {
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