using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Common
{
    public class Entity
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id
        {
            get;
            set;
        }
        
        /// <summary>
        /// Determines whether entity is new.
        /// </summary>
        [Bindable(false)]
        [ScaffoldColumn(false)]
        public virtual bool IsNew
        {
            get
            {
                return Id == Guid.Empty;
            }
        }

        /// <summary>
        /// EF object state.
        /// </summary>
        [Bindable(false)]
        [ScaffoldColumn(false)]
        [NotMapped]
        public virtual State State
        {
            get;
            set;
        }
    }
}