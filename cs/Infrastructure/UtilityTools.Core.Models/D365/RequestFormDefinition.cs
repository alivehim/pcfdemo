using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.D365
{
    public class RequestFormDefinition
    {
        public string aia_apv_request_formid { get; set; }

        public string aia_name { get; set; }

        public string _aia_sys_wf_request_value { get; set; }

        public int aia_com_priority { get; set; }

        public int? aia_sys_leadtime_reminder { get; set; }
    }

    public class RequestFormCollection
    {
        public IList<RequestFormDefinition> value { get; set; }
    }

    public class eaa_form_collection
    {
        public IList<eaa_form> value { get; set; }
    }

    public class eaa_form
    {
        public string aia_eaa_formid { get; set; }
        public int aia_sys_form_type { get; set; }
        public string _aia_eaa_requester_value { get; set; }
        public string _aia_eaa_local_approver_value { get; set; }

        public string _aia_eaa_sub_cate_nontax_value { get; set; }
        public decimal? aia_eaa_pocket { get; set; }
        public string _aia_eaa_entity_sign_value { get; set; }

        public string _aia_eaa_entity_fee_value { get; set; }
        public string aia_eaa_est_start_date { get; set; }
        //public string aia_eaa_approver_date { get; set; }
        //public string aia_on_behalf_rcfo_submit_date { get; set; }
        //public string aia_eaa_indrect_taxes_base { get; set; }
        //public string aia_eaa_head_date { get; set; }
        public string aia_eaa_description_long { get; set; }
        //public string aia_eaa_attachments { get; set; }
        public string aia_eaa_sub_cate_tax { get; set; }
        public string aia_eaa_act_currency { get; set; }
        public decimal? aia_eaa_usd_rate { get; set; }
        public string aia_eaa_requester_date { get; set; }
        public string aia_eaa_entity_sign { get; set; }
        public string aia_eaa_reason_for_pro_firm { get; set; }
        public bool aia_eaa_defined_policy { get; set; }
        public string aia_eaa_requester { get; set; }
        public decimal? aia_eaa_pocket_base { get; set; }
        public string aia_eaa_fee_other_remarks { get; set; }
        public string aia_eaa_countries { get; set; }
        public string aia_eaa_final_date { get; set; }
        public string aia_eaa_entity_fee { get; set; }
        public string aia_eaa_fee_included_audit_fee_template { get; set; }
        public string aia_eaa_workflow_stage { get; set; }
        public decimal? aia_eaa_indrect_taxes { get; set; }
        public decimal? aia_eaa_est_cost { get; set; }
        public decimal? aia_eaa_est_cost_base { get; set; }
        public string aia_eaa_sub_cate_pre_approved_svc { get; set; }
        public string aia_reason_for_engaging_pwc { get; set; }
        public bool aia_eaa_fee_basis { get; set; }
        public string aia_eaa_pre_approval_svc_by_ext_auditor { get; set; }
        public int aia_eaa_form_status { get; set; }
        public int aia_stage { get; set; }
        public string aia_eaa_local_approver { get; set; }
        public int? aia_eaa_category { get; set; }
        public bool aia_eaa_completed { get; set; }
        public string aia_eaa_why_not_included_audit_fee_template { get; set; }
        public string aia_eaa_est_duration { get; set; }
        public int aia_eaa_approval_status { get; set; }
        public string aia_eaa_name_of_pro { get; set; }
        public bool aia_eaa_fully_budgeted { get; set; }
        public decimal? aia_eaa_usd_est_indrect_taxes { get; set; }
        public decimal? aia_eaa_act_taxes { get; set; }
        public decimal? aia_eaa_act_fee { get; set; }
        public string aia_eaa_tax_date { get; set; }
        public decimal? aia_eaa_base_fee { get; set; }
        public decimal? aia_eaa_usd_act_pocket { get; set; }
        public string aia_eaa_define_pwc { get; set; }
        public decimal? aia_eaa_act_cost_base { get; set; }
        public decimal? aia_eaa_usd_est_cost { get; set; }
        public decimal? aia_eaa_act_cost { get; set; }
        public string aia_eaa_state_why_permissible { get; set; }
        public decimal? aia_eaa_usd_act_taxes { get; set; }
        public bool aia_eaa_recurring { get; set; }
        public decimal? aia_eaa_usd_act_cost { get; set; }
        public bool aia_eaa_policy_read { get; set; }
        public string aia_eaa_completion_date { get; set; }
        public int? aia_eaa_quarter { get; set; }
        public decimal? aia_eaa_usd_est_pocket { get; set; }
        public string aia_eaa_description_short { get; set; }
        public string aia_eaa_cost_arrangement_detail { get; set; }
        public decimal? aia_eaa_act_pocket { get; set; }
        public decimal? aia_eaa_act_fee_base { get; set; }
        public string aia_name { get; set; }
        public string aia_eaa_department { get; set; }
        public decimal? aia_eaa_usd_act_fee { get; set; }
        public decimal? aia_eaa_base_fee_base { get; set; }
        public string aia_eaa_est_end_date { get; set; }
        public int? aia_eaa_report_year { get; set; }
        public decimal? aia_eaa_act_pocket_base { get; set; }
        public string aia_eaa_sub_cate_nontax { get; set; }
        public decimal? aia_eaa_act_taxes_base { get; set; }
        public decimal? aia_eaa_usd_est_base_fee { get; set; }
        public decimal? aia_eaa_est_usd_rate { get; set; }

    }
}
