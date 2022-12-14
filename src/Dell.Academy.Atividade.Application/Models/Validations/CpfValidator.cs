using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dell.Academy.Atividade.Application.Models.Validations
{
    public static class CpfValidator
    {
        public const int CpfSize = 11;

        public static bool Validate(string cpf)
        {
            var cpfNumeros = OnlyNumbers(cpf);

            if (!ValidSize(cpfNumeros)) return false;
            return !HasRepeatDigits(cpfNumeros) && HasValidDigits(cpfNumeros);
        }

        public static string OnlyNumbers(string valor)
        {
            var onlyNumber = new StringBuilder();
            foreach (var s in valor)
            {
                if (char.IsDigit(s))
                {
                    onlyNumber.Append(s);
                }
            }

            return onlyNumber.ToString().Trim();
        }

        private static bool ValidSize(string valor) => valor.Length == CpfSize;

        private static bool HasRepeatDigits(string valor)
        {
            string[] invalidNumbers =
            {
                "00000000000",
                "11111111111",
                "22222222222",
                "33333333333",
                "44444444444",
                "55555555555",
                "66666666666",
                "77777777777",
                "88888888888",
                "99999999999"
            };

            return invalidNumbers.Contains(valor);
        }

        private static bool HasValidDigits(string valor)
        {
            var number = valor.Substring(0, CpfSize - 2);
            var digitoVerificador = new DigitoVerificador(number)
                .ComMultiplicadoresDeAte(2, 11)
                .Substituindo("0", 10, 11);
            var firstDigit = digitoVerificador.CalculaDigito();
            digitoVerificador.AddDigito(firstDigit);
            var secondDigit = digitoVerificador.CalculaDigito();

            return string.Concat(firstDigit, secondDigit) == valor.Substring(CpfSize - 2, 2);
        }

        public class DigitoVerificador
        {
            private string _numero;
            private const int Modulo = 11;
            private readonly List<int> _multiplicadores = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 };
            private readonly IDictionary<int, string> _substituicoes = new Dictionary<int, string>();
            private readonly bool _complementarDoModulo = true;

            public DigitoVerificador(string numero)
            {
                _numero = numero;
            }

            public DigitoVerificador ComMultiplicadoresDeAte(int primeiroMultiplicador, int ultimoMultiplicador)
            {
                _multiplicadores.Clear();
                for (var i = primeiroMultiplicador; i <= ultimoMultiplicador; i++)
                    _multiplicadores.Add(i);

                return this;
            }

            public DigitoVerificador Substituindo(string substituto, params int[] digitos)
            {
                foreach (var i in digitos)
                {
                    _substituicoes[i] = substituto;
                }
                return this;
            }

            public void AddDigito(string digito) => _numero = string.Concat(_numero, digito);

            public string CalculaDigito() => _numero.Length <= 0 ? "" : GetDigitSum();

            private string GetDigitSum()
            {
                var soma = 0;
                for (int i = _numero.Length - 1, m = 0; i >= 0; i--)
                {
                    var produto = (int)char.GetNumericValue(_numero[i]) * _multiplicadores[m];
                    soma += produto;

                    if (++m >= _multiplicadores.Count) m = 0;
                }

                var mod = (soma % Modulo);
                var resultado = _complementarDoModulo ? Modulo - mod : mod;

                return _substituicoes.ContainsKey(resultado) ? _substituicoes[resultado] : resultado.ToString();
            }
        }
    }
}