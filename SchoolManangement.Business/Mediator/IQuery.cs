using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.Mediator
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
