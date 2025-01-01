using Bogus;
using JornadaMilhasV1.Modelos;
namespace JornadaMilhas.Test.Integracao;

//DataBuilder
//O padrão Data Builder é um padrão de projeto que pode ser utilizado em testes de integração(e também em testes unitários) para facilitar a criação de dados de teste complexos de forma eficiente e legível.Ele é particularmente útil quando os dados de teste têm uma estrutura complexa e/ou requerem muitos atributos para serem configurados.
//A ideia básica por trás do padrão Data Builder é criar um construtor de objetos que permite configurar facilmente os atributos do objeto de teste.Além disso, se a estrutura dos objetos de dados mudar no futuro, só precisamos ajustar o construtor correspondente, mantendo os testes de integração relativamente independentes de mudanças na implementação interna das classes.
//Para conhecer mais sobre o padrão de projeto data builder, suas implicações e outras possibilidades de utilização
//https://refactoring.guru/pt-br/design-patterns/builder
public class PeriodoDataBuilder : Faker<Periodo>
{
    public DateTime? DataInicial { get; set; }
    public DateTime? DataFinal { get; set; }
    public PeriodoDataBuilder()
    {
        CustomInstantiator(f =>
        {
            DateTime dataInicio = DataInicial ?? f.Date.Soon();
            DateTime dataFinal = DataFinal ?? dataInicio.AddDays(30);
            return new Periodo(dataInicio, dataFinal);
        });
    }

    public Periodo Build() => Generate();
}
