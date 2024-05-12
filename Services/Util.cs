using ClosedXML.Excel;
using ControladorOnion.Models;
using ControladorOnion.Models.SystemModels;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ControladorOnion.Services
{
    public class Util
    {

        public static (List<Pedido> Pedidos, List<string> Erros)  ValidarPlanilhaDePedidos(string caminhoDoArquivo)
        {
            List<Pedido> dados = new List<Pedido>();

            var errosIdentificados = new List<string>();

            using (var workbook = new XLWorkbook(caminhoDoArquivo))
            {
                var worksheet = workbook.Worksheet(1); // Supondo que os dados estejam na primeira planilha

                // Supondo que os dados começam da segunda linha
                int linhaAtual = 2;
                while (!worksheet.Cell(linhaAtual, 1).IsEmpty())
                {
                    Pedido pedido = new Pedido
                    {
                        CPFouCNPJ = worksheet.Cell(linhaAtual, 1).GetString(),
                        NomeOuRazaoSocial = worksheet.Cell(linhaAtual, 2).GetString(),
                        CEP = worksheet.Cell(linhaAtual, 3).GetString(),
                        Produto = worksheet.Cell(linhaAtual, 4).GetString(),
                        NumeroPedido = int.Parse(worksheet.Cell(linhaAtual, 5).GetString()),
                        Data = worksheet.Cell(linhaAtual, 6).GetDateTime()
                    };



                    var retornoValidacaoPedido = ValidarPedido(pedido, linhaAtual);
                    if (retornoValidacaoPedido.Valido)
                    {
                        dados.Add(pedido);
                    }
                    else
                    {
                        errosIdentificados.Add(retornoValidacaoPedido.Erro);
                    }
                    
                    linhaAtual++;
                }
            }

            return (dados, errosIdentificados);
        }

        public static (bool Valido, string Erro) ValidarPedido(Pedido pedido, int linhaDaPlanilhaAtual)
        {
            // Validar CPF ou CNPJ
            if (!ValidarDocumento(pedido.CPFouCNPJ))
            {
                return (false, "CPF ou CNPJ inválido na celula: " + ObterReferenciaCelula((linhaDaPlanilhaAtual,1)));
            }
            // Validar CEP
            var cep = RemoverCaracteresNaoNumericos(pedido.CEP);
            if (string.IsNullOrEmpty(cep) || cep.Length != 8 || !int.TryParse(cep, out _))
            {
                return (false, "CEP inválido na celula: " + ObterReferenciaCelula((linhaDaPlanilhaAtual, 3)));
            }
            // Validar data
            if (pedido.Data == default(DateTime))
            {
                return (false, "Data inválida na celula: " + ObterReferenciaCelula((linhaDaPlanilhaAtual, 6)));
            }

            else
            {
                return (true, ""); 
            }

          
        }

        public static (bool Valido, string Erro, string Campo) ValidarPedido(Pedido pedido)
        {
            // Validar CPF ou CNPJ
            if (!ValidarDocumento(pedido.CPFouCNPJ))
            {
                return (false, "CPF ou CNPJ inválido, verifique e tente novamente", "CPFouCNPJ");
            }
            // Validar CEP
            var cep = RemoverCaracteresNaoNumericos(pedido.CEP);
            if (string.IsNullOrEmpty(cep) || cep.Length != 8 || !int.TryParse(cep, out _))
            {
                return (false, "CEP inválido", "CEP");
            }
            // Validar data
            if (pedido.Data == default(DateTime))
            {
                return (false, "Data inválida", "Data");
            }

            // Validar número do pedido
            int numeroPedido;
            if (!int.TryParse(pedido.NumeroPedido.ToString(), out numeroPedido))
            {
                return (false, "Número do pedido invalido", "NumeroPedido");
            }
            else
            {
                return (true, "", "");
            }


        }

        private static bool ValidarDocumento(string documento)
        {
            documento = RemoverCaracteresNaoNumericos(documento);

            if (documento.Length == 11)
            {
                return ValidarCPF(documento);
            }
            else if (documento.Length == 14)
            {
                return ValidarCNPJ(documento);
            }
            else
            {
                return false;
            }
        }



        private static string RemoverCaracteresNaoNumericos(string texto)
        {
            return texto.Replace(".", "").Replace("-", "").Replace("/", "");
        }

        private static bool ValidarCPF(string cpf)
        {
            // Remove caracteres não numéricos e espaços em branco
            string cpfNumerico = new string(cpf.Where(char.IsDigit).ToArray());

            // Verifica se o CPF tem 11 dígitos após a remoção dos caracteres não numéricos
            if (cpfNumerico.Length != 11 || cpfNumerico.Distinct().Count() == 1)
            {
                return false;
            }

            // Calcula o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(cpfNumerico[i].ToString()) * (10 - i);
            }
            int digitoVerificador1 = 11 - (soma % 11);
            if (digitoVerificador1 >= 10)
            {
                digitoVerificador1 = 0;
            }

            // Verifica se o primeiro dígito verificador corresponde ao fornecido
            if (int.Parse(cpfNumerico[9].ToString()) != digitoVerificador1)
            {
                return false;
            }

            // Calcula o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(cpfNumerico[i].ToString()) * (11 - i);
            }
            int digitoVerificador2 = 11 - (soma % 11);
            if (digitoVerificador2 >= 10)
            {
                digitoVerificador2 = 0;
            }

            // Verifica se o segundo dígito verificador corresponde ao fornecido
            if (int.Parse(cpfNumerico[10].ToString()) != digitoVerificador2)
            {
                return false;
            }

            // CPF válido
            return true;
        }

        private static bool ValidarCNPJ(string cnpj)
        {
            cnpj = RemoverCaracteresNaoNumericos(cnpj);

            if (cnpj.Length != 14)
            {
                return false;
            }

            int[] multiplicadoresPrimeiroDigito = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadoresSegundoDigito = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma = 0;
            for (int i = 0; i < 12; i++)
            {
                soma += int.Parse(cnpj[i].ToString()) * multiplicadoresPrimeiroDigito[i];
            }

            int resto = soma % 11;
            int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cnpj[12].ToString()) != digitoVerificador1)
            {
                return false;
            }

            soma = 0;
            for (int i = 0; i < 13; i++)
            {
                soma += int.Parse(cnpj[i].ToString()) * multiplicadoresSegundoDigito[i];
            }

            resto = soma % 11;
            int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cnpj[13].ToString()) != digitoVerificador2)
            {
                return false;
            }

            return true;
        }

        private static string ObterReferenciaCelula((int linha, int coluna) celula)
        {
            int coluna = celula.coluna;
            int linha = celula.linha;
            return Convert.ToChar('A' + coluna - 1).ToString() + linha.ToString();
        }
    }
}
