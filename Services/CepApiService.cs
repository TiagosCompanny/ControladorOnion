using DocumentFormat.OpenXml.Drawing.Diagrams;
using Newtonsoft.Json;
using System.Security.Cryptography.Xml;

namespace ControladorOnion.Services
{
    public class CepApiService
    {

        public async Task<(string Uf, string Cidade)> ObterUfECidade(string cep)
        {
            string url = $"https://viacep.com.br/ws/{cep}/json/";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json); // Para versões antigas do .NET, pode usar JsonConvert
                    if (data.erro == null)
                    {
                        string cidade = data.localidade;
                        string uf = data.uf;

                        return (uf, cidade);
                    }
                    else
                    {
                        return ("CEP não encontrado", "C");
                    }
                }
                else
                {
                    return ("Erro ao conectar ao serviço", "Erro");
                }
            }
        }

        public string ObterRegiaoPorUF(string uf)
        {
            switch (uf.ToUpper())
            {
                // Norte
                case "AC":
                case "AP":
                case "AM":
                case "PA":
                case "RO":
                case "RR":
                case "TO":
                    return "Norte";

                // Nordeste
                case "AL":
                case "BA":
                case "CE":
                case "MA":
                case "PB":
                case "PE":
                case "PI":
                case "RN":
                case "SE":
                    return "Nordeste";

                // Centro-Oeste
                case "DF":
                case "GO":
                case "MT":
                case "MS":
                    return "Centro-Oeste";

                // Sul
                case "PR":
                case "RS":
                case "SC":
                    return "Sul";

                // Sudeste
                case "ES":
                case "MG":
                case "RJ":
                case "SP":
                    return "Sudeste";

                default:
                    return "Região não identificada";
            }
        }

        public int CalcularTempoDeEntregaEmDias(string uf)
        {
            if (uf.ToUpper() == "SP")
                return 0;

            var regiao = ObterRegiaoPorUF(uf);
            switch (regiao)
            {
                case "Norte":
                case "Nordeste":
                    return 10;

                case "Centro-Oeste":
                case "Sul":
                    return 5;

                case "Sudeste":
                    return 1;

                default:
                    throw new ArgumentException("Região não identificada");
            }
        }

        public decimal ObterPorcentagemCalculoFrete(string uf)
        {
            if (uf.ToUpper() == "SP")
                return 0.0m;

            var regiao = ObterRegiaoPorUF(uf);

            switch (regiao)
            {
                case "Norte":
                case "Nordeste":
                    return 0.30m;

                case "Centro-Oeste":
                case "Sul":
                    return 0.20m;

                case "Sudeste":
                    return 0.10m;

                default:
                    throw new ArgumentException("Região não identificada");
            }
        }
    }
}
