using WebTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTools.Controllers {
    public class FinancyController : Controller {
        // GET: Financy
        
        public ActionResult Financy(DataFinancy financy) {

            ClassFinancy cf = new ClassFinancy();

            financy.Result = new List<object>();
            financy.message = "";
            financy.PercList = GetAllPercTypes();
            financy.PortionList = GetAllPortionTypes();
            Index();
            if (financy.grossValue > 0) {
                decimal tresSalarios = financy.salary * 3;
                decimal limite = 1000000.00M;
                cf.fees = financy.SelectedPercIds[0];

                decimal limiteEmprestimo = Math.Min(tresSalarios, limite);

                cf.valorEmprestimo = financy.grossValue;//valor
                decimal valorParcela = obterValorParcela(cf.valorEmprestimo, financy.SelectedPortionIds[0] * 12, cf.fees);

                if (valorParcela > (financy.salary * 0.3M)) {
                    financy.message = "The amount of the installment cannot be greater than 30% ($" + financy.salary * 0.3M + ") of your salary!";
                    return View(financy);
                }

                if (cf.valorEmprestimo <= decimal.Zero) {
                    return View(financy.message = "value Zero");
                }

                decimal creditoParticipante = cf.valorEmprestimo;

                List<Object> DadosSimulator = new List<object>();

                cf.numParcela = 0;
                cf.jurosLista = 0;
                cf.amortizacaoLista = 0;
                cf.saldoAposParcela = creditoParticipante;
                cf.dataConcessao = LastDayOfMonth();
                cf.dataVencimentoPrimeiraParcela = DateTime.Today;
                cf.dataVencimentoUltimaParcela = DateTime.Today;
                cf.dataVencimentoParcelaAnterior = DateTime.Today;
                cf.diasMesVencimento = 0;

                for (int i = 1; i <= financy.SelectedPortionIds[0] * 12; i++) {
                    cf.numParcela = i;
                    cf.jurosLista = calcularJurosParcela(cf.saldoAposParcela);

                    if (i == (financy.SelectedPortionIds[0] * 12)) {
                        var diferenca = cf.saldoAposParcela - (valorParcela - cf.jurosLista);
                        if (cf.saldoAposParcela > 0)
                            cf.jurosLista -= diferenca;
                        else
                            cf.jurosLista += diferenca;
                    }

                    cf.amortizacaoLista = valorParcela - cf.jurosLista;
                    cf.saldoAposParcela -= cf.amortizacaoLista;

                    if (i == 1)
                        cf.dataVencimentoParcela = Convert.ToDateTime("01/" + cf.dataConcessao.Month + "/" + cf.dataConcessao.Year);
                    else
                        cf.dataVencimentoParcela = Convert.ToDateTime("01/" + cf.dataVencimentoParcelaAnterior.Month + "/" + cf.dataVencimentoParcelaAnterior.Year);

                    cf.dataVencimentoParcela = cf.dataVencimentoParcela.AddMonths(1);
                    cf.diasMesVencimento = DateTime.DaysInMonth(cf.dataVencimentoParcela.Year, cf.dataVencimentoParcela.Month);
                    cf.dataVencimentoParcela = new DateTime(cf.dataVencimentoParcela.Year, cf.dataVencimentoParcela.Month, cf.diasMesVencimento);
                    cf.dataVencimentoParcelaAnterior = cf.dataVencimentoParcela;

                    if (i == 1)
                        cf.dataVencimentoPrimeiraParcela = cf.dataVencimentoParcela;

                    if (i == (financy.SelectedPortionIds[0] * 12))
                        cf.dataVencimentoUltimaParcela = cf.dataVencimentoParcela;

                    DadosSimulator.Add(new {
                        CreditoParticipante = String.Format("{0:C}", creditoParticipante),
                        ValorParcelas = String.Format("{0:C}", valorParcela),
                        ValorEmprestimo = String.Format("{0:C}", cf.valorEmprestimo),
                        NumeroParcelas = financy.SelectedPortionIds[0] * 12,
                        DataCredito = cf.dataConcessao.ToString("MM/dd/yyyy"),
                        VencimentoPrimeiraParcela = cf.dataVencimentoPrimeiraParcela.ToString("MM/dd/yyyy"),
                        VencimentoUltimaParcela = cf.dataVencimentoUltimaParcela.ToString("MM/dd/yyyy"),

                        ParcelaLista = cf.numParcela,
                        DataVencimentoParcelaLista = cf.dataVencimentoParcela.ToString("MM/dd/yyyy"),
                        JurosLista = String.Format("{0:C}", cf.jurosLista),
                        AmortizacaoLista = String.Format("{0:C}", cf.amortizacaoLista),
                        ValorParcelaLista = String.Format("{0:C}", cf.valorParcela),
                        SaldoParcelaLista = String.Format("{0:C}", cf.saldoAposParcela)
                    });
                    financy.valuePortion = valorParcela;
                    financy.concessao = cf.dataConcessao;
                    financy.totalValue = cf.valorEmprestimo;

                }

            }
                        
            return View(financy);
        }

        public ActionResult Index() {
            var model = new DataFinancy {
                SelectedPercIds = new[] { 2 },
                PercList = GetAllPercTypes(),
                SelectedPortionIds = new[] { 2 },
                PortionList = GetAllPortionTypes()
            };
            return View(model);
        }

        public List<SelectListItem> GetAllPercTypes() {
            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 1; i < 90; i++) {
                items.Add(new SelectListItem { Text = i.ToString() + "%", Value = i.ToString() });
            }

            return items;
        }
        public List<SelectListItem> GetAllPortionTypes() {
            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 5; i < 31; i++) {
                items.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            return items;
        }

        public decimal obterValorParcela(decimal valorSolicitado, int prazo, decimal fees) {
            decimal EncargosMensais = Math.Round(encargosMensais(fees), 13);

            var valorParcela = Math.Round(Convert.ToDecimal(valorSolicitado * EncargosMensais) / Convert.ToDecimal((1 - Math.Pow((Convert.ToDouble(1 + EncargosMensais)), -1 * prazo))), 2);

            return valorParcela > decimal.Zero ? valorParcela : 0;
        }

        public decimal encargosMensais(decimal fees) {
            var juros = fees;
            var encargosMensais = juros / 100;

            return encargosMensais;
        }

        public decimal calcularJurosParcela(decimal saldoAposParcela) {
            decimal tJuros = 0.05M;
            var encargosMensais = calcularEncargosMensais(tJuros);
            var jurosParcela = saldoAposParcela * encargosMensais;

            return jurosParcela;
        }

        public decimal calcularEncargosMensais(decimal jurosParcela) {
            decimal juros = jurosParcela;
            decimal encargosMensais = juros / 100;

            return encargosMensais;
        }

        public static DateTime LastDayOfMonth() {
            DateTime dt = DateTime.Today;
            DateTime ss = new DateTime(dt.Year, dt.Month, 1);
            return ss.AddMonths(1).AddDays(-1);
        }
    }
}