using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtility
{

    public class Singleton<T>
        where T : new()
    {
        private static T instance = new T();
        public static T Get()
        {
            return instance;
        }
        protected Singleton() { }
    }
}
