using System;

namespace Carubbi.Utils.DataTypes
{
    /// <summary>
    /// Estrutura de dados que Representa um intervalo entre duas datas
    /// </summary>
    public struct DateRange
    {
        private DateTime? _endDate;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate
        {
            get
            {
                return _endDate;
            }
            set
            { 
                if (value.HasValue)
                {
                    _endDate = value;
                    if (!IgnoreEndTime)
                        _endDate = AdjustEndTime(_endDate.Value);
                }

               
            }
        }

        public bool IgnoreEndTime
        {
            get;
            set;
        }

        private DateTime AdjustEndTime(DateTime endDate)
        {
            endDate = endDate.AddHours(23 - endDate.Hour);
            endDate = endDate.AddMinutes(59 - endDate.Minute);
            endDate = endDate.AddSeconds(59 - endDate.Second);
            return endDate;
        }
        
        /// <summary>
        /// Verifica se é um intervalo válido, ou seja, se a data inicial é menor ou igual à final
        /// </summary>
        public bool IsValid
        {
            get
            {
                return StartDate.Value <= EndDate.Value;
            }
        }


        /// <summary>
        /// Valida se o intervalo é valido a partir de uma restrição em meses.
        /// <example>Data Inicial = 01/01/2015, Data Final = 01/05/2015
        /// Para as datas acima a chamada deste método é Válida Até 5 meses [ValidateMonthRange(5)], a partir do valor 6 a função retorna false
        /// </example>
        /// </summary>
        /// <param name="months">Quantidade de meses permitidos</param>
        /// <returns>Indicador se o intervalo é valido ou não</returns>
        public bool ValidateMonthRange(int months)
        {
            var maximumEndDate = StartDate.Value.AddMonths(months);
            return maximumEndDate >= EndDate.Value;
        }


        /// <summary>
        /// Valida se o intervalo é valido a partir de uma restrição em dias.
        /// <example>Data Inicial = 01/01/2015, Data Final = 05/01/2015
        /// Para as datas acima a chamada deste método é Válida Até 5 dias [ValidateDaysRange(5)], a partir do valor 6 a função retorna false
        /// </example>
        /// </summary>
        /// <param name="months">Quantidade de dias permitidos</param>
        /// <returns>Indicador se o intervalo é valido ou não</returns>
        public bool ValidateDaysRange(int days)
        {
            var maximumEndDate = StartDate.Value.AddDays(days);
            return maximumEndDate >= EndDate.Value;
        }


        /// <summary>
        /// Valida se o intervalo é valido a partir de uma restrição em anos.
        /// <example>Data Inicial = 01/01/2010, Data Final = 01/01/2015
        /// Para as datas acima a chamada deste método é Válida Até 5 anos [ValidateYearsRange(5)], a partir do valor 6 a função retorna false
        /// </example>
        /// </summary>
        /// <param name="months">Quantidade de anos permitidos</param>
        /// <returns>Indicador se o intervalo é valido ou não</returns>
        public bool ValidateYearsRange(int years)
        {
            var maximumEndDate = StartDate.Value.AddYears(years);
            return maximumEndDate >= EndDate.Value;
        }
    }
}
