using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Data.Challenges.Enums;
using Data.Common;

namespace Data.Challenges.Entities
{
    public class TestCase : Entity
    {
        public ICollection<CodeParameter> CodeParameters
        {
            get;
            set;
        }

        [NotMapped]
        public IEnumerable<CodeParameter> InputParameters
        {
            get
            {
                return CodeParameters.Where(x => x.Type == CodeParameterType.Input);
            }
        }

        [NotMapped]
        public IEnumerable<CodeParameter> OutputParameters
        {
            get
            {
                return CodeParameters.Where(x => x.Type == CodeParameterType.Output);
            }
        }
    }
}