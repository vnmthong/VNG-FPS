using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PYDFramework
{
    public class AudioConfig
    {
        public string path;
        public float volume;

        public AudioConfig(string path, float volume)
        {
            this.path = path;
            this.volume = volume;
        }
    }
}
