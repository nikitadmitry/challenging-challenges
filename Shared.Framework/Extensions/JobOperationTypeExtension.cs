using System;

namespace Shared.Framework.Extensions
{
    public static class JobOperationTypeExtension
    {
        public static string ToJobOperationType(this Type jobType)
        {
            return jobType.FullName;
        }
    }
}