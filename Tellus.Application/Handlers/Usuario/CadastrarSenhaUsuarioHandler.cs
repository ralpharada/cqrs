﻿using Tellus.Application.Crypto;
using Tellus.Application.Queries;
using Tellus.Application.Util;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class CadastrarSenhaUsuarioHandler : IRequestHandler<CadastrarSenhaUsuarioQuery, IEvent>
    {
        private readonly IUsuarioRepository _repository;
        private readonly EnviarEmail _enviarEmail;

        public CadastrarSenhaUsuarioHandler(IUsuarioRepository repository,
            EnviarEmail enviarEmail)
        {
            _repository = repository;
            _enviarEmail = enviarEmail;
        }

        public async Task<IEvent> Handle(CadastrarSenhaUsuarioQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                var usuario = await _repository.ObterPorHashAtivacaoCadastro(request.Hash, cancellationToken);
                if (usuario != null)
                {
                    if (request.NovaSenha == request.ConfirmarSenha)
                    {

                        usuario.Senha = Criptografia.Encrypt(request.NovaSenha);
                        usuario.HashAtivacaoCadastro = String.Empty;
                        usuario.DataAtivacaoCadastro = DateTime.UtcNow;
                        var result = await _repository.Salvar(usuario, cancellationToken);
                        success = result.ModifiedCount > 0;

                        if (success)
                        {
                            mensagem = "Senha cadastrada com sucesso!";
                            _enviarEmail.Send(usuario.Email, "Atualização de senha", "Seu acesso ao sistema foi atualizado.", null);
                        }
                        else
                        {

                            mensagem = "Falha ao tentar cadastrar a senha!";
                        }
                    }
                    else
                        mensagem = "Verifique os dados de acesso e tente novamente!";
                }
                else
                    mensagem = "É necessário gerar uma nova chave!";
            }
            catch (Exception ex)
            {
                mensagem = ex.Message;
            }
            return new ResultEvent(success, mensagem);
        }

    }
}
