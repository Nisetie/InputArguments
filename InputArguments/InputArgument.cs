namespace Tools.InputArguments
{

    public class InputArgument : InputArgumentBase
    {
        System.Action _process;

        public InputArgument(string name, string descr, System.Action process) : base(name, descr)
        {
            _process = process;
        }

        public void Process()
        {
            _process?.Invoke();
        }
    }
}
