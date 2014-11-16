using System.Linq.Expressions;
using System.Reflection;

namespace FusionMVVM.Common
{
    internal class Activator<T>
    {
        public delegate T ObjectActivator(params object[] args);

        /// <summary>
        /// Create a new instance of the given ConstructorInfo.
        /// </summary>
        /// <param name="constructor"></param>
        /// <returns></returns>
        public static ObjectActivator GetActivator(ConstructorInfo constructor)
        {
            var paramsInfo = constructor.GetParameters();

            // Create a single parameter of type object[].
            var parameters = Expression.Parameter(typeof(object[]), "args");
            var arguments = new Expression[paramsInfo.Length];

            // Pick each argument from the parameters array and create a typed expression of them.
            for (var i = 0; i < paramsInfo.Length; i++)
            {
                var index = Expression.Constant(i);
                var parameterType = paramsInfo[i].ParameterType;
                var parameterAccessor = Expression.ArrayIndex(parameters, index);
                var parameterCast = Expression.Convert(parameterAccessor, parameterType);

                arguments[i] = parameterCast;
            }

            // Make a new expression that calls the constructor with the arguments we just created.
            var expression = Expression.New(constructor, arguments);

            // Create a lambda with the new expression as body and our parameter object[] as argument.
            var lambda = Expression.Lambda(typeof(ObjectActivator), expression, parameters);

            // Return the compiled lambda.
            return (ObjectActivator)lambda.Compile();
        }
    }
}
