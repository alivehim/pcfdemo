var PROXCardAuditSDK = window.PROXCardAuditSDK || {};

PROXCardAuditSDK.onAction = async (ids) => {

    Xrm.Utility.showProgressIndicator("Loading")
    try {
        var user = await ShareSdk.getCurrentClpUserAsync()
        await Promise.all(ids.map(p => PROXCardAuditSDK.auditDetailAsync(p, true, user?.clp_userid)))

        var formid = ShareSdk.getCurrentFormId()

        const auditDetails = await PROXCardAuditSDK.getAuditDetailsAsync(formid)
        if (!auditDetails.some(p => !p.clp_isaudited)) {
            await PROXCardAuditSDK.updateAuditAsync(formid, true, user?.clp_userid)
        }

        //refresh subgrid 

        formContext = Xrm.Page.ui.formContext
        var subgrid = formContext.ui.controls.get(`Subgrid_Audit_Detail`);
        subgrid.refresh();


    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }

}

PROXCardAuditSDK.onUnAuditAction = async (ids) => {

    Xrm.Utility.showProgressIndicator("Loading")
    try {

        await Promise.all(ids.map(p => PROXCardAuditSDK.auditDetailAsync(p, false)))

        var formid = ShareSdk.getCurrentFormId()

        await PROXCardAuditSDK.updateAuditAsync(formid, false)


        //refresh subgrid 

        formContext = Xrm.Page.ui.formContext
        var subgrid = formContext.ui.controls.get(`Subgrid_Audit_Detail`);
        subgrid.refresh();


    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }

}

PROXCardAuditSDK.auditDetailAsync = (auditDetailId, isaudit, userid) => {
    var body = {
        "clp_isaudited": isaudit,
        "clp_latest_modified_on": new Date()
    }

    if (userid) {
        body["clp_latest_modified_by@odata.bind"] = `/clp_users(${userid})`
    }

    return fetch(
        `/api/data/v9.0/clp_proximity_card_audit_details(${auditDetailId})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

PROXCardAuditSDK.getAuditDetailsAsync = (auditid) => {

    return fetch(`/api/data/v9.0/clp_proximity_card_audits(${auditid})?$expand=clp_pc_audit_detail_audit`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message);
        }

        return Promise.resolve(res.clp_pc_audit_detail_audit)

    }).catch(err => {
        console.error(err)
        throw err;
    })

}

PROXCardAuditSDK.updateAuditAsync = (auditid, isaudit, userid) => {
    var body = {
        "clp_isaudited": isaudit,
        "clp_latest_modified_on": new Date(),
    }

    if (userid) {
        body["clp_latest_modified_by@odata.bind"] = `/clp_users(${userid})`
    }

    return fetch(
        `/api/data/v9.0/clp_proximity_card_audits(${auditid})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

PROXCardAuditSDK.enableButton = () => {


    var currentuserid = ShareSdk.getCurrentUserId()
    var owner = Xrm.Page.data.entity.attributes.getByName('ownerid').getValue();
    var ownerid = owner[0].id.replace(/[{}]*/g, "").toLowerCase()
    return currentuserid == ownerid || ShareSdk.isProximityCardAdmin()
}

PROXCardAuditSDK.enableUnAuditButton = () => {

    var currentuserid = ShareSdk.getCurrentUserId()
    var owner = Xrm.Page.data.entity.attributes.getByName('ownerid').getValue();
    var ownerid = owner[0].id.replace(/[{}]*/g, "").toLowerCase()
    return currentuserid == ownerid || ShareSdk.isProximityCardAdmin()

}