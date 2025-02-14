using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.InputArguments
{
    public class ArgumentsCollection
    {
        private InputArgument _help;

        private List<InputArgumentBase> _arguments = new List<InputArgumentBase>();
        private HashSet<string> _requiredArguments = new HashSet<string>();
        private Dictionary<string, List<string>> _aliases = new Dictionary<string, List<string>>();

        public ArgumentsCollection()
        {
            _help = new InputArgument("-h", "Show help.", delegate
            {
                foreach (var prop in _arguments)
                    Console.WriteLine($"{prop.Name} {(_requiredArguments.Contains(prop.Name) ? "(required)" : "")} {(_aliases[prop.Name].Count > 0 ? $"(or use: {string.Join(",", _aliases[prop.Name])})" : "")} : {prop.Description}");
                Environment.Exit(0);
            });

            AddParameter(_help);
        }

        public void AddParameter(InputArgumentBase par, bool required = false, params string[] aliases)
        {
            _arguments.Add(par);
            if (required)
                _requiredArguments.Add(par.Name);
            _aliases[par.Name] = new List<string>();
            if (aliases != null)
                _aliases[par.Name].AddRange(aliases);
        }

        public InputArgumentBase FindParameter(string name)
        {
            return _arguments.Find((InputArgumentBase p) => p.Name == name);
        }

        public bool ProcessArguments(string[] args)
        {
            var required = new HashSet<string>(_requiredArguments);

            if (args.Length == 0)
            {
                _help.Process();
                return false;
            }
            else for (int i = 0; i < args.Length; ++i)
                {
                    var arg = args[i].ToLower();

                    var exactPar = FindParameter(arg);
                    if (exactPar == null)
                    {
                        _help.Process();
                        return false;
                    }

                    required.Remove(arg);
                    foreach (var alias in _aliases[arg])
                        required.Remove(alias);

                    if (exactPar is InputArgument inputArgument)
                        inputArgument.Process();
                    else if (exactPar is IInputArgumentWithInput inputArgumentWithInput)
                    {
                        if (++i == args.Length)
                        {
                            Console.WriteLine("Parameter input error!");
                            _help.Process();
                            return false;
                        }
                        (exactPar as IInputArgumentWithInput).Process(args[i]);
                    }
                }

            if (required.Count > 0)
            {
                foreach (var arg in required)
                    Console.WriteLine($"Need required argument '{arg}'");
                _help.Process();
                return false;
            }

            return true;
        }
    }
}
