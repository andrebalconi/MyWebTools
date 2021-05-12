using WebTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTools.Controllers
{
    public class FinancyController : Controller
    {
        // GET: Financy
        public decimal portionValue;
        public decimal fees;
        public decimal totalTax;
        public decimal valorEmprestimo;
        public decimal valorParcela;
        int month;
        public ActionResult Financy(DataFinancy financy)
        {
            financy.Result = new List<object>();
            Index();
            if(financy.grossValue > 0) {
                decimal tresSalarios = financy.salary * 3;
                decimal limite = 1000000.00M;
                fees = financy.SelectedPercIds[0];

                decimal limiteEmprestimo = Math.Min(tresSalarios,limite);

                valorEmprestimo = financy.grossValue;//valor
                decimal valorParcela = obterValorParcela(valorEmprestimo,financy.SelectedPortionIds[0]*12);

                if (valorEmprestimo <= decimal.Zero)
                {
                    return View(financy.message = "value Zero");
                }

                decimal creditoParticipante = valorEmprestimo;
                
                List<Object> DadosSimulator = new List<object>();

                int numParcela = 0;
                decimal jurosLista = 0;
                decimal amortizacaoLista = 0;
                decimal saldoAposParcela = creditoParticipante;
                DateTime dataConcessao = LastDayOfMonth();
                DateTime dataVencimento;
                DateTime dataVencimentoParcela;
                DateTime dataVencimentoPrimeiraParcela = DateTime.Today;
                DateTime dataVencimentoUltimaParcela = DateTime.Today;
                DateTime dataVencimentoParcelaAnterior = DateTime.Today;
                int diasMesVencimento = 0;

                for (int i = 1; i <= financy.SelectedPortionIds[0] * 12; i++)
                {
                    numParcela = i;
                    jurosLista = calcularJurosParcela(saldoAposParcela);

                    if (i == (financy.SelectedPortionIds[0] * 12))
                    {
                        var diferenca = saldoAposParcela - (valorParcela - jurosLista);
                        if (saldoAposParcela > 0)
                            jurosLista -= diferenca;
                        else
                            jurosLista += diferenca;
                    }

                    amortizacaoLista = valorParcela - jurosLista;
                    saldoAposParcela -= amortizacaoLista;

                    if(i == 1)
                        dataVencimentoParcela = Convert.ToDateTime("01/" + dataConcessao.Month + "/" + dataConcessao.Year);
                    else
                        dataVencimentoParcela = Convert.ToDateTime("01/" + dataVencimentoParcelaAnterior.Month + "/" + dataVencimentoParcelaAnterior.Year);

                    dataVencimentoParcela = dataVencimentoParcela.AddMonths(1);
                    diasMesVencimento = DateTime.DaysInMonth(dataVencimentoParcela.Year, dataVencimentoParcela.Month);
                    dataVencimentoParcela = new DateTime(dataVencimentoParcela.Year, dataVencimentoParcela.Month, diasMesVencimento);
                    dataVencimentoParcelaAnterior = dataVencimentoParcela;

                    if (i == 1)
                        dataVencimentoPrimeiraParcela = dataVencimentoParcela;

                    if (i == (financy.SelectedPortionIds[0] * 12))
                        dataVencimentoUltimaParcela = dataVencimentoParcela;

                    DadosSimulator.Add(new
                    {
                        CreditoParticipante = String.Format("{0:C}", creditoParticipante),
                        ValorParcelas = String.Format("{0:C}", valorParcela),
                        ValorEmprestimo = String.Format("{0:C}", valorEmprestimo),
                        NumeroParcelas = financy.SelectedPortionIds[0] * 12,
                        DataCredito = dataConcessao.ToString("MM/dd/yyyy"),
                        VencimentoPrimeiraParcela = dataVencimentoPrimeiraParcela.ToString("MM/dd/yyyy"),
                        VencimentoUltimaParcela = dataVencimentoUltimaParcela.ToString("MM/dd/yyyy"),

                        ParcelaLista = numParcela,
                        DataVencimentoParcelaLista = dataVencimentoParcela.ToString("MM/dd/yyyy"),
                        JurosLista = String.Format("{0:C}",jurosLista),
                        AmortizacaoLista = String.Format("{0:C}", amortizacaoLista),
                        ValorParcelaLista = String.Format("{0:C}", valorParcela),
                        SaldoParcelaLista = String.Format("{0:C}", saldoAposParcela)
                    });
                    financy.valuePortion = valorParcela;
                    financy.concessao = dataConcessao;
                    financy.totalValue = valorEmprestimo;
                }
                
            }


            financy.PercList = GetAllPercTypes();
            financy.PortionList = GetAllPortionTypes();

            
            return View(financy);
        }

        public ActionResult Index()
        {
            var model = new DataFinancy
            {
                SelectedPercIds = new[] { 2 },
                PercList = GetAllPercTypes(),
                SelectedPortionIds = new[] {2},
                PortionList = GetAllPortionTypes()
            };
            return View(model);
        }

        public List<SelectListItem> GetAllPercTypes()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            for(int i = 1; i < 90; i++)
            {
                items.Add(new SelectListItem { Text = i.ToString()+"%", Value = i.ToString() });
            }
                        
            return items;
        }
        public List<SelectListItem> GetAllPortionTypes()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 5; i < 31; i++)
            {
                items.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            return items;
        }

        public decimal obterValorParcela(decimal valorSolicitado, int prazo)
        {
            decimal EncargosMensais = Math.Round(encargosMensais(),13);

            var valorParcela = Math.Round(Convert.ToDecimal(valorSolicitado * EncargosMensais) / Convert.ToDecimal((1 - Math.Pow((Convert.ToDouble(1 + EncargosMensais)), -1 * prazo))), 2);

            return valorParcela > decimal.Zero ? valorParcela : 0;
        }

        public decimal encargosMensais() 
        {
            var juros = fees;
            var encargosMensais = juros / 100;

            return encargosMensais;
        }

        public decimal calcularJurosParcela(decimal saldoAposParcela)
        {
            decimal tJuros = 0.05M;
            var encargosMensais = calcularEncargosMensais(tJuros);
            var jurosParcela = saldoAposParcela * encargosMensais;

            return jurosParcela;
        }

        public decimal calcularEncargosMensais(decimal jurosParcela)
        {
            decimal juros = jurosParcela;
            decimal encargosMensais = juros / 100;

            return encargosMensais;
        }

        public static DateTime LastDayOfMonth()
        {
            DateTime dt = DateTime.Today;
            DateTime ss = new DateTime(dt.Year, dt.Month, 1);
            return ss.AddMonths(1).AddDays(-1);
        }
    }
}