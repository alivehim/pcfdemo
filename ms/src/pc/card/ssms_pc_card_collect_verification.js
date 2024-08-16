var PROXCardCollectVerificationSDK = window.PROXCardCollectVerificationSDK || {};

PROXCardCollectVerificationSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {


        const holder = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.holder).getValue()
        const holderid = ShareSdk.getLookupId(holder)
        const applicant = await ShareSdk.getPROXCardApplicantByIdAsync(holderid)

        Xrm.Navigation.navigateTo({
            pageType: "webresource",
            webresourceName: "clp_ssms_pc_card_verification.html",
            data: `contractorid=${applicant.clp_contractorid}&contractorname=${applicant.clp_name}`
        }, {
            target: 2,
            position: 1,
            target: 2,
            position: 1,
            height: { value: 80, unit: "%" },
            width: { value: 70, unit: "%" },
            title: "HKID / Passport No. Verification"
        })


    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}



PROXCardCollectVerificationSDK.enableButton = () => {
    //return ShareSdk.isProximityCardAdmin() && ShareSdk.IsProxCardCollectForm()

    //disable the button referring to enhancement
    return false;
}