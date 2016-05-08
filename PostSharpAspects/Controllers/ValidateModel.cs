using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using System;
using System.Web.Mvc;

namespace PostSharpAspects.Controllers
{
    [Serializable]
    public sealed class ValidateModel : OnMethodBoundaryAspect
    {
        public bool UseWebApi { get; }
        
        public ValidateModel(bool useWebApi = false)
        {
            UseWebApi = useWebApi;
        } 

        public override void OnEntry(MethodExecutionArgs args)
        {
        }
    }

}