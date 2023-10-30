﻿using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ValidarEsqueciSenhaUsuarioAdmViaLinkQuery : Request<IEvent>
    {
        public string Parametro { get; private set; }
        public ValidarEsqueciSenhaUsuarioAdmViaLinkQuery(string parametro)
        {
            Parametro = parametro;
        }
    }
}
