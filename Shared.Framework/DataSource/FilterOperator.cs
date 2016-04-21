namespace Shared.Framework.DataSource
{
    /// <summary>
    /// Specifies filter operator
    /// </summary>
    public enum FilterOperator
    {
        /// <summary>
        /// Represents IsLessThan operator
        /// </summary>
        IsLessThan = 10,

        /// <summary>
        /// Represents IsLessThanOrEqualTo operator
        /// </summary>
        IsLessThanOrEqualTo = 20,

        /// <summary>
        /// Represents IsEqualTo operator
        /// </summary>
        IsEqualTo = 30,

        /// <summary>
        /// Represents IsNotEqualTo operator
        /// </summary>
        IsNotEqualTo = 40,

        /// <summary>
        /// Represents IsGreaterThan operator
        /// </summary>
        IsGreaterThanOrEqualTo = 50,

        /// <summary>
        /// Represents IsGreaterThan operator
        /// </summary>
        IsGreaterThan = 60,

        /// <summary>
        /// Represents StartsWith operator
        /// </summary>
        StartsWith = 70,

        /// <summary>
        /// Represents EndsWith operator
        /// </summary>
        EndsWith = 80,

        /// <summary>
        /// Represents Contains operator
        /// </summary>
        Contains = 90,

        /// <summary>
        /// Represents IsContainedIn operator
        /// </summary>
        IsContainedIn = 100,

        /// <summary>
        /// Represents DoesNotContain operator
        /// </summary>
        DoesNotContain = 110,

        Like = 120,

        IsEqualToOrNull = 130,

        
        IsNotContainedIn = 140,

        IsGreaterThanOrNull = 150,

        IsGreaterThanOrEqualToOrNull = 160,


    }
}