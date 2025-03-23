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


// 1)
var partidos = cotas.GroupBy(c => c.Partido)
    .Select(g => new { Partido = g.Key, Total = g.Sum(c => c.ValorLiquido) })
    .OrderByDescending(g => g.Total).Take(10).ToArray().Dump();

// 2)
var deputados = cotas.GroupBy(c => c.NomeParlamentar)
    .Select(g => new { Nome = g.Key, Total = g.Sum(c => c.ValorLiquido) })
    .OrderByDescending(g => g.Total).Take(5).ToArray().Dump();


// 3)
var meses3 = cotas.GroupBy(c => c.Mes)
    .Select(g => new { Mes = g.Key, Media = Math.Round(g.Average(c => c.ValorLiquido) ?? 0, 2) })
    .OrderBy(g => g.Mes).ToArray().Dump();

// 4)
var alimentacaoGastos = cotas
    .Where(c => c.Descricao.Contains("ALIMENTAÇÃO"))
    .GroupBy(c => c.NomeParlamentar)
    .Select(g => new { Nome = g.Key, Total = g.Sum(c => c.ValorLiquido) })
    .OrderByDescending(g => g.Total).ToArray().Dump();

// 5)
var fornecedores = cotas.GroupBy(c => c.Fornecedor)
    .Select(g => new { Fornecedor = g.Key, Total = g.Count() })
    .OrderByDescending(g => g.Total).Take(10).ToArray().Dump();


// 6)
var UFs = cotas.GroupBy(c => c.UF)
    .Select(g => new { UF = g.Key, Total = g.Sum(c => c.ValorLiquido) })
    .OrderByDescending(g => g.Total).ToArray().Dump();


// 7)
var meses7 = cotas.GroupBy(c => c.Mes)
    .Select(g => new {Mes = g.Key, Documentos = g.Count( c => c.DocumentoId.HasValue) })
    .OrderBy(g => g.Mes).ToArray().Dump();

// 8)
var deputadosComDespesasAltas = cotas.GroupBy(c => c.NomeParlamentar)
    .Select(g => new { Nome = g.Key, Total = g.Sum(c => c.ValorLiquido) })
    .Where(g => g.Total > 10000)
    .OrderByDescending(g => g.Total).ToArray().Dump();

// 9)
var totalPorTipoDespesa = cotas.GroupBy(c => c.Descricao)
    .Select(g => new { Descricao = g.Key, Total = g.Sum(c => c.ValorLiquido) })
    .OrderByDescending(g => g.Total).ToArray().Dump();

// 10)
var totalPorAno = cotas.GroupBy(c => c.Ano)
    .Select(g => new { Ano = g.Key, Total = g.Sum(c => c.ValorLiquido) })
    .OrderBy(g => g.Ano).ToArray().Dump();

