var PROXCardReqNewSDK = window.PROXCardReqNewSDK || {};

PROXCardReqNewSDK.onAction = async () => {

    Xrm.Utility.showProgressIndicator("Loading")
    try {



        var currentUserId = window.parent.ShareSdk.getCurrentUserId()
        var user = await window.parent.ShareSdk.getUserBySystemUserAsync(currentUserId)
        var departmentBranch = (user[window.parent.ShareSdk.CLPUserFields.dept_name] ?? '') + ' / ' + (user[window.parent.ShareSdk.CLPUserFields.branch_name] ?? '')

        const newReq = await PROXCardReqNewSDK.createPROXCardReqFormAsync(user.clp_userid, departmentBranch)

        Xrm.Navigation.navigateTo({
            pageType: 'entityrecord',
            entityName: 'clp_proximitycardrequest',
            entityId: newReq.clp_proximitycardrequestid,
            formId: window.parent.ShareSdk.Forms.Request_Main,
            data: {
            }
        })

    } catch (err) {
        window.parent.ShareSdk.showGlobalNotification(err.message, 2)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }



}


PROXCardReqNewSDK.createPROXCardReqFormAsync = (userid, departmentBranch) => {

    const currentUserId = window.parent.ShareSdk.getCurrentUserId()



    let formData = {
        "clp_name": 'New ISMS Proximity Card/Existing Proximity Card Encoding for Substation Access Requisition Form 進入中電輸電變電站 新電腦磁咭/現有磁咭編碼 申請表',
        "clp_raw_requester@odata.bind": `/systemusers(${currentUserId})`,
        "clp_Requester@odata.bind": `/clp_users(${userid})`,
        "clp_ResponsiblePerson@odata.bind": `/clp_users(${userid})`,
        "clp_re_departmentbranch": departmentBranch,
        "clp_latest_modified_on": new Date(),
        "clp_latest_modified_by@odata.bind": `/clp_users(${userid})`,
    }


    return fetch(
        `/api/data/v9.0/clp_proximitycardrequests`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'return=representation',
            },
            body: JSON.stringify(formData)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}


PROXCardReqNewSDK.enableButton = () => {
    return true
}