using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test.Integracao;

[Collection(nameof(ContextCollection))]
public class OfertaViagemDalRecuperaMaiorDesconto : IDisposable
{
    private readonly JornadaMilhasContext context;
    private readonly ContextFixture fixture;

    public OfertaViagemDalRecuperaMaiorDesconto(ContextFixture fixture)
    {
        context = fixture.Context;
        this.fixture = fixture;
    }

    //utilizado como Teardown pois temos dois testes dentro da mesma conexão e na mesma tabela
    //com isso não gera conflito nos testes, caso n tivesse haveria conflito entre ambos os testes
    //por serem na mesma tabela
    //poderia ser utilizado a biblioteca Respawn
    //https://medium.com/@kova98/easy-test-database-reset-in-net-with-respawn-d5a59f995e9d
    public void Dispose()
    {
        fixture.LimpaDadosBanco();
    }

    [Fact]
    // destino = são paulo, desconto = 40, preco = 80
    public void RetornaOfertaEspecificaQuandoDestinoSaoPauloEDesconto40()
    {
        //arrange
        fixture.CriaDadosFake();
        var rota = new Rota("Curitiba", "São Paulo");
        Periodo periodo = new PeriodoDataBuilder() { DataInicial = new DateTime(2024, 5, 20) }.Build();
       
        var ofertaEscolhida = new OfertaViagem(rota, periodo, 80)
        {
            Desconto = 40,
            Ativa = true
        };

        var dal = new OfertaViagemDAL(context);
        dal.Adicionar(ofertaEscolhida);

        Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");
        var precoEsperado = 40;

        //act
        var oferta = dal.RecuperaMaiorDesconto(filtro);

        //assert
        Assert.NotNull(oferta);
        Assert.Equal(precoEsperado, oferta.Preco, 0.0001);
    }

    [Fact]
    public void RetornaOfertaEspecificaQuandoDestinoSaoPauloEDesconto60()
    {
        //arrange
        fixture.CriaDadosFake();
        var rota = new Rota("Curitiba", "São Paulo");
        Periodo periodo = new PeriodoDataBuilder() { DataInicial = new DateTime(2024, 5, 20) }.Build();

        var ofertaEscolhida = new OfertaViagem(rota, periodo, 80)
        {
            Desconto = 20,
            Ativa = true
        };

        var dal = new OfertaViagemDAL(context);
        dal.Adicionar(ofertaEscolhida);

        Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");
        var precoEsperado = 20;

        //act
        var oferta = dal.RecuperaMaiorDesconto(filtro);

        //assert
        Assert.NotNull(oferta);
        Assert.Equal(precoEsperado, oferta.Preco, 0.0001);
    }

}
