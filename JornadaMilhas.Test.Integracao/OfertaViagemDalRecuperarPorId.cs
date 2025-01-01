using JornadaMilhas.Dados;
using Xunit.Abstractions;

namespace JornadaMilhas.Test.Integracao;

//O AssemblyFixture permite que seja compartilhada uma única instância de um fixture entre todas as classes de teste em um assembly de testes. Essas instâncias são criadas uma vez antes de qualquer teste e limpas após a conclusão de todos os testes. A maior diferença desta solução para o CollectionFixture que aplicamos anteriormente é que o AssemblyFixture permite que as fixtures sejam usadas em vários testes simultâneamente e por isso devem ser utilizadas tendo em mente esse requisito.
//https://xunit.net/docs/shared-context#assembly-fixture


//CollectionFixture compartilha o recurso para outras classes

[Collection(nameof(ContextCollection))]
public class OfertaViagemDalRecuperarPorId 
{
    private readonly JornadaMilhasContext context;

    public OfertaViagemDalRecuperarPorId(ITestOutputHelper output,
      ContextFixture fixture)
    {
        context = fixture.Context;
        output.WriteLine(context.GetHashCode().ToString());
    }

    [Fact]
    public void RetornaNuloQuandoIdInexistente()
    {
        //arrange
        var dal = new OfertaViagemDAL(context);
        //act
        var ofertaRecuperada = dal.RecuperarPorId(-2);
        //assert
        Assert.Null(ofertaRecuperada);
    }
}
