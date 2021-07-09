using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTools.Controllers {
    public class ClassFinancy {

        public decimal portionValue { get; set; }
        public decimal fees { get; set; }
        public decimal totalTax { get; set; }
        public decimal valorEmprestimo { get; set; }
        public decimal valorParcela { get; set; }
        public int month { get; set; }

        public int numParcela { get; set; }
        public decimal jurosLista { get; set; }
        public decimal amortizacaoLista { get; set; }
        public decimal saldoAposParcela { get; set; }
        public DateTime dataConcessao { get; set; }
        public DateTime dataVencimento { get; set; }
        public DateTime dataVencimentoParcela { get; set; }
        public DateTime dataVencimentoPrimeiraParcela { get; set; }
        public DateTime dataVencimentoUltimaParcela { get; set; }
        public DateTime dataVencimentoParcelaAnterior { get; set; }
        public int diasMesVencimento { get; set; }

    }
}