using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Tests.DependencySystem
{
    sealed class TestValueExtension : DependencyExtension
    {
        Single _Value;

        public TestValueExtension(Single value)
        {
            _Value = value;
        }

        protected override DependencyExpression CreateExpression(DependencyProperty property) => new TestValueExpression(property, this);

        class TestValueExpression : DependencyExpression
        {
            public TestValueExpression(DependencyProperty property, DependencyExtension expressionTemplate) 
                : base(property, expressionTemplate)
            {
            }

            protected override void OnInitialize() => ChangeValue(((TestValueExtension)ExpressionTemplate)._Value);

            protected override void OnRelease()
            {
                
            }
        }
    }
}
