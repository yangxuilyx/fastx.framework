using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace FastX.Authorization.Extensions
{
    /// <summary>
    /// AuthorizationOptionsExtensions
    /// </summary>
    public static class AuthorizationOptionsExtensions
    {
        private static readonly PropertyInfo PolicyMapProperty = typeof(AuthorizationOptions)
            .GetProperty("PolicyMap", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Gets all policies.
        ///
        /// IMPORTANT NOTE: Use this method carefully.
        /// It relies on reflection to get all policies from a private field of the <paramref name="options"/>.
        /// This method may be removed in the future if internals of <see cref="AuthorizationOptions"/> changes.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static List<string> GetPoliciesNames(this AuthorizationOptions options)
        {
            return ((IDictionary<string, AuthorizationPolicy>)PolicyMapProperty.GetValue(options))?.Keys.ToList();
        }
    }
}
