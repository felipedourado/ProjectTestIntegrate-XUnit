using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;
using Xunit.Abstractions;

namespace JornadaMilhas.Test.Integracao;

//teste without database with inmemory provider
//https://learn.microsoft.com/pt-br/ef/core/testing/testing-without-the-database#inmemory-provider
//Boas praticas de como escrever testes unitarios
//https://learn.microsoft.com/pt-br/dotnet/core/testing/unit-testing-best-practices#prefer-helper-methods-to-setup-and-teardown
//Um pouco sobre:
//Setup: O setup é a etapa em que você prepara o ambiente de teste antes da execução de cada teste. Isso pode incluir a criação de objetos necessários, a configuração de variáveis de ambiente, a inicialização de bancos de dados de teste, entre outras tarefas. O objetivo do setup é garantir que o ambiente de teste esteja pronto e configurado corretamente para a execução do teste.
//Teardown: O teardown é a etapa em que você limpa e restaura o ambiente de teste para o seu estado original após a execução de cada teste.Isso pode incluir a exclusão de objetos criados durante o setup, a limpeza de bancos de dados de teste, a restauração de variáveis de ambiente, entre outras tarefas.O objetivo do teardown é garantir que o ambiente de teste retorne ao seu estado original após a execução do teste, evitando assim a interferência entre os testes.

//ClassFixture compartilha recursos dentro da mesma classe
public class OfertaViagemDalAdicionar : IClassFixture<ContextFixture>
{
    private readonly JornadaMilhasContext context;

    //Para cada teste é criado uma nova instancia padrão do XUnit
    //utilizando o fixture mantenho criado apenas uma conexão para todos os testes
    //evitando honerar o recurso de conexão e otimizando
    public OfertaViagemDalAdicionar(ITestOutputHelper output, ContextFixture fixture)
    {
        context = fixture.Context;
        output.WriteLine(context.GetHashCode().ToString());
    }

    [Fact]
    public void RegistraOfertaNoBanco()
    {

        //arrange
        Rota rota = new("São Paulo", "Fortaleza");
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
        Rota rota = new("São Paulo", "Fortaleza");
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