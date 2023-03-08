using System;
using System.Linq.Expressions;

namespace ECT
{
    public class ECTDynamicConstructor<TReturn> : IDynamicConstructor
    {
        public ECTDynamicConstructor(Type type)
        {
            Type = type;
        }

        public Type Type { get; }
        
        Func<TReturn> ConstructorFunc
        {
            get
            {
                if (constructorFunc == null) Compile();

                return constructorFunc;
            }
        }

        Func<TReturn> constructorFunc;

        void Compile()
        {
            NewExpression constructorExpression = Expression.New(Type);
            
            Expression conversionExpression = Expression.Convert(constructorExpression, typeof(TReturn));
            
            Expression<Func<TReturn>> lambdaExpression = Expression.Lambda<Func<TReturn>>(conversionExpression);
            
            constructorFunc = lambdaExpression.Compile();
        }

        object IDynamicConstructor.Create() => Create();
        public TReturn Create()
        {
            return ConstructorFunc.Invoke();
        }
    }

    public interface IDynamicConstructor
    {
        Type Type { get; }

        public object Create();
    }
}