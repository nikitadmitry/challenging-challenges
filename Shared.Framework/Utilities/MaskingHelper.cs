namespace Shared.Framework.Utilities
{
    /// <summary>
    /// Class for masking some fields before output. Like SSN.
    /// </summary>
    public class MaskingHelper
    {
        private readonly char maskSymbol;


        public MaskingHelper(char maskSymbol)
        {
            this.maskSymbol = maskSymbol;
        }

        /// <summary>
        /// Masks input value with specified settings and padding
        /// </summary>
        /// <param name="inputParam">Input string</param>
        /// <param name="paddingLeft">Mask padding from left</param>
        /// <param name="paddingRight">Mask padding from right</param>
        /// <returns></returns>
        public string Mask(string inputParam, int paddingLeft = 0, int paddingRight = 0)
        {
            int maskLength = inputParam.Length - paddingLeft - paddingRight;
            string mask = new string(maskSymbol, maskLength);
            string leftPart = inputParam.Substring(0, paddingLeft);
            string rightPart = string.Empty;
            if (paddingRight > 0)
            {
                rightPart = inputParam.Substring(inputParam.Length - paddingRight, paddingRight);
            }
            return leftPart + mask + rightPart;
        }

        public class Constants
        {
            public const char SecurityNumberMask = 'x';
        }
    }
}
