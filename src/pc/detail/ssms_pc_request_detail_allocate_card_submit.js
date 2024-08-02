var PCREQDTLAllocateCardSubmitSDK = window.PCREQDTLAllocateCardSubmitSDK || {};

PCREQDTLAllocateCardSubmitSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {
        if (Xrm.Page.data.isValid()) {
            formContext = Xrm.Page.ui.formContext
            const inputData = Xrm.Utility.getPageContext().input.data
            if (inputData?.individualStatus) {

                if (inputData?.individualStatus === ShareSdk.PROXCardIndividualStatus.Rejected) {

                    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.is_rejected).setValue(true)
                    const type = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.app_type).getValue()

                    if (type === ShareSdk.PROXCardReqAPPType.NewISMS) {
                        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.card_no).setValue()
                        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card).setValue()
                    }
                }
                else{
                    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.is_rejected).setValue(false)
                }
                // Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.detail_status).setValue(inputData?.individualStatus)
            }

            await Xrm.Page.data.save()
            var dataXml = Xrm.Page.data.entity.getDataXml()
            console.log(dataXml)
            formContext.ui.close()
        }


    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}


PCREQDTLAllocateCardSubmitSDK.enableButton = () => {
    const inputData = Xrm.Utility.getPageContext().input.data
    if (inputData?.status === ShareSdk.PROXCardReqStatus.WaitingforCardAllocation || inputData?.status === ShareSdk.PROXCardReqStatus.RejectedbySecurity) {
        return true
    }

    return false
}