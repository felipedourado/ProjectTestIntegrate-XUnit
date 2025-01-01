using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;
using Xunit.Abstractions;

namespace JornadaMilhas.Test.Integracao;

//teste without database with inmemory provider
//https://learn.microsoft.com/pt-br/ef/core/testing/testing-without-the-database#inmemory-provider
//Boas praticas de como escrever testes unitarios
//https://learn.microsoft.com/pt-br/dotnet/core/testing/unit-testing-best-practices#prefer-helper-methods-to-setup-and-teardown
//Um pouco sobre:
//Setup: O setup � a etapa em que voc� prepara o ambiente de teste antes da execu��o de cada teste. Isso pode incluir a cria��o de objetos necess�rios, a configura��o de vari�veis de ambiente, a inicializa��o de bancos de dados de teste, entre outras tarefas. O objetivo do setup � garantir que o ambiente de teste esteja pronto e configurado corretamente para a execu��o do teste.
//Teardown: O teardown � a etapa em que voc� limpa e restaura o ambiente de teste para o seu estado original ap�s a execu��o de cada teste.Isso pode incluir a exclus�o de objetos criados durante o setup, a limpeza de bancos de dados de teste, a restaura��o de vari�veis de ambiente, entre outras tarefas.O objetivo do teardown � garantir que o ambiente de teste retorne ao seu estado original ap�s a execu��o do teste, evitando assim a interfer�ncia entre os testes.

//ClassFixture compartilha recursos dentro da mesma classe
public class OfertaViagemDalAdicionar : IClassFixture<ContextFixture>
{
    private readonly JornadaMilhasContext context;

    //Para cada teste � criado uma nova instancia padr�o do XUnit
    //utilizando o fixture mantenho criado apenas uma conex�o para todos os testes
    //evitando honerar o recurso de conex�o e otimizando
    public OfertaViagemDalAdicionar(ITestOutputHelper output, ContextFixture fixture)
    {
        context = fixture.Context;
        output.WriteLine(context.GetHashCode().ToString());
    }

    [Fact]
    public void RegistraOfertaNoBanco()
    {

        //arrange
        Rota rota = new("S�o Paulo", "Fortaleza");
        Periodo periodo = new(new DateTime(2024, 8, 20), new DateTime(2024, 8, 30));
        double preco = 350;

        var oferta = new OfertaViagem(rota, periodo, preco);
        var dal = new OfertaViagemDAL(context);

        //act
        dal.Adicionar(oferta);

        //assert
        var ofertaIncluida = dal.RecuperarPorId(oferta.Id);
        Assert.NotNull(ofertaIncluida);
        Assert.Equal(ofertaIncluida.Preco, oferta.Preco, 0.001);
    }

    [Fact]
    public void RegistraOfertaNoBancoComInformacoesCorretas()
    {
        //arrange
        Rota rota = new("S�o Paulo", "Fortaleza");
        Periodo periodo = new(new DateTime(2024, 8, 20), new DateTime(2024, 8, 30));
        double preco = 350;

        var oferta = new OfertaViagem(rota, periodo, preco);
        var dal = new OfertaViagemDAL(context);

        //act
        dal.Adicionar(oferta);

        //assert
        var ofertaIncluida = dal.RecuperarPorId(oferta.Id);
        Assert.Equal(ofertaIncluida.Rota.Origem, oferta.Rota.Origem);
        Assert.Equal(ofertaIncluida.Rota.Destino, oferta.Rota.Destino);
        Assert.Equal(ofertaIncluida.Periodo.DataInicial, oferta.Periodo.DataInicial);
        Assert.Equal(ofertaIncluida.Periodo.DataFinal, oferta.Periodo.DataFinal);
        Assert.Equal(ofertaIncluida.Preco, oferta.Preco, 0.001);
    }
}