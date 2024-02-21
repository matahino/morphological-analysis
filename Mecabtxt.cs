using System;
using System.IO;
using MeCab;

namespace test
{
    public class MeCabtxt : IMeCabtxt
    {
        public string InputText { get; set; }
        public string OutputText { get; set; }

        public void PerformMecab(string input, string output)
        {
            MeCabParam param = new MeCabParam();
            MeCabTagger tagger = null;

            try
            {
                tagger = MeCabTagger.Create(param); // Moved inside try to ensure disposal in case of initialization failure.
                string inputText = File.ReadAllText(input); // Reads text from the input file.
                MeCabNode node = tagger.ParseToNode(inputText);
                
                using (StreamWriter writer = new StreamWriter(output, false, System.Text.Encoding.UTF8)) // Ensures proper encoding and resource management.
                {
                    while (node != null)
                    {
                        // Checks if the node is meaningful to avoid writing empty nodes at the start and end.
                        if (node.CharType > 0)
                        {
                            writer.WriteLine("{0}\t{1}", node.Surface, node.Feature);
                        }
                        node = node.Next;
                    }
                }
                Console.WriteLine("形態素解析が完了し、結果をファイルに書き込みました。");
            }
            catch (Exception ex)
            {
                Console.WriteLine("エラー: " + ex.Message);
            }
            finally
            {
                tagger?.Dispose(); // Safely disposes the tagger if it has been initialized.
            }
        }
    }
}
