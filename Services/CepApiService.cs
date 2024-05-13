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

            using (HttpClient client = new())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic? data = JsonConvert.DeserializeObject(json); // Para versões antigas do .NET, pode usar JsonConvert

                    if(data is not null )
                    {
                        if (data.erro == null)
                        {
                            string cidade = data.localidade;
                            string uf = data.uf;

                            return (uf, cidade);
                        }
                        else
                        {
                            return ("CEP não encontrado", "");
                        }
                    }
                    else
                    {
                        throw new Exception("Erro Api, entrar em contato com o time de desenvolvimento");
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
            return uf.ToUpper() switch
            {
                // Norte
                "AC" or "AP" or "AM" or "PA" or "RO" or "RR" or "TO" => "Norte",
                // Nordeste
                "AL" or "BA" or "CE" or "MA" or "PB" or "PE" or "PI" or "RN" or "SE" => "Nordeste",
                // Centro-Oeste
                "DF" or "GO" or "MT" or "MS" => "Centro-Oeste",
                // Sul
                "PR" or "RS" or "SC" => "Sul",
                // Sudeste
                "ES" or "MG" or "RJ" or "SP" => "Sudeste",
                _ => "Região não identificada",
            };
        }

        public string CalcularTempoDeEntregaEmDias(string uf)
        {
            if (uf.ToUpper() == "SP")
                return "No mesmo dia";

            var regiao = ObterRegiaoPorUF(uf);
            return regiao switch
            {
                "Norte" or "Nordeste" => "10 dias úteis",
                "Centro-Oeste" or "Sul" => "5 dias úteis",
                "Sudeste" => "1 dia corrido",
                _ => throw new ArgumentException("Região não identificada"),
            };
        }

        public decimal ObterPorcentagemCalculoFrete(string uf)
        {
            if (uf.ToUpper() == "SP")
                return 0.0m;

            var regiao = ObterRegiaoPorUF(uf);

            return regiao switch
            {
                "Norte" or "Nordeste" => 0.30m,
                "Centro-Oeste" or "Sul" => 0.20m,
                "Sudeste" => 0.10m,
                _ => throw new ArgumentException("Região não identificada"),
            };
        }
    }
}
