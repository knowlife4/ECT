using System;
using System.Linq.Expressions;

namespace ECT
{
    public class ECTDynamicConstructor<TReturn>
    {
        public ECTDynamicConstructor(Type type)
        {
            Type = type;
        }

        Type Type { get; }
        
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
            
            Expression<Func<TReturn>> lambdaExpression = Expression.Lambda<Func<TReturn>>(constructorExpression);
            
            constructorFunc = lambdaExpression.Compile();
        }

        public TReturn Create()
        {
            return ConstructorFunc.Invoke();
        }
    }
}