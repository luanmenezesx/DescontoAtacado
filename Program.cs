Console.Clear();


List<ItemCatalogo> produtos = new List<ItemCatalogo>
{
    new ItemCatalogo() {Id = "1", GTIN = "7891024110348", Descricao = "SABONETE OLEO DE ARGAN 90G PALMOLIVE", ValorVarejo = 2.88m, ValorAtacado = 2.51m, Unidades = 12},
    new ItemCatalogo() {Id = "2", GTIN = "7891048038017", Descricao = "CHÁ DE CAMOMILA DR.OETKER", ValorVarejo = 4.40m, ValorAtacado = 4.37m, Unidades = 3},
    new ItemCatalogo() {Id = "3", GTIN = "7896066334509", Descricao = "TORRADA TRADICIONAL WICKBOLD PACOTE 140G", ValorVarejo = 5.19m, ValorAtacado = null, Unidades = null},
    new ItemCatalogo() {Id = "4", GTIN = "7891700203142", Descricao = "BEBIDA À BASE DE SOJA MAÇÃ ADES CAIXA 200ML", ValorVarejo = 2.39m, ValorAtacado = 2.38m, Unidades = 6},
    new ItemCatalogo() {Id = "5", GTIN = "7894321711263", Descricao = "ACHOCOLATADO PÓ ORIGINAL TODDY POTE 400G", ValorVarejo = 9.79m, ValorAtacado = null, Unidades = null},
    new ItemCatalogo() {Id = "6", GTIN = "7896001250611", Descricao = "ADOÇANTE LÍQUIDO SUCRALOSE LINEA CAIXA 25ML", ValorVarejo = 9.89m, ValorAtacado = 9.10m, Unidades = 10},
    new ItemCatalogo() {Id = "7", GTIN = "7793306013029", Descricao = "CEREAL MATINAL CHOCOLATE KELLOGGS SUCRILHOS CAIXA 320G", ValorVarejo = 12.79m, ValorAtacado = 12.35m, Unidades = 3},
    new ItemCatalogo() {Id = "8", GTIN = "7896004400914", Descricao = "COCO RALADO SOCOCO 50G", ValorVarejo = 4.20m, ValorAtacado = 4.05m, Unidades = 6},
    new ItemCatalogo() {Id = "9", GTIN = "7898080640017", Descricao = "LEITE UHT INTEGRAL 1L COM TAMPA ITALAC", ValorVarejo = 6.99m, ValorAtacado = 6.89m, Unidades = 12},
    new ItemCatalogo() {Id = "10", GTIN = "7891025301516", Descricao = "DANONINHO PETIT SUISSE COM POLPA DE MORANGO 360G DANONE", ValorVarejo = 12.99m, ValorAtacado = null, Unidades = null},
    new ItemCatalogo() {Id = "11", GTIN = "7891030003115", Descricao = "CREME DE LEITE LEVE 200G MOCOCA", ValorVarejo = 2.88m, ValorAtacado = 3.09m, Unidades = 4}
};

List<VendaProduto> pvd = new List<VendaProduto>
{
    new VendaProduto() {Id = "2", GTIN = "7891048038017", Quantidade = 1, Valor = 4.40m},
    new VendaProduto() {Id = "8", GTIN = "7896004400914", Quantidade = 4, Valor = 16.80m},
    new VendaProduto() {Id = "11", GTIN = "7891030003115", Quantidade = 1, Valor = 3.12m},
    new VendaProduto() {Id = "1", GTIN = "7891024110348", Quantidade = 6, Valor = 17.28m},
    new VendaProduto() {Id = "9", GTIN = "7898080640017", Quantidade = 24, Valor = 167.76m},
    new VendaProduto() {Id = "8", GTIN = "7896004400914", Quantidade = 8, Valor = 33.60m},
    new VendaProduto() {Id = "4", GTIN = "7891700203142", Quantidade = 8, Valor = 19.12m},
    new VendaProduto() {Id = "2", GTIN = "7891048038017", Quantidade = 1, Valor = 4.40m},
    new VendaProduto() {Id = "7", GTIN = "7793306013029", Quantidade = 3, Valor = 38.37m},
    new VendaProduto() {Id = "3", GTIN = "7896066334509", Quantidade = 2, Valor = 10.38m}
};

decimal valorSubstotal = 0;
foreach (var item in pvd)
{
    valorSubstotal += item.Valor;
}

List<DescontoProduto> ListaDescontos = new List<DescontoProduto>();
var vendasAgrupadas = new Dictionary<string, int>();

foreach (var venda in pvd)
{
    if (vendasAgrupadas.ContainsKey(venda.GTIN))
    {
        vendasAgrupadas[venda.GTIN] += venda.Quantidade;
    }
    else
    {
        vendasAgrupadas[venda.GTIN] = venda.Quantidade;
    }
}

foreach (var grupo in vendasAgrupadas)
{
    ItemCatalogo produtoEncontrado = null;

    foreach (var prod in produtos)
    {
        if (prod.GTIN == grupo.Key)
        {
            produtoEncontrado = prod;
            break;
        }
    }
    if (produtoEncontrado != null && produtoEncontrado.ValorAtacado != null &&
    grupo.Value >= produtoEncontrado.Unidades)
    {
        decimal descontoUnitario = produtoEncontrado.ValorVarejo - produtoEncontrado.ValorAtacado.Value;
        decimal descontoTotal = descontoUnitario * grupo.Value;
        ListaDescontos.Add(new DescontoProduto
        {
            GTIN = produtoEncontrado.GTIN,
            ValorDesconto = descontoTotal
        }); 
    }
}

Console.WriteLine("Descontos Aplicados: ");
foreach (var desconto in ListaDescontos)
{
    string nomeProduto = "Produto desconhecido";
    foreach (var prod in produtos)
    {
        if (prod.GTIN == desconto.GTIN)
        {
            nomeProduto = prod.Descricao;
            break;
        }
    }
    Console.WriteLine($"{desconto.GTIN} => R$ {desconto.ValorDesconto:F2}");
}

decimal totalDescontos = 0;
foreach (var d in ListaDescontos)
{
    totalDescontos += d.ValorDesconto;
}

decimal valorTotal = valorSubstotal - totalDescontos;

Console.WriteLine();
Console.WriteLine($"(+) Subtotal = R$ {valorSubstotal:F2}");
Console.WriteLine($"(-) Descontos = R$ {totalDescontos:F2}");
Console.WriteLine($"(=) Total = R$ {valorTotal:F2}");

class DescontoProduto
{
    public string GTIN { get; set; }
    public decimal ValorDesconto { get; set; }
}

class ItemCatalogo
{
    public string Id { get; set; }
    public string GTIN { get; set; }
    public string Descricao { get; set; }
    public decimal ValorVarejo { get; set; }
    public decimal? ValorAtacado { get; set; }
    public int? Unidades { get; set; }
}

class VendaProduto
{
    public string Id { get; set; }
    public string GTIN { get; set; }
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
}
