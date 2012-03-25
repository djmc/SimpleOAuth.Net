using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleOAuthTester.WP.Mango.Classes
{
    public class Messenger
    {
        private static object _locker = new object();
        private readonly static Dictionary<Type, List<object>> _dictionary;

        static Messenger()
        {
            _dictionary = new Dictionary<Type, List<object>>();
        }

        public static void Subscribe<T>(Action<T> action)
        {
            lock (_locker)
            {
                var actionType = typeof(T);

                if (!_dictionary.ContainsKey(actionType))
                {
                    var list = new List<object>();
                    _dictionary.Add(actionType, list);
                }

                _dictionary[actionType].Add(action);
            }
        }

        public static void Send<T>(T message)
        {
            lock (_locker)
            {
                var actionType = typeof(T);

                if (_dictionary.ContainsKey(actionType))
                {
                    var list = _dictionary[actionType].Select(x => (Action<T>)x);

                    foreach (var command in list)
                    {
                        command(message);
                    }
                }
            }
        }
    }
}
