using ConsoleDump;

var cotas = CotaParlamentar.LerCotasParlamentares("cota_parlamentar.csv");
cotas.OrderByDescending(c => c.ValorLiquido).Take(10).ToArray().Dump();

