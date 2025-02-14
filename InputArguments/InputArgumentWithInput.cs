using System;

namespace Tools.InputArguments
{
    public class InputArgumentWithInput : InputArgumentBase, IInputArgumentWithInput
    {
        System.Action<string> _process;

        public InputArgumentWithInput(string name, string descr, Action<string> process) : base(name, descr)
        {
            _process = process;
        }

        public void Process(string value)
        {
            _process.Invoke(value);
        }
    }
}
