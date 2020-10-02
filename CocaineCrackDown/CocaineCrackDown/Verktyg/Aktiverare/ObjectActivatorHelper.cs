using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CocaineCrackDown.Verktyg.Aktiverare {

    public static class ObjectActivatorHelper {

        public delegate T ObjectActivator<T>(params object[] args);

        public static ObjectActivator<T> GetActivator<T>(ConstructorInfo ctor) {
            Type type = ctor.DeclaringType;
            ParameterInfo[] paramsInfo = ctor.GetParameters();

            //create a single param of type object[]
            ParameterExpression param =
                Expression.Parameter(typeof(object[]) , "args");

            Expression[] argsExp =
                new Expression[paramsInfo.Length];

            //pick each arg from the params array
            //and create a typed expression of them
            for(int i = 0; i < paramsInfo.Length; i++) {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp =
                    Expression.ArrayIndex(param , index);

                argsExp[i] = Expression.Convert(paramAccessorExp , paramType);
            }

            //make a NewExpression that calls the
            //ctor with the args we just created
            NewExpression newExp = Expression.New(ctor , argsExp);

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            LambdaExpression lambda =
                Expression.Lambda(typeof(ObjectActivator<T>) , newExp , param);

            //compile it
            return (ObjectActivator<T>)lambda.Compile();
        }
    }
}