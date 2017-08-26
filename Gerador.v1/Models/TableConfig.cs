using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gerador.Models;

namespace Gerador.Models
{
    public sealed class TableToClass
    {
        public Dictionary<String, ModelConfig> GetTableMapper()
        {
            var Storage = new Dictionary<String, ModelConfig>();

            Storage.Add("MC_AGENDA", new ModelConfig() { ClassName = "Agenda", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_ALTA", new ModelConfig() { ClassName = "Alta", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_ALTERACAO", new ModelConfig() { ClassName = "Alteracao", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = false });
            Storage.Add("MC_ATENDIMENTO", new ModelConfig() { ClassName = "Atendimento", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_BENEFICIARIO", new ModelConfig() { ClassName = "Beneficiario", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_CARATER_ATENDIMENTO", new ModelConfig() { ClassName = "CaraterAtendimento", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_CARTEIRA", new ModelConfig() { ClassName = "Carteira", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_CBO", new ModelConfig() { ClassName = "Cbo", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = false });
            Storage.Add("MC_CONCELHO_PROFISSIONAL", new ModelConfig() { ClassName = "ConcelhoProfissional", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_CONTRATO", new ModelConfig() { ClassName = "Contrato", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_CREDENCIADO", new ModelConfig() { ClassName = "Credenciado", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_CREDENCIADO_ENDERECO", new ModelConfig() { ClassName = "CredenciadoEndereco", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_CREDENCIADO_ESPECIALIDADE", new ModelConfig() { ClassName = "CredenciadoEspecialidade", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_DEPENDENTE", new ModelConfig() { ClassName = "Dependente", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_DIA", new ModelConfig() { ClassName = "Dia", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_ESPECIALIDADE", new ModelConfig() { ClassName = "Especialidade", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_GLOSAS_MENSAGENS", new ModelConfig() { ClassName = "GlosasMensagens", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_GRAU_PARENTESCO", new ModelConfig() { ClassName = "GrauParentesco", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_GRAU_PARTICIPACAO", new ModelConfig() { ClassName = "GrauParticipacao", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_INDICADOR_ACIDENTE", new ModelConfig() { ClassName = "IndicadorAcidente", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_LOG_ACESSO", new ModelConfig() { ClassName = "LogAcesso", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_LOG_BENEFICIARIO", new ModelConfig() { ClassName = "LogBeneficiario", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_LOG_CONTRATO", new ModelConfig() { ClassName = "LogContrato", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_LOG_CREDENCIADO", new ModelConfig() { ClassName = "LogCredenciado", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_LOG_DEPENDENTE", new ModelConfig() { ClassName = "LogDependente", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = false });
            Storage.Add("MC_LOG_PLANO", new ModelConfig() { ClassName = "LogPlano", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_LOG_PRESTADOR", new ModelConfig() { ClassName = "LogPrestador", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_PERIODO", new ModelConfig() { ClassName = "Periodo", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_PLANO", new ModelConfig() { ClassName = "Plano", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_PLANO_CATEGORIA", new ModelConfig() { ClassName = "PlanoCategoria", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_PLANO_INATIVACAO", new ModelConfig() { ClassName = "PlanoInativacao", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_PLANO_TABELA", new ModelConfig() { ClassName = "PlanoTabela", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_PRESTADOR", new ModelConfig() { ClassName = "Prestador", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_TABELA_PROCED", new ModelConfig() { ClassName = "TabelaProced", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_TECNICA_UTILIZADA", new ModelConfig() { ClassName = "TecnicaUtilizada", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_TEMPO_DOENCA", new ModelConfig() { ClassName = "TempoDoenca", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_TIPO_ACOMODACAO", new ModelConfig() { ClassName = "TipoAcomodacao", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_TIPO_ATENDIMENTO", new ModelConfig() { ClassName = "TipoAtendimento", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_TIPO_DOENCA", new ModelConfig() { ClassName = "TipoDoenca", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_TIPO_GUIA", new ModelConfig() { ClassName = "TipoGuia", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_TIPO_GUIA_SAIDA", new ModelConfig() { ClassName = "TipoGuiaSaida", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_TIPO_INTERNACAO", new ModelConfig() { ClassName = "TipoInternacao", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_TUSS", new ModelConfig() { ClassName = "Tuss", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_UNIDADE_TEMPO", new ModelConfig() { ClassName = "UnidadeTempo", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });
            Storage.Add("MC_USUARIO", new ModelConfig() { ClassName = "Usuario", NameSpaceDto = "Metrocare.Data", NameSpaceMapper = "Metrocare.Data", NameSpaceDomain = "Metrocare.Domain", NameSpaceService = "Metrocare.Service", CreateController = true });

            return (Storage);
        }
    }
}
