using GovTebimodule.Controller.Utils;
using MFilesAPI;
using Serilog;
using Tebilib;

namespace Tebifiles.Controller.ObjectController
{
    /// <summary>
    /// Validation doc controller
    /// </summary>
    public class ParcelleController : TebibaseController
    {
        /// <summary>
        /// Associated doc
        /// </summary>
        private readonly Tebiz Parcelle;

        private readonly ConfigFE CFE;

        /// <summary>
        /// Parametred ctor
        /// </summary>
        /// <param name="vault">Current vault</param>
        /// <param name="parcelle">Current document</param>
        public ParcelleController(Vault vault, Tebiz parcelle) : base(vault)
        {
            Parcelle = parcelle;
            CFE = new ConfigFE(Vault, Config.DOSSIER_CREATION_PARCELLE); // putain
        }

        /// <summary>
        /// Creation Des Dossiers parcelles
        /// </summary>
        public void CreationDesDossiers()
        {
            Log.Information($"CreationDesDossiers - {Parcelle.DebugTitleShort}");
            string parcelleNumero = Parcelle.DisplayValue(Config.Prop_own_parcelleNumero);
            if (string.IsNullOrEmpty(parcelleNumero))
            {
                Log.Warning($"CreationDesDossiers - [{Config.Prop_own_parcelleNumero.Name}] vide...");
            }

            CreerDossier($"RF.{parcelleNumero} - 1. Enquêtes");
            CreerDossier($"RF.{parcelleNumero} - 2. Autorisations");
            CreerDossier($"RF.{parcelleNumero} - 3. Enseignes");
            CreerDossier($"RF.{parcelleNumero} - 4. Autres éléments");
        }

        void CreerDossier(string nomDuDossier)
        {
            Log.Debug($"CreationDesDossiers - CreerDossier [{nomDuDossier}]");
            PvBuilder pvBuilder = new PvBuilder(Vault)
                .Add(Config.Tm_gov_prop_own_nomDuDossier, nomDuDossier)
                .Add(Config.Prop_own_objParcelle, Parcelle)
                .SetWorkflowState(workflow: Config.Tm_gov_wf_dossier, state: Config.Tm_gov_wf_dossier_state_ouvert);
            UtilitiesGov.AddCommonDossierInfoToPVBuilder(CFE, pvBuilder);
            pvBuilder.CreateQuick();
        }
    }
}
