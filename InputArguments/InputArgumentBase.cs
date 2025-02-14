namespace Tools.InputArguments
{
    public abstract class InputArgumentBase
    {
        public string Name;
        public string Description;

        public InputArgumentBase(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
