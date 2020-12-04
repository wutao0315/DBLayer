using System;
using System.Linq.Expressions;

namespace DBLayer.Core
{
    public class OrderExpression<T> where T : new()
    {
        public OrderExpression() { }

        public OrderExpression<T> OrderBy(Expression<Func<T, object>> expression) { 
            var result=new OrderExpression<T>();
            return result;
        }
        public OrderExpression<T> OrderByDesc(Expression<Func<T, object>> expression) {
            var result = new OrderExpression<T>();
            return result;
        }
    }
}
