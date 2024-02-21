namespace test
{
    public interface IMeCabtxt
    {
        string InputText { get; set; }
        string OutputText { get; set; }
        void PerformMecab(string input, string output);
    }
}
