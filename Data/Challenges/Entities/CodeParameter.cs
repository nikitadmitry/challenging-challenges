using Data.Challenges.Enums;
using Data.Common;

namespace Data.Challenges.Entities
{
    public class CodeParameter : Entity
    {
        public string Value
        {
            get;
            set;
        }

        public CodeParameterType Type
        {
            get;
            set;
        }
    }
}