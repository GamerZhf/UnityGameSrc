namespace Service
{
    using System;

    public class SkipOnLineBreaksProcessor : IRTLProcessor
    {
        private const string LINE_BREAK = "\n";
        private readonly char SPLIT = '\n';

        public bool OnEnd(ref string finalString)
        {
            return true;
        }

        public bool OnStart(ref string originalInput)
        {
            if (originalInput.Contains("\n"))
            {
                this.SwitchOrderOfLines(ref originalInput);
                return false;
            }
            return true;
        }

        private void SwitchOrderOfLines(ref string originalInput)
        {
            char[] separator = new char[] { this.SPLIT };
            string[] strArray = originalInput.Split(separator);
            originalInput = string.Empty;
            for (int i = strArray.Length - 1; i >= 0; i--)
            {
                originalInput = originalInput + strArray[i];
                if (i != 0)
                {
                    originalInput = originalInput + this.SPLIT;
                }
            }
        }
    }
}

