using System;
using System.Reflection;

namespace Infrastructure.Aspects
{
    //THIS NAMESPACE IS WORK-IN-PROGRESS

    internal abstract class AspectAttribute : Attribute
    {
        public Type InterceptorType { get; set; }

        protected AspectAttribute(Type interceptorType)
        {
            InterceptorType = interceptorType;
        }
    }

    internal interface IInterceptionContext
    {
        object Target { get; }

        MethodInfo Method { get; }

        object[] Arguments { get; }

        object ReturnValue { get; }

        bool Handled { get; set; }
    }

    internal interface IBeforeExecutionInterceptionContext : IInterceptionContext
    {

    }


    internal interface IAfterExecutionInterceptionContext : IInterceptionContext
    {
        Exception Exception { get; }
    }

    internal interface IInterceptor<TAspect>
    {
        TAspect Aspect { get; set; }

        void BeforeExecution(IBeforeExecutionInterceptionContext context);

        void AfterExecution(IAfterExecutionInterceptionContext context);
    }

    internal abstract class InterceptorBase<TAspect> : IInterceptor<TAspect>
    {
        public TAspect Aspect { get; set; }

        public virtual void BeforeExecution(IBeforeExecutionInterceptionContext context)
        {
        }

        public virtual void AfterExecution(IAfterExecutionInterceptionContext context)
        {
        }
    }

    internal class Test_Aspects
    {
        internal class MyAspectAttribute : AspectAttribute
        {
            public int TestValue { get; set; }

            public MyAspectAttribute(): base(typeof(MyInterceptor))
            {
            }
        }

        internal class MyInterceptor : InterceptorBase<MyAspectAttribute>
        {
            public override void BeforeExecution(IBeforeExecutionInterceptionContext context)
            {
                Aspect.TestValue++;
            }

            public override void AfterExecution(IAfterExecutionInterceptionContext context)
            {
                Aspect.TestValue++;
            }
        }

        public class MyService
        {
            [MyAspect(TestValue = 41)] //Usage!
            public void DoIt()
            {

            }
        }

        public class MyClient
        {
            private readonly MyService _service;

            public MyClient(MyService service)
            {
                _service = service;
            }

            public void Test()
            {
                _service.DoIt();
            }
        }
    }
}
