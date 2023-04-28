using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpansionForCAndS
{
    public abstract class SingletonClass<T> where T : class, new()
    {
        private static T _instance;
        public static T Instance => (T)_instance ?? (_instance = new T());
    }
}
