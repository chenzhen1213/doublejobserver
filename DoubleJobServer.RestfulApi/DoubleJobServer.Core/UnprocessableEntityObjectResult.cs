using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace DoubleJobServer.Core
{
    //验证规则定义完了，下面来实施规则检查。这时就需要使用ModelState了。
    //每当请求进入到这个方法的时候，都会验证我们刚刚定义在Resource上的这些约束，如果其中一个约束没有达标，则ModelState的IsValid属性就会是false；此外如果传进来的属性类型和定义的不符，IsValid属性也会是false。
    //这里返回状态码 422 是正确的选择，但是 422 要求请求的body的语法必须是正确的，不能是null，所以前面检查是否为null的代码还需要保留。
    //由于ASP.NET Core并没有内置的帮助方法可以返回422和验证错误信息，所以我们先建立一个类用于返回 422 和验证错误信息，它继承于ObjectResult：
    //其中的SerializableError定义了一个可以被串行化的容器，该容器可以以Key-Value对的形式来保存ModelState的信息。
    class UnprocessableEntityObjectResult : ObjectResult
    {
        public UnprocessableEntityObjectResult(ModelStateDictionary modelState)
            : base(new SerializableError(modelState))
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            StatusCode = 422;
        }
    }
}
