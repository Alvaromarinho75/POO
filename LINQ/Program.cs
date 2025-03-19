using ConsoleDump;

/*
1) Total gasto por partido
2) Deputados com maior gasto individual (top 5)
3) Gasto médio por mês
4) Total gasto em alimentação por deputado (Descricao.Contains("ALIMENTAÇÃO"))
5) Lista de fornecedores mais utilizados
6) Gasto total por UF
7) Meses com maior número de documentos emitidos
8) Deputados com despesas acima de R$ 10.000,00
9) Total gasto por tipo de despesa (Descricao)
10) Total de gastos por ano
*/

var cotas = CotaParlamentar.LerCotasParlamentares("cota_parlamentar.csv");
//cotas.OrderByDescending(c => c.ValorLiquido).Take(10).ToArray().Dump();

/*
var partidos = cotas.GroupBy(c => c.Partido)
    .Select(g => new { Partido = g.Key, Total = g.Sum(c => c.ValorLiquido) })
    .OrderByDescending(g => g.Total).Take(10).ToArray().Dump();

var deputados = cotas.GroupBy(c => c.NomeParlamentar)
    .Select(g => new { Nome = g.Key, Total = g.Sum(c => c.ValorLiquido) })
    .OrderByDescending(g => g.Total).Take(5).ToArray().Dump();

var meses = cotas.GroupBy(c => c.Mes)
    .Select(g => new { Mes = g.Key, Media = Math.Round(g.Average(c => c.ValorLiquido) ?? 0, 2) })
    .OrderBy(g => g.Mes).ToArray().Dump();


var alimentacaoGastos = cotas
    .Where(c => c.Descricao.Contains("ALIMENTAÇÃO"))
    .GroupBy(c => c.NomeParlamentar)
    .Select(g => new { Nome = g.Key, Total = g.Sum(c => c.ValorLiquido) })
    .OrderByDescending(g => g.Total).ToArray().Dump();

var fornecedores = cotas.GroupBy(c => c.Fornecedor)
    .Select(g => new { Fornecedor = g.Key, Total = g.Count() })
    .OrderByDescending(g => g.Total).Take(10).ToArray().Dump();
*/
var UFs = cotas.GroupBy(c => c.UF)
    .Select(g => new { UF = g.Key, Total = g.Sum(c => c.ValorLiquido) })
    .OrderByDescending(g => g.Total).ToArray().Dump();

